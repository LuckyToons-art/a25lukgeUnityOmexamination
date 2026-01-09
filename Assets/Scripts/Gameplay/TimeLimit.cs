using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeLimit : MonoBehaviour
{
    [Header("Timer Settings")]
    public float maxTime = 300f;
    public TMP_Text timerText;

    [Header("Endgame Settings")]
    public GameObject endGamePingPrefab;  // prefab for the ping
    public Vector2 mapCenter = Vector2.zero;
    public float minTimeToTriggerPing = 300f;

    private float currentTime;
    private bool pingSpawned = false;

    void Awake()
    {
        currentTime = 0f;
        UpdateText();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0f)
            currentTime = 0f;

        UpdateText();
    }

    public void AddTime(float amount)
    {
        currentTime += amount;
        currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
        UpdateText();

        // Spawn endgame ping if enough time
        if (!pingSpawned && currentTime >= minTimeToTriggerPing && endGamePingPrefab != null)
        {
            SpawnEndGamePing();
        }
    }

    public void ConsumeDashTime(float amount = 1f)
    {
        currentTime -= amount;
        if (currentTime < 0f) currentTime = 0f;
        UpdateText();
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
}
