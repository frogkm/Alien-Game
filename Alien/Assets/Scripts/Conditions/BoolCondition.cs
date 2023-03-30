using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BoolCondition : Condition<bool>
{

    [SerializeField] public bool boolValue;

    public override bool Evaluate(bool val) {
        return val == boolValue;
    }
}
