using UnityEngine;

public class SpriteAnimator2D : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 10f;
    public bool loop = true;

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                    currentFrame = 0;
                else
                    currentFrame = frames.Length - 1;
            }

            spriteRenderer.sprite = frames[currentFrame];
        }
    }
}