const fs = require('fs');
const _ = require('lodash');

function solveSudokus() {
    const data = fs.readFileSync('sudoku.txt', 'utf-8');

    const gridBlocks = data.split(/grid.*\r\n/gi);
    gridBlocks.shift(); // remove the empty first item
    let totalValue = 0;
    for (const gridBlock of gridBlocks) {
        let grid = [];
        for (let line of gridBlock.split('\r\n')) {
            grid.push(line.split(''));
        }
        const possibleMatrix = createPossibleValueMatrix(grid);
        const solution = solveSudoku(grid, possibleMatrix);
        totalValue += Number(solution[0][0] + solution[0][1] + solution[0][2]);
        // printSolution(solution);
    }

    console.log(totalValue);
}

function printSolution(grid) {
    for (let row = 0; row < 9; row++) {
        console.log(grid[row].join(''));
    }
}

function createPossibleValueMatrix(grid) {
    // loop through the entire grid and store coordinates and possible values
    const possibleMatrix = {};
    for (let row = 0; row < 9; row++) {
        for (let col = 0; col < 9; col++) {
            if (grid[row][col] === '0') {
                const vals = { '1': true, '2': true, '3': true, '4': true, '5': true, '6': true, '7': true, '8': true, '9': true };
                setPossibleRowValues(grid, row, vals);
                setPossibleColValues(grid, col, vals);
                setPossibleBlockValues(grid, row, col, vals);
                const possibleValues = Object.keys(vals);
                possibleMatrix[`${row}${col}`] = possibleValues;
            }
        }
    }

    return possibleMatrix;
}

function setPossibleRowValues(grid, row, vals) {
    for (let col = 0; col < 9; col++) {
        if (vals[grid[row][col]]) {
            delete vals[grid[row][col]];
        }
    }
}

function setPossibleColValues(grid, col, vals) {
    for (let row = 0; row < 9; row++) {
        if (vals[grid[row][col]]) {
            delete vals[grid[row][col]];
        }
    }
}

function setPossibleBlockValues(grid, row, col, vals) {
    const startRow = Math.floor(row / 3) * 3;
    const startCol = Math.floor(col / 3) * 3;

    for (let rowBlock = startRow; rowBlock < startRow + 3; rowBlock++) {
        for (let colBlock = startCol; colBlock < startCol + 3; colBlock++) {
            if (vals[grid[rowBlock][colBlock]]) {
                delete vals[grid[rowBlock][colBlock]];
            }
        }
    }
}

function unsetPossibleRowValues(possibleMatrix, row, val) {
    for (let col = 0; col < 9; col++) {
        if (possibleMatrix[`${row}${col}`]) {
            const index = possibleMatrix[`${row}${col}`].indexOf(val);
            if (index >= 0) {
                possibleMatrix[`${row}${col}`].splice(index, 1);

                // this means we inserted a value that doesn't let an empty space
                // at this location have any possible values, we have to recurse upwards
                if (!possibleMatrix[`${row}${col}`].length) {
                    return false;
                }
            }
        }
    }

    return true;
}

function unsetPossibleColValues(possibleMatrix, col, val) {
    for (let row = 0; row < 9; row++) {
        if (possibleMatrix[`${row}${col}`]) {
            const index = possibleMatrix[`${row}${col}`].indexOf(val);
            if (index >= 0) {
                possibleMatrix[`${row}${col}`].splice(index, 1);

                // this means we inserted a value that doesn't let an empty space
                // at this location have any possible values, we have to recurse upwards
                if (!possibleMatrix[`${row}${col}`].length) {
                    return false;
                }
            }
        }
    }

    return true;
}

function unsetPossibleBlockValues(possibleMatrix, row, col, val) {
    const startRow = Math.floor(row / 3) * 3;
    const startCol = Math.floor(col / 3) * 3;

    for (let rowBlock = startRow; rowBlock < startRow + 3; rowBlock++) {
        for (let colBlock = startCol; colBlock < startCol + 3; colBlock++) {
            if (possibleMatrix[`${rowBlock}${colBlock}`]) {
                const index = possibleMatrix[`${rowBlock}${colBlock}`].indexOf(val);
                if (index >= 0) {
                    possibleMatrix[`${rowBlock}${colBlock}`].splice(index, 1);

                    // this means we inserted a value that doesn't let an empty space
                    // at this location have any possible values, we have to recurse upwards
                    if (!possibleMatrix[`${rowBlock}${colBlock}`].length) {
                        return false;
                    }
                }
            }
        }
    }

    return true;
}

function solveSudoku(grid, possibleMatrix, valIndex, valRow, valCol) {
    if (valIndex !== undefined) {
        const val = possibleMatrix[`${valRow}${valCol}`][valIndex];
        possibleMatrix[`${valRow}${valCol}`] = [];
        grid[valRow][valCol] = val;

        // update row, col, block after we inserted our value
        if (!unsetPossibleRowValues(possibleMatrix, valRow, val) ||
            !unsetPossibleColValues(possibleMatrix, valCol, val) ||
            !unsetPossibleBlockValues(possibleMatrix, valRow, valCol, val)) {
            // if we hit a sudoku roadblock, recurse back up
            return false;
        }
    }

    const emptyCells = Object.keys(possibleMatrix);
    // first find all elements with only one possible answer, then check the others
    for (let check = 1; check <= 9; check++) {
        for (let emptyCell of emptyCells) {
            const possibleValues = possibleMatrix[emptyCell];
            if (possibleValues.length === check) {
                for (let index = 0; index < check; index++) {
                    const row = Number(emptyCell[0]);
                    const col = Number(emptyCell[1]);

                    // insert our possible value
                    const solution = solveSudoku(_.cloneDeep(grid), _.cloneDeep(possibleMatrix), index, row, col);
                    // only return viable solutions, otherwise, try the next value
                    if (solution) {
                        return solution;
                    }
                }

                // tricky corner case here, need to recurse back up
                // when the entire list of possible values had issues when
                // inserting
                return false;
            }
        }
    }

    return grid;
}

solveSudokus();