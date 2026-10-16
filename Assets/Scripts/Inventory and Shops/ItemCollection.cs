using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    private AudioSource audioSource;
    private CircleCollider2D itemCollider;
    private SpriteRenderer sr;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        itemCollider = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.Play();
            itemCollider.enabled = false;
            sr.enabled = false;
            
        }
    }
}   
