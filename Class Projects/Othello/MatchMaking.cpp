//Othello matchmaking code
//06/10/2013
//Besim Avci

#include <iostream>
#include <sstream>
#include <exception>
#include <string>
#include <cctype>
#include <vector>
#include <time.h>
#include <stdlib.h>
#include <ctime>
#include <winsock.h>
#include "timeAlex.h"
#include "Alex_Ayerdi.h"
#include "Cadenas_Moreno.h"


#define TIMELIMIT 20

bool exceededTimeLimit(long limit, timeval start);

int main(int argc, char * argv[])
{

	Alex_Ayerdi * playerOne = new Alex_Ayerdi();
	Cadenas_Moreno * playerTwo = new Cadenas_Moreno();
	
	//randomly assign as black or white
	srand((int)time(NULL));
	int flipBlack = rand() % 2;
	int row = 0, col = 0;
	int consecutivepasses = 0;
	bool continueGame[2];
	continueGame[0] = continueGame[1] = true;
	timeval start;
	
	if (flipBlack)
	{
		do
		{
			gettimeofday(&start, NULL);
			continueGame[0] = playerOne->play_square(row, col);
			if (exceededTimeLimit(TIMELIMIT, start)){
                    cout<<"Black (Alex) timed out"<<endl;
                    break;
            }
            gettimeofday(&start, NULL);
			continueGame[1] = playerTwo->play_square(row, col);
			if (exceededTimeLimit(TIMELIMIT, start)){
                    cout<<"White (player two) timed out"<<endl;
                    break;
            }

			if (continueGame[0] ==false && continueGame[1] == false) 
			{
				break;
			}
		} while (true);

		int score = playerOne->score();
		if(score == 0)
			cout << "Tie game." << endl;
		else if(score > 0)
			cout << "Black (Alex) wins by " << score << endl;
		else if(score < 0)
			cout << "White (player two) wins by " << -score << endl;
	
		cout << "Press Enter to Exit" << endl;

		cin.ignore();
	}
	else
	{
		do
		{
			gettimeofday(&start, NULL);
			continueGame[1] = playerTwo->play_square(row, col);
			if (exceededTimeLimit(TIMELIMIT, start)){
                    cout<<"Black (player two) timed out"<<endl;
                    break;
            }
			gettimeofday(&start, NULL);
			continueGame[0] = playerOne->play_square(row, col);
			if (exceededTimeLimit(TIMELIMIT, start)){
                    cout<<"White (Alex) timed out"<<endl;
                    break;
            }

			if (continueGame[0] ==false && continueGame[1] == false) 
			{
				break;
			}
		} while (true);

		int score = playerOne->score();
		if(score == 0)
			cout << "Tie game." << endl;
		else if(score > 0)
			cout << "Black (player two) wins by " << score << endl;
		else if(score < 0)
			cout << "White (Alex) wins by " << -score << endl;
	
		cout << "Press Enter to Exit" << endl;

		cin.ignore();
	}

	

	return 0;
}

bool exceededTimeLimit(long limit, timeval start){
    timeval t;
	gettimeofday(&t, NULL);
	time_t timeElapsedInSeconds = (t.tv_sec - start.tv_sec);
	long total = timeElapsedInSeconds;	
	return total >= limit;
}
