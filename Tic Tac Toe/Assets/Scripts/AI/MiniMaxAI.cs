using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxAI
{
    // Total number of evaluated positions
    public static int nodes = 0;
    // Total number of evaluated positions + branches intersections
    public static int globalNodes = 0;

    public static TicTacToeBoard.LegalMove FindBestMoveUsingMiniMax(TicTacToeBoard ticTacToeBoard, bool isAIX)
    {
        nodes = 0;
        globalNodes = 0;
        return FindBestMove(ticTacToeBoard, 1, true, isAIX);
    }

    private static TicTacToeBoard.LegalMove FindBestMove(TicTacToeBoard board, int depth, bool maxToMove, bool isAIX)
    {
        // Create list to store evaluated moves
        List<TicTacToeBoard.LegalMove> evaluatedMoves = new List<TicTacToeBoard.LegalMove>();

        // Evaluate each move
        foreach (TicTacToeBoard.LegalMove move in board.GetAllLegalMoves())
        {
            // Simulate the move being played
            TicTacToeBoard currentlyEvaluatedPosition = new TicTacToeBoard(board.dimensions, board.winLenght, board.xToMove, board.moveCount, board.GetCoppyOfBoardState());
            currentlyEvaluatedPosition.MakeMove(move.x, move.y);

            // Create a new legal move 
            TicTacToeBoard.LegalMove currentlyEvaluatedMove = new TicTacToeBoard.LegalMove
            {
                x = move.x,
                y = move.y
            };

            // Updates total global node count
            globalNodes++;

            if (currentlyEvaluatedPosition.gameState == TicTacToeBoard.GameState.GameOn) 
            {
                // Further evaluation is required
                currentlyEvaluatedMove.score = FindBestMove(currentlyEvaluatedPosition, depth + 1, !maxToMove, isAIX).score;
            }
            else
            {
                // Updates node count
                nodes++;

                // The game has reached a decisive result and the evaluation is retrieved
                if (isAIX)
                {
                    currentlyEvaluatedMove.score = EvaluatePosition(currentlyEvaluatedPosition);
                }
                else
                {
                    currentlyEvaluatedMove.score = -EvaluatePosition(currentlyEvaluatedPosition);
                }            
            }

            evaluatedMoves.Add(currentlyEvaluatedMove);
        }

        // Choose the best move
        if (maxToMove)
        {
            return GetMax(evaluatedMoves);
        }
        else
        {
            return GetMin(evaluatedMoves);
        }
    }

    private static TicTacToeBoard.LegalMove GetMax(List<TicTacToeBoard.LegalMove> moves)
    {
        // Place holder for th current max value
        TicTacToeBoard.LegalMove currentMax = moves[0];

        // Find the max value
        foreach(TicTacToeBoard.LegalMove move in moves)
        {
            if (currentMax.score < move.score)
            {
                currentMax = move;
            }
        }

        // Return the max value
        return currentMax;
    }

    private static TicTacToeBoard.LegalMove GetMin(List<TicTacToeBoard.LegalMove> moves)
    {
        // Place holder for th current min value
        TicTacToeBoard.LegalMove currentMin = moves[0];

        // Find the min value
        foreach (TicTacToeBoard.LegalMove move in moves)
        {
            if (currentMin.score > move.score)
            {
                currentMin = move;
            }
        }

        // Return the min value
        return currentMin;
    }

    public static int EvaluatePosition(TicTacToeBoard board)
    {
        if (board.gameState == TicTacToeBoard.GameState.Draw)
        {
            return 0;
        }
        else
        {
            if (board.gameState == TicTacToeBoard.GameState.XWon)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
