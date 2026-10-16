using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip walkingMusic;

    [SerializeField] private AudioClip attackClip;

    private Rigidbody2D rb;
    private Animator animator;
    private bool wasAttacking = false;
    private float movementThreshold = 0.1f;
    private bool wasDead = false;
    private float savedDefaultTime = 0f;
    private float savedDefaultTimeWalking = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (defaultMusic != null)
        {
            audioSource.clip = defaultMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (rb == null || audioSource == null) return;

        // Attack sound
        if (animator != null)
        {
            bool isAttacking = animator.GetBool("isAttacking");
            if (isAttacking && !wasAttacking)
            {
                if (attackClip != null)
                {
                    audioSource.PlayOneShot(attackClip);
                }
            }
            wasAttacking = isAttacking;
        }

        // Death check for default music pause/resume
        bool isDead = StatsManager.Instance == null ? false : StatsManager.Instance.currentHealth <= 0;

        if (isDead)
        {
            if (!wasDead && audioSource.isPlaying && audioSource.clip == defaultMusic)
            {
                savedDefaultTime = audioSource.time;
                audioSource.Stop();
            }
            wasDead = true;
        }
        else if (wasDead)
        {
            wasDead = false;
            if (defaultMusic != null && audioSource.clip != defaultMusic)
            {
                audioSource.Stop();
                audioSource.clip = defaultMusic;
                audioSource.loop = true;
                audioSource.time = savedDefaultTime;
                audioSource.Play();
            }
        }

        // Movement speed
        float speed = rb.linearVelocity.magnitude;

        // Music switching logic (default/walking only)
        AudioClip targetClip = null;
        bool targetLoop = true;

        if (speed > movementThreshold)
        {
            if (walkingMusic != null)
            {
                targetClip = walkingMusic;
            }
            else if (defaultMusic != null)
            {
                targetClip = defaultMusic;
            }
        }
        else if (defaultMusic != null)
        {
            targetClip = defaultMusic;
        }

        if (targetClip != null)
        {
            if (audioSource.clip != targetClip || audioSource.loop != targetLoop)
            {
                // Save time if switching from default to walking or vice versa
                if (audioSource.clip == defaultMusic)
                {
                    savedDefaultTimeWalking = audioSource.time;
                }
                else if (audioSource.clip == walkingMusic)
                {
                    savedDefaultTimeWalking = audioSource.time;
                }
                audioSource.Stop();
                audioSource.clip = targetClip;
                audioSource.loop = targetLoop;

                // Resume from saved time if switching to default music
                if (targetClip == defaultMusic)
                {
                    audioSource.time = savedDefaultTimeWalking;
                }
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
