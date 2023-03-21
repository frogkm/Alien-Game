using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum Combiner {
    AND,
    OR,
    NOT
}

[Serializable]
public class MultiExpression : Expression
{

    

    [SerializeField] private Combiner combiner;
    //[SerializeField] private int operatoru;
    [SerializeField] private List<Expression> expressions;
}
