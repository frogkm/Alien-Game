using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IntCondition : Condition<int>
{
    public enum IntOperator 
    {
        eq,
        lt,
        gt,
        lseq,
        gteq,
        neq
    }

    [SerializeField] public IntOperator intOperator;
    [SerializeField] public int intValue;


    public override bool Evaluate(int val) {
        return val == intValue;
    }
}
