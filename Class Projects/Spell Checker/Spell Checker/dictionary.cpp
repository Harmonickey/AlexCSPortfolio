#include "dictionary.h"

DICTIONARY::DICTIONARY(){}

DICTIONARY::~DICTIONARY(){}

void DICTIONARY::add(char *item)
{
	hashTable.add(item);
}

void DICTIONARY::remove(char *item)
{
	hashTable.remove(item);
}

bool DICTIONARY::lookup(const char *item)
{
	return hashTable.lookup(item);
}

void DICTIONARY::addPrime(int i, int prime)
{
	hashTable.assignPrime(i, prime);
}
