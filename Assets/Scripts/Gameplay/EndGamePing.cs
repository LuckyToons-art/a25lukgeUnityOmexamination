using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePing : MonoBehaviour
{
    [Header("Visuals")]
    public float rotationSpeed = 100f;

    [Header("Audio")]
    public AudioClip spawnSound; // plays when ping appears
    public AudioClip collectSound; // plays when player reaches the ping
    private AudioSource audioSource;

    private bool soundPlayed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        // Play spawn sound when ping appears
        if (spawnSound != null && !soundPlayed)
        {
            audioSource.PlayOneShot(spawnSound);
            soundPlayed = true;
        }
    }

    void Update()
    {
        // Rotate for visual effect
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player touched the ping
        if (collision.GetComponent<PixelDashMovement2D>() != null)
        {
            // Play collect sound
            if (collectSound != null)
                audioSource.PlayOneShot(collectSound);

            Debug.Log("Player reached the center! Game Over!");

            // Replace this with your game over logic
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
