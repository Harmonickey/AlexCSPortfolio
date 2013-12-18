// ext2 definitions from the real driver in the Linux kernel.
#include "ext2fs.h"

// This header allows your project to link against the reference library. If you
// complete the entire project, you should be able to remove this directive and
// still compile your code.
#include "reference_implementation.h"

// Definitions for ext2cat to compile against.
#include "ext2_access.h"



///////////////////////////////////////////////////////////
//  Accessors for the basic components of ext2.
///////////////////////////////////////////////////////////

// Return a pointer to the primary superblock of a filesystem.
struct ext2_super_block * get_super_block(void * fs) {
  return fs + SUPERBLOCK_OFFSET;  
}


// Return the block size for a filesystem.
__u32 get_block_size(void * fs) {
  return EXT2_BLOCK_SIZE(get_super_block(fs));    
}


// Return a pointer to a block given its number.
// get_block(fs, 0) == fs;
void * get_block(void * fs, __u32 block_num) {
  return fs + block_num * get_block_size(fs);
}


// Return a pointer to the first block group descriptor in a filesystem. Real
// ext2 filesystems will have several of these, but, for simplicity, we will
// assume there is only one.
struct ext2_group_desc * get_block_group(void * fs, __u32 block_group_num) {
  return fs + SUPERBLOCK_OFFSET + SUPERBLOCK_SIZE + block_group_num * EXT2_DESC_SIZE(get_super_block(fs));
}


// Return a pointer to an inode given its number. In a real filesystem, this
// would require finding the correct block group, but you may assume it's in the
// first one.
struct ext2_inode * get_inode(void * fs, __u32 inode_num) {
  struct ext2_super_block* s = get_super_block(fs);
  __u32 inode_per_group = EXT2_INODES_PER_GROUP(s);
  __u32 group_offset = (inode_num - 1) / inode_per_group;
  __u32 inode_offset = (inode_num - 1) % inode_per_group;
  __u32 inode_table_block = get_block_group(fs, group_offset)->bg_inode_table; 
  return get_block(fs, inode_table_block) + inode_offset * EXT2_INODE_SIZE(s); 
}


///////////////////////////////////////////////////////////
//  High-level code for accessing filesystem components by path.
///////////////////////////////////////////////////////////

// Chunk a filename into pieces.
// split_path("/a/b/c") will return {"a", "b", "c"}.
//
// This one's a freebie.
char ** split_path(char * path) {
    int num_slashes = 0;
    for (char * slash = path; slash != NULL; slash = strchr(slash + 1, '/')) {
        num_slashes++;
    }

    // Copy out each piece by advancing two pointers (piece_start and slash).
    char ** parts = (char **) calloc(num_slashes, sizeof(char *));
    char * piece_start = path + 1;
    int i = 0;
    for (char * slash = strchr(path + 1, '/');
         slash != NULL;
         slash = strchr(slash + 1, '/')) {
        int part_len = slash - piece_start;
        parts[i] = (char *) calloc(part_len + 1, sizeof(char));
        strncpy(parts[i], piece_start, part_len);
        piece_start = slash + 1;
        i++;
    }
    // Get the last piece.
    parts[i] = (char *) calloc(strlen(piece_start) + 1, sizeof(char));
    strncpy(parts[i], piece_start, strlen(piece_start));
    return parts;
}


// Convenience function to get the inode of the root directory.
struct ext2_inode * get_root_dir(void * fs) {
    return get_inode(fs, EXT2_ROOT_INO);
}


// Given the inode for a directory and a filename, return the inode number of
// that file inside that directory, or 0 if it doesn't exist there.
// 
// name should be a single component: "foo.txt", not "/files/foo.txt".
__u32 get_inode_from_dir(void * fs, struct ext2_inode * dir, 
        char * name) {
  void* start;
  void* dir_block;
  for (int i = 0; i < EXT2_N_BLOCKS; i++) {
    start = get_block(fs, (dir->i_block[i]));
    dir_block = start;
    while (((dir_block - start) % get_block_size(fs) != 0) || ((dir_block - start)==0))    
    {
	struct ext2_dir_entry* temp = (struct ext2_dir_entry*)dir_block;
	if (strncmp(temp->name, name, strlen(name)) == 0) return temp->inode;  
  	dir_block += temp->rec_len;
    }
  }
  return 0;
  //return _ref_get_inode_from_dir(fs,dir,name);
}

// Find the inode number for a file by its full path.
// This is the functionality that ext2cat ultimately needs.
__u32 get_inode_by_path(void * fs, char * path) {
    // get number of parts - length of the parts array returned from split_parts
    int num_parts=0;
    int curr=0;
    while (path[curr]!='\0') {
       if (path[curr] == '/') num_parts++;
       curr++;
    }
    //printf("Number of parts: %d \n", num_parts);
    struct ext2_inode* root = get_root_dir(fs);
    struct ext2_inode* curr_inode = root;
    char** parts = split_path(path);
    __u32 inode_num;
    for (int i = 0; i < num_parts; i++)
    {
      char* dir = parts[i]; 
      inode_num = get_inode_from_dir(fs, curr_inode, dir);
      curr_inode = get_inode(fs, inode_num);          
    }
    return inode_num;  
}
