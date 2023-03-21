using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueLine
{   

    [SerializeField] private string speaker_id;

    [TextArea(3,7)]
    [SerializeField] private string message;


    public string getMessage() {
        return message;
    }

    public string getSpeakerId() {
        return speaker_id;
    }
    

}
