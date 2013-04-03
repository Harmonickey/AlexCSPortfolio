#include <iostream>
#include <fstream>
#include <cctype>
#include <stdio.h>
#include <string.h>
#include "definitions.h"
#include "hashTableProject.h"
#include "dictionary.h"
#include "spellchecker.h"

using namespace std;

int main(int argc, char *argv[])
{
	
	DICTIONARY *dictionary = new DICTIONARY; //want to make dynamic with pointer because we'll have to reference it with the spellchecker later
	SPELLCHECKER spellChecker;  //initialize the spell checker

	if (argc < 2)
	{
		cout << "NOT ENOUGH ARGUMENTS" << endl;
		cout << "Need <filename> as argument" << endl;
		cout << "Press Enter to Exit..." << endl;
		cin.ignore();
		return 0;
	}

	//we need two streams taking input from the wordlist file and the prime numbers file
	ifstream wordList;
	ifstream primeList;  //the prime number file is reformatted to read each line as one of the prime numbers for easy file reading

	primeList.open("primeNumbers.txt", ios::in);
	if (!primeList.is_open())
	{
		cout << "File 'primeNumbers.txt' cannot be opened" << endl;  //not open, just terminate
		cout << "Press Enter to Exit..." << endl;
		cin.ignore();
		return 0;
	}
	char primeLine[MAX_PRIME_LENGTH];  //set the prime length to 10 because we won't need anything larger than that probably

	for (int i = 0; primeList.peek() > 0; i++)
	{
		memset(primeLine, 0, MAX_PRIME_LENGTH);

		primeList.getline(primeLine, MAX_PRIME_LENGTH);

		int prime = convertToInt(primeLine);
		
		dictionary->addPrime(i, prime);  //add the prime number to the list of primes for later use in double hashing
	}
	
	primeList.close();
	wordList.open("program4wordlist.txt", ios::in); //open the file
	if (!wordList.is_open())
	{
		cout << "File 'program4wordlist.txt' cannot be opened" << endl;  //not open, just terminate
		cout << "Press Enter to Exit..." << endl;
		cin.ignore();
		return 0;
	}

	//NOTE: the max word length as found in the dictionary is 45 characters officially
	char inword[MAX_WORD_LENGTH];  //got from file 
	char outword[MAX_WORD_LENGTH]; //give to dictionary
	bool isFirstLine = true;
	while (wordList.peek() > 0)
	{
		memset(inword, 0, MAX_WORD_LENGTH);
		memset(outword, 0, MAX_WORD_LENGTH);

		wordList.getline(inword, MAX_WORD_LENGTH - 1);

		
		if (!isFirstLine)
		{
			for (int i = 0; inword[i] != 0; i++)
			{
				if (iscntrl((unsigned)inword[i]) != 0 ||
					isspace((unsigned)inword[i]) != 0)
					break; //if we've found a crazy linux character then we break
				
				outword[i] = toupper(inword[i]); //change everything to upper case first
			}

			dictionary->add(outword);  //then insert the upper case version of the word into the dictionary
		}
		isFirstLine = false;
	}
	wordList.close();

	//a separate stream for the input file that we want to spell check
	ifstream inputFile;

	inputFile.open("program4input.txt", ios::in);

	if (!inputFile.is_open())
	{
		cout << "File 'program4input.txt' cannot be opened" << endl;  //not open, just terminate
		cout << "Press Enter to Exit..." << endl;
		cin.ignore();
		return 0;
	}

	//array of separate tokens in a line  
	//NOTE: declared dynamically in the heap due to large number of possible tokens
	char **tokenList = new char*[MAX_TOKENS_INLINE];  

	//the char array of a single line
	//NOTE: declared dynamically in the heap due to large number of possible chars in a line
	char *line = new char[MAX_INPUT_LENGTH];

	while (inputFile.peek() > 0)
	{
		memset(line, 0, MAX_INPUT_LENGTH);

		inputFile.getline(line, MAX_INPUT_LENGTH - 1);

		cout << "INPUT: " << line << endl;  //display the input line

		int numberOfTokens = parseCmdLine(line, tokenList);  //parse the tokens in the line

		for (int i = 0; i < numberOfTokens; i++)
		{
			int j = 0;
			while(tokenList[i][j] != 0)
			{
				tokenList[i][j] = toupper(tokenList[i][j]);  //convert each token into uppercase format for comparsions in the dictionary
				j++;
			}
			
			bool foundInDictionary = dictionary->lookup(tokenList[i]);  //lookup the word in the dictionary
			if (!foundInDictionary)
			{
				cout << "Word Not Found: " << tokenList[i] << endl;
				cout << "Did you mean: " << endl;

				spellChecker.check(tokenList[i], dictionary);  //suggestions made in spell checker
				
				cout << endl;
				cout << "Press Enter to Continue..." << endl;
				cin.ignore();
			}

			
		}

		//return the memory used to make the tokens
		for (int i = 0; i < numberOfTokens; i++) 
		{
			delete tokenList[i];
		}
	}

	delete [] tokenList;
	//release memory by line
	delete [] line;

	cout << "Press Enter to Exit Program " << endl;
	cin.ignore();

	return 0;
}