// Othello.cpp : Defines the entry point for the console application.
//
#include "Alex_Ayerdi.h"

Alex_Ayerdi::Alex_Ayerdi() {
	for(int i=0; i<8;i++)
		for(int j=0; j<8; j++)
			squares[i][j] = 0;
	squares[3][3]=-1;
	squares[4][4]=-1;
	squares[3][4]=1;
	squares[4][3]=1;

	tries = 0;
	player = -1;  //start as whiteplayer
}

string Alex_Ayerdi::toString() {
	stringstream s;
	char cforvalplusone[] = {'W', '_', 'B'};
	s << "  1 2 3 4 5 6 7 8" << endl;
	for(int i=0; i<8;i++) {
		s << i+1 << '|';
		for(int j=0; j<8; j++)
			s << cforvalplusone[squares[i][j]+1] << '|';
		s << endl;
	}
	return s.str();
}

void Alex_Ayerdi::copyBoard(Alex_Ayerdi * b)
{
	for (int i = 1; i < 9; i++)
	{
		for (int j = 1; j < 9; j++)
		{
			squares[i-1][j-1] = b->get_square(i, j);
		}
	}
}

//returns if player with val has some valid move in this configuration
bool Alex_Ayerdi::has_valid_move(int val) {
	for(int i=0; i<8;i++)
		for(int j=0; j<8; j++)
			if(move_is_valid(i+1, j+1, val))
				return true;
	return false;
}

//r and c zero indexed here
//checks whether path in direction rinc, cinc results in flips for val
//will actually flip the pieces along path when doFlips is true
bool Alex_Ayerdi::check_or_flip_path(int r, int c, int rinc, int cinc, int val, bool doFlips) {
	int pathr = r + rinc;
	int pathc = c + cinc;
	if(pathr < 0 || pathr > 7 || pathc < 0 || pathc > 7 || squares[pathr][pathc]!=-1*val)
		return false;
	//check for some chip of val's along the path:
	while(true) {
		pathr += rinc;
		pathc += cinc;
		if(pathr < 0 || pathr > 7 || pathc < 0 || pathc > 7 || squares[pathr][pathc]==0)
			return false;
		if(squares[pathr][pathc]==val) {
			if(doFlips) {
				pathr=r+rinc;
				pathc=c+cinc;
				while(squares[pathr][pathc]!=val) {
					squares[pathr][pathc]=val;
					pathr += rinc;
					pathc += cinc;
				}
			}
			return true;
		}
	}
	return false;	
}


//returns whether given move is valid in this configuration
bool Alex_Ayerdi::move_is_valid(int row, int col, int val) {
	int r = row-1;
	int c = col-1;
	if(r < 0 || r > 7 || c < 0 || c > 7)
		return false;
	//check whether space is occupied:
	if(squares[r][c]!=0)
		return false;
	//check that there is at least one path resulting in flips:
	for(int rinc = -1; rinc <= 1; rinc++)
		for(int cinc = -1; cinc <= 1; cinc++) {
			if(check_or_flip_path(r, c, rinc, cinc, val, false))
				return true;
		}
	return false;
}

//executes move if it is valid.  Returns false and does not update Alex_Ayerdi otherwise
bool Alex_Ayerdi::play_square(int row, int col, int val) {
	
	if(!move_is_valid(row, col, val))
		return false;
	squares[row-1][col-1] = val;
	for(int rinc = -1; rinc <= 1; rinc++)
		for(int cinc = -1; cinc <= 1; cinc++) {
			check_or_flip_path(row-1, col-1, rinc, cinc, val, true);
		}
	return true;
}

//executes move if it is valid. Returns false and does not update Alex_Ayerdi otherwise
//Makes computer make its move and returns the computer's move in row, and col
bool Alex_Ayerdi::play_square(int &row, int &col){
	int blackplayer = 1;
	int whiteplayer = -1;

	if (row == 0 && col == 0)
	{
		setPlayer(blackplayer);
	}
	else
	{
		if (player == blackplayer)
			play_square(row, col, whiteplayer);
		if (player == whiteplayer)
			play_square(row, col, blackplayer);
	}

	//generate your own move
	if(full_board())
	{
		row = -1;
		col = -1;
		return false;
	}
	else 
	{
		cout << toString() << "..." << endl;
		vector<int> cell;
		this->reset_tries();
		if (!has_valid_move(player))
		{
			cout << "Alex_Ayerdi must pass." << endl;
			row = -1;
			col = -1;
			return false;
		}
		else 
		{
			int alphaStart = -INFINITY;
			int betaStart = INFINITY;

			//generate move
			make_minimax_alphabeta_cpu_move(this, player, cell, alphaStart, betaStart);
			//play move
			if (!play_square(cell[0], cell[1], player))
			{
				row = -1;
				col = -1;
				return false;
			}
			
			//pass to opposing player
			row = cell[0];
			col = cell[1];
		}
			
		cout << toString();
	}

	return true;
}

bool Alex_Ayerdi::full_board() {
	for(int i=0; i<8;i++)
		for(int j=0; j<8; j++)
			if(squares[i][j]==0)
				return false;
	return true;
}

//returns score, positive for X player's advantage
int Alex_Ayerdi::score() {
	int sum =0;
	for(int i=0; i<8;i++)
		for(int j=0; j<8; j++)
			sum+=squares[i][j];
	return sum;
}

int Alex_Ayerdi::get_square(int row, int col) {
	return squares[row-1][col-1];
}

int make_minimax_alphabeta_cpu_move(Alex_Ayerdi * b, int playerval, vector<int> &cell, int alpha, int beta) {
	
	//if we have gotten to check to a full board then return the end score
	if (b->full_board()) return utility(b);

	int resultMin = INFINITY;
	int resultMax = -INFINITY;

	//a technical check for if we made a move or not
	//False: then we never got to make a move (a pass)
	//so we must handle that siuation
	//True: continue on as normal
	bool madeMove = false;

	//With a max of 10 plies we can make it under 15 seconds
	if (b->get_tries() == 10) return utility(b); 

	//for max player
	if (playerval == MAX)
	{
		//check among all the children
		for (int i = 1; i < 9; i++)
		{
			for (int j = 1; j < 9; j++)
			{
				//choose the empty squares
				if (b->get_square(i, j) == 0)
				{
					//save a temporary board for before making a move
					Alex_Ayerdi tempAlex_Ayerdi;
					tempAlex_Ayerdi.copyBoard(b);

					//play the move
					if (b->play_square(i, j, playerval) == false)
					{
						//if we can't play this move then move on
						//no need to remove anything since nothing was placed							
						continue; 
					}
					else
					{
						//since this was a valid move change this flag
						madeMove = true;
					}

					//increase the amount of tries since we made a valid move (down the next ply)
					b->increase_tries();

					//get the utilty from making that test move
					resultMax = make_minimax_alphabeta_cpu_move(b, MIN, cell, alpha, beta);
					
					//if our score is greater than the max we've seen...
					if (resultMax > alpha)
					{
						//update the max (alpha)
						alpha = resultMax;

						//not all moves are valid, only save the ones on the first ply 
						if (b->get_tries() == 1)
						{
							cell.clear();
							cell.push_back(i);
							cell.push_back(j);
						}
					}
					//if we find that our path leads to a corner piece...
					if ((i == 1 && j == 1) ||
						(i == 8 && j == 8) ||
						(i == 1 && j == 8) ||
						(i == 8 && j == 1)) 
					{
						b->copyBoard(&tempAlex_Ayerdi);
						b->decrease_tries();

						return MAX;  //return MAX because this is a really good move
					}
					//if our alpha max is greater or equal to our current beta min then cut off
					if (alpha >= beta)
					{
						
						b->copyBoard(&tempAlex_Ayerdi);  //return to initial state
						b->decrease_tries(); //decrease the amount of tries since we're going backward

						//alpha cutoff (alpha-beta pruning)
						return alpha;
					}


					b->copyBoard(&tempAlex_Ayerdi); //return to initial state
					b->decrease_tries();  //decrease the amount of tries since we're going backward
				}
			}
		}

		//if we never made a move then we need to return the score at that point
		if (madeMove == false) 
			return utility(b);
		
		return alpha;  //return the utility of the best move we've found
	}
	else
	{
		//same deal as the max player, but with min idea
		for (int i = 1; i < 9; i++)
		{
			for (int j = 1; j < 9; j++)
			{
				if (b->get_square(i, j) == 0)
				{
					Alex_Ayerdi tempAlex_Ayerdi;
					tempAlex_Ayerdi.copyBoard(b);

					if (b->play_square(i, j, playerval) == false)
					{
						continue;
					}
					else
					{
						madeMove = true;
					}
					b->increase_tries();

					resultMin = make_minimax_alphabeta_cpu_move(b, MAX, cell, alpha, beta);

					if (resultMin < beta)
					{
						beta = resultMin;

						if (b->get_tries() == 1)
						{
							cell.clear();
							cell.push_back(i);
							cell.push_back(j);
						}
					}

					if ((i == 1 && j == 1) ||
						(i == 8 && j == 8) ||
						(i == 1 && j == 8) ||
						(i == 8 && j == 1)) 
					{
						b->copyBoard(&tempAlex_Ayerdi);
						b->decrease_tries();

						return MIN;  //this is a really good move for the other player
					}
					if (alpha >= beta)
					{
						b->copyBoard(&tempAlex_Ayerdi);
						b->decrease_tries();

						//beta cutoff (alpha-beta pruning)
						return beta;
					}
					
					b->copyBoard(&tempAlex_Ayerdi);
					b->decrease_tries();
				}
			}
		}

		if (madeMove == false)
			return utility(b);

		return beta;
	}
}
/*
	Same idea as make_minimax_alphabeta_cpu_move but without the alpha beta checking
*/
int make_minimax_cpu_move(Alex_Ayerdi * b, int playerval, vector<int> &cell) {
	
	if (b->full_board()) return utility(b);

	int minplay = INFINITY;
	int maxplay = -INFINITY;

	int resultMin = INFINITY;
	int resultMax = -INFINITY;

	bool madeMove = false;

	if (b->get_tries() == 5) return utility(b); 

	if (playerval == MAX)
	{
		//go among all the children
		for (int i = 1; i < 9; i++)
		{
			for (int j = 1; j < 9; j++)
			{
				//choose the empty squares
				if (b->get_square(i, j) == 0)
				{
					//play a test square
					Alex_Ayerdi tempAlex_Ayerdi;
					tempAlex_Ayerdi.copyBoard(b);

					if (b->play_square(i, j, playerval) == false)
					{
						continue; //if we can't play any moves, return the utility
					}
					else
					{
						madeMove = true;
					}

					b->increase_tries();

					//get the utilty from making that test move
					resultMax = make_minimax_cpu_move(b, MIN, cell);

					if (resultMax >= maxplay) // save the maximum outcome
					{
						maxplay = resultMax;

						
						if (b->get_tries() == 1)
						{
							cell.clear();
							cell.push_back(i);
							cell.push_back(j);
						}

					}

					//reset the move so we can try another
					
					if ((i == 1 && j == 1) ||
						(i == 8 && j == 8) ||
						(i == 1 && j == 8) ||
						(i == 8 && j == 1)) 
					{
						b->copyBoard(&tempAlex_Ayerdi);
						b->decrease_tries();

						return MAX;
					}

					b->copyBoard(&tempAlex_Ayerdi);
					b->decrease_tries();
				}
			}
		}

		if (madeMove == false) 
			return utility(b);
		
		return maxplay;  //return the best move we found
	}
	else
	{
		//same deal as the max player, but with min idea
		for (int i = 1; i < 9; i++)
		{
			for (int j = 1; j < 9; j++)
			{
				if (b->get_square(i, j) == 0)
				{
					Alex_Ayerdi tempAlex_Ayerdi;
					tempAlex_Ayerdi.copyBoard(b);

					if (b->play_square(i, j, playerval) == false)
					{
						continue;
					}
					else
					{
						madeMove = true;
					}
					b->increase_tries();

					resultMin = make_minimax_cpu_move(b, MAX, cell);

					if (resultMin <= minplay)  // save the minimum outcome
					{
						minplay = resultMin;

						//not all moves are valid, only save the best one from the first ply
						if (b->get_tries() == 1)
						{
							cell.clear();
							cell.push_back(i);
							cell.push_back(j);
						}

					}

					
					if ((i == 1 && j == 1) ||
						(i == 8 && j == 8) ||
						(i == 1 && j == 8) ||
						(i == 8 && j == 1)) 
					{
						b->copyBoard(&tempAlex_Ayerdi);
						b->decrease_tries();

						return MIN;
					}
					

					b->copyBoard(&tempAlex_Ayerdi);
					b->decrease_tries();
				}
			}
		}

		if (madeMove == false)
			return utility(b);

		return minplay;
	}
}

int utility(Alex_Ayerdi * b)
{

	if (b->score() > 0) //if max ends up with more by now, return win utility
	{ 
		return MAX;
	}
	else if (b->score() < 0)  //if min player ends up with more now, return lose utility
	{
		return MIN;
	}
	else 
		return 0;  //if a tie, return tie utility
}

const char * WhichPlayer(int num)
{
	//if the player num is 1 then they are black, otherwise they are white
	return num == 1 ? "Black" : "White";
}

int humanPlay(Alex_Ayerdi * b, int humanPlayer, int & consecutivePasses)
{

	int row, col;
	if(!b->has_valid_move(humanPlayer)) 
	{
		//player doesn't have a valid move
		cout << "Human must pass." << endl;
		consecutivePasses++;
		return 0;
	}
	else 
	{
		//help to make a valid move
        consecutivePasses = 0;
		cout << "Your move row (1-8): ";
		cin >> row;
		if(cin.fail()){
                std::cerr<< "Illegal move."<<endl;
                cin.clear();
                cin.ignore();
				return 0;
        }
		cout << "Your move col (1-8): ";
		cin >> col;
		if(cin.fail())
		{
            std::cerr<< "Illegal move."<<endl;
            cin.clear();
            cin.ignore();
			return 0;
        }
		if(!b->play_square(row, col, humanPlayer)) {
            cout << "Illegal move." << endl;
			return 0;
        }        
	}
	return 1;
}

int computerPlay(Alex_Ayerdi * b, int cpuPlayer, int & consecutivePasses)
{
	//move for computer:
	if(b->full_board())
		return 0;
	else {
		cout << b->toString() << "..." << endl;
		vector<int> cell;
		b->reset_tries();
		if (!b->has_valid_move(cpuPlayer))
		{
			//computer doesn't have a valid move
			cout << "Computer must pass." << endl;
			consecutivePasses++;
		}
		else 
		{
			int alphaStart = -INFINITY;
			int betaStart = INFINITY;
			//run algorithm to play an optimal move
			make_minimax_alphabeta_cpu_move(b, cpuPlayer, cell, alphaStart, betaStart);
			//play that move
			b->play_square(cell[0], cell[1], cpuPlayer);
				
			consecutivePasses = 0;
					
			cout << endl;
			cout << "(" << cell[0] << ", " << cell[1] << ")" << endl;
		}
			
		cout << b->toString();
	}
	return 1;
}

void play() {

	//randomly assign yourself as black or white
	srand((int)time(NULL));
	int playerNum = rand() % 2;

	cout << "By random selection, you are " << WhichPlayer(playerNum) << endl;

	Alex_Ayerdi * b = new Alex_Ayerdi();
	
	if (playerNum) b->setPlayer(playerNum);  //if 1 then set as 1
	else b->setPlayer(-1);  //else set as -1

	int humanPlayer = b->getPlayer();  
	int cpuPlayer = humanPlayer == 1 ? -1 : 1;

	//start by displaying the board
	cout << b->toString();
	int consecutivePasses = 0;
	bool human = humanPlayer == 1 ? true : false;
	while(!b->full_board() && consecutivePasses<2) {
		
		//player to go first is whomever has value 1 (black), -1 (white) goes second
		if (human)
		{
			//ask for the human play
			if (!humanPlay(b, humanPlayer, consecutivePasses)) continue;
			human = false;
		}
		else
		{
			//play the computer move
			if (!computerPlay(b, cpuPlayer, consecutivePasses)) continue;
			human = true;
		}
	}

	//tally up the score and report it
	int score = b->score();
	if(score == 0)
		cout << "Tie game." << endl;
	else if(score > 0)
		cout << "Black wins by " << score << endl;
	else if(score < 0)
		cout << "White wins by " << -score << endl;
	
	cout << "Press Enter to Exit" << endl;

	cin.ignore();
	cin.ignore();
}

int main(int argc, char * argv[])
{
	//SINGLE PLAYER
	play();
	return 0;

	//MULTI-PLAYER
	Alex_Ayerdi * playerOne = new Alex_Ayerdi();
	Alex_Ayerdi * playerTwo = new Alex_Ayerdi();
	
	//randomly assign as black or white
	srand((int)time(NULL));
	int flipBlack = rand() % 2;
	int row, col;

	if (flipBlack)
	{
		row = 0;
		col = 0;
		playerOne->play_square(row, col);

		bool keepPlay = true;
		int consecutivepasses = 0;

		do
		{
			//play the computer against itself with player 2 as white
			keepPlay = playerTwo->play_square(row, col);
			keepPlay = playerOne->play_square(row, col);

			if (!keepPlay) 
			{
				consecutivepasses++;
				keepPlay = true;
			}
			if (consecutivepasses > 2) 
				keepPlay = false;
		} while (keepPlay);

	}
	else
	{
		row = 0;
		col = 0;
		playerTwo->play_square(row, col);

		bool keepPlay = true;
		int consecutivepasses = 0;
		do
		{
			//play the computer against itself with player 1 as White
			keepPlay = playerOne->play_square(row, col);
			keepPlay = playerTwo->play_square(row, col);

			if (!keepPlay) 
			{
				consecutivepasses++;
				keepPlay = true;
			}
			if (consecutivepasses > 2) 
				keepPlay = false;
		} while (keepPlay);
	}

	//tally up the scores
	int score = playerOne->score();
	if(score == 0)
		cout << "Tie game." << endl;
	else if(score > 0)
		cout << "Black wins by " << score << endl;
	else if(score < 0)
		cout << "White wins by " << -score << endl;
	
	cout << "Press Enter to Exit" << endl;

	cin.ignore();

	return 0;
}
