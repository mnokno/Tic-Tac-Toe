using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{

    public BoardUISettings settings;
    public Color boardClolor;
    public Transform container;
    public Sprite square;

    private float scale;

    // Start is called before the first frame update
    public void Start()
    {
        // Calculates scale
        scale = (settings.symbolSize * settings.dimensions) + (settings.lineWidth + settings.paddingLength * 2) * (settings.dimensions - 1);

        // Generates the board
        GenerateBoard();
    }    
    
    private void GenerateBoard()
    {
        // Generate vertical lines
        for (int i = 0; i < settings.dimensions - 1; i++)
        {
            // Each line is generated as a rectangle
            GameObject currentLine = new GameObject($"Vertical line {i+1}");
            currentLine.transform.parent = container;
            
            // Adds the rectangle
            SpriteRenderer spriteRenderer = currentLine.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = square;
            spriteRenderer.color = boardClolor;

            // Set correct location
            float xPos = -scale / 2f + (settings.symbolSize * (i + 1)) + (settings.paddingLength * (1 + 2 * i)) + (settings.lineWidth / 2f) + (settings.lineWidth * i);
            currentLine.transform.position = new Vector3(xPos, 0, 0);

            // Set correct scale
            currentLine.transform.localScale = new Vector3(settings.lineWidth, scale, 1);
        }

        // Generate horizontal lines
        for (int i = 0; i < settings.dimensions - 1; i++)
        {
            // Each line is generated as a rectangle
            GameObject currentLine = new GameObject($"Horizontal line {i+1}");
            currentLine.transform.parent = container;

            // Adds the rectangle
            SpriteRenderer spriteRenderer = currentLine.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = square;
            spriteRenderer.color = boardClolor;

            // Set correct location
            float yPos = -scale / 2f + (settings.symbolSize * (i + 1)) + (settings.paddingLength * (1 + 2 * i)) + (settings.lineWidth / 2f) + (settings.lineWidth * i);
            currentLine.transform.position = new Vector3(0, yPos, 0);

            // Set correct scale
            currentLine.transform.localScale = new Vector3(scale, settings.lineWidth, 1);
        }
    }

    [System.Serializable]
    public struct BoardUISettings
    {
        public int dimensions;
        public float symbolSize;
        public float lineWidth;
        public float paddingLength;
    }
}
