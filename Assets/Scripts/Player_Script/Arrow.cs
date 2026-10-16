using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb;
    
    public Vector2 direction = Vector2.right;
    
    public float lifeSpan = 2f;
    public float speed = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 
    public LayerMask enemyLayer;
    public LayerMask obstracleLayer;
    public SpriteRenderer sr;
    public Sprite buriedSprite;

    public int damage;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
 
    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifeSpan);    
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<Enemy_Health>().ChangeHealth(-damage);
            collision.gameObject.GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
             AttachToTarget(collision.gameObject.transform);
        }
        else if((obstracleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
                AttachToTarget(collision.gameObject.transform);
        }
    }
  
    private void AttachToTarget(Transform target)
    {   
        sr.sprite = buriedSprite; // Change to the buried sprite
        
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Make the arrow kinematic to stop physics interactions

        transform.SetParent(target); // Attach the arrow to the target
        
    }
}
