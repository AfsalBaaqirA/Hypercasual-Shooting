using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private AudioClip musicClip;

    public GameState GameState { get; private set; }
    public GameSettings gameSettings { get; internal set; }

    private int playerCoins = 0;
    private string weaponName = "No Weapon";

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
    }

    private void Start()
    {
        gameSettings = new GameSettings();
        gameSettings.isMusicOn = ClientPrefs.GetMusicToggle();
        gameSettings.isSoundOn = ClientPrefs.GetSoundEffectsToggle();
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

public class GameSettings
{
    public bool isMusicOn;
    public bool isSoundOn;
}