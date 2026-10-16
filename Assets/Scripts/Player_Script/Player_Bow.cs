using UnityEngine;

public class Player_Bow : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject arrowPrefab;

    public PlayerMovement playerMovement;

    private Vector2 aimDirection = Vector2.right;

    public float shootCooldown = 0.5f;
    private float ShootTimer;

    public Animator anim;

    // Update is called once per frame
    void Update()
    {   
        ShootTimer -= Time.deltaTime;
        HandleAiming();
        if (Input.GetButtonDown("Shoot") &&  ShootTimer <= 0)
        {
                playerMovement.isShooting = true;
                anim.SetBool("isShooting", true);
        }
        
    }

    private void OnEnable()
    {
        anim.SetLayerWeight(0, 0); // Enable the aiming layer
        anim.SetLayerWeight(1, 1); // Enable the aiming layer
    }

    private void OnDisable()
    {
        anim.SetLayerWeight(0, 1); // Disable the aiming layer
        anim.SetLayerWeight(1, 0); // Disable the aiming layer
    }

    private void HandleAiming()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            aimDirection = new Vector2(horizontal, vertical).normalized;
            anim.SetFloat("aimX", aimDirection.x);
            anim.SetFloat("aimY", aimDirection.y);
            // float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            // launchPoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void Shoot()
    {       
        if (ShootTimer <= 0) 
        { // Prevent shooting if still in cooldown
           Arrow arrow =  Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity).GetComponent<Arrow>();
              arrow.direction = aimDirection;
              ShootTimer = shootCooldown;
              
        }
            anim.SetBool("isShooting", false);
            playerMovement.isShooting = false;
    }

}
