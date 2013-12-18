Homework 4
Alex Ayerdi, Patrick Weston, Daniel Zuo

--- Design Approach ---

Because the functions listed in ext2_access.c built upon each other, we started with the simplest functions and worked our way up in complexity. We also made sure to utilize macros and definitions to maintain consistency with the filesystem structure. Additionally, we used some of our implemented functions later on in the more complex functions.

--- Code Explanation ---

get_super_block
Given a pointer to the filesystem, the superblock is located at a fixed offset from the beginning. Using the built-in macro for this offset, the function simply adds this offset to the filesystem pointer and returns it.

get_block_size
The block size is stored as a field inside of the superblock. Using our previous get_super_block function to get the superblock, this function simply returns the value stored by the block size field.

get_block
Given a pointer to a filesystem and a block number, this function calculates the pointer to a given block. This is handled much like an array index. We take the initial file system pointer and add an offset. This offset is the block number multiplied by the block size.

get_block_group
Our filesystem has only one block group descriptor that starts after the superblock. Given a pointer to the filesystem, we offset to the superblock and then offset an additional amount to move beyond the superblock. This gets us to the first block group descriptor. We then add an additional offset that is the group number multiplied by the block group descriptor's size to access later block groups.

get_inode
We again utilize the assumption that our filesystem has only one block group. An inode is special in that its first member starts at 1. To find the group that an inode is in, we divide the inode's number (after being subtracted from 1) by the number of inodes per group. Using our get_block_group function we access the inode table from that group.

Then, within this group, we calculate the potential offset by using the modulo operation on the inode number and the number of inodes per group. We add the inode offset by the inode size to calculate an offset into the group. Our function then returns this pointer.

get_inode_from_dir
This function returns the inode number of a file in a directory given the file's name and directory and a pointer to the filesystem. We loop over all blocks, comparing to see if the file name matches the name we are looking for. If it doesn't match, we add the record length to a pointer to advance to the next entry.

get_inode_by_path
This function works similarly to get_inode_from_dir except it returns the inode number for a file based on the file's full path. First, it splits the path into multiple parts, one for each directory and subdirectory where the file is stored. 

Starting at the root, and the first part of the path, it uses our function get_inode_from_dir to return the inode for the matching block. It then repeats this process inside of the second part of the path (the subdirectory a file may be in). It continues until it finds the file. At this point it returns the inode number.
