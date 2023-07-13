using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private AudioClip musicClip;

    public GameState GameState { get; private set; }

    private int playerCoins;
    private string weaponName;

    public int PlayerCoins
    {
        get { return playerCoins; }
        set
        {
            playerCoins = value;
            UIManager.Instance.UpdateCoinsUI(playerCoins);
        }
    }

    public string WeaponName
    {
        get { return weaponName; }
        set
        {
            weaponName = value;
            UIManager.Instance.UpdateWeaponNameUI(weaponName);
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Set default values
        PlayerCoins = 0;
        WeaponName = "No Weapon";
    }

    private void Start()
    {
        GameState = GameState.Started;
        AudioManager.Instance.PlayMusic(musicClip);
    }

    public void GameOver()
    {
        GameState = GameState.Over;

        // Display game over UI
        UIManager.Instance.ShowGameOverUI();
    }

    public void GameWon()
    {
        GameState = GameState.Won;

        // Display win UI
        UIManager.Instance.ShowWinUI();
    }

    public void RestartGame()
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