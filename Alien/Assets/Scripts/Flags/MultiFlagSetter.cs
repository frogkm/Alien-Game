using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MultiFlagSetter
{
    [SerializeField] private List<FlagSetter<bool>> boolFlagSetters;
    [SerializeField] private List<FlagSetter<int>> intFlagSetters;


    public void SetFlags() {
        foreach(FlagSetter<bool> boolSetter in boolFlagSetters) {
            boolSetter.SetFlag();
        }

        foreach(FlagSetter<int> intSetter in intFlagSetters) {
            intSetter.SetFlag();
        }
    }

    /*
    public void InitFlags(FlagDictionary localFlagDictionary) {
        foreach (FlagSetter<bool> setter in boolFlagSetters) {

        }
        foreach (FlagSetter<int> setter in intFlagSetters) {
            
        }
    }
    */
}
