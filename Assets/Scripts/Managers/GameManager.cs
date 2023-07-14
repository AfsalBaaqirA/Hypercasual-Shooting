using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private float killPlaneY = -10f;

    public GameState GameState { get; private set; }

    private int playerCoins = 0;
    private string weaponName = "No Weapon";

    public int PlayerCoins
    {
        get => playerCoins;
        set
        {
            playerCoins = value;
            UIManager.Instance.UpdateCoinsUI(playerCoins);
        }
    }

    public string WeaponName
    {
        get => weaponName;
        set
        {
            weaponName = value;
            UIManager.Instance.UpdateWeaponNameUI(weaponName);
        }
    }

    public float KillPlaneY => killPlaneY;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GameState = GameState.Started;
        AudioManager.Instance.PlayMusic(musicClip);
    }

    private void Update()
    {
        if (GameState == GameState.Started && PlayerController.Instance.transform.position.y < killPlaneY)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        GameState = GameState.Over;
        UIManager.Instance.ShowGameOverUI();
    }

    public void GameWon()
    {
        GameState = GameState.Won;
        UIManager.Instance.ShowWinUI();
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public bool IsGameInProgress() => GameState == GameState.Started;
}

public enum GameState
{
    Started,
    Over,
    Won
}
