using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueResponse
{
    [SerializeField] private string response; 
    [SerializeField] private Dialogue responseDialogue; 


    public string getResponse() {
        return response;
    }

    public Dialogue getResponseDialogue() {
        return responseDialogue;
    }

}
