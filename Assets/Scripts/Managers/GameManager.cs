using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGameOver = false;
    private bool isGameWon = false;

    private void Update()
    {
        if (isGameOver || isGameWon)
        {
            // Check for input to reset the scene
            if (Input.GetMouseButtonDown(0))
            {
                ResetScene();
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // Display game over UI
        UIManager.Instance.ShowGameOverUI();
    }

    public void GameWon()
    {
        isGameWon = true;

        // Display win UI
        UIManager.Instance.ShowWinUI();
    }

    private void ResetScene()
    {
        // Reset the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
