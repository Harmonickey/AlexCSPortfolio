#include <stdlib.h>
#include <stdio.h>
#include <pthread.h>
#include <unistd.h>

#include "thread_pool.h"

/**
 *  @struct threadpool_task
 *  @brief the work struct
 *
 *  Feel free to make any modifications you want to the function prototypes and structs
 *
 *  @var function Pointer to the function that will perform the task.
 *  @var argument Argument to be passed to the function.
 */

#define RUNNING 0
#define STOP 1

typedef struct threadpool_task {
    void (*function)(void *);
    int argument;
    void* next;
} threadpool_task_t;


struct threadpool_t {
  pthread_mutex_t lock;
  pthread_cond_t notify;
  pthread_t *threads;
  //to manage the task queue
  threadpool_task_t *head;
  threadpool_task_t *tail;
  // Keep track of the state of the threadpool (running or stopping)
  int state;
  int thread_count;
  int task_queue_size_limit;
  int task_count;
};

/**
 * @function void *threadpool_work(void *threadpool)
 * @brief the worker thread
 * @param threadpool the pool which own the thread
 */
static void *thread_do_work(void *threadpool);

/*
 * Create a threadpool, initialize variables, etc
 *
 */
threadpool_t *threadpool_create(int thread_count, int queue_size)
{
    // Get the pool struct
    threadpool_t* pool = (threadpool_t*)malloc(sizeof(threadpool_t));
    // Set the thread count, queue size, and task count
    pool->thread_count = thread_count;
    pool->task_queue_size_limit = queue_size;
    pool->task_count = 0;
    // Also set up a mutex for the pool along with a condition variable
    pthread_mutex_init(&(pool->lock),NULL);
    pthread_cond_init(&(pool->notify),NULL);
    pool->head = NULL;
    pool->tail = NULL;
    pool->state = RUNNING;
    // Initialize a bunch of threads
    pool->threads = (pthread_t*)malloc(thread_count*sizeof(pthread_t));  
    /* start running each thread */
    int i;
    for (i=0; i < thread_count; i++) {
        // Create the thread, add it to the queue, and run thread_do_work
        pthread_create(&(pool->threads[i]),NULL, thread_do_work, (void*)pool);
    }
    return pool;
}


/*
 * Add a task to the threadpool
 *
 */
void threadpool_add_task(threadpool_t *pool, void (*function)(void *), void* argument)
{
    //int err = 0;
    /* Get the lock */
    pthread_mutex_lock(&(pool->lock));
    /* Add task to queue */
    if (pool->task_count < pool->task_queue_size_limit) {
        threadpool_task_t* newTask = (threadpool_task_t*)malloc(sizeof(threadpool_task_t));
        newTask->function = function;
        newTask->argument = *(int*)(argument);
        newTask->next = NULL;
        
    if (pool->head == NULL) {
        pool->head = newTask;
        pool->tail = newTask;
        } else {
            pool->tail->next = newTask;
            pool->tail = newTask;
        }
    	pool->task_count++;   
    }
    /* otherwise just discard the incoming task! */
    /* pthread_cond_broadcast and unlock */
    pthread_cond_broadcast(&(pool->notify));
    pthread_mutex_unlock(&(pool->lock)); 
    //return err;
}



/*
 * Destroy the threadpool, free all memory, destroy treads, etc
 *
 */
int threadpool_destroy(threadpool_t *pool)
{
    int err = 0;
   
    pthread_mutex_lock(&(pool->lock));
    pool->state = STOP;
    pthread_mutex_unlock(&(pool->lock));

    /* Wake up all worker threads */
    pthread_cond_broadcast(&(pool->notify));
        
    /* Join all worker thread */
    int i;
    for (i=0; i < pool->thread_count; i++) {
        pthread_join(pool->threads[i], NULL);
    }

    //skipped for now, may need to use err as the 2nd arg for pthread_join
    /* Only if everything went well do we deallocate the pool */
    pthread_mutex_destroy(&(pool->lock));
    pthread_cond_destroy(&(pool->notify));
    free(pool->threads); /* free threads array */ 

    /* free task queue */
    threadpool_task_t* curr = pool->head;
    threadpool_task_t* next; 
    while (curr != NULL) {
        next = curr->next;
        free(curr);
        curr = (threadpool_task_t*)next;	 
    }  

    free(pool); /* free pool struct */
    return err;
}



/*
 * Work loop for threads. Should be passed into the pthread_create() method.
 *
 */
static void *thread_do_work(void *p)
{  
    threadpool_t* pool = (threadpool_t*)p;
    while(pool->state != STOP) {
        /* Lock must be taken to wait on conditional variable */
        pthread_mutex_lock(&(pool->lock));        

        /* Wait on condition variable, check for spurious wakeups. When returning from pthread_cond_wait(), do some task. */
         
        while (pool->task_count <= 0) {
          pthread_cond_wait(&(pool->notify), &(pool->lock));
        }

        /* Grab our task from the queue */
        threadpool_task_t* task = pool->head;
          pool->head = (threadpool_task_t*)(task->next);
        pool->task_count--;

        /* Unlock mutex for others */
        pthread_mutex_unlock(&(pool->lock));
        /* Start the task */
        (*task->function)((void*)(&task->argument));        
        /* free the task */
        free(task);
    }
    pthread_exit(NULL);
    return((void*)NULL);
}

