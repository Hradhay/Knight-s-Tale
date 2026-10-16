using UnityEngine;

public class ShopSound : MonoBehaviour 
{
    public static ShopSound Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip transactionClip;

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        } 
        else 
        {
            Destroy(gameObject);
        }
    }

    public void PlayTransactionSound() 
    {
        if (audioSource != null && transactionClip != null)
        {
            audioSource.PlayOneShot(transactionClip);
        }
    }
}
