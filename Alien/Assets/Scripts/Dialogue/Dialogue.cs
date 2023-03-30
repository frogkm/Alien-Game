using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private List<DialogueLine> dialogueLines;
    [SerializeField] private List<DialogueResponse> dialogueResponses;
    [SerializeField] private MultiFlagSetter multiFlagSetter;

    public List<DialogueLine> getDialogueLines() {
        return dialogueLines;
    }

    public List<DialogueResponse> getDialogueResponses() {
        return dialogueResponses;
    }

    public void SetFlags() {
        multiFlagSetter.SetFlags();
    }

    /*
    public void InitFlags(FlagDictionary localFlagDictionary) {
        multiFlagSetter.InitFlags(localFlagDictionary);
    }
    */
}
