using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NPC npc;
    private DialogueManager dialogueManager;

    void Start() {
        dialogueManager = GameObject.Find("GameManager").GetComponent<DialogueManager>();
        npc.InitFlags();
    }

    public void TalkTo() {
        dialogueManager.StartDialogue(npc.FetchDialogue());
        //npc.flagDictionary.SetFlag("times_spoken", npc.flagDictionary.GetIntFlag("times_spoken") + 1);
    }
    
}
