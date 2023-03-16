using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NPCLinker
{
    [SerializeField] private string nameId;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite portrait;
    

    public NPCLinker()
    {
        if (displayName is null) {
            displayName = nameId;
        }
    }
}
