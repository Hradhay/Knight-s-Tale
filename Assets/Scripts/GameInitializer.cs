using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.ResetStats();

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.ResetInventory();
    }
}