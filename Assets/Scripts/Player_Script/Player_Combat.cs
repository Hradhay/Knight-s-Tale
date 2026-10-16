using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Transform attackPoint;
    // public float WeaponRange = 1f;
    // public float KnockbackForce = 50f;
    // public float KnockbackTime = 0.15f;
    // public float stunTime = 0.3f;
    public LayerMask enemyLayer;
    // public int damage = 1;
    public Animator anim;

    public float cooldown = 1f;
    private float timer;


    public void Update()
    {
        timer -= Time.deltaTime;
    }

    public void Attack() 
    {
        if(timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            
           
            timer = cooldown;
        }
        
    }

    public void DealDamage()
    {
        if (StatsManager.Instance == null)
        {
            Debug.LogWarning("StatsManager.Instance is null in PlayerCombat.DealDamage!");
            return;
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll( attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
            if (enemyHealth != null)
            {
                enemyHealth.ChangeHealth(-StatsManager.Instance.damage);
            }
            else
            {
                Debug.LogWarning("No Enemy_Health component on " + enemy.name);
            }

            Enemy_Knockback knockback = enemy.GetComponent<Enemy_Knockback>();
            if (knockback != null)
            {
                knockback.Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime);
            }
            else
            {
                Debug.LogWarning("No Enemy_Knockback component on " + enemy.name);
            }
        }
    }

    public void FinishAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null || StatsManager.Instance == null)
            return;
            
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, StatsManager.Instance.weaponRange);
    }
}
