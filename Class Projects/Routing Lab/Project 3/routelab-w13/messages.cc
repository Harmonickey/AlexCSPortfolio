#include "messages.h"

using namespace std;

#if defined(GENERIC)
ostream &RoutingMessage::Print(ostream &os) const
{
  os << "RoutingMessage()";
  return os;
}
#endif


#if defined(LINKSTATE)

ostream &RoutingMessage::Print(ostream &os) const
{
  os<<"src:"<<this->src<<" dest:"<<this->dest<<" new lat:"<<updatedLat<<" seq num:"<<seqNum<<endl;
  return os;
}

RoutingMessage::RoutingMessage()
{}

RoutingMessage::RoutingMessage(unsigned source, unsigned destination, double newLat, int seq)
{
  this->src = source;
  this->dest = destination;
  this->updatedLat = newLat;
  this->seqNum = seq;
}


RoutingMessage::RoutingMessage(const RoutingMessage &rhs)
{
  this->src = rhs.src;
  this->dest = rhs.dest;
  this->updatedLat = rhs.updatedLat;
  this->seqNum = rhs.seqNum;
}

#endif

//For DISTANCE_VECTOR
#if defined(DISTANCEVECTOR)

ostream &RoutingMessage::Print(ostream &os) const
{
  os<<"From: "<<this->neighbor<<"\n"<<"Type: "<<this->type<<"\n";
  if (type == LINK_UPDATE) {
    os<<"New latency: "<<this->updatedLat<<"\n";
  } else if (type == DV_UPDATE) {
    map<int,double>::iterator itr;
    for (itr=updatedDV->begin();itr!=updatedDV->end();itr++){
      int dest = itr->first;
      double dv = itr->second;
      os<<"Cost to:"<<dest<<"     DV: "<<dv<<"\n";
    }
  } else {
    os<<"Error: unknown message type\n";
  }
  return os;
}

RoutingMessage::RoutingMessage(int t)
{
  type = t;
  changedOrNot = false;
}


RoutingMessage::RoutingMessage(const RoutingMessage &rhs)
{
  this->neighbor = rhs.neighbor;
  this->updatedDV = rhs.updatedDV;
  this->updatedLat = rhs.updatedLat;
  this->type = rhs.type;
  this->changedOrNot = rhs.changedOrNot;
}

RoutingMessage& RoutingMessage::operator=(const RoutingMessage &rhs){
  this->neighbor = rhs.neighbor;
  this->updatedDV = rhs.updatedDV;
  this->updatedLat = rhs.updatedLat;
  this->type = rhs.type;
  this->changedOrNot = rhs.changedOrNot;
  return *this;
}

void RoutingMessage::AddDVUpdates(unsigned n, map<int,double>* dv){
  neighbor = n;
  updatedDV = dv;
}

void RoutingMessage::AddLinkUpdate(unsigned n, double l){
  neighbor = n;
  updatedLat = l;
}
#endif
