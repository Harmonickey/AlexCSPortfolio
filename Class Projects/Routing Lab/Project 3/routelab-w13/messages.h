#ifndef _messages
#define _messages

#include <iostream>

using namespace std;

#if defined(GENERIC)
struct RoutingMessage {
 public:
  ostream & Print(ostream &os) const;
};
#endif

#if defined(LINKSTATE)
struct RoutingMessage {

  RoutingMessage();
  RoutingMessage(unsigned src, unsigned dest, double updatedLat, int seqNum);
  RoutingMessage(const RoutingMessage &rhs);
  RoutingMessage &operator=(const RoutingMessage &rhs);
  
  unsigned src;
  unsigned dest;
  double updatedLat;
  int seqNum;

  ostream & Print(ostream &os) const;
};
#endif

#if defined(DISTANCEVECTOR)

#include <map>

//two types of messages
#define LINK_UPDATE 0
#define DV_UPDATE 1

class RoutingMessage {
 public:
  unsigned neighbor;
  map<int,double>  *updatedDV;
  double updatedLat;
  bool changedOrNot;
  int type;
  
  RoutingMessage(int t);
  RoutingMessage(const RoutingMessage &rhs);
  RoutingMessage &operator=(const RoutingMessage &rhs);
  virtual void AddDVUpdates(unsigned n, map<int,double> *dv);
  virtual void AddLinkUpdate(unsigned n, double l);
  ostream & Print(ostream &os) const;
};
#endif


inline ostream & operator<<(ostream &os, const RoutingMessage &m) { return m.Print(os);}

#endif
