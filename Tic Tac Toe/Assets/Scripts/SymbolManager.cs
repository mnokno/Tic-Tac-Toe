using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolManager : MonoBehaviour
{
    public Sprite xSprite;
    public Sprite oSprite;
    public Sprite noneSprite;

    public Transform container;
    [SerializeField] private Image xColorRef;
    [SerializeField] private Image oColorRef;

    private GameManager gameManager;
    private BoardGenerator.BoardUISettings settings;
    private GameObject[,] symbols;
    private float scale;

    // Start is called before the first frame update
    public void Start()
    {
        // Finds settings and gameManager
        settings = FindObjectOfType<BoardGenerator>().settings;
        gameManager = FindObjectOfType<GameManager>();

        // Initializes symbols array
        symbols = new GameObject[settings.dimensions, settings.dimensions];
        // Calculates scale
        scale = (settings.symbolSize * settings.dimensions) + (settings.lineWidth + settings.paddingLength * 2) * (settings.dimensions - 1);

        // Imitate all symbols
        InitiateSymbols();
    }

    public void InitiateSymbols()
    {
        // Initiate all symbols as noneSprite
        for (int x = 0; x < settings.dimensions; x++)
        {
            for (int y = 0; y < settings.dimensions; y++)
            {
                // Create game object
                GameObject symbol = new GameObject($"Symbol x: {x}, y: {y}");
                symbol.transform.parent = container;

                // Add sprite renderer
                SpriteRenderer spriteRenderer = symbol.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = noneSprite;

                // Set correct scale
                float symbolScale = settings.symbolSize / 1.7f;
                symbol.transform.localScale = new Vector3(symbolScale, symbolScale, 0);

                // Set correct location
                float xPos = (-scale / 2f) + ((settings.symbolSize * (x + 0.5f)) + ((settings.lineWidth + (2 * settings.paddingLength)) * x));
                float yPos = (-scale / 2f) + ((settings.symbolSize * (y + 0.5f)) + ((settings.lineWidth + (2 * settings.paddingLength)) * y));
                symbol.transform.localPosition = new Vector3(xPos, yPos, 0);

                // Add the symbol to the symbols array
                symbols[x, y] = symbol;
            }
        }
    }

    public void UpdateBoard()
    {
        // Update each symbols base on the current board state
        TicTacToeBoard.SpaceState[,] boardState = gameManager.ticTacToeBoard.GetCoppyOfBoardState();
        
        for (int x = 0; x < gameManager.dimensions; x++)
        {
            for (int y = 0; y < gameManager.dimensions; y++)
            {
                // Update the symbol
                if (boardState[x, y] == TicTacToeBoard.SpaceState.Empty)
                {
                    symbols[x, y].GetComponent<SpriteRenderer>().sprite = noneSprite;
                }
                else if (boardState[x, y] == TicTacToeBoard.SpaceState.X)
                {
                    symbols[x, y].GetComponent<SpriteRenderer>().sprite = xSprite;
                    symbols[x, y].GetComponent<SpriteRenderer>().color = xColorRef.color;
                }
                else if (boardState[x, y] == TicTacToeBoard.SpaceState.O)
                {
                    symbols[x, y].GetComponent<SpriteRenderer>().sprite = oSprite;
                    symbols[x, y].GetComponent<SpriteRenderer>().color = oColorRef.color;
                }
            }
        }
        
    }

    public void UpdateSymbol(int x, int y, TicTacToeBoard.SpaceState to)
    {
        // Check if the passed ordinates are valid
        if (x < settings.dimensions && y < settings.dimensions) 
        {
            // Change the symbol
            if (to == TicTacToeBoard.SpaceState.Empty)
            {
                symbols[x, y].GetComponent<SpriteRenderer>().sprite = noneSprite;
            }
            else if (to == TicTacToeBoard.SpaceState.X)
            {
                symbols[x, y].GetComponent<SpriteRenderer>().sprite = xSprite;
                symbols[x, y].GetComponent<SpriteRenderer>().color = xColorRef.color;
            }
            else if (to == TicTacToeBoard.SpaceState.O)
            {
                symbols[x, y].GetComponent<SpriteRenderer>().sprite = oSprite;
                symbols[x, y].GetComponent<SpriteRenderer>().color = oColorRef.color;
            }
            
        }
        else
        {
            // Raises Error incorrect dimension
            Debug.LogError($"Row: {x}, Column: {y} is outside the bound of the board. Board dimensions : {settings.dimensions}");
        }
    }
}
