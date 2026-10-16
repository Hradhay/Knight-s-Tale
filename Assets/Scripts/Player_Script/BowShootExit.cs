using UnityEngine;

public class BowShootExit : MonoBehaviour
{
    public Player_Bow bowScript;
    
    public void ExitShoot()
    {
        if (bowScript != null)
        {
            bowScript.anim.SetBool("isShooting", false);
            bowScript.playerMovement.isShooting = false;
        }
    }
}
