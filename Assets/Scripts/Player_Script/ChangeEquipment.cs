using UnityEngine;

public class ChangeEquipment : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerCombat combat;
    public Player_Bow bow;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("ChangeEquipment"))
        {
            combat.enabled = !combat.enabled;
            bow.enabled = !bow.enabled;
        }
    }
}
