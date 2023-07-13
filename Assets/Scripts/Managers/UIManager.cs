using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private GameObject gamePausedUI;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI gameOverCoinsText;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip weaponChangeSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ShowUI(hudUI);
        coinsText.text = GameManager.Instance.PlayerCoins.ToString();
        weaponNameText.text = GameManager.Instance.WeaponName;
    }

    public void ShowGameOverUI()
    {
        HideUI(hudUI);
        gameOverText.text = "You Lose!";
        gameOverCoinsText.text = GameManager.Instance.PlayerCoins.ToString();
        ShowUI(gameOverUI);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySoundEffect(gameOverSound);
    }

    public void ShowWinUI()
    {
        HideUI(hudUI);
        gameOverText.text = "You Win!";
        gameOverCoinsText.text = GameManager.Instance.PlayerCoins.ToString();
        ShowUI(gameOverUI);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySoundEffect(winSound);
    }

    public void ShowUI(GameObject ui)
    {
        HideAllUI();
        ui.SetActive(true);
    }

    public void HideUI(GameObject ui)
    {
        ui.SetActive(false);
    }

    public void HideAllUI()
    {
        HideUI(gameOverUI);
        HideUI(hudUI);
        HideUI(gamePausedUI);
    }

    public void UpdateCoinsUI(int coins)
    {
        AudioManager.Instance.PlaySoundEffect(coinSound);
        coinsText.text = coins.ToString();
    }

    public void UpdateWeaponNameUI(string weaponName)
    {
        AudioManager.Instance.PlaySoundEffect(weaponChangeSound);
        weaponNameText.text = weaponName;
    }

    public void OnRestartButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffect(buttonClickSound);
        GameManager.Instance.RestartGame();
    }

    public void OnQuitButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffect(buttonClickSound);
        GameManager.Instance.QuitGame();
    }

    public void OnPlayButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffect(buttonClickSound);
        // GameManager.Instance.StartGame();
    }

    public void OnPauseButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffect(buttonClickSound);
        GameManager.Instance.PauseGame();
        ShowUI(gamePausedUI);
    }

    public void OnResumeButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffect(buttonClickSound);
        GameManager.Instance.ResumeGame();
        ShowUI(hudUI);
    }

    public void OnMainMenuButtonClicked()
    {
        AudioManager.Instance.PlaySoundEffect(buttonClickSound);
        // GameManager.Instance.LoadMainMenu();
    }
}
