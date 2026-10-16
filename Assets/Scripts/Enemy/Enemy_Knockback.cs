using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Knockback : MonoBehaviour
{
     private Rigidbody2D rb;
     private Enemy_Movements enemy_Movement;

     private void Start()
     {
          rb = GetComponent<Rigidbody2D>();
          enemy_Movement = GetComponent<Enemy_Movements>();
     }

     public void Knockback(Transform playerTransform, float KnockbackForce, float KnockbackTime, float stunTime)
    {
        enemy_Movement.ChangeState(EnemyState.Knockback);
        StartCoroutine(StunTimer( KnockbackTime,stunTime));
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = direction * KnockbackForce;
        Debug.Log("Enemy Knockback");
        
    }

    IEnumerator StunTimer( float KnockbackTime, float stunTime)
    {
        yield return new WaitForSeconds(KnockbackTime);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemy_Movement.ChangeState(EnemyState.Idle);
    }
}
