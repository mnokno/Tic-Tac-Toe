using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public InputManager inputManager;
    public int x;
    public int y;
      
    private void OnMouseDown()
    {
        inputManager.HandleButtonPressed(x, y);
    }
}
