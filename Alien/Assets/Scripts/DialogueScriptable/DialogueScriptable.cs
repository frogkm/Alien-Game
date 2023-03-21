using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue")]
public class DialogueScriptable : ScriptableObject
{
    [SerializeField] private List<DialogueLine> dialogueLines;
    [SerializeField] private List<DialogueResponse> dialogueResponses;

    public List<DialogueLine> getDialogueLines() {
        return dialogueLines;
    }

    public List<DialogueResponse> getDialogueResponses() {
        return dialogueResponses;
    }
}
