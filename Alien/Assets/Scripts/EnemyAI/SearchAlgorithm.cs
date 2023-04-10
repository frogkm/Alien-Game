using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SearchAlgorithm
{
    public abstract PathNode[] GetPath(PathNode start, PathNode target);
  
}
