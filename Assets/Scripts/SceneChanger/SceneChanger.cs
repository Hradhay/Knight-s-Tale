using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneChanger : MonoBehaviour
{
        public string sceneToLoad;
        public Animator fadeAnim;
        public float fadeTime = .5f;
        public Vector2 newPlayerPosition;
        private Transform player;
        private bool isTeleporting = false; // Flag to prevent multiple triggers

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isTeleporting && collision.CompareTag("Player"))
            {
                player = collision.transform;
                isTeleporting = true; // Set flag to prevent multiple calls
                Debug.Log($"Teleport triggered to scene: {sceneToLoad}"); // Confirmation log
                
                fadeAnim.Play("FadeToWhite");
                StartCoroutine(DelayFade());
            }
        }

        IEnumerator DelayFade()
        {
            yield return new WaitForSeconds(fadeTime);
            
            // Null check for player before repositioning
            if (player != null)
            {
                player.position = newPlayerPosition;
            }
            
            SceneManager.LoadScene(sceneToLoad);
        }
}
