/*
 * Aaron Templeton
 * U0734119
 *
 */
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <unistd.h>
#include <string.h>
#include <limits.h>
#include "mm.h"
#include "memlib.h"

/* always use 16-byte alignment */
#define ALIGNMENT 16

/* rounds up to the nearest multiple of ALIGNMENT */
#define ALIGN(size) (((size) + (ALIGNMENT-1)) & ~(ALIGNMENT-1))

/* rounds up to the nearest multiple of mem_pagesize() */
#define PAGE_ALIGN(size) (((size) + (mem_pagesize()-1)) & ~(mem_pagesize()-1))

typedef size_t block_header;
typedef size_t block_footer;

#define OVERHEAD (sizeof(block_header) + sizeof(block_footer))
#define HDRP(bp) ((char *)(bp) - sizeof(block_header))
#define FTRP(bp) ((char *)(bp) + GET_SIZE(HDRP(bp))-OVERHEAD)
#define NEXT_BLKP(bp) ((char *)(bp) + GET_SIZE(HDRP(bp)))
#define PREV_BLKP(bp) ((char *)(bp)-GET_SIZE((char *)(bp)-OVERHEAD))
// Given a pointer to a header, get or set its value
#define GET(p)      (*(size_t *)(p))
#define PUT(p, val) (*(size_t *)(p) = (val)) 

// Combine a size and alloc bit
#define PACK(size, alloc) ((size) | (alloc))

// Given a header pointer, get the alloc or size 
#define GET_ALLOC(p) (GET(p) & 0x1)
#define GET_SIZE(p)  (GET(p) & ~0xF)

#define MAX(a,b) ((a) > (b) ? (a) : (b))

typedef struct list_node{
  struct list_node* prev;
  struct list_node* next;
}list_node;

//define helper functions
static void set_allocated(void *b, size_t size);
static void extend(size_t s);
static void* coalesce(void *bp);
static void add_node(void* bp);
static void delete_node(void* bp);
void print_free(void* current);
int smart_chunk(int s);

//globals
void *current_avail = NULL;
int current_avail_size = 0;
static list_node* first_free;
int prev_chunk = 0;
/* 
 * mm_init - initialize the malloc package.
 */
int mm_init(void)
{
 
  current_avail = NULL;
  current_avail_size = 0;
  first_free = NULL;
  prev_chunk = 0;
  return 0;
}

/* 
 * mm_malloc - Allocate a block by using bytes from current_avail,
 *     grabbing a new page if necessary.
 */
void *mm_malloc(size_t size)
{
  int new_size =  ALIGN(MAX(size, sizeof(list_node*)) + OVERHEAD);
  //  printf("%d\n", new_size);
 
 if(first_free == NULL)
    {
      extend(new_size * 52);
    }
  
  list_node* current = first_free;
  while(1)
    {
      // printf("current: %p\n", current);
      // printf("size: %d\n", GET_SIZE(HDRP(current)));

      if(GET_SIZE(HDRP(current)) >= new_size)
	{
	  set_allocated(current, new_size);
	  return (void *)current;
	}
      if(current->next == NULL)
	{
	  extend(new_size * 52);
	  current=first_free;
	}
      else
	{
	  current = current->next;
	}
    }
  
 
}

int smart_chunk(int s)
{
int new =  ALIGN(MAX(s, sizeof(list_node*)) + OVERHEAD);
  return new * 2;
}

/*
 * mm_free
 */
void mm_free(void *ptr)
{
  size_t size = GET_SIZE(HDRP(ptr));
  PUT(HDRP(ptr), PACK(size,0));
  PUT(FTRP(ptr), PACK(size,0));
  //  printf("coal\n");
  void* p =  coalesce(ptr);
  // printf("in free\n");
  // print_free(p); 

  void* sent = PREV_BLKP(p);
  void* term = NEXT_BLKP(p);
  if(GET_SIZE(HDRP(sent)) == 16 && GET_ALLOC(HDRP(sent)) == 1 && GET_SIZE(HDRP(term)) == 0 && GET_ALLOC(HDRP(term)) == 1){
    // printf("free %p\n",p);
     delete_node(p);
     mem_unmap(p-32, GET_SIZE(HDRP(p))+32);
   
  }

}

/*
 * Set a block to allocated
 * Update block headers/footers as needed
 * Update free list if applicable
 * Split block if applicable
 */
static void set_allocated(void *b, size_t size)
{
  size_t oldsize = GET_SIZE(HDRP(b));
   PUT(HDRP(b), PACK(size, 1));
   PUT(FTRP(b), PACK(size,1));
   delete_node(b); 
  // printf("allocated del: %p\n", b);

  if((oldsize-size) >= 32)//split page and add new free block
    {
      char *t = (char *)b;
      //add new block with size
      size_t newsize = oldsize - size;
      PUT(HDRP(NEXT_BLKP(b)), PACK(newsize, 0));     
      PUT(FTRP(NEXT_BLKP(b)), PACK(newsize, 0));
      add_node(t+size);
      //   printf("allo add: %p\n",NEXT_BLKP(b));
      // printf("next %d, t + size %d, b %d, newh %d newf %d\n", NEXT_BLKP(b), t+size, b, HDRP(NEXT_BLKP(b)), FTRP(NEXT_BLKP(b)));
      // printf("oldsize %d, size %d, newsize %d\n",oldsize, size, oldsize-size);
      // printf("begin: %d, hdr: %d, term: %d\n", b, HDRP(b), b+oldsize);
       // exit(0);
    }
  else
    {
      PUT(HDRP(b), PACK(oldsize,1));
      PUT(FTRP(b), PACK(oldsize,1));
    }
 

}

/*
 * Request more memory by calling mem_map
 * Initialize the new chunk of memory as applicable
 * Update free list if applicable
 */
static void extend(size_t size)
{
  current_avail_size = PAGE_ALIGN(size);
  current_avail = mem_map(current_avail_size);

  char *t = (char *)current_avail;

  PUT(t+8, PACK(16,1));
  PUT(t+16, PACK(16,1));
  PUT(t+24, PACK(current_avail_size-32,0));
  PUT(FTRP(t+32), PACK(current_avail_size-32,0));
  PUT(FTRP(t+32)+8, PACK(0,1));

  add_node(t+32);
  // printf("extend: %p\n", t+32);
  //   printf("size: %d, current_avail_size: %d, hdr: %d\n",size, current_avail_size, GET_SIZE(HDRP(t+32)));
  //printf("term: %d, beggin: %d\n", FTRP(t+32)+8, t);


}

/*
 * Coalesce a free block if applicable
 * Returns pointer to new coalesced block
 */
static void* coalesce(void *bp)
{
  void* prev_payload = PREV_BLKP(bp);
  void* next_payload = NEXT_BLKP(bp);
  size_t prev_alloc = GET_ALLOC(HDRP(prev_payload));
  size_t next_alloc = GET_ALLOC(HDRP(next_payload));
  
  //case 1, both are allocated
  if(prev_alloc && next_alloc)
    {
      add_node(bp);
      //printf("case 1 %p\n", bp);
      return bp;
    }
  
  //case 2, previous is allocated
 else if(prev_alloc && !next_alloc)
    {
        
      size_t size = GET_SIZE(HDRP(bp)) + GET_SIZE(HDRP(next_payload));

      PUT(HDRP(bp), PACK(size,0));
      PUT(FTRP(next_payload), PACK(size,0));
      // printf("case 2 d: %p, a: %p\n", next_payload, bp);
      
      delete_node(next_payload);
       add_node(bp);
         
      return bp;
    }
  
  //case 3, next allocated
 else if(!prev_alloc && next_alloc)
    {
      size_t size = GET_SIZE(HDRP(bp)) + GET_SIZE(HDRP(prev_payload));

      PUT(HDRP(prev_payload), PACK(size,0));
      PUT(FTRP(bp), PACK(size,0));
      // add_node(prev_payload);
      // printf("case 3 %p\n", prev_payload);
      return prev_payload;
    }

  //case 4, both unallocated
 else if(!prev_alloc && !next_alloc)
    {
         
      size_t size = GET_SIZE(HDRP(bp)) + GET_SIZE(HDRP(prev_payload)) + GET_SIZE(HDRP(next_payload));
      
      PUT(HDRP(prev_payload), PACK(size,0));
      PUT(FTRP(next_payload), PACK(size,0));
      // printf("case 4 d: %p, a!: %p\n",next_payload, prev_payload);
     
      delete_node(next_payload);
      //add_node(prev_payload);
      //      print_free(next_payload);
      return prev_payload;
      
    }
  
  return bp;
}

static void add_node(void* bp)
{
  
  list_node* new = (list_node *)bp;
  // printf("add node %p\n", bp);
  if(first_free == NULL)
    {
      first_free = new;
      first_free->next = NULL;
      first_free->prev = NULL;
    }
  else
    {
      first_free->prev = new;
      new->next = first_free;
      new->prev = NULL;
      first_free = new;
    }
  
}

static void delete_node(void* bp)
{
  list_node* delete = (list_node *)bp;
  list_node* prev_node = delete->prev;
  list_node* next_node = delete->next;
 
 if(delete == first_free){

    if(first_free->next != NULL)
      {
	first_free=first_free->next;
	first_free->prev = NULL;
      }
    else
      {
	first_free->next=NULL;
	first_free->prev=NULL;
	first_free = NULL;
      }
 }
 else if(prev_node == NULL && next_node != NULL){
    first_free = next_node;
    first_free->prev = NULL;
  }
  else if(next_node == NULL && prev_node != NULL){
    prev_node->next = NULL;
  }
  else if(next_node != NULL && prev_node != NULL){
    //both previous and next are not null
    // printf("delete node %p, %p, %p, %p\n", prev_node, next_node, delete, bp);
     // print_free(prev_node);
    next_node->prev = prev_node;
    prev_node->next = next_node;

  }
}

void print_free(void* current)
{

  list_node* temp = first_free;
  while(1){
    if(GET_ALLOC(HDRP(temp))){
        printf("Block in free list still allocated\n");
        exit(0);
      }
      if(temp == current){
	printf("current in free list\n");
        break;
      }
	if(temp->next == NULL){
	  printf("Node not in the free list\n");
	  break;
	}
	temp = temp->next;
      
  }
}
