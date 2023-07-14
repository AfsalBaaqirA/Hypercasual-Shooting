using System.IO;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [SerializeField] private string mediaExportPath = "AdsMedia"; // Path to export captured media
    [SerializeField] private Camera captureCamera; // Camera used for capturing screenshots or recording videos
    [SerializeField] private int captureThreshold = 10; // Number of seconds between each capture
    [SerializeField] private string pickedUpMorePowerfulWeapon = "Launcher"; // Name of the more powerful weapon
    private const int largeKillCountThreshold = 5;
    private const float fewSecondsThreshold = 5f;
    public int CaptureThreshold => captureThreshold;
    public string PickedUpMorePowerfulWeapon => pickedUpMorePowerfulWeapon;
    private float timeElapsed = 0f;
    private int enemyKillCount = 0;
    private PlayerController player;

    private bool enemyOnPlayerDeath = false;

    public int EnemyKillCount
    {
        get => enemyKillCount;
        set
        {
            enemyKillCount = value;
            timeElapsed = 0f;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        player = PlayerController.Instance;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameInProgress())
            return;

        // Check if the player has killed a certain number of enemies in a short period of time
        if (enemyKillCount >= largeKillCountThreshold && timeElapsed <= fewSecondsThreshold)
        {
            // Capture screenshot
            CaptureScreenshot();
            enemyKillCount = 0;
        }

        // Increment timeElapsed
        timeElapsed += Time.deltaTime;
    }

    // Method to capture screenshots of specific moments
    public void CaptureScreenshot()
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"Screenshot_{timestamp}.png";
        string filePath = Path.Combine(mediaExportPath, fileName);

        // Create the directory if it doesn't exist
        Directory.CreateDirectory(mediaExportPath);

        // Capture screenshot
        ScreenCapture.CaptureScreenshot(filePath);
    }

    // Method to trigger the capture process for specific moments
    public void TriggerEnemyOnPlayerDeath()
    {
        if (enemyOnPlayerDeath)
            return;
        // Capture screenshot
        CaptureScreenshot();
        enemyOnPlayerDeath = true;
    }
}
