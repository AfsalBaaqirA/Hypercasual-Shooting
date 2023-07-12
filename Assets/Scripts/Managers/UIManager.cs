using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    public void ShowWinUI()
    {
        winUI.SetActive(true);
    }
}
