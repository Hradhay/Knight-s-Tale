using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPCState { Default, Idle, Patrol, Wander, Talk }

    public NPCState currentState = NPCState.Wander;
    public NPCState defaultState;

    public NPC_Patrol patrol;
    public NPC_Wander wander;
    public NPC_Talk talk;

    private void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {
        // if (currentState == newState) return;

        // currentState = newState;

        // if (patrol != null)
        //     patrol.enabled = newState == NPCState.Patrol;

        // if (wander != null)
        //     wander.enabled = newState == NPCState.Wander;

        // if (talk != null)
        //     talk.enabled = newState == NPCState.Talk;

        currentState = newState;
        patrol.enabled = newState == NPCState.Patrol;
        wander.enabled = newState == NPCState.Wander;
        talk.enabled = newState == NPCState.Talk;


    }

    // public void StartTalking()
    // {
    //     SwitchState(NPCState.Talk);
    // }

    // public void StopTalking()
    // {
    //     SwitchState(defaultState);
    // }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            SwitchState(NPCState.Talk);
        }

    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            SwitchState(defaultState);
        }
        
    }

}