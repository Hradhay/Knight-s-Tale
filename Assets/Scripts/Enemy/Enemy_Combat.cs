using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;  
    public Transform attackPoint;   
    public float WeaponRange;
    public float knockbackForce;
    public float stunTime;
    public LayerMask playerLayer;


        // Check if it is a player then -damage 
   
        
        // Enemy Attack Player
    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, WeaponRange, playerLayer);

        if(hits.Length > 0)
        {
            // PlayerHealth playerHealth = hits[0].GetComponent<PlayerHealth>();
            // if(playerHealth != null)
            //     playerHealth.ChangeHealth(-damage);
             hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
             hits[0].GetComponent<PlayerMovement>().Knockback(transform, knockbackForce, stunTime);
            
        }
     }

}
