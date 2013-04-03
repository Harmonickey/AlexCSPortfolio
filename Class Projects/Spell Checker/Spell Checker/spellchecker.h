#include <iostream>

using namespace std;

class DICTIONARY;

class SPELLCHECKER
{
public:
	SPELLCHECKER();
	~SPELLCHECKER();

	void CheckSwap(char *item, DICTIONARY *&d);
	void CheckInsert(char *item, DICTIONARY *&d);
	void CheckDelete(char *item, DICTIONARY *&d);
	void CheckReplace(char *item, DICTIONARY *&d);
	void CheckSplit(char *item, DICTIONARY *&d);

	void check(char *item, DICTIONARY *&d);

};