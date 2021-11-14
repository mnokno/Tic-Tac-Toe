using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class AlphaBateAI
{
    // Total number of evaluated positions
    public static int nodes = 0;
    // Total number of evaluated positions + branches intersections
    public static int globalNodes = 0;
    public static List<TicTacToeBoard.LegalMove> evaluatedMoves = new List<TicTacToeBoard.LegalMove>();

    public static TicTacToeBoard.LegalMove FindBestMoveUsingAlphaBate(TicTacToeBoard ticTacToeBoard, bool isAIX)
    {
        nodes = 0;
        globalNodes = 0;
        //return FindBestMove(ticTacToeBoard, isAIX);
        return FindBestMoveThreaded(ticTacToeBoard, isAIX);
    }

    private static TicTacToeBoard.LegalMove FindBestMove(TicTacToeBoard board, bool isAIX)
    {

        // Create list to store evaluated moves
        evaluatedMoves.Clear();

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

            if (isAIX)
            {
                currentlyEvaluatedMove.score = EvaluatePosition(currentlyEvaluatedPosition, 1, false, -999, 999);
            }
            else
            {
                currentlyEvaluatedMove.score = EvaluatePosition(currentlyEvaluatedPosition, 1, true, -999, 999);
            }

            evaluatedMoves.Add(currentlyEvaluatedMove);

            Debug.Log($"x: {currentlyEvaluatedMove.x}, y: {currentlyEvaluatedMove.y}, score {currentlyEvaluatedMove.score}");
        }

        if (isAIX)
        {
            return GetMax(evaluatedMoves);
        }
        else
        {
            return GetMin(evaluatedMoves);
        }

    }

    private static TicTacToeBoard.LegalMove FindBestMoveThreaded(TicTacToeBoard board, bool isAIX)
    {
        // Create list to store evaluated moves
        evaluatedMoves.Clear();

        foreach (TicTacToeBoard.LegalMove move in board.GetAllLegalMoves())
        {
            Task t = new Task(() => EvalMoveTask(move, board, isAIX));
            t.Start();
        }

        int targetCount = board.GetAllLegalMoves().Count;
        while (true)
        {
            Thread.Sleep(100);
            if (evaluatedMoves.Count == targetCount)
            {
                break;
            }
        }

        if (isAIX)
        {
            return GetMax(evaluatedMoves);
        }
        else
        {
            return GetMin(evaluatedMoves);
        }

    }

    public static void EvalMoveTask(TicTacToeBoard.LegalMove move, TicTacToeBoard board, bool isAIX)
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

        if (isAIX)
        {
            currentlyEvaluatedMove.score = EvaluatePosition(currentlyEvaluatedPosition, 1, false, -999, 999);
        }
        else
        {
            currentlyEvaluatedMove.score = EvaluatePosition(currentlyEvaluatedPosition, 1, true, -999, 999);
        }

        evaluatedMoves.Add(currentlyEvaluatedMove);

        Debug.Log($"x: {currentlyEvaluatedMove.x}, y: {currentlyEvaluatedMove.y}, score {currentlyEvaluatedMove.score}");
    }

    private static float EvaluatePosition(TicTacToeBoard board, int depth, bool maxToMove, float alpha, float beta)
    {
        // Return static evaluation
        if (board.gameState != TicTacToeBoard.GameState.GameOn)
        {
            nodes++;
            return StaticEvaluation(board);
        }

        if (maxToMove)
        {
            // Find best for max
            float maxEval = -999;

            foreach(TicTacToeBoard.LegalMove move in board.GetAllLegalMoves())
            {
                TicTacToeBoard currentlyEvaluatedPosition = new TicTacToeBoard(board.dimensions, board.winLenght, board.xToMove, board.moveCount, board.GetCoppyOfBoardState());
                currentlyEvaluatedPosition.MakeMove(move.x, move.y);

                float eval = EvaluatePosition(currentlyEvaluatedPosition, depth + 1, false, alpha, beta);
                maxEval = Mathf.Max(maxEval, eval);

                if (eval == 1)
                {
                    break;
                }
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return maxEval;
        }
        else
        {
            // Find best for min
            float minEval = 999;

            foreach (TicTacToeBoard.LegalMove move in board.GetAllLegalMoves())
            {
                TicTacToeBoard currentlyEvaluatedPosition = new TicTacToeBoard(board.dimensions, board.winLenght, board.xToMove, board.moveCount, board.GetCoppyOfBoardState());
                currentlyEvaluatedPosition.MakeMove(move.x, move.y);

                float eval = EvaluatePosition(currentlyEvaluatedPosition, depth + 1, true, alpha, beta);
                minEval = Mathf.Min(minEval, eval);

                if (eval == -1)
                {
                    break;
                }
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return minEval;
        }
    }

    private static TicTacToeBoard.LegalMove GetMax(List<TicTacToeBoard.LegalMove> moves)
    {
        // Place holder for th current max value
        TicTacToeBoard.LegalMove currentMax = moves[0];

        // Find the max value
        foreach (TicTacToeBoard.LegalMove move in moves)
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

    public static int StaticEvaluation(TicTacToeBoard board)
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
