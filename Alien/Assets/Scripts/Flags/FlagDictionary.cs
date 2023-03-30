using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlagDictionary
{
    public Dictionary<string, bool> boolDictionary;
    public Dictionary<string, int> intDictionary;

    public static FlagDictionary flagDictionary = new FlagDictionary();

    public FlagDictionary() {
        boolDictionary = new Dictionary<string, bool>();
        intDictionary = new Dictionary<string, int>();

        //if (isNpc) {
        //    InitNPCVals();
        //}
    }

    //private void InitNPCVals() {
    //    SetFlag("times_spoken", 0);
    //}

    public void SetFlag(string flag, object val) {
        switch(val.GetType()) {
            case Type boolType when boolType == typeof(bool):
                boolDictionary[flag] = (bool) val;
                break;
            case Type intType when intType == typeof(int):
                intDictionary[flag] = (int) val;
                break;
        }
    }


    public bool GetBoolFlag(string flag) {
        return boolDictionary[flag];
    }

    public int GetIntFlag(string flag) {
        return intDictionary[flag];
    }
}
