using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // add or remove an interactionEvent component to this obj
    public bool useEvents;
    [SerializeField]
    //displays message to player
    public string promptMessage;

    public virtual string onLook() 
    {
        return promptMessage;
    }

    //Will be called from player
   public void BaseInteract()
   {
    if(useEvents)
        GetComponent<InteractionEvent>().OnInteract.Invoke();
    Interact();
   }

   protected virtual void Interact()
   {

   }
}
