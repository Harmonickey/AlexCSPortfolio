// Sudoku.cpp : Basic class for holding a Sudoku board, reading a board from files, a writing a board to the screen
//

#include<iostream>
#include<fstream>
#include<sstream>
#include<math.h>

#include<string>
#include<vector>

using namespace std;

class Board {
	//NOTE: Everything is indexed starting at 1...except my code

	int dim;                     //dimensions of the board
	int ** cells;                //matrix of the cells
	long backtrack;              //total backtracks
	long minimumremaining;       //total calls to get a minimum remaining variable
	long forwardchecking;        //total forward checks in inferencing
	long leastconstraining;
	long mostconstraining;
	vector<int> ** valueList;    //matrix of possible values and number of values for each cell
public:
	Board (int);  
	~Board();

	bool set_square_value(int,int,int, vector<int*> &);                          //row, col, value
	int get_square_value(int,int);                               //return value from row, col
	static Board * fromFile(string);                             //create the board from file
	bool checkForVictory();                                      //check the board if it's solved
	int get_dim() {return dim;}                                  //get the board's dimensions
	string displayResults();


	void SelectUnassignedCell_MRV(int* cell);                        //select a best cell with h(n) = min num possible values
	int SelectLeastConstrainingValue(int row, int col);
	int SelectMostConstrainingVariable(int row, int col);

	bool removePossibleValues(int row, int col, int val, vector<int*> &savedCoordinates);        //remove the possible value (val) from (row, col)
	
	/*
		Display the board to a stringstream that you can use for cout
		You can display either (or any combination) of cell values, number of possible values at each cell,
		and the actual possible values at each cell
	*/
	string toString(bool cell, bool numValues, bool values);

	/*
		Recursive algorithm that uses backtracking, forward search inference, and 
		minimum restricted value to select new variables to assign
	*/
	bool BacktrackSearch();
};

Board::Board(int d) {
	//initialize all values in the board
	if(d > 62)
		throw ("Dimensions must be at most 62");
	dim = d;
	cells = new int*[dim];
	valueList = new vector<int>*[dim];

	//set up an array of possible values for cells
	int * myArray = (int *)malloc(sizeof(int) * dim);
	for (int i=0; i < dim; i++)
		myArray[i] = i + 1;

	//this creates the cell and value list matrices
	for(int i=0; i<dim;i++) {
		cells[i] = new int[dim];
		valueList[i] = new vector<int>[dim];
		for(int j=0; j<dim;j++) {
			cells[i][j] = 0;  //set all the values to 0
			valueList[i][j].insert(valueList[i][j].begin(), myArray, myArray+dim);  //set the possible values
		}
	}
	
	free (myArray);

	backtrack = 0;
	minimumremaining = 0;
	forwardchecking = 0;
	leastconstraining = 0;
	mostconstraining = 0;
}

Board::~Board() {
	//delete all the cells in the board
	for(int i=0; i<dim;i++) {
		delete [] cells[i];
		delete [] valueList[i];
	}
	delete [] cells;
	delete [] valueList;
}

string Board::displayResults()
{
	stringstream s;
	s << "Backtrack: " << backtrack << endl;
	s << "Forward Checking: " << forwardchecking << endl;
	s << "MRV: " << minimumremaining << endl;
	s << "LCV: " << leastconstraining << endl;
	s << "MCV: " << mostconstraining << endl;

	return s.str();

}

string Board::toString(bool cell, bool numValues, bool values) {
	stringstream s;
	
	if (cell)
	{
		for(int i=0; i<dim;i++) {
			for(int j=0; j<dim; j++) {
				if(cells[i][j]==0)
					s << '_';  //if no value, use placeholder
				else
					s << cells[i][j];  //else put on the value
			}
			s << endl;
		}
		s << endl;
	}
	else if (numValues)
	{
		for (int i = 0; i < dim; i++) {
			for(int j = 0; j < dim; j++) {
				s << valueList[i][j].size();  //size will hold the number of possible values at this spot
			}
			s << endl;
		}

		s << endl;
	}
	else if (values)
	{
		for (int i = 0; i < dim; i++)
		{
			for (int j = 0; j < dim; j++)
			{
				//display out the actual possible values at this spot in the matrix
				int spaces = dim - valueList[i][j].size();
				for (int m = 0; m < spaces; m++) s << " ";
				for (int k = 0; k < (int)valueList[i][j].size(); k++) {
					if (cells[i][j] == 0)
					{
						s << valueList[i][j][k];
					}
				}
				s << " ";
			}
			s << endl;
			s << endl;
		}
		s << endl;
	}
	return s.str();
}

bool Board::set_square_value(int row, int col, int val, vector<int*> &savedCoordinates) {
	cells[row-1][col-1] = val;   //set the value
	
	return this->removePossibleValues(row - 1, col - 1, val, savedCoordinates);  //remove possible values from corresponding row and col cells
}

int Board::get_square_value(int row, int col) {
	return cells[row-1][col-1];  //return the value
}

bool Board::removePossibleValues(int row, int col, int val, vector<int *> &savedCoordinates) 
{
	//establish arc consistency
	
	for (int i = 0; i < dim; i++)
	{
		if (cells[row][i] == 0) //for cells not already assigned
		{
			forwardchecking++;
			//if we find that it's empty then there is a consistency problem, return error
			if (valueList[row][i].empty()) return false;
			if (valueList[row][i].size() == 1 && valueList[row][i].front() == val) return false;
			//otherwise iterate through the row and erase (val) from the valueLists
			vector<int>::iterator itr = valueList[row][i].begin();
			while(itr != valueList[row][i].end())
			{
				if (*itr == val)
				{
					//save these coordinates for later use in case this assignment fails
					int * coordinates = (int *)malloc(sizeof(int) * 2);
					coordinates[0] = row;
					coordinates[1] = i;
					savedCoordinates.push_back(coordinates);
					itr = valueList[row][i].erase(itr);
					break;
				}
				
				itr++;
				
			}
		}
		if (cells[i][col] == 0)
		{
			forwardchecking++;
			//same here but for the column
			if (valueList[i][col].empty()) return false;
			if (valueList[i][col].size() == 1 && valueList[i][col].front() == val) return false;
			vector<int>::iterator itr = valueList[i][col].begin();
			while (itr != valueList[i][col].end())
			{
				if (*itr == val)
				{
					int * coordinates = (int *)malloc(sizeof(int) * 2);
					coordinates[0] = i;
					coordinates[1] = col;
					savedCoordinates.push_back(coordinates);
					itr = valueList[i][col].erase(itr);
					break;
				}
				
				itr++;
				
			}
		}
	}

	//same here but for the 'small box'
	int dimsqrt = (int)(sqrt((double)dim));
	int whichBoxCol = (col / dimsqrt) * dimsqrt;
	int whichBoxRow = (row / dimsqrt) * dimsqrt;

	for (int i = whichBoxRow; i < whichBoxRow + dimsqrt; i++) 
	{
		for (int j = whichBoxCol; j < whichBoxCol + dimsqrt; j++) 
		{
			if (cells[i][j] == 0)
			{
				forwardchecking++;
				if (valueList[i][j].empty()) return false;
				if (valueList[i][j].size() == 1 && valueList[i][j].front() == val) return false;
				vector<int>::iterator itr = valueList[i][j].begin();
				while (itr != valueList[i][j].end()) 
				{
					if (*itr == val)
					{
						int * coordinates = (int *)malloc(sizeof(int) * 2);
						coordinates[0] = i;
						coordinates[1] = j;
						savedCoordinates.push_back(coordinates);
						itr = valueList[i][j].erase(itr);
						break;
					}
					itr++;
				}	
			}
		}
	}
	return true;
}

Board * Board::fromFile(string inFileName) {
  //read from the file and set values
  string line;
  ifstream inFile (inFileName);

  Board * out;
  if (inFile.is_open()) {
	  getline (inFile,line);
	  int d = atoi(line.c_str());
	  out = new Board(d);
	  getline (inFile, line);
	  int numVals = atoi(line.c_str());
	  for(int i=0; i<numVals;i++) {
		string field;
		getline (inFile,field, '\t');
		int row = atoi(field.c_str());
		getline (inFile,field, '\t');
		int col = atoi(field.c_str());
		getline (inFile,field);
		int val = atoi(field.c_str());

		vector<int *> s2; //dummy parameter
		//set the square at (row, col) with value (val)
		out->set_square_value(row, col, val, s2); 
	  }
  }

  inFile.close();
  return out;
}

bool Board::checkForVictory() {
	unsigned long victory = 0;
	//optimization: check if it's filled:
	for(int i=1; i<dim+1;i++)
		for(int j=1;j<dim+1;j++)
			if(this->get_square_value(i,j)==0)
				return false;
	for(int i=1; i<dim+1; i++) 
		victory += 1 << i;
	//check rows and columns:
	for(int i=1; i<dim+1;i++) {
		unsigned long rowTotal = 0;
		unsigned long columnTotal = 0;
		for(int j=1; j<dim+1; j++) {
			rowTotal += 1 << this->get_square_value(i, j);
			columnTotal += 1 << this->get_square_value(j, i);
		}
		if(rowTotal!=victory||columnTotal!=victory)
			return false;
	}
	int dimsqrt = (int)(sqrt((double)dim));
	//check little squares:
	cout << "checking little squares" << endl;
	for(int i=0;i<dimsqrt;i++) {
		for(int j=0;j<dimsqrt;j++) {
			unsigned long squareTotal = 0;
			for(int k=1; k<dimsqrt+1;k++) {
				for(int m=1; m<dimsqrt+1;m++) {
					squareTotal += 1 << this->get_square_value(i*dimsqrt+k, j*dimsqrt+m);
					cout << this->get_square_value(i*dimsqrt+k, j*dimsqrt+m);
				}
				cout << endl;
			}
			if(squareTotal != victory)
				return false;
		}
	}
	return true;
}

void Board::SelectUnassignedCell_MRV(int * cell)
{
	
	//used to find out the most constrained variable
	int minValue = dim;  //the minimum value must start at the worst case
	
	//row and col used for the return value to know which cell was picked
	//amount and maxAmount used to find out the most constraining variable
	int row, col, amount, maxAmount = 0;

	for (int i = 0; i < dim; i++)
	{
		for (int j = 0; j < dim; j++)
		{
			if (cells[i][j] == 0) //we're only interested in unassigned cells
			{
				minimumremaining++;

				int size = valueList[i][j].size();  
				if (size < minValue) //checking for most constrained variable...
				{
					row = i;
					col = j;
					minValue = size;
						
					maxAmount = this->SelectMostConstrainingVariable(i, j);  //get the amount of constraints attached to
				}
				else if (size == minValue)  //checking for most constraining variable...
				{
					//since this have the same MRV, break the tie by seeing if it has more constraints attached to it...
					amount = this->SelectMostConstrainingVariable(i, j);

					if (amount > maxAmount) 
					{
						row = i;
						col = j;
						maxAmount = amount;
					}
				}
			}
		}
	}
	
	//Our variable of which we can start to try and assign
	cell[0] = row;
	cell[1] = col;	
}

int Board::SelectMostConstrainingVariable(int row, int col)
{
	mostconstraining++;
	
	int amount = 0;  //used to count how many participants are in the constraints
	
	//if we find an unassigned cell increase the amount, don't count already counted cells!
	for (int i = 0; i < dim; i++) 
	{
		if (cells[row][i] == 0 && i != col)
		{
			amount++;	
		}
		if (cells[i][col] == 0 && i != row)
		{
			amount++;
		}
	}

	int dimsqrt = (int)(sqrt((double)dim));
	int whichBoxCol = (col / dimsqrt) * dimsqrt;
	int whichBoxRow = (row / dimsqrt) * dimsqrt;
	for (int i = whichBoxRow; i < whichBoxRow + dimsqrt; i++) 
	{
		for (int j = whichBoxCol; j < whichBoxCol + dimsqrt; j++)
		{
			if (cells[i][j] == 0 && (i != row && j != col))
			{
				amount++;	
			}
		}
	}

	//return the amount for comparison later
	return amount;
}

int Board::SelectLeastConstrainingValue(int row, int col)
{
	
	
	int minAmount = dim * dim * dim;  //need to start in worst case
	int value = 0;
	if (valueList[row][col].size() > 1)
	{
		vector<int>::iterator itr = valueList[row][col].begin();
		value = *itr;
		while (itr != valueList[row][col].end())
		{
			int amount = 0;
			for (int i = 0; i < dim; i++) {
				if (cells[row][i] == 0 && i != col)
				{
					//if the value is found in the valueList that means there is one more cooperator in (row, col)'s constraints
					if (find(valueList[row][i].begin(), valueList[row][i].end(), *itr) != valueList[row][i].end())
					{
						//found the value
						amount++;
					}
				}
				if (cells[i][col] == 0 && i != row)
				{
					if (find(valueList[i][col].begin(), valueList[i][col].end(), *itr) != valueList[i][col].end())
					{
						//found the value
						amount++;
					}
				}
			}
			int dimsqrt = (int)(sqrt((double)dim));
			int whichBoxCol = (col / dimsqrt) * dimsqrt;
			int whichBoxRow = (row / dimsqrt) * dimsqrt;
			for (int i = whichBoxRow; i < whichBoxRow + dimsqrt; i++) {
				for (int j = whichBoxCol; j < whichBoxCol + dimsqrt; j++)
				{
					if (cells[i][j] == 0 && (i != row || j != col))
					{
						
						if (find(valueList[i][j].begin(), valueList[i][j].end(), *itr) != valueList[i][j].end())
						{
							amount++;
						}
					}
				}
			}

			leastconstraining++;
			//if the value is seen less in the constraints of neighbors...
			if (amount < minAmount) 
			{
				//minAmount = amount;
				minAmount = amount;
				value = *itr;
			}
			itr++;
		}
		return value;
	}
	
	return valueList[row][col][0];
}

/*
	Much of the documentation here is from the algorithm in the book...
*/
bool Board::BacktrackSearch()
{
	if (backtrack >= 2000000) return false;

	//if assignment is complete, return assignment
	if (this->checkForVictory()) return true;

	int* cell = (int *)malloc(sizeof(int) * 2);
	this->SelectUnassignedCell_MRV(cell);  //Select an Unassigned Variable (cell)
	
	//store as row and col
	int row = cell[0];
	int col = cell[1];

	//get least constrained
	//push to front
	//then create iterator
	int insertedValue = this->SelectLeastConstrainingValue(row, col);

	//foreach value in Order Domain Values
	vector<int>::iterator itr = valueList[row][col].begin();
	//remove from the possible values in order to...
	if (valueList[row][col].size() != 1 && valueList[row][col][0] != insertedValue)
	{
		while (itr != valueList[row][col].end())
		{
			if (*itr == insertedValue)
			{
				itr = valueList[row][col].erase(itr);
				break;
			}
			itr++;
		}
		//push it to the front so it's used first
		valueList[row][col].insert(valueList[row][col].begin(), insertedValue);
	}

	itr = valueList[row][col].begin();
	while (itr != valueList[row][col].end())
	{
		insertedValue = *itr;

		vector<int *> savedCoordinates;

		
		//this function also asks whether the variable assignment is valid
		bool worked = this->set_square_value(row + 1, col + 1, insertedValue, savedCoordinates); //add var=value to assignment
		
		backtrack++;
		//if value is consistent with assignment && inferences != failure
		if (worked)
		{
			//system("cls");
			//cout << this->toString(true, false, false);
			if (this->BacktrackSearch()) //if result != failure
				return true;  //return result
		}

		
		cells[row][col] = 0; //remove var=value

		//return the board to the previous state in the backtrack, use our savedCoordinates to assign correctly
		vector<int *>::iterator saveItr = savedCoordinates.begin();
		while (saveItr != savedCoordinates.end())
		{
			int row = (*saveItr)[0];
			int col = (*saveItr)[1];
			valueList[row][col].push_back(insertedValue);
			free ((*saveItr));
			saveItr++;
		}

		itr++; //check the next value
		
	}
	
	free(cell);

	return false;
}


int main(int argc, char* argv[])
{
	string filename;
	cout << "Name of the file reading from? : ";
	cin >> filename;
	cout << endl;

	//read from file and create board
	Board * b = Board::fromFile(filename);

	//run the algorithm
	if(b->BacktrackSearch())
	{
		cout << b->toString(true, false, false);
		cout << "You found a solution!!" << endl;
		cout << "Results: " <<  endl;
		cout << b->displayResults() << endl;

	}
	else
	{
		cout << b->displayResults() << endl;
		cout << "No Solution Set" << endl;
	}

	cin.ignore();
	cin.ignore();

	return 0;
}
