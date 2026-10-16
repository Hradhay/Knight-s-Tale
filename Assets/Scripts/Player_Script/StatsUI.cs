using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
        public GameObject[] statsSlots;
        public CanvasGroup statsCanvas;

        private bool statsOpen = false;


        private void Start()
        {
            UpdateAllStats();
        }


        private void Update()
        {
        if (Input.GetButtonDown("ToggleStats"))
        {
            if (statsOpen)
            {
                Time.timeScale = 1f; // Resume the game when stats are closed
                UpdateAllStats(); // Update stats display when closing the stats UI
                statsCanvas.alpha = 0f;
                statsCanvas.blocksRaycasts = false;
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0f; // Pause the game when stats are open
                UpdateAllStats(); // Update stats display when opening the stats UI
                statsCanvas.alpha = 1f;
                statsCanvas.blocksRaycasts = true;
                statsOpen = true;
            }
        }
        }



        public void UpdateDamage()
    {
        statsSlots[0].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Damage: " + StatsManager.Instance.damage;
    }

         public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Speed: " + StatsManager.Instance.speed;
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
    }
}
