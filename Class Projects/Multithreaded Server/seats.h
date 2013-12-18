#ifndef _SEAT_OPERATIONS_H_
#define _SEAT_OPERATIONS_H_

#include <pthread.h>

typedef enum 
{
    AVAILABLE, 
    PENDING, 
    OCCUPIED
} seat_state_t;

typedef struct seat_struct
{
    int id;
    int customer_id;
    seat_state_t state;
    struct seat_struct* next;
} seat_t;

pthread_mutex_t seat_mutex[20];

void load_seats(int);
void unload_seats();

void list_seats(char* buf, int bufsize);
//void view_seat(char* buf, int bufsize, int seat_num, int customer_num, int customer_priority);
//void confirm_seat(char* buf, int bufsize, int seat_num, int customer_num, int customer_priority);
//void cancel(char* buf, int bufsize, int seat_num, int customer_num, int customer_priority);
void view_seat(char* buf, int bufsize, int seat_num, int customer_num);
void confirm_seat(char* buf, int bufsize, int seat_num, int customer_num);
void cancel(char* buf, int bufsize, int seat_num, int customer_num);
#endif
