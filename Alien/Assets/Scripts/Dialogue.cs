using Newtonsoft.Json;

[System.Serializable]
public class Dialogue
{   
    [JsonProperty("id")]
    public int id;
    [JsonProperty("speaker")]
    public string speaker;
    [JsonProperty("message")]
    public string message;
    
}

[System.Serializable]
public class DialogueMessage : Dialogue
{
    [JsonProperty("next_id")]
    public int next_id;
}

[System.Serializable]
public class DialogueQuestion : Dialogue
{   
    [JsonProperty("next_ids")]
    public int[] next_ids;
    [JsonProperty("answers")]
    public string[] answers;
}

