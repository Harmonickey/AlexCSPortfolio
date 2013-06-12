#ifndef ALEX_AYERDI_H
#define ALEX_AYERDI_H

#include<iostream>
#include<sstream>
#include <exception>
#include<string>
#include <cctype>
#include <vector>
#include <time.h>
#include <stdlib.h>

using namespace std;

#define INFINITY 100;
#define MAX 1
#define MIN -1

class Alex_Ayerdi {
	int squares[8][8];
	int tries;
	int player;

public:
	Alex_Ayerdi();
	string toString();
	int score();
	bool full_board();
	void copyBoard(Alex_Ayerdi * b);

	bool play_square(int, int, int);
	bool play_square(int&, int&);
	int get_square(int, int);

	bool move_is_valid(int, int, int);
	bool has_valid_move(int);
	bool check_or_flip_path(int, int, int, int, int, bool);

	void increase_tries() {tries++;}
	void decrease_tries() {tries--;}
	void reset_tries() {tries = 0;}
	int get_tries() {return tries;}
	
	void setPlayer(int num) {player = num;}
	int getPlayer() {return player;}

	int eval(int cpuval);
	int free_neighbors(int i, int j);

};

int utility(Alex_Ayerdi * b);
int make_minimax_cpu_move(Alex_Ayerdi * b, int playerval, vector<int> &cell);
int make_minimax_alphabeta_cpu_move(Alex_Ayerdi * b, int playerval, vector<int> & cell, int alpha, int beta);
int humanPlay(Alex_Ayerdi * b, int humanPlayer, int & consecutivePasses);
int computerPlay(Alex_Ayerdi * b, int cpuPlayer, int & consecutivePasses);

#endif