using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    public float timeBonus = 20f;
    public AudioClip collectSound;
    public GameObject collectEffect;

    private AudioSource audioSource;
    private TimeLimit timeLimit;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();

        timeLimit = FindObjectOfType<TimeLimit>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PixelDashMovement2D>() != null)
            Collect();
    }

    void Collect()
    {
        if (collectSound != null) audioSource.PlayOneShot(collectSound);
        if (collectEffect != null) Instantiate(collectEffect, transform.position, Quaternion.identity);

        timeLimit?.AddTime(timeBonus);

        Destroy(gameObject);
    }
}
