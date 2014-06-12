#ifndef _table
#define _table

#include "link.h"
#include "messages.h"
#include <iostream>
#include <map>

using namespace std;
class Link;

#if defined(GENERIC)
class Table {
  // Students should write this class
 public:
  ostream & Print(ostream &os) const;
};
#endif


#if defined(LINKSTATE)
#include <vector>
#include <set>
#include <deque>

class Table {
  // Students should write this class
 public:
  map<int,map<int,int> > storedSeq;
  map<int, map<int,double> > topo;
  map<int, int> fwdTable;
  vector<int> knownNodes;
  
  unsigned nodeNumber;

  Table();
  Table(unsigned n, deque<Link*>* Links);
  Table(const Table *rhs);
  virtual bool processMessage(const RoutingMessage* m);
  virtual void updateTables();
  virtual void expandMatrix(unsigned newNode);
  virtual bool setChecker(set<int>* N);
  virtual map<int,int> GetFwdTable() const;
  virtual void PrintTopo();
  virtual void PrintSeq();
  virtual void PrintFwd();
  ostream & Print(ostream &os) const;
};
#endif

#if defined(DISTANCEVECTOR)

#include <deque>
#include <vector>

class Table {
 
 public:
  //unsigned nodeNumber;
  map<int, double> latencies;
  map<int, map<int, double> > distanceVectorMat;
  map<int, int> routing;
  vector<int> knownNodes;
  unsigned nodeNumber;
  
  //member functions
  Table();
  Table(unsigned n, deque<Link*>* Links); //constructor, take in node number and outgoing links
  Table(const Table *rhs); //given another table, copy all fields
  virtual RoutingMessage* InitDistVecMat();
  virtual RoutingMessage* UpdateDistVecMat(const RoutingMessage* m);
  virtual map<int,int> GetFwdTable() const;
  virtual void PrintDV();
  virtual bool FindSmallestDV(unsigned dest);
  ostream & Print(ostream &os) const;
};
#endif

inline ostream & operator<<(ostream &os, const Table &t) { return t.Print(os);}

#endif
