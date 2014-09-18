#include "minet_socket.h"
#include <stdlib.h>
#include <ctype.h>
#include <string>
#include <iostream>

using namespace std;

#define BUFSIZE 1024

int write_n_bytes(int fd, char * buf, int count);

int main(int argc, char * argv[]) {
    
    char * server_name = NULL;
    int server_port = 0;
    char * server_path = NULL;

    int sock = 0;
    int rc = -1;
    int datalen = 0;
    bool ok = false;
    struct sockaddr_in sa;
    //FILE * wheretoprint = stdout;
    struct hostent * site = NULL;
    char * req = NULL;

    char buf[BUFSIZE + 1];
    //char * bptr = NULL;
    //char * bptr2 = NULL;
    //char * endheaders = NULL;
   
    //struct timeval timeout;
    fd_set set;

    /*parse args */
    if (argc != 5) {
	fprintf(stderr, "usage: http_client k|u server port path\n");
	exit(-1);
    }

    server_name = argv[2];
    server_port = atoi(argv[3]);
    server_path = argv[4];

    /* initialize minet */
    if (toupper(*(argv[1])) == 'K') { 
	minet_init(MINET_KERNEL);
    } else if (toupper(*(argv[1])) == 'U') { 
	minet_init(MINET_USER);
    } else {
	fprintf(stderr, "First argument must be k or u\n");
	exit(-1);
    }

    /* create socket */
    sock = minet_socket(SOCK_STREAM);

    // Do DNS lookup
    /* Hint: use gethostbyname() */
    site  = gethostbyname(server_name);
    string address_s;
    if (site == NULL) {
      printf("DNS lookup failed");
      exit(0);
    } 

    /* set address */
    memcpy(&sa.sin_addr, site->h_addr, site->h_length);
    sa.sin_port = htons(server_port);
    sa.sin_family = AF_INET;

    /* connect socket */
    minet_connect(sock, (struct sockaddr_in*) &sa); 
    
    /* send request */
    req = (char*)malloc(strlen("GET HTTP/1.0\r\n\r\n")+strlen(server_path)+1);
    sprintf(req, "GET %s HTTP/1.0\r\n\r\n", server_path);
    write_n_bytes(sock, req, strlen(req));

    /* wait till socket can be read */
    /* Hint: use select(), and ignore timeout for now. */
    FD_ZERO(&set);
    FD_SET(sock, &set);
    minet_select(sock+1, &set, NULL, NULL, NULL);

    /* first read loop -- read headers */
    

    /* examine return code */   
    //Skip "HTTP/1.0"
    //remove the '\0'
    // Normal reply has return code 200
    
    string response="";
    string header="";
    string data="";
    while ((rc = minet_read(sock, buf, BUFSIZE))>0) {
      buf[rc]='\0';
      datalen += rc;
      response += string(buf);
    } 
    
    int header_end = response.find("\r\n\r\n",0);
    header = response.substr(0,header_end);
    data = response.substr(header_end+4);

    string return_code;
    return_code = header.substr(9,3);
    if (return_code == "200"){ ok = true;}

    /* print first part of response */
    printf(header.c_str());
    printf("\r\n\r\n");
    printf(data.c_str());

    /* second read loop -- print out the rest of the response */
    
    /*close socket and deinitialize */
    minet_close(sock);
    minet_deinit();
    free(req);
    if (ok) {
        printf("success!");
	return 0;
    } else {
        printf("failure!");
	return -1;
    }
}

int write_n_bytes(int fd, char * buf, int count) {
    int rc = 0;
    int totalwritten = 0;

    while ((rc = minet_write(fd, buf + totalwritten, count - totalwritten)) > 0) {
	totalwritten += rc;
    }
    
    if (rc < 0) {
	return -1;
    } else {
	return totalwritten;
    }
}


