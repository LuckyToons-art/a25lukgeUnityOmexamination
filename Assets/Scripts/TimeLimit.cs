using UnityEngine;
using TMPro;

public class TimeLimit : MonoBehaviour
{
    [Header("Time Settings")]
    public float maxTime = 30f;
    public float dashTimeCost = 2f;

    [Header("Text")]
    public TMP_Text timerText;

    float currentTime;
    bool active = true;

    void Start()
    {
        currentTime = maxTime;

        if (!timerText)
            Debug.LogError("TimeLimit: TimerText not assigned!");

        UpdateText();
    }

    void Update()
    {
        if (!active) return;

        currentTime -= Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0f, maxTime);

        UpdateText();

        if (currentTime <= 0f)
            TimeUp();
    }

    public void ConsumeDashTime()
    {
        currentTime -= dashTimeCost;
        currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
        UpdateText();
    }

    void UpdateText()
    {
        if (!timerText) return;

        timerText.text = Mathf.CeilToInt(currentTime).ToString();

        // Color warning when low
        if (currentTime <= maxTime * 0.25f)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;
    }

    void TimeUp()
    {
        active = false;
        timerText.text = "0";
        Debug.Log("Time's up!");
    }
}
