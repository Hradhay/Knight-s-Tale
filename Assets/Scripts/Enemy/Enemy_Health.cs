using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Health : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Reward")]
    public int expReward = 5;

    // 🔽 ADD THIS
[Header("Loot System")]
public GameObject[] dropPrefabs;

[Range(0f, 1f)]
public float dropChance = 0.5f;   // Chance to drop power-up

public int minQuantity = 1;
public int maxQuantity = 2;




    [Header("UI (Choose ONE)")]
    public Slider healthBar;          // Option 1: Slider
    public Image healthFill;          // Option 2: Image Fill (better for pixel games)

    // Event for XP system
    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated OnMonsterDefeated;

    private void Start()
    {
        currentHealth = maxHealth;

        // Setup UI
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthFill != null)
        {
            healthFill.fillAmount = 1f; // full
        }
    }

    // 🔥 MAIN FUNCTION (Use this everywhere)
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        // Clamp health
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthUI();

            // Trigger XP event safely
            OnMonsterDefeated?.Invoke(expReward);
            // Loot drop logic
            DropLoot();
            // Destroy enemy
            Destroy(gameObject);
            return;
        }

        UpdateHealthUI();
    }

    // Optional helper (if you prefer calling Damage)
    public void Damage(int damageAmount)
    {
        ChangeHealth(-damageAmount);
    }

    public void Heal(int healAmount)
    {
        ChangeHealth(healAmount);
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    // 🔄 UI Update Method
    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (healthFill != null)
        {
            healthFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
void DropLoot()
{
    if (dropPrefabs.Length == 0) return;

    if (Random.value > dropChance) return;

    int index = Random.Range(0, dropPrefabs.Length);
    GameObject prefabToSpawn = dropPrefabs[index];

    Vector2 offset = Random.insideUnitCircle * 0.5f;

    Instantiate(prefabToSpawn, (Vector2)transform.position + offset, Quaternion.identity);
}
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Enemy_Health : MonoBehaviour
// {
//     public int expReward = 5;
//     public delegate void MonsterDefeated(int exp);
//     public static event MonsterDefeated OnMonsterDefeated;

//     public int currentHealth;
//     public int maxHealth;

    
//     private Animator anim;
//     private bool isDead = false;

//     private void Start()
//     {
//         currentHealth = maxHealth;
//         anim = GetComponent<Animator>(); // get animator
//     }

//     public void ChangeHealth(int amount)
//     {
//         if (isDead) return; // prevent multiple death calls

//         currentHealth += amount;

//         if (currentHealth > maxHealth)
//         {
//             currentHealth = maxHealth;
//         }
//         else if (currentHealth <= 0)
//         {
//             Die();
//         }
//     }

    
//     void Die()
//     {
//         isDead = true;

//         OnMonsterDefeated?.Invoke(expReward);

//         // play death animation
//         if (anim != null)
//         {
//             anim.SetBool("isDead", true);
//         }

//         // stop movement if exists
//         Rigidbody2D rb = GetComponent<Rigidbody2D>();
//         if (rb != null)
//         {
//             rb.linearVelocity = Vector2.zero;
//         }

//         // disable collider so player can't hit dead body
//         Collider2D col = GetComponent<Collider2D>();
//         if (col != null)
//         {
//             col.enabled = false;
//         }

//         // destroy after delay (animation time)
//         StartCoroutine(DestroyAfterDelay(1.5f));
//     }

//     IEnumerator DestroyAfterDelay(float delay)
//     {
//         yield return new WaitForSeconds(delay);
//         Destroy(gameObject);
//     }

    

//     public int GetHealth(){
//         return currentHealth;
//     }

//     public void Damage(int damageAmount){
//         ChangeHealth(-damageAmount); 
//     }

//     public void Heal(int healAmount){
//         ChangeHealth(healAmount); 
//     }
// }



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Enemy_Health : MonoBehaviour
// {
//     public int expReward = 5;
//     public delegate void MonsterDefeated(int exp);
//     public static event MonsterDefeated OnMonsterDefeated;
//     public int currentHealth;
//     public int maxHealth;
//     // ---------------------------------


//     private void Start()
//     {
//         currentHealth = maxHealth;
//     }

//     public void ChangeHealth(int amount)
//     {
//         currentHealth += amount;

//         if (currentHealth > maxHealth)
//         {
//             currentHealth = maxHealth;
//         }
//         else if (currentHealth <= 0)
//         {
//             OnMonsterDefeated(expReward);
//             Destroy(gameObject);
//         }
//     }
// // -------------------------------------------------------------------------------------------------------------------

// // private int health;
// // public HealthSystem(int maxhealth){
// //     this.maxhealth = maxhealth;
// //     health = maxhealth;

// // }

// public int GetHealth(){
//     return currentHealth;
// }

// public void Damage(int damageAmount){
//     currentHealth -= damageAmount;
//     if(currentHealth<0) currentHealth = 0;
// }

// public void Heal(int healAmount){
//     currentHealth += healAmount;
//     if(currentHealth >  maxHealth) currentHealth = maxHealth;
// }
//     // private void Die()
//     // {
//     //     // Add death logic here (e.g., play animation, drop loot, etc.)
//     //     Destroy(gameObject);
//     // }

    
// }


