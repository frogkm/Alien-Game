using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BoolExpression : Expression
{
    [SerializeField] private string name;
    [SerializeField] private bool value;
}
