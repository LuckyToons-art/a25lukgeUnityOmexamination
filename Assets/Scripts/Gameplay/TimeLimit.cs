using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeLimit : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 200f;           // Timer starts at 200 seconds
    public float maxTime = 400f;             // Optional max time clamp
    public TMP_Text timerText;

    [Header("Endgame Settings")]
    public GameObject endGamePingPrefab;     // prefab for the ping
    public Vector2 mapCenter = Vector2.zero; 
    public float minTimeToTriggerPing = 300f;

    private float currentTime;
    private bool pingSpawned = false;
    private bool gameOverTriggered = false;

    void Awake()
    {
        currentTime = startTime;
        UpdateText();
    }

    void Update()
    {
        if (gameOverTriggered) return;

        // Countdown timer
        currentTime -= Time.deltaTime;
        if (currentTime < 0f)
            currentTime = 0f;

        UpdateText();

        // Game over if timer reaches 0
        if (currentTime <= 0f)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Add time to the timer (coins/enemy kills)
    /// </summary>
    public void AddTime(float amount)
    {
        if (gameOverTriggered) return;

        currentTime += amount;
        currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
        UpdateText();

        // Spawn endgame ping if threshold reached
        if (!pingSpawned && currentTime >= minTimeToTriggerPing && endGamePingPrefab != null)
        {
            SpawnEndGamePing();
        }
    }

    /// <summary>
    /// Reduce timer when player dashes
    /// </summary>
    public void ConsumeDashTime(float amount = 1f)
    {
        if (gameOverTriggered) return;

        currentTime -= amount;
        if (currentTime < 0f) currentTime = 0f;
        UpdateText();

        if (currentTime <= 0f)
            GameOver();
    }

    void UpdateText()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }

    void SpawnEndGamePing()
    {
        Instantiate(endGamePingPrefab, mapCenter, Quaternion.identity);
        pingSpawned = true;
        Debug.Log("Endgame ping spawned! Reach the center to finish the game.");
    }

    void GameOver()
    {
        gameOverTriggered = true;
        Debug.Log("Time ran out! Game Over!");
        // Replace this with your game over logic:
        SceneManager.LoadScene("GameOverScene");
    }
}
