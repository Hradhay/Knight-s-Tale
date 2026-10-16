using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Clamp to 0 (important)
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateBar();

        // Hide when dead
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void UpdateBar()
    {
        float healthPercent = currentHealth / maxHealth;

        // Update fill
        healthBarFill.fillAmount = healthPercent;

        // 🎨 Custom color logic
        if (healthPercent > 0.5f)
        {
            // Green → Yellow
            healthBarFill.color = Color.Lerp(Color.yellow, Color.green, (healthPercent - 0.5f) * 2);
        }
        else
        {
            // Red → Yellow
            healthBarFill.color = Color.Lerp(Color.red, Color.yellow, healthPercent * 2);
        }
    }
}