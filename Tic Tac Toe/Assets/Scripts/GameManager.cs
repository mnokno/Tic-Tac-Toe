using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{

    public bool AIEnable;
    public bool AIX;
    public int dimensions;
    public int winLenght;

    [HideInInspector]
    public TicTacToeBoard ticTacToeBoard { get; private set; }
    private SymbolManager symbolManager;

    private bool isHumanToMove;

    private bool isAIMoveCalculated;
    private TicTacToeBoard.LegalMove AIMove;

    // Awake is called before start
    private void Awake()
    {
        // Create new TicTacToeBoard board
        ticTacToeBoard = new TicTacToeBoard(dimensions, winLenght);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Finds symbol manger
        symbolManager = FindObjectOfType<SymbolManager>();

        // AI starts the game if AI is x
        if (AIEnable)
        {
            // X always starts to if AI is x then its not human turn to move
            isHumanToMove = !AIX;
            if (AIX)
            {
                MakeAIMove();
            }
        }
        else
        {
            isHumanToMove = true;
        }
    }

    private void Update()
    {
        if (!isHumanToMove)
        {
            if (isAIMoveCalculated)
            {
                TryToMakeAIMove(AIMove.x, AIMove.y);
                FindObjectOfType<AIInfoDisplayManager>().UpdateEval(AIMove.score);
            }
        }
    }

    public void Reset()
    {
        // Deletes old board
        foreach (Transform container in transform)
        {
            foreach (Transform child in container)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        // Create new TicTacToeBoard board
        ticTacToeBoard = new TicTacToeBoard(dimensions, winLenght);
        // Generates the UI elements and started the game
        FindObjectOfType<BoardGenerator>().Start();
        FindObjectOfType<InputManager>().Start();
        FindObjectOfType<SymbolManager>().Start();
        Start();
    }

    public void TryToMakeHumanMove(int x, int y)
    {
        // Only allows to make a move if the game is on
        if (ticTacToeBoard.gameState == TicTacToeBoard.GameState.GameOn)
        {
            // Only allow the human to make a move if its human turn to make a move
            if (isHumanToMove)
            {
                // Only allow to make the move if the space is empty
                if (ticTacToeBoard.boardState[x, y] == TicTacToeBoard.SpaceState.Empty)
                {
                    // Makes the move
                    ticTacToeBoard.MakeMove(x, y);

                    // Update the UI of the ticTacToe board
                    if (!ticTacToeBoard.xToMove)
                    {
                        symbolManager.UpdateSymbol(x, y, TicTacToeBoard.SpaceState.X);
                    }
                    else
                    {
                        symbolManager.UpdateSymbol(x, y, TicTacToeBoard.SpaceState.O);
                    }

                    // If AI is enable the turn will switch and the AI will make a move
                    if (AIEnable)
                    {
                        isAIMoveCalculated = false;
                        isHumanToMove = false;
                        MakeAIMove();
                    }
                }
            }
        }
    }

    public void TryToMakeAIMove(int x, int y)
    {
        // Only allows to make a move if its the AI turn to move
        if (!isHumanToMove)
        {
            // Only allows to make the move if the space is empty
            if (ticTacToeBoard.boardState[x, y] == TicTacToeBoard.SpaceState.Empty)
            {
                // Make the move
                ticTacToeBoard.MakeMove(x, y);

                // Update the UI of the ticTacToe board
                if (!ticTacToeBoard.xToMove)
                {

                    symbolManager.UpdateSymbol(x, y, TicTacToeBoard.SpaceState.X);
                }
                else
                {
                    symbolManager.UpdateSymbol(x, y, TicTacToeBoard.SpaceState.O);
                }

                // Finished the AI move and allows human to make a move
                isHumanToMove = true;
            }
        }
    }

    public void MakeAIMove()
    {
        // Only allows to make a move if the game is on going
        if (ticTacToeBoard.gameState == TicTacToeBoard.GameState.GameOn)
        {
            Task t = new Task(() => MakeMoveUsing_AlphaBateAI());
            t.Start();
        }
    }

    /// <summary>
    /// This option is used to test ai related to output data transfer between threads
    /// </summary>
    public void MakeMoveUsing_GuessingAI()
    {
        TicTacToeBoard.LegalMove AImove = GuessingAI.Guess(ticTacToeBoard.GetAllLegalMoves());

        AIMove = AImove;
        isAIMoveCalculated = true;
    }

    /// <summary>
    /// This funacion caluclatest the best move without alpha beta
    /// </summary>
    public void MakeMoveUsing_MiniMaxAI()
    {
        TicTacToeBoard.LegalMove AImove = MiniMaxAI.FindBestMoveUsingMiniMax(ticTacToeBoard, AIX);
        Debug.Log($"AI best choices is x: {AImove.x}, y: {AImove.y} with evaluation of {AImove.score}");

        AIMove = AImove;
        isAIMoveCalculated = true;
    }

    public void MakeMoveUsing_AlphaBateAI()
    {
        TicTacToeBoard.LegalMove AImove = AlphaBateAI.FindBestMoveUsingAlphaBate(ticTacToeBoard, AIX);
        Debug.Log($"AI best choices is x: {AImove.x}, y: {AImove.y} with evaluation of {AImove.score}");

        AIMove = AImove;
        isAIMoveCalculated = true;
    }
}
