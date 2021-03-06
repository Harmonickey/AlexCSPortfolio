#include <stdlib.h>
#include <signal.h>
#include <ctype.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <string.h>
#include <stdio.h>
#include <unistd.h>
#include <stdbool.h>
#include <errno.h>
#include <pthread.h>

#include "thread_pool.h"
#include "seats.h"
#include "util.h"

#define BUFSIZE 1024
#define FILENAMESIZE 100

void shutdown_server(int);

int listenfd;
threadpool_t* threadpool;

int main(int argc,char *argv[])
{
    int flag, num_seats = 20;
    int connfd = 0;
    struct sockaddr_in serv_addr;

    char send_buffer[BUFSIZE];
    
    listenfd = 0; 

    int server_port = 8080;

    if (argc > 1)
    {
        num_seats = atoi(argv[1]);
    } 

    if (server_port < 1500)
    {
        fprintf(stderr,"INVALID PORT NUMBER: %d; can't be < 1500\n",server_port);
        exit(-1);
    }
    
    if (signal(SIGINT, shutdown_server) == SIG_ERR) 
        printf("Issue registering SIGINT handler");

    listenfd = socket(AF_INET, SOCK_STREAM, 0);
    if ( listenfd < 0 ){
        perror("Socket");
        exit(errno);
    }
    flag = 1;
    setsockopt( listenfd, SOL_SOCKET, SO_REUSEADDR, &flag, sizeof(flag) );

    // initialize the threadpool
    // Set the number of threads and size of the queue
    threadpool = threadpool_create(10, 800); //parameters subject to change

    // Load the seats;
    load_seats(num_seats); //read from argv

    // set server address 
    memset(&serv_addr, '0', sizeof(serv_addr));
    memset(send_buffer, '0', sizeof(send_buffer));
    serv_addr.sin_family = AF_INET;
    serv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    serv_addr.sin_port = htons(server_port);

    // bind to socket
    if ( bind(listenfd, (struct sockaddr*) &serv_addr, sizeof(serv_addr)) != 0)
    {
        perror("socket--bind");
        exit(errno);
    }

    // listen for incoming requests
    listen(listenfd, 10);

    //thread mutex init
    int i;
    for (i = 0; i < 20; i++) {
	pthread_mutex_init(&seat_mutex[i], NULL);
    }

    // handle connections loop (forever)
    while(1)
    {
        connfd = accept(listenfd, (struct sockaddr*)NULL, NULL);
        threadpool_add_task(threadpool, (void*)&handle_connection, (void*)&connfd);
    }
   
    for (i = 0; i < 20; i++) {
        pthread_mutex_destroy(&seat_mutex[i]);
    }
    
    threadpool_destroy(threadpool);
    return 0;
}

void shutdown_server(int signo){
    threadpool_destroy(threadpool);
    unload_seats();
    close(listenfd);
    exit(0);
}
