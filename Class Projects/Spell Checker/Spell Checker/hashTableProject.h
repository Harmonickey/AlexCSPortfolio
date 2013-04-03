#include <iostream>
#include <cctype>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "definitions.h"

using namespace std;

int parseCmdLine(char cline[], char *tkList[]);
int convertToInt(char *s);

int parseCmdLine(char cline[], char *tklist[])
{
	
	int startTokenPosition = -1;   //when >-1 then it is a valid position found
	int endTokenPosition = 0;      //when >0 then it is a valid position found
	
	int token_length = 0;          //the length of the token
	int next_token = 0;   //the next token of tklist, also serves as the count for the number of tokens
	string line(cline);
	
	for (unsigned int i = 0; i < MAX_INPUT_LENGTH; i++)
	{ 
		//if we've found the token terminator or the end of the whole line

		if (i < line.size())
		{
			//if it's not punctuation, a space, or a control character then continue
			if (ispunct((unsigned)line[i]) ||
				isspace((unsigned)line[i]) ||
				iscntrl((unsigned)line[i]) ||
				line.substr(i, 1).compare("\r\n") == 0)  //have this extra check here just incase the computer uses a different type of formatting
			{
				if (startTokenPosition != -1)  //need to find a start position, so keep searching
				{
				
					endTokenPosition = i;
				  
					token_length = endTokenPosition - startTokenPosition;  //length of the token

					tklist[next_token] = (char *)malloc(token_length + 1);  //allocate memory to pointer
				
					memset(tklist[next_token], 0, token_length + 1);  //set that pointer to 0

					memcpy(tklist[next_token], &cline[startTokenPosition], token_length);  //copy that specific part

					next_token++;  //increment to get the next token
					startTokenPosition = -1;  //reset the start position
					//		NOTE: endTokenPosition doesn't need to be reset because it gets a new value anyway 
					//      each time the token terminator is found
				}
			}
			else if (!isspace((unsigned)line[i]))  //if it's non-blank
			{
				//find the begining of the token position
				if (startTokenPosition < 0)  //if start position not already assinged
				{
					startTokenPosition = i;  //make the token start when we found the first non-blank character
				}
			}
		}
		if (i == line.size())
		{
			if (startTokenPosition != -1)  //need to find a start position, so keep searching
			{
				
				endTokenPosition = i;
				  
				token_length = endTokenPosition - startTokenPosition;  //length of the token

				tklist[next_token] = (char *)malloc(token_length + 1);  //allocate memory to pointer
				
				memset(tklist[next_token], 0, token_length + 1);  //set that pointer to 0

				memcpy(tklist[next_token], &cline[startTokenPosition], token_length);  //copy that specific part

				next_token++;  //increment to get the next token
				startTokenPosition = -1;  //reset the start position
				//		NOTE: endTokenPosition doesn't need to be reset because it gets a new value anyway 
				//      each time the token terminator is found
			}
		}
	}
	return next_token;  //return the amount of tokens we found on the line
}

int convertToInt(char *s)
{
	int tokenNumber = 0;
	int i = 0;

	while (s[i] != 0)  //keep going until we reach the end of the token
	{
		tokenNumber = (tokenNumber * 10) + (s[i] - 48);  //convert to decimal value
		i++;
	}

	return tokenNumber;
}



