spellcheck: hashTableProject.o spellchecker.o hashTable.o dictionary.o
	g++ -Wall -g hashTableProject.o spellchecker.o hashTable.o dictionary.o -o spellcheck

hashTableProject.o: hashTableProject.h hashTableProject.cpp spellchecker.h spellchecker.cpp dictionary.h dictionary.cpp definitions.h
	g++ -Wall -c hashTableProject.cpp spellchecker.cpp dictionary.cpp

spellchecker.o: spellchecker.h spellchecker.cpp dictionary.h dictionary.cpp
	g++ -Wall -c spellchecker.cpp dictionary.cpp

hashTable.o: hashTable.h hashTable.cpp definitions.h
	g++ -Wall -c hashTable.cpp

dictionary.o: dictionary.h dictionary.cpp
	g++ -Wall -c nodes.cpp

tar:
	tar cfv spellcheck.tar hashTableProject.h hashTableProject.cpp spellchecker.h spellchecker.cpp dictionary.h dictionary.cpp hashTable.h hashTable.cpp definitions.h

clean:
	\rm *.o *~ spellcheck