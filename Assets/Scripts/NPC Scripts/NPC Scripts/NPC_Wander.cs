using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC_Wander : MonoBehaviour
{
    [Header("Wander Area")]
    public float wanderWidth = 5f;
    public float wanderHeight = 5f;
    public float speed = 2f;
    public float pauseDuration = 1f;

    public Vector2 startingPosition;
    private Vector2 target;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isPaused;
    private bool isWaiting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        startingPosition = transform.position; // ✅ Set starting position
    }

    private void OnEnable()
    {
        StartCoroutine(PauseAndPickNewDestination());
    }

    private void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(transform.position, target) < 0.1f && !isWaiting)
        {
            StartCoroutine(PauseAndPickNewDestination());
        }

        Move();
    }

    private void Move()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        // Flip sprite
        if ((direction.x > 0 && transform.localScale.x < 0) ||
            (direction.x < 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(
                transform.localScale.x * -1,
                transform.localScale.y,
                transform.localScale.z
            );
        }

        rb.linearVelocity = direction * speed;
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isWaiting = true;
        isPaused = true;

        rb.linearVelocity = Vector2.zero;

        if (anim != null)
            anim.Play("Idle");

        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();

        isPaused = false;
        isWaiting = false;

        if (anim != null)
            anim.Play("Walk");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(!enabled) return;
        StartCoroutine(PauseAndPickNewDestination());
    }

    private Vector2 GetRandomTarget()
    {
        float halfWidth = wanderWidth / 2f;
        float halfHeight = wanderHeight / 2f;

        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0:
                return new Vector2(
                    startingPosition.x - halfWidth,
                    Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)
                );

            case 1:
                return new Vector2(
                    startingPosition.x + halfWidth,
                    Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)
                );

            case 2:
                return new Vector2(
                    Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth),
                    startingPosition.y - halfHeight
                );

            default:
                return new Vector2(
                    Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth),
                    startingPosition.y + halfHeight
                );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            startingPosition,
            new Vector3(wanderWidth, wanderHeight, 0)
        );
    }
}





// using System.Collections;
// using UnityEngine;

// public class NPC_Wander : MonoBehaviour
// {
//     [Header("Wander Area")]
//     public float wanderWidth = 5f;
//     public float wanderHeight = 5f;
//     public float speed = 2f;
//      public float pauseDuration = 1f;


//      public Vector2 startingPosition;
//     private Vector2 target;

//       private Rigidbody2D rb;
//       private Animator anim;

//       private bool isPaused;
//        private bool isWaiting;


//     private void Awake(){
//         rb = GetComponent<Rigidbody2D>();
//         anim = GetComponentInChildren<Animator>();
//     }

//     private void OnEnable()
//      {
//         StartCoroutine(PauseAndPickNewDestination());
//     }






//     private void Update(){
//         if(isPaused){
//             rb.linearVelocity = Vector2.zero;
//             return;
//         }

//         if (Vector2.Distance(transform.position, target) < 0.1f && !isWaiting)
//         {
//             StartCoroutine(PauseAndPickNewDestination());
//         }

//         Move();

//     }

//        private void Move()
//     {
//         Vector2 direction = (target - (Vector2)transform.position).normalized;

//         // Flip sprite
//         if ((direction.x > 0 && transform.localScale.x < 0) ||
//             (direction.x < 0 && transform.localScale.x > 0))
//         {
//             transform.localScale = new Vector3(
//                 transform.localScale.x * -1,
//                 transform.localScale.y,
//                 transform.localScale.z
//             );
//         }

//         rb.linearVelocity = direction * speed;
//     }


//     IEnumerator PauseAndPickNewDestination(){
//         isPaused = true;
//         anim.Play("Idle");
//         yield return new WaitForSeconds(pauseDuration);

// target = GetRandomTarget();
//         isPaused = false;
//         anim.Play("Walk");
//     }


//     public void OnCollisionEnter2D(Collision2D collision){
//         StartCoroutine(PauseAndPickNewDestination());
//     }

//      private Vector2 GetRandomTarget()
//     {
//         float halfWidth = wanderWidth / 2f;
//         float halfHeight = wanderHeight / 2f;

//         int edge = Random.Range(0, 4);

//         switch (edge)
//         {
//             case 0:
//                 return new Vector2(
//                     startingPosition.x - halfWidth,
//                     Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)
//                 );

//             case 1:
//                 return new Vector2(
//                     startingPosition.x + halfWidth,
//                     Random.Range(startingPosition.y - halfHeight, startingPosition.y + halfHeight)
//                 );

//             case 2:
//                 return new Vector2(
//                     Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth),
//                     startingPosition.y - halfHeight
//                 );

//             default:
//                 return new Vector2(
//                     Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth),
//                     startingPosition.y + halfHeight
//                 );
//         }
//     }


//      private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireCube(
//             startingPosition,
//             new Vector3(wanderWidth, wanderHeight, 0)
//         );
//     }



// }