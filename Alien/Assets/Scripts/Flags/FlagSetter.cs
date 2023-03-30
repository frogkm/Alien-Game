using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FlagSetter<T>
{
    [SerializeField] private bool isLocal;
    [SerializeField] private string flagName;
    [SerializeField] private T val;


    public void SetFlag() {
        FlagDictionary dict = FlagDictionary.flagDictionary;
        dict.SetFlag(flagName, val);
    }
}
