using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    // public int currentHealth = 5;
    // public int maxHealth = 100;
    
    public TMP_Text healthText;
    private Camera mainCamera;
    private Rect originalViewportRect;
    public bool isDead = false;

    public void Start()
    {
        mainCamera = Camera.main;
        
        // Store the original viewport rect and ensure it's set correctly
        if (mainCamera != null)
        {
            originalViewportRect = mainCamera.rect;
            
            // Force the viewport rect to be (0,0,1,1) to avoid out of frustum errors
            mainCamera.rect = new Rect(0, 0, 1, 1);
        }
        
        if (healthText != null)
            healthText.text = "HP: " + StatsManager.Instance.currentHealth.ToString() + "/" + StatsManager.Instance.maxHealth.ToString();
    }

    public void OnDestroy()
    {
        // Restore original viewport when destroyed
        if (mainCamera != null)
        {
            mainCamera.rect = originalViewportRect;
        }
    }

    // FIX: Removed Update and ClampToViewport - they were incorrectly hiding the health text

    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        StatsManager.Instance.currentHealth = Mathf.Clamp(StatsManager.Instance.currentHealth, 0, StatsManager.Instance.maxHealth);

        // FIX: Added null check for healthText
        if (healthText != null)
            healthText.text = "HP: " + StatsManager.Instance.currentHealth.ToString() + "/" + StatsManager.Instance.maxHealth.ToString();

        if (StatsManager.Instance.currentHealth <= 0  && !isDead)
        {
            // gameObject.SetActive(false);
            isDead = true;

    Debug.Log("Player Died");

    // Stop player actions safely
    DisablePlayerLogic();

    // Trigger cutscene
    GameController.instance.PlayCutscene3();
        }
    }

    void DisablePlayerLogic()
{
    // Disable movement script
    PlayerMovement movement = GetComponent<PlayerMovement>();
    if (movement != null)
        movement.enabled = false;

    // Disable collider (optional)
    Collider2D col = GetComponent<Collider2D>();
    if (col != null)
        col.enabled = false;

    // Stop rigidbody motion
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
        rb.linearVelocity = Vector2.zero;
}
}
