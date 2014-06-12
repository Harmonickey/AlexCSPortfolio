#include <assert.h>
#include <cstring>
#include "btree.h"


KeyValuePair::KeyValuePair()
{}


KeyValuePair::KeyValuePair(const KEY_T &k, const VALUE_T &v) : 
  key(k), value(v)
{}


KeyValuePair::KeyValuePair(const KeyValuePair &rhs) :
  key(rhs.key), value(rhs.value)
{}


KeyValuePair::~KeyValuePair()
{}


KeyValuePair & KeyValuePair::operator=(const KeyValuePair &rhs)
{
  return *( new (this) KeyValuePair(rhs));
}

BTreeIndex::BTreeIndex(SIZE_T keysize, 
		       SIZE_T valuesize,
		       BufferCache *cache,
		       bool unique) 
{
  superblock.info.keysize=keysize;
  superblock.info.valuesize=valuesize;
  buffercache=cache;
  // note: ignoring unique now
}

BTreeIndex::BTreeIndex()
{
  // shouldn't have to do anything
}


//
// Note, will not attach!
//
BTreeIndex::BTreeIndex(const BTreeIndex &rhs)
{
  buffercache=rhs.buffercache;
  superblock_index=rhs.superblock_index;
  superblock=rhs.superblock;
}

BTreeIndex::~BTreeIndex()
{
  // shouldn't have to do anything
}


BTreeIndex & BTreeIndex::operator=(const BTreeIndex &rhs)
{
  return *(new(this)BTreeIndex(rhs));
}


ERROR_T BTreeIndex::AllocateNode(SIZE_T &n)
{
  n=superblock.info.freelist;

  if (n==0) { 
    return ERROR_NOSPACE;
  }

  BTreeNode node;

  node.Unserialize(buffercache,n);

  assert(node.info.nodetype==BTREE_UNALLOCATED_BLOCK);

  superblock.info.freelist=node.info.freelist;

  superblock.Serialize(buffercache,superblock_index);

  buffercache->NotifyAllocateBlock(n);

  return ERROR_NOERROR;
}


ERROR_T BTreeIndex::DeallocateNode(const SIZE_T &n)
{
  BTreeNode node;

  node.Unserialize(buffercache,n);

  assert(node.info.nodetype!=BTREE_UNALLOCATED_BLOCK);

  node.info.nodetype=BTREE_UNALLOCATED_BLOCK;

  node.info.freelist=superblock.info.freelist;

  node.Serialize(buffercache,n);

  superblock.info.freelist=n;

  superblock.Serialize(buffercache,superblock_index);

  buffercache->NotifyDeallocateBlock(n);

  return ERROR_NOERROR;

}

ERROR_T BTreeIndex::Attach(const SIZE_T initblock, const bool create)
{
  ERROR_T rc;

  superblock_index=initblock;
  assert(superblock_index==0);

  if (create) {
    // build a super block, root node, and a free space list
    //
    // Superblock at superblock_index
    // root node at superblock_index+1
    // free space list for rest
    BTreeNode newsuperblock(BTREE_SUPERBLOCK,
			    superblock.info.keysize,
			    superblock.info.valuesize,
			    buffercache->GetBlockSize());
    newsuperblock.info.rootnode=superblock_index+1;
    newsuperblock.info.freelist=superblock_index+2;
    newsuperblock.info.numkeys=0;

    buffercache->NotifyAllocateBlock(superblock_index);

    rc=newsuperblock.Serialize(buffercache,superblock_index);

    if (rc) { 
      return rc;
    }
    
    BTreeNode newrootnode(BTREE_ROOT_NODE,
			  superblock.info.keysize,
			  superblock.info.valuesize,
			  buffercache->GetBlockSize());
    newrootnode.info.rootnode=superblock_index+1;
    newrootnode.info.freelist=superblock_index+2;
    newrootnode.info.numkeys=0;

    buffercache->NotifyAllocateBlock(superblock_index+1);

    rc=newrootnode.Serialize(buffercache,superblock_index+1);

    if (rc) { 
      return rc;
    }

    for (SIZE_T i=superblock_index+2; i<buffercache->GetNumBlocks();i++) { 
      BTreeNode newfreenode(BTREE_UNALLOCATED_BLOCK,
			    superblock.info.keysize,
			    superblock.info.valuesize,
			    buffercache->GetBlockSize());
      newfreenode.info.rootnode=superblock_index+1;
      newfreenode.info.freelist= ((i+1)==buffercache->GetNumBlocks()) ? 0: i+1;
      
      rc = newfreenode.Serialize(buffercache,i);

      if (rc) {
	return rc;
      }

    }
  }

  // OK, now, mounting the btree is simply a matter of reading the superblock 

  return superblock.Unserialize(buffercache,initblock);
}
    

ERROR_T BTreeIndex::Detach(SIZE_T &initblock)
{
  return superblock.Serialize(buffercache,superblock_index);
}
 

ERROR_T BTreeIndex::LookupOrUpdateInternal(const SIZE_T &node,
					   const BTreeOp op,
					   const KEY_T &key,
					   VALUE_T &value)
{
  BTreeNode b;
  ERROR_T rc;
  SIZE_T offset;
  KEY_T testkey;
  SIZE_T ptr;

  rc= b.Unserialize(buffercache,node);

  if (rc!=ERROR_NOERROR) { 
    return rc;
  }

  switch (b.info.nodetype) { 
  case BTREE_ROOT_NODE:
  case BTREE_INTERIOR_NODE:
    // Scan through key/ptr pairs
    //and recurse if possible
    for (offset=0;offset<b.info.numkeys;offset++) { 
      rc=b.GetKey(offset,testkey);
      if (rc) {  return rc; }
      if (key<testkey) {
	// OK, so we now have the first key that's larger
	// so we ned to recurse on the ptr immediately previous to 
	// this one, if it exists
	rc=b.GetPtr(offset,ptr);
        
	if (rc) { return rc; }
	return LookupOrUpdateInternal(ptr,op,key,value);
      }
    }
    // if we got here, we need to go to the next pointer, if it exists
    if (b.info.numkeys>0) { 
      rc=b.GetPtr(b.info.numkeys,ptr);
      if (rc) { return rc; }
      return LookupOrUpdateInternal(ptr,op,key,value);
    } else {
      // There are no keys at all on this node, so nowhere to go
      return ERROR_NONEXISTENT;
    }
    break;
  case BTREE_LEAF_NODE:
    // Scan through keys looking for matching value
    for (offset=0;offset<b.info.numkeys;offset++) { 
      rc=b.GetKey(offset,testkey);
      if (rc) {  return rc; }
      if (testkey==key) { 
	if (op==BTREE_OP_LOOKUP) { 
	  return b.GetVal(offset,value);
	} else { 
	  // BTREE_OP_UPDATE
	  // Update, then Serialize to disk
	  rc =  b.SetVal(offset, value);
          if (rc) { return rc; }
          return b.Serialize(buffercache, node);
	}
      }
    }
    return ERROR_NONEXISTENT;
    break;
  default:
    // We can't be looking at anything other than a root, internal, or leaf
    return ERROR_INSANE;
    break;
  }  

  return ERROR_INSANE;
}

ERROR_T BTreeIndex::InsertInternal(const SIZE_T &node,
				   const BTreeOp op,
				   const KEY_T &key,
				   VALUE_T &value,
                                   list<SIZE_T> previousNode)
{
  BTreeNode b;
  ERROR_T rc;
  SIZE_T offset;
  KEY_T insertkey = key; //keep an extra copy for sorted insertion
  KEY_T testkey;
  VALUE_T testvalue;
  SIZE_T ptr;

  rc= b.Unserialize(buffercache,node);

  if (rc!=ERROR_NOERROR) { 
    return rc;
  }

  switch (b.info.nodetype) { 
  case BTREE_ROOT_NODE:

    //special case for when the root is empty
    if (b.info.numkeys == 0) {
      SIZE_T newNode;
      rc = AllocateNode(newNode);
      if (rc) { return rc; }
      // make the root a pseudo-leaf node temporarily
      b.info.nodetype = BTREE_LEAF_NODE;
      superblock.info.rootnode = newNode;
      b.Serialize(buffercache, newNode);
      //recurse so we can do the actual insertion
      return InsertInternal(newNode, op, key, value, previousNode);
    }
  case BTREE_INTERIOR_NODE:
    // Scan through key/ptr pairs
    // and recurse if possible
    for (offset=0;offset<b.info.numkeys;offset++) { 
      rc=b.GetKey(offset,testkey);
      if (rc) {  return rc; }
      if (key<testkey) {
	// OK, so we now have the first key that's larger
	// so we ned to recurse on the ptr immediately previous to 
	// this one, if it exists
	rc=b.GetPtr(offset,ptr);
        previousNode.push_front(node);
	if (rc) { return rc; }
	return InsertInternal(ptr,op,key,value,previousNode);
      }
    }

    // if we got here, we need to go to the next pointer, if it exists
    if (b.info.numkeys>0) { 
      //make sure to save this node for splitting, traversing back up the tree
      rc=b.GetPtr(b.info.numkeys,ptr);
      previousNode.push_front(node);
      if (rc) { return rc; }
      return InsertInternal(ptr,op,key,value,previousNode);
    } else {
      
      return ERROR_NONEXISTENT;
    }
    break;
  case BTREE_LEAF_NODE:
    if (b.info.numkeys>0) {

      //we are saying that if there are X slots, then we need to split of X-1 slots full
      SIZE_T slots_left = b.info.GetNumSlotsAsLeaf() - b.info.numkeys - 1;

      if (slots_left > 0) {

        for (offset=0; offset<b.info.numkeys;offset++) {
          rc=b.GetKey(offset, testkey);
          if (rc) { return rc; }
          //don't insert into the leaf until we find a key
          //      that's bigger, then move all the other keys right
          if (insertkey < testkey) {
            rc=b.GetVal(offset, testvalue);
            rc=b.SetKey(offset,insertkey);
            rc=b.SetVal(offset,value);
            insertkey = testkey;
            value = testvalue;
          } else if (key == testkey) { return ERROR_CONFLICT; } //unique only!
        }
        //insert the key (or final key being pushed right)
        b.info.numkeys++;
        rc = b.SetKey(b.info.numkeys - 1, insertkey); 
        if (rc) { return rc; }
        rc = b.SetVal(b.info.numkeys - 1, value);
        if (rc) { return rc; }

        //we need to split on the root node (special case acting as a leaf node)
        if (slots_left == 1 && superblock.info.rootnode == node)
        {
 
          BTreeNode leftleaf = b;
          BTreeNode rightleaf = b;

          leftleaf.info.numkeys = b.info.numkeys / 2;
          rightleaf.info.numkeys = (b.info.numkeys % 2 == 0) ? b.info.numkeys / 2 : b.info.numkeys / 2 + 1;

          leftleaf.info.nodetype = BTREE_LEAF_NODE;
          rightleaf.info.nodetype = BTREE_LEAF_NODE;

          for (SIZE_T i=0,j=0; i < b.info.numkeys; i++,j++) {
            //assign the values to the new nodes
            if (i == b.info.numkeys / 2) j = 0;
            if (i < b.info.numkeys / 2)
            {
              b.GetKey(i, testkey);
              leftleaf.SetKey(j, testkey);
              b.GetVal(i, testvalue);
              leftleaf.SetVal(j, testvalue);
            } else {
              b.GetKey(i, testkey);
              rightleaf.SetKey(j, testkey);
              b.GetVal(i, testvalue);
              rightleaf.SetVal(j, testvalue);
            }
          } 
            
          SIZE_T rightnode;
          SIZE_T leftnode;
          AllocateNode(leftnode);
          AllocateNode(rightnode);

          BTreeNode newRoot = b;

          newRoot.info.nodetype = BTREE_ROOT_NODE;
          newRoot.info.numkeys = 1;
 
          newRoot.SetPtr(0, leftnode);
          newRoot.SetPtr(1, rightnode);

          //propagate the splitting key into the root
          rightleaf.GetKey(0, testkey);
          newRoot.SetKey(0, testkey);

          SIZE_T newRootNode;
          AllocateNode(newRootNode);

          superblock.info.rootnode = newRootNode;
          newRoot.Serialize(buffercache, newRootNode);
          leftleaf.Serialize(buffercache, leftnode);
          rightleaf.Serialize(buffercache, rightnode);
        } else if (slots_left == 1) { // normal splitting cases
          
          BTreeNode parent; 
          do 
          {
            // we split on the leaf node
            if (b.info.nodetype == BTREE_LEAF_NODE)
            {
              BTreeNode leftleaf = b;
              BTreeNode rightleaf = b;

              leftleaf.info.numkeys = b.info.numkeys / 2;
              rightleaf.info.numkeys = (b.info.numkeys % 2 == 0) ? b.info.numkeys / 2 : b.info.numkeys / 2 + 1;

              leftleaf.info.nodetype = BTREE_LEAF_NODE;
              rightleaf.info.nodetype = BTREE_LEAF_NODE;

              for (SIZE_T i=0,j=0; i < b.info.numkeys; i++,j++) {
                  //assign the key/values to the new nodes
                  if (i == b.info.numkeys / 2) j = 0; //this is where we assign the rest to the right side
                  if (i < b.info.numkeys / 2)
                  {
                      b.GetKey(i, testkey);
                      leftleaf.SetKey(j, testkey);
                      b.GetVal(i, testvalue);
                      leftleaf.SetVal(j, testvalue);
                  } else {
                      b.GetKey(i, testkey);
                      rightleaf.SetKey(j, testkey);
                      b.GetVal(i, testvalue);
                      rightleaf.SetVal(j, testvalue);
                  }
              } 

              SIZE_T leftNode;
              SIZE_T rightNode;
              AllocateNode(leftNode);
              AllocateNode(rightNode);

              //need to get the saved parent node to this leaf so
              //     we can point it to the new leaves
              SIZE_T parentNode = previousNode.front(); 
              SIZE_T& parentNodeAdr = parentNode;
              parent.Unserialize(buffercache, parentNodeAdr);
              previousNode.pop_front();

              //insert the propagated key into the parent in sorted order
              //    and move other keys right if needed
              rightleaf.GetKey(0, insertkey);  //the key to be propagated up
              int foundPlace = 0;
              SIZE_T insertLocation = 0;
              for (offset=0; offset<parent.info.numkeys;offset++) {
                  rc=parent.GetKey(offset, testkey);
                  if (rc) { return rc; }
                  if (insertkey < testkey) {
                      if (!foundPlace) insertLocation = offset;
                      rc=parent.SetKey(offset,insertkey);
                      insertkey = testkey;
                      foundPlace = 1;
                  } 
              }
              parent.info.numkeys++;
              parent.SetKey(parent.info.numkeys - 1, insertkey);
              if (foundPlace)
              {  //propagate pointers forward across the node if needed
                for (offset=parent.info.numkeys - 1; offset>=insertLocation + 1;offset--) {
                   rc=parent.GetPtr(offset, ptr);
                   rc=parent.SetPtr(offset + 1, ptr);
                }

                parent.SetPtr(insertLocation, leftNode);
                parent.SetPtr(insertLocation + 1, rightNode);
              } else {
                parent.SetPtr(parent.info.numkeys - 1, leftNode);
                parent.SetPtr(parent.info.numkeys, rightNode);
              }
              
              //assign a new 'current node' (which is b...) as the parent
              if (b.info.GetNumSlotsAsInterior() - 1 == parent.info.numkeys)
              {
                b = parent;
              } 
              parent.Serialize(buffercache, parentNode);
              leftleaf.Serialize(buffercache, leftNode);
              rightleaf.Serialize(buffercache, rightNode);
              
            } else if (b.info.nodetype == BTREE_INTERIOR_NODE) {

              // need to handle the split and also propagate the ptrs
              BTreeNode leftleaf = b;
              BTreeNode rightleaf = b;

              leftleaf.info.numkeys = b.info.numkeys / 2;
              rightleaf.info.numkeys = (b.info.numkeys % 2 == 0) ? b.info.numkeys / 2 : b.info.numkeys / 2 + 1;

              leftleaf.info.nodetype = BTREE_INTERIOR_NODE;
              rightleaf.info.nodetype = BTREE_INTERIOR_NODE;

              for (SIZE_T i=0,j=0; i <= b.info.numkeys; i++,j++) {
                  //assign the keys and ptrs to the new nodes
                  if (i == b.info.numkeys / 2) {
                      //this is where we assign the rest to the right leaf
                      b.GetPtr(i, ptr);
                      leftleaf.SetPtr(j, ptr);
                      j = 0;
                  }
                  if (i < b.info.numkeys / 2)
                  {
                      b.GetKey(i, testkey);
                      leftleaf.SetKey(j, testkey);
                      b.GetPtr(i, ptr);
                      leftleaf.SetPtr(j, ptr);
                  } else {
                      //make sure we stop the assignment of keys even though
                      //     we need to assign +1 more pointer
                      if (i != b.info.numkeys) {
                        b.GetKey(i, testkey);
                        rightleaf.SetKey(j, testkey);
                      }
                      b.GetPtr(i, ptr);
                      rightleaf.SetPtr(j, ptr);
                  }
              } 

              SIZE_T leftNode;
              SIZE_T rightNode;
              AllocateNode(leftNode);
              AllocateNode(rightNode);

              SIZE_T parentNode = previousNode.front(); 
              SIZE_T& parentNodeAdr = parentNode;
              parent.Unserialize(buffercache, parentNodeAdr);
              previousNode.pop_front();

              rightleaf.GetKey(0, insertkey);
              int foundPlace = 0;
              SIZE_T insertLocation = 0;
              for (offset=0; offset<parent.info.numkeys;offset++) {
                  rc=parent.GetKey(offset, testkey);
                  if (rc) { return rc; }
                  if (insertkey < testkey) {
                      if (!foundPlace) insertLocation = offset;
                      rc=parent.SetKey(offset,insertkey);
                      insertkey = testkey;
                      foundPlace = 1;
                  } 
              }
              parent.info.numkeys++;
              parent.SetKey(parent.info.numkeys - 1, insertkey);
              if (foundPlace)
              {  //propagate pointers forward across the node
                for (offset=parent.info.numkeys - 1; offset>=insertLocation + 1;offset--) {
                   rc=parent.GetPtr(offset, ptr);
                   rc=parent.SetPtr(offset + 1, ptr);
                }

                parent.SetPtr(insertLocation, leftNode);
                parent.SetPtr(insertLocation + 1, rightNode);
              } else {
                parent.SetPtr(parent.info.numkeys - 1, leftNode);
                parent.SetPtr(parent.info.numkeys, rightNode);
              }

              if (b.info.GetNumSlotsAsInterior() - 1 == parent.info.numkeys)
              {
                b = parent;
              }

              parent.Serialize(buffercache, parentNode);
              leftleaf.Serialize(buffercache, leftNode);
              rightleaf.Serialize(buffercache, rightNode);
               
            } else { // split on the root... BTREE_ROOT_NODE
              
              BTreeNode leftleaf = b;
              BTreeNode rightleaf = b;

              leftleaf.info.numkeys = b.info.numkeys / 2;
              rightleaf.info.numkeys = (b.info.numkeys % 2 == 0) ? b.info.numkeys / 2 : b.info.numkeys / 2 + 1;

              leftleaf.info.nodetype = BTREE_INTERIOR_NODE;
              rightleaf.info.nodetype = BTREE_INTERIOR_NODE;

              for (SIZE_T i=0,j=0; i <= b.info.numkeys; i++,j++) {
                  if (i == b.info.numkeys / 2) {
                      b.GetPtr(i, ptr);
                      leftleaf.SetPtr(j, ptr);
                      j = 0;
                  }
                  if (i < b.info.numkeys / 2)
                  {
                      b.GetPtr(i, ptr);
                      leftleaf.SetPtr(j, ptr);  
		      b.GetKey(i, testkey);
                      leftleaf.SetKey(j, testkey);

                  } else {
                      b.GetPtr(i, ptr);
                      rightleaf.SetPtr(j, ptr);
                      if (i != b.info.numkeys) {
                        b.GetKey(i, testkey);
                        rightleaf.SetKey(j, testkey);
                      }

                  }
              } 

              BTreeNode newRoot = b;

              SIZE_T leftnode;
              SIZE_T rightnode;
              AllocateNode(leftnode);
              AllocateNode(rightnode);

              //the only difference here is that we need to create a new root node
              SIZE_T newRootNode;
              AllocateNode(newRootNode);

              newRoot.info.nodetype = BTREE_ROOT_NODE;
              newRoot.info.numkeys = 1;

              newRoot.SetPtr(0, leftnode);
              rightleaf.GetKey(0, testkey);
              newRoot.SetKey(0, testkey);

              newRoot.SetPtr(1, rightnode);

              superblock.info.rootnode = newRootNode;
              newRoot.Serialize(buffercache, newRootNode);
              leftleaf.Serialize(buffercache, leftnode);
              rightleaf.Serialize(buffercache, rightnode);
              
              return ERROR_NOERROR;  //because we just split root
            }

          } while (b.info.GetNumSlotsAsInterior() - 1 == parent.info.numkeys);
        } else { 
          // just serialize the node as normal
          rc = b.Serialize(buffercache, node);
          if (rc) { return rc; }
        }
        return ERROR_NOERROR;
      }
      return ERROR_NOERROR;
    } else {
      // fill in the first key value because it's an empty root
      //     pseudo-leaf in this extremely special case
      b.info.numkeys = 1;
      rc = b.SetKey(0, key);
      if (rc) { return rc; }
      rc = b.SetVal(0, value);   
      if (rc) { return rc; }  
      rc = b.Serialize(buffercache, node);
      if (rc) { return rc; }
      return ERROR_NOERROR;
    }

    break;
  default:
    // We can't be looking at anything other than a root, internal, or leaf
    return ERROR_INSANE;
    break;
  }  

  return ERROR_INSANE;
}

static ERROR_T PrintNode(ostream &os, SIZE_T nodenum, BTreeNode &b, BTreeDisplayType dt)
{
  KEY_T key;
  VALUE_T value;
  SIZE_T ptr;
  SIZE_T offset;
  ERROR_T rc;
  unsigned i;

  if (dt==BTREE_DEPTH_DOT) { 
    os << nodenum << " [ label=\""<<nodenum<<": ";
  } else if (dt==BTREE_DEPTH) {
    os << nodenum << ": ";
  } else {
  }

  switch (b.info.nodetype) { 
  case BTREE_ROOT_NODE:
  case BTREE_INTERIOR_NODE:
    if (dt==BTREE_SORTED_KEYVAL) {
    } else {
      if (dt==BTREE_DEPTH_DOT) { 
      } else { 
	os << "Interior: ";
      }
      for (offset=0;offset<=b.info.numkeys;offset++) { 
	rc=b.GetPtr(offset,ptr);
	if (rc) { return rc; }
	os << "*" << ptr << " ";
	// Last pointer
	if (offset==b.info.numkeys) break;
	rc=b.GetKey(offset,key);
	if (rc) {  return rc; }
	for (i=0;i<b.info.keysize;i++) { 
	  os << key.data[i];
	}
	os << " ";
      }
    }
    break;
  case BTREE_LEAF_NODE:
    if (dt==BTREE_DEPTH_DOT || dt==BTREE_SORTED_KEYVAL) { 
    } else {
      os << "Leaf: ";
    }
    for (offset=0;offset<b.info.numkeys;offset++) { 
      if (offset==0) { 
	// special case for first pointer
	rc=b.GetPtr(offset,ptr);
	if (rc) { return rc; }
	if (dt!=BTREE_SORTED_KEYVAL) { 
	  os << "*" << ptr << " ";
	}
      }
      if (dt==BTREE_SORTED_KEYVAL) { 
	os << "(";
      }
      rc=b.GetKey(offset,key);
      if (rc) {  return rc; }
      for (i=0;i<b.info.keysize;i++) { 
	os << key.data[i];
      }
      if (dt==BTREE_SORTED_KEYVAL) { 
	os << ",";
      } else {
	os << " ";
      }
      rc=b.GetVal(offset,value);
      if (rc) {  return rc; }
      for (i=0;i<b.info.valuesize;i++) { 
	os << value.data[i];
      }
      if (dt==BTREE_SORTED_KEYVAL) { 
	os << ")\n";
      } else {
	os << " ";
      }
    }
    break;
  default:
    if (dt==BTREE_DEPTH_DOT) { 
      os << "Unknown("<<b.info.nodetype<<")";
    } else {
      os << "Unsupported Node Type " << b.info.nodetype ;
    }
  }
  if (dt==BTREE_DEPTH_DOT) { 
    os << "\" ]";
  }
  return ERROR_NOERROR;
}
  
ERROR_T BTreeIndex::Lookup(const KEY_T &key, VALUE_T &value)
{
  return LookupOrUpdateInternal(superblock.info.rootnode, BTREE_OP_LOOKUP, key, value);
}

ERROR_T BTreeIndex::Insert(const KEY_T &key, const VALUE_T &value)
{
  VALUE_T val = value;
  list<SIZE_T> previousNode;
  return InsertInternal(superblock.info.rootnode, BTREE_OP_INSERT, key, val, previousNode); 
}
  
ERROR_T BTreeIndex::Update(const KEY_T &key, const VALUE_T &value)
{
  VALUE_T val = value;  
  return LookupOrUpdateInternal(superblock.info.rootnode, BTREE_OP_UPDATE, key, val);
}

  
ERROR_T BTreeIndex::Delete(const KEY_T &key)
{
  // This is optional extra credit 
  //
  // 
  return ERROR_UNIMPL;
}

  
//
//
// DEPTH first traversal
// DOT is Depth + DOT format
//

ERROR_T BTreeIndex::DisplayInternal(const SIZE_T &node,
                                    int * firstOne,
				    ostream &o,
				    BTreeDisplayType display_type) const
{
  KEY_T testkey;
  SIZE_T ptr;
  BTreeNode b;
  ERROR_T rc;
  SIZE_T offset;

  rc= b.Unserialize(buffercache,node);

  if (rc!=ERROR_NOERROR) { 
    return rc;
  }

  rc = PrintNode(o,node,b,display_type);
  
  if (rc) { return rc; }

  if (display_type==BTREE_DEPTH_DOT) { 
    o << ";";
  }

  if (display_type!=BTREE_SORTED_KEYVAL) {
    o << endl;
  }

  switch (b.info.nodetype) { 
  case BTREE_ROOT_NODE:
    if (b.info.numkeys>0) { 
      for (offset=0;offset<=b.info.numkeys;offset++) { 
	rc=b.GetPtr(offset,ptr);
	if (rc) { return rc; }
	if (display_type==BTREE_DEPTH_DOT) { 
	  o << node << " -> "<<ptr<<";\n";
	}
	rc=DisplayInternal(ptr,firstOne,o,display_type);
	if (rc) { return rc; }
      }
    }
    return ERROR_NOERROR;
    break;
  case BTREE_INTERIOR_NODE:
    if (b.info.numkeys>0) {
      for (offset=(*firstOne) ? 0 : 1;offset<=b.info.numkeys;offset++) {
        if (offset == 0) *firstOne = 0; //only look at the farthest left leaf in our tree overall
	rc=b.GetPtr(offset,ptr);
	if (rc) { return rc; }
	if (display_type==BTREE_DEPTH_DOT) { 
	  o << node << " -> "<<ptr<<";\n";
	}
	rc=DisplayInternal(ptr,firstOne,o,display_type);
	if (rc) { return rc; }
      }
    }
    return ERROR_NOERROR;
    break;
  case BTREE_LEAF_NODE:
    return ERROR_NOERROR;
    break;
  default:
    if (display_type==BTREE_DEPTH_DOT) { 
    } else {
      o << "Unsupported Node Type " << b.info.nodetype ;
    }
    return ERROR_INSANE;
  }

  return ERROR_NOERROR;
}


ERROR_T BTreeIndex::Display(ostream &o, BTreeDisplayType display_type) const
{
  ERROR_T rc;
  int firstOne = 1;
  if (display_type==BTREE_DEPTH_DOT) { 
    o << "digraph tree { \n";
  }
  rc=DisplayInternal(superblock.info.rootnode,&firstOne,o,display_type);
  if (display_type==BTREE_DEPTH_DOT) { 
    o << "}\n";
  }
  
  return ERROR_NOERROR;
}

ERROR_T BTreeIndex::TraverseTree(const SIZE_T &node, int * firstOne)
{
  BTreeNode b;
  SIZE_T ptr;
  SIZE_T offset;
  ERROR_T rc;
  int isLeafRoot = 1;

  rc = b.Unserialize(buffercache, node);

  switch(b.info.nodetype) {
    case BTREE_ROOT_NODE:
      if (superblock.info.rootnode == node) {
        //good... so search its children
        if (b.info.numkeys>0) {
          for (offset=0;offset<=b.info.numkeys;offset++) {
            rc=b.GetPtr(offset,ptr);
            if (rc) { return rc; }
            rc=TraverseTree(ptr, firstOne);
            if (rc) { return rc; }
          }
        }
      } else {
        return ERROR_INSANE;
      }
      return ERROR_NOERROR;
      break;
    case BTREE_INTERIOR_NODE:
      if (superblock.info.rootnode != node) {
        if (b.info.numkeys>0) {
          for (offset=(*firstOne) ? 0 : 1;offset<=b.info.numkeys;offset++) {
            if (offset == 0) *firstOne = 0;
            rc=b.GetPtr(offset,ptr);
            if (rc) { return rc; }
            rc=TraverseTree(ptr, firstOne);
            if (rc) { return rc; }
          }
        }
      } else {
        return ERROR_INSANE;
      }
      return ERROR_NOERROR;

      break;
    case BTREE_LEAF_NODE:
      // there is a case at the beginning where the root
      // is treated as a leaf for a while in our implementation...
      rc = b.GetPtr(0, ptr);
      // if we don't get back a pointer, then we are the "pseudo-leaf" root
      if (rc == 0) { isLeafRoot = 1; }
      else { isLeafRoot = 0; }

      if (superblock.info.rootnode == node && !isLeafRoot) {
        return ERROR_INSANE;
      }

      return ERROR_NOERROR;
      break;
    default:
      return ERROR_INSANE;
  }

  return ERROR_NOERROR;
}


ERROR_T BTreeIndex::SanityCheck()
{
  //check that superblock.info.rootnode is the only pointer pointing
  //  to the root node
  int firstOne = 1;
  ERROR_T rc;
  cout << "Does superblock rootnode only point to the root?" << endl;
  rc=TraverseTree(superblock.info.rootnode, &firstOne);
  if (rc) {return rc;}
  cout << "No errors, so YES!" << endl;

  //output the keys in order
  cout << "Sorted Keys" << endl;
  Print(cout);  

  return ERROR_NOERROR;
}
  


ostream & BTreeIndex::Print(ostream &os) const
{
  
  Display(os, BTREE_SORTED_KEYVAL);  
  return os;
}




