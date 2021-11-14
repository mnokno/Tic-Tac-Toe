using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuessingAI
{
    public static TicTacToeBoard.LegalMove Guess(List<TicTacToeBoard.LegalMove> legalMoves)
    {
        // Plays a random move
        return legalMoves[Random.Range(0, legalMoves.Count)];
    }
}
