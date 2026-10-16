using UnityEngine;

public class QuestSound : MonoBehaviour 
{
    public static QuestSound Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip acceptClip;
    [SerializeField] private AudioClip declineClip;
    [SerializeField] private AudioClip completeClip;

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

    public void PlayAcceptSound() 
    {
        PlayClip(acceptClip);
    }

    public void PlayDeclineSound() 
    {
        PlayClip(declineClip);
    }

    public void PlayCompleteSound() 
    {
        PlayClip(completeClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
