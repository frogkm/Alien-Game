using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="NPC")]
public class NPC : ScriptableObject
{
    [SerializeField] public string nameId;
    [SerializeField] public string displayName;
    [SerializeField] public Sprite portrait;

    [SerializeField] public List<DialogueCondition> dialogueConditions;


    public NPC()
    {

        if (displayName is null) {
            displayName = nameId;
        }
    }

    public Dialogue FetchDialogue() {
        for (int i = dialogueConditions.Count - 1; i >= 0; i--) {
            if (dialogueConditions[i].Evaluate()) {
                return dialogueConditions[i].dialogue;
            }
        }
        return null;
    }

    public void InitFlags() {
        foreach (DialogueCondition condition in dialogueConditions) {      
            condition.InitFlags();
        }
    }


}
