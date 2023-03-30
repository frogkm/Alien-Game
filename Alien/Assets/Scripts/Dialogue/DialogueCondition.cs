using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Reflection.TypeInfo;


[Serializable]
public class DialogueCondition
{
    [SerializeField] private List<BoolCondition> boolConditions;
    [SerializeField] private List<IntCondition> intConditions;
    [SerializeField] public Dialogue dialogue;



    public bool Evaluate() {
        foreach (BoolCondition condition in boolConditions) {
            FlagDictionary dict = FlagDictionary.flagDictionary;

            if (!condition.Evaluate(dict.GetBoolFlag(condition.flagName))) {
                return false;
            }
        }
        foreach (IntCondition condition in intConditions) {
            FlagDictionary dict = FlagDictionary.flagDictionary;

            if (!condition.Evaluate(dict.GetIntFlag(condition.flagName))) {
                return false;
            }
        }
        return true;
    }

    public void InitFlags() {
        foreach (BoolCondition condition in boolConditions) {
            FlagDictionary dict = FlagDictionary.flagDictionary;
            dict.SetFlag(condition.flagName, false);
        }
        foreach (IntCondition condition in intConditions) {
            FlagDictionary dict = FlagDictionary.flagDictionary;
            dict.SetFlag(condition.flagName, 0);
        }
    }
}
