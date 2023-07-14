using System.IO;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [SerializeField] private string mediaExportPath = "ScreenShots";
    [SerializeField] private Camera captureCamera;
    [SerializeField] private int captureThreshold = 10;
    [SerializeField] private string morePowerfulWeapon = "Launcher";
    private const int largeKillCountThreshold = 5;
    private const float fewSecondsThreshold = 5f;

    public int CaptureThreshold => captureThreshold;
    public string MorePowerfulWeapon => morePowerfulWeapon;

    private float timeElapsed;
    private int enemyKillCount;
    private PlayerController player;
    private bool isPlayerDeathCaptured;

    private void Awake()
    {
        Instance = this;
        timeElapsed = 0f;
        enemyKillCount = 0;
        isPlayerDeathCaptured = false;
    }

    private void Start()
    {
        player = PlayerController.Instance;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameInProgress())
            return;

        if (enemyKillCount >= largeKillCountThreshold && timeElapsed <= fewSecondsThreshold)
        {
            CaptureScreenshot();
            enemyKillCount = 0;
        }

        timeElapsed += Time.deltaTime;
    }

    public void CaptureScreenshot()
    {
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"Screenshot_{timestamp}.png";
        string filePath = Path.Combine(mediaExportPath, fileName);
        Directory.CreateDirectory(mediaExportPath);
        ScreenCapture.CaptureScreenshot(filePath);
    }

    public void TriggerEnemyOnPlayerDeath()
    {
        if (isPlayerDeathCaptured)
            return;

        CaptureScreenshot();
        isPlayerDeathCaptured = true;
    }

    public void IncreaseEnemyKillCount()
    {
        enemyKillCount++;
    }
}
