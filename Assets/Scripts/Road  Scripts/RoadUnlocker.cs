using UnityEngine;

public class RoadUnlocker : MonoBehaviour
{
    public GameObject hiddenRoad;

    private bool isUnlocked = false;

    public void UnlockRoad()
    {
        if (isUnlocked) return;

        isUnlocked = true;

        if (hiddenRoad != null)
        {
            hiddenRoad.SetActive(true);
            Debug.Log("Road Unlocked!");
        }
    }
}