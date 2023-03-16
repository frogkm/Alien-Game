using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class DialogueLoader
{

    public static Dictionary<int, Dialogue> loadDialogue(TextAsset jsonFile) {
    
        DialogueList dialogueList = JsonConvert.DeserializeObject<DialogueList>(jsonFile.text);
        Dictionary<int, Dialogue> dialogueDict = new Dictionary<int, Dialogue>();

        foreach (object item in dialogueList.dialogue_list)
        {
            if (item is JObject json)
            {
                if (json.ContainsKey("next_id"))
                {
                    DialogueMessage message = JsonConvert.DeserializeObject<DialogueMessage>(json.ToString());
                    dialogueDict[message.id] = message;
                }
                else if (json.ContainsKey("next_ids"))
                {
                    DialogueQuestion message = JsonConvert.DeserializeObject<DialogueQuestion>(json.ToString());
                    dialogueDict[message.id] = message;
                }
                else
                {
                    Debug.LogError("Unknown dialogue object: " + json.ToString());
                }
            }
            else
            {
                Debug.LogError("Invalid dialogue object: " + item);
            }
        }
        return dialogueDict;
    }
}
