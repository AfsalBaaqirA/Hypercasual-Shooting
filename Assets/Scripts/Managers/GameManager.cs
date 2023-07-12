using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState GameState { get; private set; }

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

    public void GameOver()
    {
        GameState = GameState.Over;

        // Display game over UI
        UIManager.Instance.ShowGameOverUI();

        Debug.Log("Game Over!");
    }

    public void GameWon()
    {
        GameState = GameState.Won;

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

public enum GameState
{
    Started,
    Over,
    Won
}
