#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "spellchecker.h"
#include "dictionary.h"

SPELLCHECKER::SPELLCHECKER(){}

SPELLCHECKER::~SPELLCHECKER(){}

void SPELLCHECKER::CheckSwap(char *item, DICTIONARY *&dictionary)
{
	for (unsigned int i = 0; i < strlen(item) - 1; i++)
	{
		char swapOne = item[i];
		char swapTwo = item[i + 1];

		//swap the characters
		item[i + 1] = swapOne;
		item[i] = swapTwo;

		bool found = dictionary->lookup(item);
		if (found)
		{
			cout << item << endl;
		}

		//swap back the characters
		item[i] = swapOne;
		item[i + 1] = swapTwo;

	}

}
void SPELLCHECKER::CheckInsert(char *item, DICTIONARY *&dictionary)
{
	int length = strlen(item) + 1;
	
	for (unsigned int i = 0; i < strlen(item) + 1; i++)
	{
		char *checkWord = (char *)malloc(length);
		memset(checkWord, 0, length + 1);
		for (unsigned int j = 0; j != i; j++)
		{
			checkWord[j] = item[j];
		}

		for (unsigned int k = i; k < strlen(item); k++)
		{
			checkWord[k + 1] = item[k];
		}

		for (int l = 65; l < 91; l++)
		{
			checkWord[i] = (char)l;
			bool found = dictionary->lookup(checkWord);
			if (found)
				cout << checkWord << endl;
		}

	}
}
void SPELLCHECKER::CheckDelete(char *item, DICTIONARY *&dictionary)
{
	
	unsigned int length = strlen(item) - 1;
	char *tempWord = (char *)malloc(length);

	for (unsigned int i = 0; i < strlen(item); i++)
	{
		char *checkWord = (char *)malloc(length);
		memset(checkWord, 0, length + 1);
		for (unsigned int j = 0; j != i; j++)
		{
			checkWord[j] = item[j];
		}

		for (unsigned int k = i; k < strlen(item) - 1; k++)
		{
			checkWord[k] = item[k + 1];
		}

		if (strcmp(tempWord, checkWord) != 0)
		{
			strcpy(tempWord, checkWord);
			bool found = dictionary->lookup(checkWord);
			if (found)
				cout << checkWord << endl;
		}
		
	}
}
void SPELLCHECKER::CheckReplace(char *item, DICTIONARY *&dictionary)
{
	
	int length = strlen(item);
	char *checkWord = (char *)malloc(length);
	
	for (int i = 0; i < length; i++)
	{
		strcpy(checkWord, item);
		for (int l = 65; l < 91; l++)
		{
			checkWord[i] = (char)l;
			bool found = dictionary->lookup(checkWord);
			if (found)
				cout << checkWord << endl;
		}

	}
}
void SPELLCHECKER::CheckSplit(char *item, DICTIONARY *&dictionary)
{
	string str(item);

	for (unsigned int i = 0; i < str.size() - 1; i++)
	{
		
		string str2 = str.substr(0, i + 1);
		string str3 = str.substr(i + 1, str.size() - (i + 1));

		bool found1 = dictionary->lookup(str2.c_str());
		bool found2 = dictionary->lookup(str3.c_str());

		if (found1 && found2)
		{
			cout << str2.c_str() << " " << str3.c_str() << endl;
		}
	}
}

void SPELLCHECKER::check(char *item, DICTIONARY *&d)
{
	CheckSwap(item, d);
	CheckInsert(item, d);
	CheckDelete(item, d);
	CheckReplace(item, d);
	CheckSplit(item, d);

}