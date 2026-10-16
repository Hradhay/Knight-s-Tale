using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public  StatsUI statsUI;
    public TMP_Text healthText;

[Header("Combat Stats")]
    public int damage = 1;
    
    public float weaponRange = 1f;
    
    public float knockbackForce = 10f;
    
    public float knockbackTime = 0.15f;
    public float stunTime = 0.3f;

    [Header("Movement Stats")]
    public float speed = 5f;

    [Header("Health Stats")]
    public int currentHealth = 100;
    public int maxHealth = 100;

    // Static fallback properties
    public static float Speed => Instance?.speed ?? 5f;
    public static int Damage => Instance?.damage ?? 1;
    public static float WeaponRange => Instance?.weaponRange ?? 1f;
    public static float KnockbackForce => Instance?.knockbackForce ?? 2f;
    public static float KnockbackTime => Instance?.knockbackTime ?? 0.15f;
    public static float StunTime => Instance?.stunTime ?? 0.3f;
    public static int CurrentHealth => Instance?.currentHealth ?? 100;
    public static int MaxHealth => Instance?.maxHealth ?? 100;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        statsUI = FindFirstObjectByType<StatsUI>();
        if (healthText == null)
        {
            var texts = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
            foreach (var t in texts)
            {
                if (t != null && t.text.Contains("HP:"))
                {
                    healthText = t;
                    break;
                }
            }
        }
    }
    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
        if (healthText != null)
            healthText.text = "HP:" + currentHealth +"/ "+maxHealth;
    }
    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthText != null)
            healthText.text = "HP:" + currentHealth +"/"+maxHealth;
    }
    public void UpdateSpeed(int amount)
    {
        if (Instance != null)
        {
            speed += amount;
            if (statsUI != null)
                statsUI.UpdateAllStats();
        }
        else
        {
            Debug.LogWarning("StatsManager.Instance is null in UpdateSpeed!");
        }
    }

    public void ResetStats()
    {
        // Reset to defaults
        maxHealth = 100;
        currentHealth = maxHealth;
        damage = 1;
        speed = 5f;
        weaponRange = 1f;
        knockbackForce = 2.7f;
        knockbackTime = 0.15f;
        stunTime = 0.3f;
        
        // Update UI
        if (healthText != null)
            healthText.text = "HP:" + currentHealth + "/" + maxHealth;
        
        // Update stats UI if available
        if (statsUI != null)
            statsUI.UpdateAllStats();
    }
}
