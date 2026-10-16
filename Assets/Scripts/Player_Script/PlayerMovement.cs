using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public int facingDirection = 1;

    public Rigidbody2D rb;
    public Animator anim;

    private bool isKnockedBack;
    public bool isShooting;

    public PlayerCombat player_Combat;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing on " + gameObject.name);
        }

// StatsManager validation handled by fallback properties
    }


    private void Update()
    {
        if(Input.GetButtonDown("Slash") && player_Combat.enabled  == true) 
        {
            player_Combat.Attack();
        }
    }
   

    // Update is called 50x frames
    void FixedUpdate()
    {
        if(isShooting == true)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else if(!isKnockedBack)
        {
         float horizontal = Input.GetAxis("Horizontal");
         float vertical = Input.GetAxis("Vertical");

            if(horizontal > 0 && transform.localScale.x < 0 ||
               horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));
            if (rb != null)
            {
                float currentSpeed = StatsManager.Speed;
                rb.linearVelocity = new Vector2(horizontal, vertical) * currentSpeed;
            }
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        // Update animator after flip for correct vertical direction if needed
        // Note: If animator needs signed vertical for up/down facing, set without Abs here or via trigger
    }
    
    public void Knockback(Transform enemy, float force, float stunTime)
    {
        if (!gameObject.activeInHierarchy) return;
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force;
        StartCoroutine(KnockbackCorouter(stunTime)); // Adjust the duration as needed
    }

    IEnumerator KnockbackCorouter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

}
