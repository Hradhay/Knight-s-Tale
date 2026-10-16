using UnityEngine;

public class Cutscene2Trigger : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            GameController.instance.PlayCutscene2();
        }
    }
}