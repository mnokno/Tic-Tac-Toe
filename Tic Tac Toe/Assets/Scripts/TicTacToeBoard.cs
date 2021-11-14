using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeBoard
{

    public int dimensions { get; private set; }
    public int winLenght { get; private set; }


    public bool xToMove { get; private set; } = true;
    public int moveCount { get; private set; } = 0;
    public GameState gameState { get; private set; } = GameState.GameOn;
    public SpaceState[,] boardState { get; private set; }

    private int lineLenght = 0;
    private SpaceState lineType = SpaceState.Empty;

    // Start is called before the first frame update 
    public TicTacToeBoard(int dimensions, int winLenght)
    {
        this.dimensions = dimensions;
        this.winLenght = winLenght;

        // Validates dimensions and winLenght
        ValidateParameters();
        // Initialize boardState array
        boardState = new SpaceState[dimensions, dimensions];
        // Sets all states to empty
        InitateBoardState();
    }

    public TicTacToeBoard(int dimensions, int winLenght, bool xToMove, int moveCount, SpaceState[,] boardState)
    {
        this.dimensions = dimensions;
        this.winLenght = winLenght;
        this.xToMove = xToMove;
        this.moveCount = moveCount;
        this.boardState = boardState;
    }

    public void MakeMove(int x, int y)
    {
        // Check if the passed ordinates are valid
        if (x < dimensions && y < dimensions)
        {
            // Updates the board position
            if (xToMove)
            {
                boardState[x, y] = SpaceState.X;
            }
            else
            {
                boardState[x, y] = SpaceState.O;
            }

            // Updates move count
            moveCount++;
            // Next player to move is updated
            xToMove = !xToMove;
            // Update bard state
            UpdateGameState();
        }
        else
        {
            // Raises Error incorrect dimension
            Debug.LogError($"Row: {x}, Column: {y} is outside the bound of the board. Board dimensions : {dimensions}");
        }
    }

    private void ValidateParameters()
    {
        // Dimension most satisfy: dimension > 2
        if (dimensions <= 2)
        {
            Debug.LogError($"Invalid parameters: dimension most satisfy: dimension > 2. Current dimensions = {dimensions}.");
        }

        // Wining line most satisfy: 2 < wining line <= dimension
        if (2 >= winLenght || winLenght > dimensions)
        {
            Debug.LogError($"Invalid parameters: winLenght most satisfy: 2 < winLenght <= dimension. Current winLenght = {winLenght}, dimension = {dimensions}.");
        }
    }

    private void InitateBoardState()
    {
        // Imitates all values to Empty
        for (int x = 0; x < dimensions; x++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                boardState[x, y] = SpaceState.Empty;
            }
        }
    }

    private void ResetLineCounter()
    {
        lineLenght = 0;
        lineType = SpaceState.Empty;
    }

    private void NextInLine(SpaceState nextSymbolInLine)
    {
        // Keeps track of the line length, if line length is the winLenght then it marks the winner
        if (nextSymbolInLine != SpaceState.Empty)
        {
            if (nextSymbolInLine == lineType)
            {
                lineLenght++;
                // Check if the line is wining
                if (lineLenght == winLenght)
                {
                    if (lineType == SpaceState.X)
                    {
                        gameState = GameState.XWon;
                    }
                    else
                    {
                        gameState = GameState.OWon;
                    }
                }
            }
            else
            {
                lineLenght = 1;
                lineType = nextSymbolInLine;
            }
        }
        else
        {
            lineLenght = 0;
            lineType = SpaceState.Empty;
        }
    }

    private void UpdateGameState()
    {
        //- This function will update the state of the game 
        int winingSideLines = dimensions - winLenght;

        // Check if win is possible
        if (moveCount < ( winLenght * 2 - 1))
        {
            return;
        }

        // Check for vertical wins
        for (int i = 0; i < dimensions; i++)
        {
            ResetLineCounter();
            for (int j = 0; j < dimensions; j++)
            {
                NextInLine(boardState[i, j]);
            }
        }

        // Check for horizontal wins
        for (int i = 0; i < dimensions; i++)
        {
            ResetLineCounter();
            for (int j = 0; j < dimensions; j++)
            {
                NextInLine(boardState[j, i]);
            }
        }

        // Check for positive diagonal wins
        // Diagonals starting on the y-axis
        for (int i = 1; i < winingSideLines+1; i++)
        {
            ResetLineCounter();
            for (int j = 0; j < dimensions - i; j++)
            {
                NextInLine(boardState[j, i + j]);
            }
        }
        // Diagonals starting on the x-axis
        for (int i = 1; i < winingSideLines + 1; i++)
        {
            ResetLineCounter();
            for (int j = 0; j < dimensions - i; j++)
            {
                NextInLine(boardState[i + j, j]);
            }
        }
        // Leading diagonals
        ResetLineCounter();
        for (int i = 0; i < dimensions; i++)
        {
            NextInLine(boardState[i, i]);
        }

        // Check for negative diagonal wins
        // Diagonals starting on the y-axis
        for (int i = 1; i < winingSideLines + 1; i++)
        {
            ResetLineCounter();
            for (int j = 0; j < dimensions - i; j++)
            {
                NextInLine(boardState[j, dimensions - (i + j +1)]);
            }
        }
        // Diagonals starting on the x-axis
        for (int i = 1; i < winingSideLines + 1; i++)
        {
            ResetLineCounter();
            for (int j = 0; j < dimensions - i; j++)
            {
                NextInLine(boardState[i + j, dimensions - (j + 1)]);
            }
        }
        // Leading diagonals
        ResetLineCounter();
        for (int i = 0; i < dimensions; i++)
        {
            NextInLine(boardState[i, dimensions - (i + 1)]);
        }

        // Check for drawn
        if (gameState == GameState.GameOn)
        {
            if (dimensions * dimensions == moveCount)
            {
                gameState = GameState.Draw;
            }
        }
    }

    // Getters
    public SpaceState[,] GetCoppyOfBoardState()
    {
        // Returns board sate
        return Coppy(boardState);
    }

    public T[,] Coppy<T>(T[,] input)
    {
        T[,] result = new T[input.GetLength(0), input.GetLength(1)]; //Create a result array that is the same length as the input array
        for (int x = 0; x < input.GetLength(0); x++) //Iterate through the horizontal rows
        {
            for (int y = 0; y < input.GetLength(1); y++) //Iterate through the vertical rows
            {
                result[x, y] = input[x, y]; //Change result x,y to input x,y
            }
        }
        return result;
    }

    public List<LegalMove> GetAllLegalMoves()
    {
        // Create a list of legal moves
        List<LegalMove> legalMoves = new List<LegalMove>();

        // Loop over the bard state and add all empty spaces as legal moves
        for (int x = 0; x < dimensions; x++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                if (boardState[x, y] == SpaceState.Empty)
                {

                    legalMoves.Add(new LegalMove { x = x, y = y });
                }
            }
        }

        // Return the list of legal moves
        return legalMoves;
    }

    // Enums and struct
    public enum SpaceState
    {
        Empty,
        X,
        O
    }

    public enum GameState
    {
        GameOn,
        XWon,
        OWon,
        Draw
    }

    public struct LegalMove
    {
        public int x;
        public int y;
        public float score;
    }

}
