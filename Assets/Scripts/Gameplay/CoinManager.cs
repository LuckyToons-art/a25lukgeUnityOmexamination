using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int totalCoins = 0;
    public TMP_Text coinText; // assign UI text in inspector

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = totalCoins.ToString();
    }
}
