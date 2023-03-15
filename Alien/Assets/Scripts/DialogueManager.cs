using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class DialogueManager : MonoBehaviour
{

    [SerializeField] private TextAsset jsonFile;

    private Dictionary<int, Dialogue> dialogueDict;


    void Start()
    {
        dialogueDict = new Dictionary<int, Dialogue>();
        DialogueList dialogueList = JsonConvert.DeserializeObject<DialogueList>(jsonFile.text);

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

        
    }

    private void printDict() {
        foreach (KeyValuePair<int, Dialogue> keyVal in dialogueDict) {
            Debug.Log(string.Format("{0}: {1}, by {2}", keyVal.Key, keyVal.Value.message, keyVal.Value.speaker));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
