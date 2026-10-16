using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    public RoadUnlocker roadUnlocker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roadUnlocker.UnlockRoad();
        }
    }
}