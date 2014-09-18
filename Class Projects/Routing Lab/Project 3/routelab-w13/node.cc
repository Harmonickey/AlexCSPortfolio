#include "node.h"
#include "context.h"
#include "error.h"


Node::Node(const unsigned n, SimulationContext *c, double b, double l) : 
    number(n), context(c), bw(b), lat(l) 
{
  tableIsSet = false;
}

Node::Node() 
{ throw GeneralException(); }

Node::Node(const Node &rhs) : 
  number(rhs.number), context(rhs.context), bw(rhs.bw), lat(rhs.lat) {
  routingTable = rhs.GetRoutingTable();
  tableIsSet = rhs.tableIsSet;
}

Node & Node::operator=(const Node &rhs) 
{
  return *(new(this)Node(rhs));
}

void Node::SetNumber(const unsigned n) 
{ number=n;}

unsigned Node::GetNumber() const 
{ return number;}

void Node::SetLatency(const double l)
{ lat=l;}

double Node::GetLatency() const 
{ return lat;}

void Node::SetBW(const double b)
{ bw=b;}

double Node::GetBW() const 
{ return bw;}

Node::~Node()
{}

// Implement these functions  to post an event to the event queue in the event simulator
// so that the corresponding node can recieve the ROUTING_MESSAGE_ARRIVAL event at the proper time
void Node::SendToNeighbors(const RoutingMessage *m)
{
  context->SendToNeighbors(this, m);
}

void Node::SendToNeighbor(const Node *n, const RoutingMessage *m)
{
  context->SendToNeighbor(this, n, m);
}

deque<Node*> *Node::GetNeighbors()
{
  return context->GetNeighbors(this);
}

deque<Link*> *Node::GetOutgoingLinks()
{
  return context->GetOutgoingLinks(this);
}

void Node::SetTimeOut(const double timefromnow)
{
  context->TimeOut(this,timefromnow);
}


bool Node::Matches(const Node &rhs) const
{
  return number==rhs.number;
}


#if defined(GENERIC)
void Node::LinkHasBeenUpdated(const Link *l)
{
  cerr << *this << " got a link update: "<<*l<<endl;
  //Do Something generic:
  SendToNeighbors(new RoutingMessage);
}


void Node::ProcessIncomingRoutingMessage(const RoutingMessage *m)
{
  cerr << *this << " got a routing messagee: "<<*m<<" Ignored "<<endl;
}


void Node::TimeOut()
{
  cerr << *this << " got a timeout: ignored"<<endl;
}

Node *Node::GetNextHop(const Node *destination) const
{
  return 0;
}

Table *Node::GetRoutingTable() const
{
  return new Table;
}


ostream & Node::Print(ostream &os) const
{
  os << "Node(number="<<number<<", lat="<<lat<<", bw="<<bw<<")";
  return os;
}

#endif

#if defined(LINKSTATE)


void Node::LinkHasBeenUpdated(const Link *l)
{
	if (!tableIsSet){
		routingTable = new Table(number,this->GetOutgoingLinks());
		tableIsSet = true;
		this->PrintAll();
	} 
	map<int,map<int,int> > temp = routingTable->storedSeq;
	unsigned src = l->GetSrc();
	unsigned dest = l->GetDest();
	double lat = l->GetLatency();
	int seq = temp[src][dest]+1;
	RoutingMessage* m = new RoutingMessage(src,dest,lat,seq);
	this->SendToNeighbors(m);
	routingTable->processMessage(m);
	//this->PrintAll();
	cerr << *this<<": Link Update: "<<*l<<endl;
}

void Node::PrintAll(){
  routingTable->PrintTopo();
  routingTable->PrintSeq();
  routingTable->PrintFwd();
}

void Node::ProcessIncomingRoutingMessage(const RoutingMessage *m)
{
  if (routingTable->processMessage(m)) {
    //this->PrintAll();
    this->SendToNeighbors(m);
  }	
  cerr << *this << " Routing Message: "<<*m;
}

void Node::TimeOut()
{
  cerr << *this << " got a timeout: ignored"<<endl;
}

Node *Node::GetNextHop(const Node *destination) const
{
  map<int,int> fwdTable = routingTable->GetFwdTable();
  unsigned nodeNum = destination->GetNumber();
  unsigned nextHop = fwdTable[nodeNum];
  Node* tempNode = new Node(nextHop,0,0,0);
  Node* nextNode = context->FindMatchingNode(tempNode);
  tempNode = new Node(*nextNode);
  return tempNode;
}

Table *Node::GetRoutingTable() const
{
  Table*  tempTable = new Table(this->routingTable);
  return tempTable;
}


ostream & Node::Print(ostream &os) const
{
  os << "Node(number="<<number<<", lat="<<lat<<", bw="<<bw<<")";
  return os;
}
#endif


#if defined(DISTANCEVECTOR)

Node::Node(const unsigned n, SimulationContext *c, double b, double l, Table* t):number(n),context(c),bw(b),lat(l),routingTable(t)
{}

void Node::LinkHasBeenUpdated(const Link *l)
{
  if (!tableIsSet){
    routingTable = new Table(number,this->GetOutgoingLinks());
    tableIsSet = true;
    RoutingMessage* initMessage = routingTable->InitDistVecMat();
    this->SendToNeighbors(initMessage);
    cout<<"Message sent out from initialization"<<endl;
  } else {
  RoutingMessage m(LINK_UPDATE);
  m.AddLinkUpdate(l->GetDest(),l->GetLatency());
  RoutingMessage* outMessage = routingTable->UpdateDistVecMat(&m);
  if (outMessage->changedOrNot) {
	this->SendToNeighbors(outMessage);
	cout<<"Update sent out!"<<endl;
  }
  }
  cerr << *this<<": Link Update: "<<*l<<endl;
}


void Node::ProcessIncomingRoutingMessage(const RoutingMessage *m)
{
  RoutingMessage*  outMessage = routingTable->UpdateDistVecMat(m);
  if (outMessage->changedOrNot) {
	this->SendToNeighbors(outMessage);
	cout<<"Update sent out!"<<endl;
	//routingTable->PrintDV();
  }
}

void Node::TimeOut()
{
  cerr << *this << " got a timeout: ignored"<<endl;
}


Node *Node::GetNextHop(const Node *destination) const
{
  map<int,int> fwdTable = routingTable->GetFwdTable();
  unsigned nodeNum = destination->GetNumber();
  unsigned nextHop = fwdTable[nodeNum];
  Node* tempNode = new Node(nextHop,0,0,0);
  Node* nextNode = context->FindMatchingNode(tempNode);
  tempNode = new Node(*nextNode);
  return tempNode;
}

Table *Node::GetRoutingTable() const
{
  Table*  tempTable = new Table(this->routingTable);
  return tempTable;
}


ostream & Node::Print(ostream &os) const
{
  os << "Node(number="<<number<<", lat="<<lat<<", bw="<<bw;
  return os;
}
#endif
