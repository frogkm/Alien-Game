using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : Interactable
{

    [SerializeField] int defaultDialogueId;
    [SerializeField] DialogueManager dialogueManager;

    public override void Interact() {
        dialogueManager.StartConversation(defaultDialogueId);
    }
}
