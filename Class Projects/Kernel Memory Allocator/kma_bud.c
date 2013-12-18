/***************************************************************************
 *  Title: Kernel Memory Allocator
 * -------------------------------------------------------------------------
 *    Purpose: Kernel memory allocator based on the buddy algorithm
 *    Author: Stefan Birrer
 *    Copyright: 2004 Northwestern University
 ***************************************************************************/
/***************************************************************************
 *  ChangeLog:
 * -------------------------------------------------------------------------
 *    Revision 1.2  2009/10/31 21:28:52  jot836
 *    This is the current version of KMA project 3.
 *    It includes:
 *    - the most up-to-date handout (F'09)
 *    - updated skeleton including
 *        file-driven test harness,
 *        trace generator script,
 *        support for evaluating efficiency of algorithm (wasted memory),
 *        gnuplot support for plotting allocation and waste,
 *        set of traces for all students to use (including a makefile and README of the settings),
 *    - different version of the testsuite for use on the submission site, including:
 *        scoreboard Python scripts, which posts the top 5 scores on the course webpage
 *
 *    Revision 1.1  2005/10/24 16:07:09  sbirrer
 *    - skeleton
 *
 *    Revision 1.2  2004/11/05 15:45:56  sbirrer
 *    - added size as a parameter to kma_free
 *
 *    Revision 1.1  2004/11/03 23:04:03  sbirrer
 *    - initial version for the kernel memory allocator project
 *
 ***************************************************************************/
#ifdef KMA_BUD
#define __KMA_IMPL__

/************System include***********************************************/
#include <assert.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
//#include <time.h>
//#include <sys/time.h>
/************Private include**********************************************/
#include "kma_page.h"
#include "kma.h"

/************Defines and Typedefs*****************************************/
/*  #defines and typedefs should have their names in all caps.
 *  Global variables begin with g. Global constants with k. Local
 *  variables should be in all lower case. When initializing
 *  structures and arrays, line everything up in neat columns.
 */
/*
typedef struct {
	 void* prev;
	 void* block;
	 void* next;
} buf;
*/

#define WSIZE 4 // Word size (bytes)
#define MINSIZE 16        // Smallest block size in our implementation (bytes)  
#define BLKHDR 12 //3 WSIZE HDR
#define WSIZE_11 44 //11 * WSIZE

#define MAX(x,y) ((x)>(y)? (x):(y))
#define PACK(size, alloc) ((size) | (alloc))
#define IF_ALLOC(p) (GET(p) & 0x1) 
#define WHERE_BUD(p,s) ((((unsigned int)(p-BASEADDR(p)))/s) & 0x1) //return 0 if buddy is on the right, return 1 if buddy on the left

/* Read and write a word at address p */
#define GET(p) (*(unsigned int*)(p))
#define PUT(p,val) (*(unsigned int*)(p) = (val))
#define PUT_NEXT(p,val) (*((void**)(p+WSIZE)) = (void*)(val))
#define GET_NEXT(p) (*(void**)(p+WSIZE))
#define PUT_PREV(p,val) (*((void**)(p+(WSIZE << 1))) = (void*)(val))
#define GET_PREV(p) (*(void**)(p+(WSIZE << 1)))
#define GETSIZE(p) (GET(p) & ~0x7) //all the block sizes are larger than 16

//typedef unsigned long long timestamp_t;
void kma_init();
void* kma_find_block(kma_size_t size);
kma_page_t* kma_get_page();
void* kma_coalesce(void* ptr);
void* kma_split(void* ptr, kma_size_t size);
//void print_page(void* ptr); 
void fl_add(void* ptr);
void fl_remove(void* ptr);


//static timestamp_t get_timestamp();
/************Global Variables*********************************************/
static kma_page_t* master = NULL; 
/*
static double max_time = 0;
static double the_time = 0;
static double max_time_free = 0;
static double the_time_free = 0;
static int num_requests = 0;
static int total_requested = 0;
static int total_satisfied = 0;
*/
/************Function Prototypes******************************************/
	
/************External Declaration*****************************************/

/**************Implementation***********************************************/

void*
kma_malloc(kma_size_t size)
{       
   /* size + header size larger than page size, do nothing and return null */
   if ((size + BLKHDR) > PAGESIZE) return NULL;
   
   /* if master page is not set up, call kma_init */
   if (master == NULL) { 
	kma_init();	
   }
   
   /* find the page to insert */ 
   void* block = kma_find_block(size);
   
   //remove the block from freelist
   fl_remove(block);

   /* split the blocks */
   void* alloc = kma_split(block, size);

   return alloc + BLKHDR;
}

void 
kma_free(void* ptr, kma_size_t size)
{ 
  void* blk_hdr = ptr - BLKHDR;
  unsigned int blk_size = GETSIZE(blk_hdr); //read block size
  PUT(blk_hdr, PACK(blk_size,0)); //set to unallocated
  void* temp = kma_coalesce(blk_hdr); //coalesce with neighbors
  //check if necessary to free page
  if (GETSIZE(temp) ==  PAGESIZE) {
	//loop through master page and find the kma_page_t corresponding to this page
	unsigned int num_page = GET(master->ptr);
	void* curr = master->ptr + 11*WSIZE;
	int itr = 1;
	while ((((*(kma_page_t**)curr)->ptr - temp) != 0) && (itr <= num_page))
        {
		curr = curr + sizeof(kma_page_t*);
		itr++;
	}

	free_page((*(kma_page_t**)curr));
	
  	//delete from the list, move all following kma_page_t* one element forward
  	unsigned int k = (curr- master->ptr -WSIZE_11)/sizeof(kma_page_t*);
	while (k+1 < num_page) {
	   kma_page_t** prev = (kma_page_t**)(master->ptr + WSIZE_11 + k*sizeof(kma_page_t*));
	   kma_page_t*  next_p = *(kma_page_t**)(master->ptr +WSIZE_11 + (k+1)*sizeof(kma_page_t*));
           *prev = next_p;
	   k++;
	}
	num_page--;
	PUT(master->ptr,num_page); //decrease page count in master page
 	
	//if no page in master page, free master page too
	if (num_page == 0) {
	   free_page(master);
  	   master = NULL;
	}
  }
  //return; 
}

/* set up the master page */
void kma_init() {
  /* request for a new page for master page */	
  master = get_page();	
  void* ptr = master->ptr;
  unsigned int num = 0;
  PUT(ptr, num); //place a special header for the master page, now nothing in the page                                				      
  //initiate the freelist 
  void** freelist = (void**)(master->ptr+WSIZE);
  int i = 0;
  
  while (i<=10) 
  {
	freelist[i] = NULL;
	i++;
  }
 
  //return
}

/* loop through available pages in master page and find the first page with enough space */
void* kma_find_block(kma_size_t size) 
{
   //unsigned int num = GET(ptr); //read the page count curerntly in master page
   
   bool found = 0; 
	
   unsigned int k = 0x1<<4;
   int idx = 0; //index in freelist
   while (k-BLKHDR < size) 
   {
      idx++;	
      k=k<<1; //find the closest power of two;
   }
  

   /* Go through each free list */  
   void** freelist = (void**)(master->ptr+WSIZE);
   while (!found && idx<=9) {
	  if (freelist[idx] != NULL) { 
		found = 1;
		return freelist[idx];
	  }
 	idx++;
   }


   
   /* find the first page that have enough space
   while (!found && i <= num) {
 	kma_page_t* p_t = *((kma_page_t**)(ptr+WSIZE+(i-1)*sizeof(kma_page_t*)));
	void* p = p_t->ptr;
	
	void*  curr_pos = p;
	while ((unsigned int)(curr_pos - p) < PAGESIZE){
		if ((!IF_ALLOC(curr_pos)) && (GETSIZE(curr_pos)-WSIZE >= size)) {
			return curr_pos;
		} else {
			curr_pos += GETSIZE(curr_pos);
		} 
	}
	i++; //check next page;	
   }
   */

   //if code goes here, then no existing page has enough space
   //get a new page!
   kma_page_t* np = kma_get_page();
   return np->ptr;
}

kma_page_t* kma_get_page()
{
  void* ptr = master->ptr;
  unsigned int num = GET(ptr); //read the page count currently in master page
  kma_page_t* newpage = get_page(); //request a new page
  void* temp = ptr + WSIZE_11 + num*sizeof(kma_page_t*);
  *(kma_page_t**)temp = newpage; //place the new kma_page_t* in master page
  
  PUT(ptr, ++num); //update page count
  PUT(newpage->ptr, PACK(PAGESIZE,0)); // place block header in the newly allocated page
  
  /* record in the freelist */
  fl_add(newpage->ptr);
  return newpage;
}
 
void* kma_split(void* ptr, kma_size_t size) 
{
  	unsigned int blk_size= GETSIZE(ptr);
	unsigned int half = blk_size >> 1;
	
	if (((half - BLKHDR) < size) || (blk_size == MINSIZE)) {
	    // cannot split any more and need to allocate the block; 
            PUT(ptr, PACK(blk_size,1));
	    return ptr;
	}

	// split!
	PUT(ptr, PACK(half,0));  //update header
	void* next_half = ptr+half;
	PUT(next_half, PACK(half,0));  //insert the second header
   	//insert next_half to free list 
	fl_add(next_half); 
 	return kma_split(ptr, size); //recurse
}

void* kma_coalesce(void* ptr) 
{
  unsigned int size = GETSIZE(ptr);
  
  /* for debugging use
  void* base_addr = BASEADDR(ptr);
  unsigned int offset = (ptr - base_addr)/size;
  printf("%d", offset);
  */

  int bud = WHERE_BUD(ptr, size); //0 if bud on the right, 1 if bud on the left
   

  /* corner condition: size == PAGESIZE */
  if (size == PAGESIZE) return ptr;

  /* find bud location */
  void* bud_addr;
  if (bud) bud_addr = ptr - size; //bud on the left 
  else     bud_addr = ptr + size; //bud on the right  
  
  bool can_coalesce;
  can_coalesce = (!IF_ALLOC(bud_addr)) && (GETSIZE(ptr) == GETSIZE(bud_addr));
  
  if (!can_coalesce) {
	//bud allocated - do nothing,simply return, this is great!
	fl_add(ptr);
  	return ptr;
  }
  //we need to coalesce!  	
  if (bud) {
        fl_remove(bud_addr);
  		PUT(bud_addr, PACK(size<<1,0)); //bud on the left, update bud header
	return kma_coalesce(bud_addr);
  }

  fl_remove(bud_addr); 
  PUT(ptr, PACK(size<<1,0)); //bud on the right, update our header
  return kma_coalesce(ptr); 	
}
/*
void print_page(void* ptr) 
{ 
  void* curr = ptr; 
  bool exit = 0;
  printf("Page");
  while (!exit) {
	printf("...[%d,%d]",GETSIZE(curr),IF_ALLOC(curr));
	curr += GETSIZE(curr);
 	if (curr == (ptr+PAGESIZE)) exit = 1; //exit condition
  }
  printf("\n");
  printf("\n");
  return;
}
*/
/* compute the log2 of a number */
int log_two(int num) {
	int k=0x1; //k starts from our minimize block size 16
	int i=0;
	while (k < num) {
		i++;
		k=k<<1;
	}
	return i;
}

/* add an element to the front of the freelist */
void fl_add(void* ptr)
{
  void** freelist = (void**)(master->ptr + WSIZE);
  unsigned int size = GETSIZE(ptr);
  int idx = log_two(size)-4;
  PUT_PREV(ptr, NULL);
  PUT_NEXT(ptr,freelist[idx]);
  if (freelist[idx] != NULL) PUT_PREV(freelist[idx],ptr);
  freelist[idx] = ptr; //front = new
  //return;
}

/* remove an element from the freelist */
void fl_remove(void* ptr)
{
  void** freelist = (void**)(master->ptr+WSIZE);
  unsigned int size = GETSIZE(ptr);
  int idx = log_two(size) - 4;
  /*
  void* curr = freelist[idx];
  void* prev = NULL;
  while ((curr- ptr)!=0 && (curr != NULL))
  {
 	prev = curr;
	curr = GET_NEXT(curr);
  }
  */

  //if (curr == NULL) return; //shouldn't really happen
  
  if (GET_NEXT(ptr) != NULL) PUT_PREV(GET_NEXT(ptr),GET_PREV(ptr));

  if (GET_PREV(ptr) == NULL) //ptr is the first element 
  { 
	freelist[idx] = GET_NEXT(ptr);
  } else {
        PUT_NEXT(GET_PREV(ptr), GET_NEXT(ptr));
  }
  //return;
}
/*
static timestamp_t get_timestamp()
{
  struct timeval now;
  gettimeofday(&now, NULL);
  return now.tv_usec + (timestamp_t)now.tv_sec * 1000000;
}
*/
#endif // KMA_BUD
