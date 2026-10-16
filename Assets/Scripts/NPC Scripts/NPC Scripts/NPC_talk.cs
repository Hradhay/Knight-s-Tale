using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public Animator interactAnim;
    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;
    // public DialogueSO  dialogueSO;


    // private DialogueManager dialogueManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // private void Start()
    // {
    //     dialogueManager = GameManager.instance.DialogueManager;
    // }

    private void OnDestroy()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.Play("Idle");
        interactAnim.Play("Open");
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        if (anim != null)
            anim.Play("Idle", 0);

        if (interactAnim != null)
            interactAnim.Play("Open", 0);

    }

    private void OnDisable()
    {
        if (interactAnim != null && interactAnim.gameObject.activeInHierarchy)
            interactAnim.Play("Close", 0);

        rb.bodyType = RigidbodyType2D.Dynamic;
        
    }


    private void Update(){
        if(Input.GetButtonDown("Interact")){
            if(GameManager.Instance.DialogueManager.isDialogueActive){
                GameManager.Instance.DialogueManager.AdvanceDialogue();

            }else{
                if(GameManager.Instance.DialogueManager.CanStartDialogue()){
                CheckForNewConversation();
                GameManager.Instance.DialogueManager.StartDialogue(currentConversation);
                }
            }
        }
    }



    private void CheckForNewConversation(){
        for(int i = 0; i < conversations.Count; i++ ){
            var convo = conversations[i];
            if(convo != null && convo.IsConditionMet()){
                currentConversation = convo;

                if(convo.removeAfterPlay){
                    conversations.RemoveAt(i);
                }
                
                if(convo.removeTheseOnPlay != null && convo.removeTheseOnPlay.Count > 0){
                    foreach(var toRemove in convo.removeTheseOnPlay){
                        conversations.Remove(toRemove);
                    }
                }
 
                break;
            }
        }
    }
    private void OnQuestAccepted_RemoveOfferings(QuestSO acceptedQuest)
    {
        for( int i = conversations.Count - 1; i >= 0; i--)
        {
            var convo = conversations[i];
            if (convo == null)
            {
                continue;
            }
            if (convo.offerQuestOnEnd == acceptedQuest)
            {
                conversations.RemoveAt(i);
            }
        }
    }

}
