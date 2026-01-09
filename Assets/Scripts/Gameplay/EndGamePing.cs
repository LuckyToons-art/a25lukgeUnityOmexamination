using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePing : MonoBehaviour
{
    [Header("Visuals")]
    public float rotationSpeed = 100f;

    [Header("Audio")]
    public AudioClip spawnSound;   // plays when ping appears
    public AudioClip collectSound; // plays when player reaches the ping
    private AudioSource audioSource;

    private bool soundPlayed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        if (spawnSound != null && !soundPlayed)
        {
            audioSource.PlayOneShot(spawnSound);
            soundPlayed = true;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PixelDashMovement2D>() != null)
        {
            if (collectSound != null)
                audioSource.PlayOneShot(collectSound);

            Debug.Log("Player reached the center! Game Over!");
            SceneManager.LoadScene("GameOverScene"); // replace with your scene
        }
    }
}
