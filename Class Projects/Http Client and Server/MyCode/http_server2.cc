#include "minet_socket.h"
#include <stdlib.h>
#include <fcntl.h>
#include <ctype.h>
#include <sys/stat.h>
#include <string>
#include <iostream>
#include <fstream>

using namespace std;

#define BUFSIZE 1024
#define FILENAMESIZE 100
#define BACKLOG 5

int handle_connection(int);
int writenbytes(int,char *,int);
int readnbytes(int,char *,int);

int main(int argc,char *argv[])
{
  int server_port;
  int sock,sock2;
  struct sockaddr_in sa,sa2;
  int rc,i;
  fd_set readlist;
  fd_set connections;
  int maxfd;

  /* parse command line args */
  if (argc != 3)
  {
    fprintf(stderr, "usage: http_server1 k|u port\n");
    exit(-1);
  }
  server_port = atoi(argv[2]);
  if (server_port < 1500)
  {
    fprintf(stderr,"INVALID PORT NUMBER: %d; can't be < 1500\n",server_port);
    exit(-1);
  }

  /* initialize and make socket */
  if (toupper(*(argv[1])) == 'K'){
	minet_init(MINET_KERNEL);
  } else if (toupper(*(argv[1])) == 'U') {
	minet_init(MINET_USER);
  } else {
	fprintf(stderr, "First argument must be k or u\n");
	exit(-1);
  }

  sock = minet_socket(SOCK_STREAM);
 
  /* set server address*/
  memset(&sa, 0, sizeof(sa));
  sa.sin_port = htons(server_port);
  sa.sin_addr.s_addr = htonl(INADDR_ANY);
  sa.sin_family = AF_INET;

  /* bind listening socket */
  rc = minet_bind(sock, &sa);
  if (rc < 0) printf("Bind error\n");

  /* start listening */
  rc = minet_listen(sock, BACKLOG);
  if (rc < 0) printf("Listen error\n");

  /* connection handling loop */

  FD_ZERO(&connections);
  FD_SET(sock, &connections);
  maxfd = sock;

  while(1)
  {
    /* create read list */
    readlist = connections;
    /* do a select */
    rc = minet_select(maxfd+1, &readlist, NULL, NULL, NULL);
    if (rc < 0) printf("Select error\n");

    /* process sockets that are ready */
    for (i=3; i <= maxfd; i++)
    {
	if (FD_ISSET(i, &readlist)) {
	   /* for the accept socket, add accepted connection to connections */
	   if (i == sock) {
		sock2 = minet_accept(sock, (struct sockaddr_in *) &sa2);
		if (sock2 > 0) {
		    FD_SET(sock2, &connections);
		    if (sock2 > maxfd) {
			maxfd = sock2;
		    }
		} else if (sock2 == -1) {
		    printf("Accept Error\n");
		}
	   }
           else /* for a connection socket, handle the connection */
           {
	        rc = handle_connection(i);
		if (rc<0) {
		    printf("Unsuccessful connection\n");
		} else {
		    printf("Successful connection\n");
		}
		FD_CLR(i, &connections);
           }
        }
     }
  }
  minet_deinit();
}

int handle_connection(int sock2)
{
  //char filename[FILENAMESIZE+1];
  int rc;
  //int fd;
  struct stat filestat;
  char buf[BUFSIZE+1];
  string request = "";
  //char *headers;
  //char *endheaders;
  //char *bptr;
  int datalen=0;
  char *ok_response_f = "HTTP/1.0 200 OK\r\n"\
                      "Content-type: text/plain\r\n"\
                      "Content-length: %d \r\n\r\n";
  char ok_response[100];
  char *notok_response = "HTTP/1.0 404 FILE NOT FOUND\r\n"\
                         "Content-type: text/html\r\n\r\n"\
                         "<html><body bgColor=black text=white>\n"\
                         "<h2>404 FILE NOT FOUND</h2>\n"\
                         "</body></html>\n";
  bool ok=false;

  /* first read loop -- get request and headers*/
  bzero(buf, BUFSIZE+1);
  while ((rc = minet_read(sock2, buf, BUFSIZE)) > 0) {
	buf[rc] = '\0';
	datalen += rc;
	request += string(buf);
	string last_digits = request.substr(request.length() - 4);
	if (last_digits == "\r\n\r\n") {
	    break;
	}
  }

  /* parse request to get file name */
  /* Assumption: this is a GET request and filename contains no spaces*/
  int filename_end = request.find("HTTP/1.0") - 2;
  int filename_len = filename_end - 4;
  if (filename_len > FILENAMESIZE) {
	printf("Error: file name is too long\n");
	return -1;
  }
  string filename_s = request.substr(5, filename_len);
  printf("Attempting to open file: %s ... \n", filename_s.c_str());
    /* try opening the file */
  ifstream myfile;
  myfile.open(filename_s.c_str(), ifstream::in);

  if (myfile.is_open()) {
      ok = true;
  }

  /* send response */
  if (ok)
  {
    stat(filename_s.c_str(), &filestat);
    int filesize = filestat.st_size;
    sprintf(ok_response, ok_response_f, filesize);
    /* send headers */
    if ((rc = writenbytes(sock2, ok_response, strlen(ok_response))) <= 0) {
	printf("Error: header sending failure\n");
	minet_error();
	ok = false;
    }
    /* send file */
    while (!myfile.eof()) {
	myfile.read(buf, BUFSIZE);
	int read_size = myfile.gcount();
	buf[read_size] = '\0';
	if (minet_write(sock2, buf, read_size + 1) < 0) {
	    printf("Error: file sending failure\n");
	    minet_error();
	    ok = false;
	}
    }
    myfile.close();
  }
  else	// send error response
  {
	if ((rc = writenbytes(sock2, notok_response, strlen(notok_response))) <= 0) {
	    printf("Error: error response sending failure.\n");
	    minet_error();
	    ok = false;
	}
	printf("Could not open file %s ... \n", filename_s.c_str());
  }

  /* close socket and free space */
  if (myfile != NULL) myfile.close();
  
  minet_close(sock2);
  
  if (ok)
    return 0;
  else
    return -1;
}

int readnbytes(int fd,char *buf,int size)
{
  int rc = 0;
  int totalread = 0;
  while ((rc = minet_read(fd,buf+totalread,size-totalread)) > 0)
    totalread += rc;

  if (rc < 0)
  {
    return -1;
  }
  else
    return totalread;
}

int writenbytes(int fd,char *str,int size)
{
  int rc = 0;
  int totalwritten =0;
  while ((rc = minet_write(fd,str+totalwritten,size-totalwritten)) > 0)
    totalwritten += rc;

  if (rc < 0)
    return -1;
  else
    return totalwritten;
}

