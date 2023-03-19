# Tic-Tac-Toe
Tic Tac Toe game implementation where the player can play against an AI that uses a Minimax algorithm with Alpha-Beta Pruning to determine its moves.

## Minimax Algorithm with Alpha-Beta Pruning
The Minimax algorithm is a decision-making algorithm commonly used in two-player games. It searches through all possible future moves of the game and chooses the move that leads to the best outcome for the player, assuming that the other player is also making the best possible moves.

However, searching through all possible moves can be very time-consuming, especially for games with a large number of possible moves. This is where Alpha-Beta Pruning comes in. Alpha-Beta Pruning is an optimization technique that reduces the number of nodes searched by the Minimax algorithm by eliminating branches of the game tree that are guaranteed to not lead to the optimal solution.

In the Tic Tac Toe game, the game tree is relatively small and simple, so the benefits of Alpha-Beta Pruning may not be as noticeable as in more complex games. However, it still serves as a good example of how Alpha-Beta Pruning can be used to optimize Minimax.
