using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGameOver = false;
    private bool isGameWon = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Hide the game over UI
        UIManager.Instance.HideGameOverUI();
    }

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

        Debug.Log("Game Over!");
    }

    public void GameWon()
    {
        isGameWon = true;

        // Display win UI
        UIManager.Instance.ShowWinUI();

        Debug.Log("You Win!");
    }

    private void ResetScene()
    {
        // Reset the scene
        SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
