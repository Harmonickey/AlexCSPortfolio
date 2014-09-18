#include "minet_socket.h"
#include <stdlib.h>
#include <ctype.h>
#include <fcntl.h>
#include <sys/stat.h>
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
  int rc;
  int read_int;

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
  if (toupper(*(argv[1])) == 'K') {
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
  sa.sin_port=htons(server_port);
  sa.sin_addr.s_addr=htonl(INADDR_ANY);
  sa.sin_family=AF_INET;

  /* bind listening socket */
  read_int = minet_bind(sock, &sa);
  if (read_int < 0)
	cerr << "Binding failed\n";

  /* start listening */
  read_int = minet_listen(sock, BACKLOG);
  if (read_int < 0)
	cerr << "Listening failed\n";

  /* connection handling loop */
  while(1)
  {
    if ((sock2 = minet_accept(sock, (struct sockaddr_in *)&sa2)) < 0)
    {
	minet_close(sock);
	minet_deinit();
	exit(-1);
    }
    /* handle connections */
    rc = handle_connection(sock2);
  }
}

int handle_connection(int sock2)
{
  //char filename[FILENAMESIZE+1];
  int rc;
  //int fd;
  struct stat filestat;
  char buf[BUFSIZE+1];
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
                         "<h2>404 FILE NOT FOUND</h2>\n"
                         "</body></html>\n";
  bool ok=false;
  int read_int;
  string request = "";
  /* first read loop -- get request and headers*/
  while ((read_int = minet_read(sock2, buf, BUFSIZE)) > 0)
  {
	buf[read_int] = '\0';
	datalen += read_int;
	request += string(buf);
	string end_of_message = request.substr(request.length()-4);
	if (end_of_message == "\r\n\r\n")
	    break;	
  }

  int filename_end = request.find("HTTP/1.0") - 2;
  int filename_len = filename_end - 4;
  if (filename_len > FILENAMESIZE) {
	printf("File name is too long!\n");
	return -1;
  }

  string filename_s = request.substr(5, filename_len);

  /* parse request to get file name */
  /* Assumption: this is a GET request and filename contains no spaces*/
  printf("Attempting to open file: %s ... \n", filename_s.c_str());

  ifstream myfile;
  myfile.open(filename_s.c_str(), ifstream::in);

    /* try opening the file */
  if (myfile.is_open())
  {
	ok = true;
  }

  if (ok)
  {
	/* send response */
	stat(filename_s.c_str(), &filestat);  /* check to see if the file is valid */ 
        datalen = filestat.st_size; /* get the size of the file ! */
	
	sprintf(ok_response, ok_response_f, datalen);

	/*send headers */
	rc = writenbytes(sock2, ok_response, strlen(ok_response_f));
        if (rc <= 0)
        {
    	    printf("Error: header sending failure\n");
            ok = false;
	}	

	/* send file */
	while (!myfile.eof())
        {
	    myfile.read(buf, BUFSIZE);
	    int read_int = myfile.gcount();
	    buf[read_int] = '\0';
		    
	    rc = minet_write(sock2, buf, read_int + 1);
	    if (rc < 0)
	    {
		printf("File sending failure\n");
		ok = false;
	    }
	}
	myfile.close();
	printf("File sent sucessfully!\n");
  }
  else
  {
	/* send error response */
	rc = minet_write(sock2, notok_response, strlen(notok_response));
	//rc = writenbytes(sock2, notok_response, strlen(notok_response));
	if (rc < 0)
	{
	    printf("Error: response sending failure\n");
	    ok = false;
	}
	printf("Could not open file %s ... \n", filename_s.c_str());
  }

  /* close socket and free space */

  if (myfile != NULL)
	myfile.close();
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

