using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Transform container;
    public Sprite square;

    private GameManager gameManager;
    private BoardGenerator.BoardUISettings settings;
    private float scale;
    private GameObject[,] buttons;

    // Start is called before the first frame update
    public void Start()
    {
        // Finds settings and gameManager
        settings = FindObjectOfType<BoardGenerator>().settings;
        gameManager = FindObjectOfType<GameManager>();

        // Initializes symbols array
        buttons = new GameObject[settings.dimensions, settings.dimensions];
        // Calculates scale
        scale = (settings.symbolSize * settings.dimensions) + (settings.lineWidth + settings.paddingLength * 2) * (settings.dimensions - 1);

        // Initializes all buttons
        InitiateButtons();
    }

    public void HandleButtonPressed(int x, int y)
    {
        // This function should be called by button that is pressed
        gameManager.TryToMakeHumanMove(x, y);
        //Debug.Log(gameManager.ticTacToeBoard.gameState);
    }

    public void InitiateButtons()
    {
        // Initializes all button as invisible sprite of a rectangle
        for (int x = 0; x < settings.dimensions; x++)
        {
            for (int y = 0; y < settings.dimensions; y++)
            {
                // Create the game object
                GameObject button = new GameObject($"Button x: {x}, y: {y}");

                // Set parent
                button.transform.parent = container;

                // Add BoxCollider2D
                button.AddComponent<BoxCollider2D>();

                // Add sprite render
                SpriteRenderer spriteRenderer = button.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = square;
                spriteRenderer.enabled = false;

                // Set location
                float xPos = (-scale / 2f) + ((settings.symbolSize * (x + 0.5f)) + ((settings.lineWidth + (2 * settings.paddingLength)) * x));
                float yPos = (-scale / 2f) + ((settings.symbolSize * (y + 0.5f)) + ((settings.lineWidth + (2 * settings.paddingLength)) * y));
                button.transform.localPosition = new Vector3(xPos, yPos, 0);

                // Set scale
                float buttonScale = settings.paddingLength * 2 + settings.symbolSize;
                button.transform.localScale = new Vector3(buttonScale, buttonScale, 1);

                // Add button handler
                ButtonHandler buttonHandler = button.AddComponent<ButtonHandler>();
                buttonHandler.x = x;
                buttonHandler.y = y;
                buttonHandler.inputManager = this;

                // Add the button to the buttons array
                buttons[x, y] = button;

            }
        }
    }

}
