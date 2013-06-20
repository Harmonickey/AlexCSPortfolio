// TicTacToe.cpp : Defines the entry point for the console application.
//

#include<iostream>
#include<sstream>

#include<string>
#include<vector>

using namespace std;

#define INFINITY 100
#define MAX 1
#define MIN -1

int alphabeta;

class Board {
	int squares[3][3];
	
public:
	Board();
	Board(bool firstPlayer);
	int humanPlayer;
	int cpuPlayer;
	string toString();
	void play_square(int, int, int);
	int get_square(int, int);
	int winner();
	bool full_board();
};

int utility(Board * b);

Board::Board(bool firstPlayer) {
	for(int i=0; i<3;i++)
		for(int j=0; j<3; j++)
			squares[i][j] = 0;

	if (firstPlayer == 1)
	{
		humanPlayer = 1;
		cpuPlayer = -1;
	}
	else
	{
		humanPlayer = -1;
		cpuPlayer = 1;
	}
}

string Board::toString() {
	stringstream s;
	char cforvalplusone[] = {'O', '_', 'X'};
	s << " _ _ _" << endl;
	for(int i=0; i<3;i++) {
		s << '|';
		for(int j=0; j<3; j++)
			s << cforvalplusone[squares[i][j]+1] << '|';
		s << endl;
	}
	return s.str();
}

void Board::play_square(int row, int col, int val) {
	squares[row-1][col-1] = val;
}

bool Board::full_board() {
	for(int i=0; i<3;i++)
		for(int j=0; j<3; j++)
			if(squares[i][j]==0)
				return false;
	return true;
}

//returns winner or zero if none
int Board::winner() {
	//check rows:
	for(int row=0; row<3; row++)
		if(squares[row][0]!=0 && squares[row][0]==squares[row][1] && squares[row][0]==squares[row][2])
			return squares[row][0];
	//check cols:
	for(int col=0; col<3; col++)
		if(squares[0][col]!=0 && squares[0][col]==squares[1][col] && squares[0][col]==squares[2][col])
			return squares[0][col];
	//check diagonals:
	if(squares[0][0]!=0 && squares[0][0]==squares[1][1] && squares[0][0]==squares[2][2])
		return squares[0][0];
	if(squares[2][0]!=0 && squares[2][0]==squares[1][1] && squares[2][0]==squares[0][2])
		return squares[2][0];
	return 0;
}

int Board::get_square(int row, int col) {
	return squares[row-1][col-1];
}

bool make_simple_cpu_move(Board * b, int cpuval) {
	for(int i=1; i<4; i++)
		for(int j=1; j<4; j++)
			if(b->get_square(i, j)==0) {
				b->play_square(i, j, cpuval);
				return true;
			}
	return false;
}

int make_minimax_move_alphabeta(Board * b, int playerval, vector<int> &cell, int alpha, int beta, int depth) {

	int resultMin = INFINITY;
	int resultMax = -INFINITY;
	int maxplay = alpha;
	int minplay = beta;

	if (b->winner() || b->full_board()) 
	{
		return utility(b);
	}

	if (playerval == MAX)
	{
		//check among all the children
		for (int i = 1; i <= 3; i++)
		{
			for (int j = 1; j <= 3; j++)
			{
				//choose the empty squares
				if (b->get_square(i, j) == 0)
				{
					//play a test square
					b->play_square(i, j, playerval);

					//get the utilty from making that test move
					resultMax = make_minimax_move_alphabeta(b, MIN, cell, alpha, beta, depth + 1);
					
					//if our score is greater than the max we've seen...
					if (resultMax >= maxplay)
					{
						//update the max (alpha)
						maxplay = resultMax;

						//not all moves are valid, only save the ones on the first ply 
						if (depth == 1)
						{
							cell.clear();
							cell.push_back(i);
							cell.push_back(j);
						}
					}
					
					//if our alpha max is greater than our current beta then...
					if (alpha > beta)
					{
						
						b->play_square(i, j, 0);
						//alpha cutoff (alpha-beta pruning)
						return alpha;
					}

					b->play_square(i, j, 0);  
				}
			}
		}
		
		return maxplay;  //return the utility of the best move we've found
	}
	else
	{
		//same deal as the max player, but with min idea
		for (int i = 1; i <= 3; i++)
		{
			for (int j = 1; j <= 3; j++)
			{
				if (b->get_square(i, j) == 0)
				{
					//play a test square
					b->play_square(i, j, playerval);

					resultMin = make_minimax_move_alphabeta(b, MAX, cell, alpha, beta, depth + 1);

					if (resultMin <= minplay)
					{
						minplay = resultMin;
					
						if (depth == 1)
						{
							cell.clear();
							cell.push_back(i);
							cell.push_back(j);
						}
					
					}

					if (alpha > beta)
					{
						b->play_square(i, j, 0);

						//beta cutoff (alpha-beta pruning)
						return beta;
					}
					
					b->play_square(i, j, 0);
				}
			}
		}
		
		return minplay;
	}
}

int utility(Board * b)
{
	if (b->winner() == MAX) //if max player wins, return win utility
	{ 
		return MAX;
	}
	else if (b->winner() == MIN)  //if min player wins, return lose utility
	{
		return MIN;
	}
	else 
		return 0;  //if a tie, return tie utility
}

bool playerMove(Board * b)
{
	int row, col;
	cout << "Your move row (1-3): ";
	cin >> row;
	cout << "Your move col (1-3): ";
	cin >> col;

	if(b->get_square(row, col)!=0) {
		cout << "Square already taken." << endl;
		return false;
	}
	else
	{
		b->play_square(row, col, b->humanPlayer);
	}

	return true;
}

void computerMove(Board * b)
{
	vector<int> cell;
	int negStart = -INFINITY;
	int posStart = INFINITY;
	int depth = 1;
	
	make_minimax_move_alphabeta(b, b->cpuPlayer, cell, negStart, posStart, depth);
	
	b->play_square(cell[0], cell[1], b->cpuPlayer);

}

void play() {

	int answer;
	do
	{
		cout << "Player One Human?  1 for yes, 0 for no : ";
		cin >> answer;
	} while (answer != 1 && answer != 0);
	
	Board * b = new Board(answer ? true : false);

	cout << b->toString();
	//depending on if you chose to be player one, then isPlayerMore will change to true or false
	bool isPlayerMove = answer ? true : false;

	while(!b->full_board()&&b->winner()==0) {
		//alternate players
		if (isPlayerMove)
		{
			if (!playerMove(b))
				continue;
			if(b->full_board() || b->winner()!=0)
				break;
		}
		else
		{
			cout << b->toString() << "..." << endl;
			computerMove(b);
			cout << b->toString();
		}
		isPlayerMove = !isPlayerMove;
	}

	//check for winner or tie
	if(b->winner()==0)
		cout << "Cat game." << endl;
	else if(b->winner()==b->cpuPlayer)
		cout << "Computer wins." << endl;
	else if(b->winner()==b->humanPlayer)
		cout << "You win." << endl;

	//end the game
	cin.ignore();
	cout << endl << "Press Enter to exit." << endl;
	cin.ignore();
}

int main(int argc, char * argv[])
{
	play();
	return 0;
}
