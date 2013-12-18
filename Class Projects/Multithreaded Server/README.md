PROJECT 3: AIRLINE RESERVATION SYSTEM

The objective of this project was to implement a multithreaded web server using threads, threadpools, mutex locks, and synchonization techniques.

--- Thread Pool Design ---
We implemented our thread pool as an array of threads. The thread pool had a common mutex lock and condition variable, along with a state (either running or stopping). The mutex lock and condition variable allowed us to synchornize use such that only one thread was modifying the queue  at a time. 

The task queue was created as a linked list queue, and the thread pool had pointers to both the head and tail of this list.

--- Seat State Management ---
We created a mutex for each seat in the airplane. This way, locks were done more quickly (since they were around a smaller amount of data). This allowed more threads to be active at a time, as opposed to locking the entire selection of seats.

We locked around each time a message was printed to the buffer, or the seat's information was modified. 

--- Thread Work Loop ---
When each thread is initialized, it enters into a loop that only stops when the entire pool's state changes to "stopping". On each entry into this loop, the thread gets the pool's lock to ensure that no two threads work on the same task at the same time. 

If the task queue is empty, the thread wants until the pool signals that a new task has been added, using a conditional variable. Once a task is present, the thread takes the task from the head of the task queue, removes it from the queue, and runs the task's function with the supplied arguments.

--- Optimizations ---
To optimize our multi-threaded server, we removed unnecssary parameters that were passed into function calls. We also removed any and all printf statements.
