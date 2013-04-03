#include "hashTable.h"
#include "definitions.h"

HASHTABLE::HASHTABLE()
{
	//array of strings
	words = new string[TABLE_SIZE];  //60397 (which is the prime number just greater than the number of words in the dictionary, 60389)
	
	//set each pointer string to be NULL for convention
	for (int i = 0; i < TABLE_SIZE; i++)
	{
		words[i] = "NULL";
	}

}

HASHTABLE::~HASHTABLE()
{
	delete[] words;  //delete the array
}

void HASHTABLE::add(char *item)
{
	//calculate the hash code.
	//store the word as a new string object
	//get the new hashnumber that is scaled by the table size
	unsigned int code = hashCode(item);
	string newWord(item);
	unsigned int hashNumber = code%TABLE_SIZE;

	//if the spot is empty (i.e. NULL) then assign the spot to the new word
	if (words[hashNumber].compare("NULL") == 0)
	{
		words[hashNumber] = newWord;
	}
	else
	{
		//if the spot isn't empty then make a new hashcode with doublehashing
		//repeat until the spot is empty (i.e. NULL) and then assign the new word to the spot
		for (int i = 1; words[hashNumber].compare("NULL") != 0; i++)
		{
			hashNumber = (code + doubleHash(i, primes, TABLE_SIZE))%TABLE_SIZE;
		}

		words[hashNumber] = newWord;
		
	}


}

void HASHTABLE::assignPrime(int i, int prime)
{
	primes[i] = prime; //give the hash table another prime number in the array
}

bool HASHTABLE::lookup(const char *item)
{
	//same process as adding to the dictionary
	unsigned int code = hashCode(item);
	string newWord(item);
	unsigned int hashNumber = code%TABLE_SIZE;
	
	//when it's empty that means that the word cannot be in the dictionary otherwise it would have been hashed the same way
	if (words[hashNumber].compare("NULL") == 0)
	{
		return false;
	}
	else
	{
		//if it isn't empty then we first check if it is the right word we're looking for
		//if not, then we create a new hashnumber with the double hash
		//repeat until we get to NULL or the word is found
		for (int i = 1; words[hashNumber].compare("NULL") != 0; i++)
		{	
			if (words[hashNumber].compare(newWord) == 0)
			{
				return true; //the word was found
			}

			hashNumber = (code + doubleHash(i, primes, TABLE_SIZE))%TABLE_SIZE;
			
		}

		return false;
	}
}

void HASHTABLE::remove(char *item)
{

	//same process as looking up in the dictionary
	unsigned int code = hashCode(item);
	string newWord(item);
	unsigned int hashNumber = code%TABLE_SIZE;

	//if we find an empty space then we cannot remove it because it doesn't exist in the dictionary
	if (words[hashNumber].compare("NULL") == 0)
	{
		cout << "No word " << item << " in word list." << endl;
	}
	else
	{
		//same process as looking up, except we change the spot to "DELETED" if found
		for (int i = 1; words[hashNumber].compare("NULL") != 0; i++)
		{
			if (words[hashNumber].compare(newWord) == 0)
			{
				//this will allow for the word to be deleted essentially but
				//still allow for lookup() to find the words in the dictionary without a rehash
				//this is because the lookup() is only trying to find words that aren't "NULL"... i.e. "DELETED" != "NULL"
				words[hashNumber] = "DELETED";
				cout << "Deleted " << item << " in word list." << endl;
				return; //since found, then exit
			}

			hashNumber = (code + doubleHash(i, primes, TABLE_SIZE))%TABLE_SIZE;
			
		}
		
		cout << "No word " << item << " in word list." << endl;
	}
}

unsigned int HASHTABLE::hashCode(const char*p)
{
	unsigned int h = 0;
	//change made in second argument in loop to go to the end of the character array instead of the length of the table
	for (int i = 0; p[i] != 0; i++)
	{
		if (iscntrl((unsigned)p[i]) != 0 ||
			isspace((unsigned)p[i]) != 0)
			break; //if we've found a crazy linux character then stop assigning to h
		h = (h<<5)|(h>>27);
		h += (unsigned int)p[i];
	}

	return h;
}

unsigned int HASHTABLE::doubleHash(int i, int primes[], int len)
{
	//i will be > 0 when the first hash doesn't work out
	if (i > 0)
	{
		//this is just a cheap way to increase k until we have found the prime number that is greater than the table size 
		int k;
		for (k = 0; primes[k] < len; k++){}

		int R = primes[k - 1];  //a prime number smaller than the table size

		return (R - (i%R));
	}
	return 0;
}
