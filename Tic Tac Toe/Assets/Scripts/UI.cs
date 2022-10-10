using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.UI
{
    public class UI : MonoBehaviour
    {
        // Coaches the target image
        private Image image;

        private void Start()
        {
            image = this.GetComponent<Image>();
        }

        /// <summary>
        /// Assigned to the restart button, start new game and swaps starting player
        /// </summary>
        public void Restart()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.AIX = !gameManager.AIX;
            gameManager.Reset();
        }

        /// <summary>
        /// Quits the application
        /// </summary>
        public void Exit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Changes color of the button
        /// </summary>
        public void CangeColor()
        {
            image.color = new Color(Random.value, Random.value, Random.value, 1);
        }
    }
}
