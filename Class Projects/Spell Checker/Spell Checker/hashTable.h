#include <iostream>
#include "definitions.h"

using namespace std;

class HASHTABLE
{
private:
	string *words;
	
	int primes[877];

public:
	HASHTABLE();
	~HASHTABLE();
	
	void add(char *item);
	void remove(char *item);
	bool lookup(const char *item);

	void assignPrime(int i, int prime);

	unsigned int hashCode(const char*p);
	unsigned int doubleHash(int i, int primes[], int len);

};

