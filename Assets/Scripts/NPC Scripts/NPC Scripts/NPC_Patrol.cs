using UnityEngine;
using System.Collections;

public class NPC_Patrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Vector2[] patrolPoints;
    public float speed = 2f;
    public float pauseDuration = 1.5f;

    private Vector2 target;

    private Rigidbody2D rb;
    private Animator anim;
    private int currentPointIndex = 0;
    private bool isPaused = false;

    void start(){
         rb = GetComponent<Rigidbody2D>();
         anim = GetComponentInChildren<Animator>();
         StartCoroutine(SetPatrolPoint());
    }

    void update(){
        if(isPaused){
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 direction = ((Vector3)target - transform.position).normalized;
        if(direction.x<0 && transform.localScale.x >0 || direction.x >0 && transform.localScale.x < 0  ){
            transform.localScale  = new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z ); 
        }

        rb.linearVelocity = direction * speed;

        if(Vector2.Distance(transform.position, target) < .1f){
            StartCoroutine(SetPatrolPoint());
        }
    }

    IEnumerator SetPatrolPoint(){
        isPaused = true;
        anim.Play("Idle");
        yield return new WaitForSeconds(pauseDuration);

        currentPointIndex = (currentPointIndex + 1 ) % patrolPoints.Length; 
        target = patrolPoints[currentPointIndex];
        isPaused = false;
        anim.Play("Walk");


    }
    
}

