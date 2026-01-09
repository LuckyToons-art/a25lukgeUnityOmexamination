using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text loreText;            // Assign your TextMeshProUGUI component here
    public string[] loreLines;           // Array of lines to show
    public string nextSceneName = "SampleScene"; // Scene name exactly as in Build Settings
    public float typingSpeed = 0.03f;    // Speed of typing effect

    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        if (loreText == null)
        {
            Debug.LogError("Assign a TextMeshProUGUI component to loreText!");
            return;
        }

        ShowNextLine();
    }

    void Update()
    {
        // Wait for player input to advance
        if (Input.anyKeyDown)
        {
            if (isTyping)
            {
                // Complete the current line instantly
                loreText.text = loreLines[currentLine];
                isTyping = false;
            }
            else
            {
                currentLine++;
                if (currentLine < loreLines.Length)
                {
                    ShowNextLine();
                }
                else
                {
                    // All lines shown → load main game scene
                    SceneManager.LoadScene(nextSceneName);
                }
            }
        }
    }

    void ShowNextLine()
    {
        StartCoroutine(TypeLine(loreLines[currentLine]));
    }

    System.Collections.IEnumerator TypeLine(string line)
    {
        isTyping = true;
        loreText.text = "";
        foreach (char c in line)
        {
            loreText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}
