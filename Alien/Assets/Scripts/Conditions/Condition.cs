using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition<T>
{
    [SerializeField] public bool isLocal = false;
    [SerializeField] public string flagName;


    public abstract bool Evaluate(T val);
}
