#include "table.h"
#include <algorithm>
#include <cmath>
#include <iomanip>

#if defined(GENERIC)
ostream & Table::Print(ostream &os) const
{
  // WRITE THIS
  os << "Table()";
  return os;
}
#endif

#if defined(LINKSTATE)
Table::Table(){}

Table::Table(const Table *rhs){
  this->storedSeq = rhs->storedSeq;
  this->topo = rhs->topo;
  this->fwdTable = rhs->fwdTable;
  this->nodeNumber = rhs->nodeNumber;
}

Table::Table(unsigned n, deque<Link*>* Links)
{  
  nodeNumber = n;
  deque<Link*>::iterator itr;
  unsigned num;
  double latency;

  fwdTable.insert(pair<int,int>(nodeNumber,nodeNumber)); //maybe not, add ourself into the routing table
  knownNodes.push_back(nodeNumber); //push ourselves into knownNodes
  for (itr=Links->begin(); itr!=Links->end(); itr++){
    knownNodes.push_back((*itr)->GetDest());
  }
  
  //initialize storedSeq mat
  int s = knownNodes.size();
  map<int,int> innerMap;
  map<int,double> innerMap2;
  for (int i=0; i<s; i++) { //outer loop for row (from) 
    innerMap.clear();
    for (int j=0; j<s; j++) { //inner loop for colunm (cost to) 
      innerMap.insert(pair<int,int>(knownNodes[j],-1));
      innerMap2.insert(pair<int,double>(knownNodes[j],9999999.0));
    }
    storedSeq.insert(pair<int,map<int,int> >(knownNodes[i],innerMap));
    topo.insert(pair<int,map<int,double> >(knownNodes[i], innerMap2));
  }

  topo[nodeNumber][nodeNumber] = 9999999.0;
  storedSeq[nodeNumber][nodeNumber] = -1;
  //update our topo and fwd table
  for (itr=Links->begin();itr!=Links->end(); itr++) 
    {
      num = (*itr)->GetDest();
      latency = (*itr)->GetLatency();
      topo[nodeNumber][num] = latency;
      fwdTable.insert(pair<int,int>(num,num)); //if go to one of our neighbors, it is our next stop!
	  //routeLength.insert(pair<int,double>(num,latency)); //if go to one of our neighbors, our routeLength would just be the latency
    }
}

bool Table::processMessage(const RoutingMessage* m)
{
  unsigned src, dest;
  int seqNum;
  double newLat;
  src = m->src;
  dest = m->dest;
  newLat = m->updatedLat;
  seqNum = m->seqNum;
  
  if  (find(knownNodes.begin(), knownNodes.end(), src) == knownNodes.end()) {
	expandMatrix(src);
	//fwdTable.insert(pair<int,int>(src,-1)); //do not have meaning, just put newNode into fwdTable so we have a record
  } 

  if  (find(knownNodes.begin(), knownNodes.end(), dest) == knownNodes.end()) {
    expandMatrix(dest);
  }
  
  if (seqNum > storedSeq[src][dest]) {
    storedSeq[src][dest] = seqNum;
    topo[src][dest] = newLat;
    this->updateTables();
    return true;
  } else {
    return false;
  }
}

void Table::expandMatrix(unsigned newNode)
{
      unsigned from;
      /*-------------Expand  dimension of dv matrix ------------------*/
      //add new colunm
      for (unsigned i=0; i < knownNodes.size(); i++) {
		from = knownNodes[i];
				//update***  changed dest to newNode ***
		(topo[from]).insert(pair<int,double>(newNode, 9999999.0));
		(storedSeq[from]).insert(pair<int,int>(newNode,-1));
      }
      
      knownNodes.push_back(newNode);
      
      //here we are going to insert a new row
      //build innerMap first
      map<int,double> innerMap;
	  map<int, int> innerMap2;
      for (unsigned i=0; i<knownNodes.size(); i++) {
		innerMap.insert(pair<int,double>(knownNodes[i],9999999.0));
		innerMap2.insert(pair<int,int>(knownNodes[i],-1));
      }
      
      topo.insert(pair<int,map<int,double> >(newNode,innerMap));
      storedSeq.insert(pair<int,map<int,int> >(newNode,innerMap2));
      fwdTable.insert(pair<int,int>(newNode,newNode));
      /*------------------------Expansion end ------------------------*/
}


void Table::updateTables()
{
	set<int> N;
	map<int,double> routeLength;
  
	unsigned dest, smallest_num, neighbor;
	double length;
	double lat;
	double smallest_lat;
	int counter = 0;
	N.insert(nodeNumber);
	//initialization
	map<int, double> temp = topo[nodeNumber];
	map<int, double>::iterator itr;
	for (itr=temp.begin();itr!=temp.end();itr++) {
	        dest = itr->first; //get dest
		lat = itr->second; //get lat
		if (dest!=nodeNumber) {
		  if (lat < 9999999.0) {routeLength.insert(pair<int,double>(dest, lat));}
			else {routeLength.insert(pair<int,double>(dest, 9999999.0));}
		}
	}

	do {
	  /*
	  cout<<"-----print routeLength-----"<<endl;
	  for(itr=routeLength.begin(); itr!=routeLength.end(); itr++) {
	    dest = itr->first;
	    length = itr->second;
	    cout<<"dest: "<<dest<<"      route length: "<<length<<endl;
	  } 
	  cout<<"------end of routelength------"<<endl;
	  */
	        smallest_lat = INFINITY;
		for (itr=routeLength.begin(); itr!=routeLength.end(); itr++)
		{
			dest = itr->first;
			length = itr->second;	
			if ((length < smallest_lat) && (N.find(dest) == N.end())) { //check dest is not currently in N and is smaller than original smallest lat
				smallest_num = dest;
				smallest_lat = length; //update
			}
		}
		if (topo[nodeNumber][smallest_num]<9999999.0) {neighbor = smallest_num;}
		//neighbor = smallest_num; //remember where we hop on
		N.insert(smallest_num);
		//update D(v) for each neighbor of dest and not in N
		map<int,double> temp = topo[smallest_num];
		for (itr=temp.begin(); itr!=temp.end(); itr++) //loop through neighbors of smallest_num 
		{
			dest = itr->first;
			//lat = itr->second;
			if ((N.find(dest) == N.end()) && (topo[smallest_num][dest] < 999999.0)) { //dest not in N and dest is a neighbor
				if ((topo[smallest_num][dest]+routeLength[smallest_num]) < routeLength[dest]) {
					routeLength[dest] = topo[smallest_num][dest]+routeLength[smallest_num];
					fwdTable[dest] = neighbor;
				}
			}
		}
		counter++;
	} while(setChecker(&N));
}
//check whether set N is equal to all nodes that we have known
bool Table::setChecker(set<int>* N)
{
	//why == N.end() ? 
	for (unsigned i=0; i < knownNodes.size(); i++) {
		if (N->find(knownNodes[i]) == N->end()) {
		  //cout<<knownNodes[i]<<" is never in N! node number:"<<nodeNumber<<endl;
		  return true;} //if we have something in knownNodes that is not in N, return true and continue loop
	}
	return false;
}

map<int, int> Table::GetFwdTable() const
{
    return fwdTable;
}

void Table::PrintTopo()
{
  cout<<"----------------[NODE: "<<nodeNumber<<" Topo]--------------"<<endl;
  int from, to;
  for (unsigned i=0; i<knownNodes.size()+1; i++){
    for (unsigned j=0; j<knownNodes.size()+1; j++){
      if (i==0 && j==0) {cout<<setw(5)<<"X";}
      else if (i==0) {cout<<setw(5)<<knownNodes[j-1];}
      else if (j==0) {cout<<setw(5)<<knownNodes[i-1];}
      else {
	from = knownNodes[i-1];
	to = knownNodes[j-1];
	cout<<setw(5)<<topo[from][to];
      }
    }
    cout<<"\n";
  }
  cout<<"--------------- END OF TOPO ----------------"<<endl;
}

void Table::PrintSeq()
{
  cout<<"----------------[NODE: "<<nodeNumber<<" Seq]----------------"<<endl;
  int from, to;
  for (unsigned i=0; i<knownNodes.size()+1; i++){
    for (unsigned j=0; j<knownNodes.size()+1; j++){
      if (i==0 && j==0) {cout<<setw(5)<<"X";}
      else if (i==0) {cout<<setw(5)<<knownNodes[j-1];}
      else if (j==0) {cout<<setw(5)<<knownNodes[i-1];}
      else {
	from = knownNodes[i-1];
	to = knownNodes[j-1];
	cout<<setw(5)<<storedSeq[from][to];
      }
    }
    cout<<"\n";
  } //outer loop
  cout<<"----------------- END OF SEQ ----------------"<<endl;
}

void Table::PrintFwd()
{
  cout<<"-----------------[NODE: "<<nodeNumber<<" Fwd]------------"<<endl;
  map<int,int>::iterator itr;
  unsigned dest, nextHop;
  for (itr=fwdTable.begin();itr!=fwdTable.end();itr++){
    dest = itr->first;
    nextHop = itr->second;
    cout<<setw(5)<<dest<<setw(5)<<nextHop<<endl;
  }
  cout<<"-------------------END OF FWD-----------------"<<endl;
}
#endif

#if defined(DISTANCEVECTOR)

Table::Table(){}

Table::Table(const Table *rhs){
  this->latencies = rhs->latencies;
  this->distanceVectorMat = rhs->distanceVectorMat;
  this->routing = rhs->routing;
  this->knownNodes = rhs->knownNodes;
  this->nodeNumber = rhs->nodeNumber;
  //this->ourNode = rhs->ourNode;
}

Table::Table(unsigned n, deque<Link*>* Links){
  nodeNumber = n;
  //deque<Node*> *Neighbors = n->GetNeighbors();
  //deque<Link*> *Links = n->GetOutgoingLinks();
  deque<Link*>::iterator itr;
  unsigned num;
  double latency;
  latencies.insert(pair<int,double>(nodeNumber,0.0)); //add latency to our node itself which is 0.0
  knownNodes.push_back(nodeNumber); //add ourself into known nodes
  routing.insert(pair<int,int>(nodeNumber,nodeNumber)); //add ourself into the routing table
  for (itr=Links->begin(); itr!=Links->end(); itr++){
    latency = (*itr)->GetLatency();
    num = (*itr)->GetDest();
    latencies.insert(pair<int,double>(num,latency)); //initialize all link costs to our neighbors
	routing.insert(pair<int,int>(num,num)); //populate forward table
	knownNodes.push_back(num); //populate known nodes
  }
  //this->InitDistVecMat();
}

RoutingMessage* Table::InitDistVecMat(){
  int s = knownNodes.size();
  map<int,double> innerMap;
  //set everything to infinity for now
  for (int i=0; i<s; i++) { //outer loop for row (from) 
    innerMap.clear();
    for (int j=0; j<s; j++) { //inner loop for colunm (cost to) 
      innerMap.insert(pair<int,double>(knownNodes[j],INFINITY));
    }
    distanceVectorMat.insert(pair<int,map<int,double> >(knownNodes[i],innerMap));	
  }

  RoutingMessage* initMessage = new RoutingMessage(DV_UPDATE);
  map<int,double>::iterator itr;
  unsigned dest;
  double dv;
  distanceVectorMat[nodeNumber][nodeNumber]=0.0;
  for (itr=latencies.begin();itr!=latencies.end();itr++){
    dest = itr->first; //get dest node
    dv = itr->second; //get dv
	distanceVectorMat[nodeNumber][dest] = dv; //set correct dv
  }
  
  initMessage->changedOrNot = true;
  initMessage->AddDVUpdates(nodeNumber,&distanceVectorMat[nodeNumber]);
  cout<<"Print from Initialization"<<endl;
  this->PrintDV();
  return initMessage;
}

RoutingMessage* Table::UpdateDistVecMat(const RoutingMessage* m){
  unsigned dest;
  double updatedDV;
  int type = (*m).type;
  map<int,double>::iterator itr;
  bool ifChanged = false;
  unsigned neighbor = (*m).neighbor;
  
  switch (type){
  case LINK_UPDATE:{
    RoutingMessage* LinkChange = new RoutingMessage(DV_UPDATE); 
    //update latency
    latencies[neighbor] = (*m).updatedLat; 
    distanceVectorMat[nodeNumber][neighbor] = latencies[neighbor];
    routing[neighbor] = neighbor;

    for (unsigned i=0; i < knownNodes.size(); i++) { //loop through all the knownNodes and compute new dv
      dest = knownNodes[i];
	  if (FindSmallestDV(dest)) {
		ifChanged = true;
	  }
    } //outer loop
      LinkChange->changedOrNot = ifChanged;
      LinkChange->AddDVUpdates(nodeNumber,&distanceVectorMat[nodeNumber]);
      cout<<"Print from Link update"<<endl;
      this->PrintDV();
      return LinkChange;
    break;
  }
  case DV_UPDATE:{
    RoutingMessage* DVChange = new RoutingMessage(DV_UPDATE);
    unsigned from;
    //double newDV;
    //map<int,double>::iterator itr;
    map<int,double>* updates = (*m).updatedDV;
    //bool ifChanged = false;  
    for (itr=updates->begin(); itr!=updates->end(); itr++){ //big loop to process each dv update from neighbor
    
     dest = itr->first; //retrieve node num
     updatedDV = itr->second; //retrieve new dv value
    
    //check if we have dest in our knownNodes
    if (find(knownNodes.begin(), knownNodes.end(), dest) != knownNodes.end()){ //we have dest in our knownNodes already
      //update dv for neighbor in our table
      distanceVectorMat[neighbor][dest] = updatedDV;
      if (FindSmallestDV(dest)) {
		  ifChanged = true;
	  }
    } else { //we do not have dest in our knownNodes      
      ifChanged = true;
      /*-------------Expand  dimension of dv matrix ------------------*/
      //add new colunm
      for (unsigned i=0; i<knownNodes.size(); i++) {
	from = knownNodes[i];
	(distanceVectorMat[from]).insert(pair<int,double>(dest, INFINITY)); //insert new column and initialize dv to infinity
      }
      
      knownNodes.push_back(dest);
      
      //here we are going to insert a new row
      //build innerMap first
      map<int,double> innerMap;
      for (unsigned i=0; i<knownNodes.size(); i++) {
	innerMap.insert(pair<int,double>(knownNodes[i],INFINITY));
      }
      
      //add the row
      distanceVectorMat.insert(pair<int,map<int,double> >(dest,innerMap));
      /*------------------------Expansion end ------------------------*/
      
      //Now we need to update dv from src to dest in our table
      distanceVectorMat[neighbor][dest] = updatedDV;
      //update dv from our node to dest in our table DV(ourNode,dest)=cost(ourNode,neighbor)+DV(neighbor,dest)
      distanceVectorMat[nodeNumber][dest] = latencies[neighbor] + updatedDV;
      routing.insert(pair<int,int>(dest,neighbor));
    } //if 
   } //for
    
    //send out our dv to our neighbors if there are changes
    DVChange->changedOrNot = ifChanged;
    DVChange->AddDVUpdates(nodeNumber,&distanceVectorMat[nodeNumber]);
    cout<<"Print from DV update"<<endl;
    this->PrintDV();
    return DVChange;
    break;
  }
  default: {
    cout<<"Error: neither of the two defined types of message. The type is:"<<type<<endl;
    exit(0);
  }
  }//switch
} //end of routine

/* ---
bool Table::GetNeighborNode(unsigned n, Node* &ptr){
  deque<Node*>* Neighbors = ourNode->GetNeighbors();
  deque<Node*>::iterator itr;
  unsigned nodeNum;
  for (itr=Neighbors->begin();itr!=Neighbors->end();itr++){
    nodeNum=(*itr)->GetNumber();
    if (nodeNum==n) {
      ptr=(*itr);
      return true;
    }
  }
  return false;
}
----*/

bool Table::FindSmallestDV(unsigned dest) {
	unsigned tempNum;
	double tempDV, newDV,oldDV, lat;
	map<int,double>::iterator itr;
	bool changed = false;
	if (dest == nodeNumber) { //if dest is our node, dv = 0, no change is made
       distanceVectorMat[nodeNumber][dest] = 0;
       routing[dest] = nodeNumber;
	   return false;
     } else {
      oldDV = distanceVectorMat[nodeNumber][dest];
      tempDV = INFINITY;
      newDV = tempDV;
      for (itr=latencies.begin(); itr!=latencies.end(); itr++) { //loop through all our neighbors
		tempNum = itr->first;
		lat = itr->second;
		if (tempNum != nodeNumber) { //skip nodeNumber because nodeNumber is not actually a neighbor
			if (dest == tempNum) {
				tempDV = lat;
			} else {
				tempDV = lat + distanceVectorMat[tempNum][dest];
			}
			if (tempDV < newDV) {
				routing[dest] = tempNum;
				newDV = tempDV;
			}
		}
      }
      distanceVectorMat[nodeNumber][dest] = newDV;
      if (newDV!=oldDV) {
			changed = true;
      } //if newDV and we need to update
	}
	return changed;
}

map<int,int> Table::GetFwdTable() const{
  return routing;
}

void Table::PrintDV() {
  cout<<"----------[NODE: "<<nodeNumber<<"]------------"<<endl;
  int from, to;
  for (unsigned i=0; i<knownNodes.size()+1; i++) {
    for (unsigned j=0; j<knownNodes.size()+1; j++) {
      if (i==0 && j==0) {cout<<setw(5)<<"X";}
      else if (i==0) {cout<<setw(5)<<knownNodes[j-1];}
      else if (j==0) {cout<<setw(5)<<knownNodes[i-1];}
      else {
	from = knownNodes[i-1];
	to = knownNodes[j-1];
	cout<<setw(5)<<distanceVectorMat[from][to];
      }
    } //inner loop
    cout<<"\n";
  } //outer loop
  cout<<"-------------------------------"<<endl;
}

#endif
