using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class DialogueList
{
    [JsonProperty("dialogue_list")]
    public List<object> dialogue_list;

}
