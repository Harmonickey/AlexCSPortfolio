#include <iostream>
#include "hashTable.h"

using namespace std;

class DICTIONARY
{
private:
	HASHTABLE hashTable;
public:
	DICTIONARY();
	~DICTIONARY();

	void add(char *item);
	void remove(char *item);
	bool lookup(const char *item);
	
	void addPrime(int i, int prime);

};

