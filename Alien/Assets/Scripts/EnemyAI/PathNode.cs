using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
    public bool walkable;
    public readonly int x;
    public readonly int y;
    private PathNode[,] grid;

    public PathNode(PathNode[,] grid, bool walkable, int x, int y) {
        this.grid = grid;
        this.walkable = walkable;
        this.x = x;
        this.y = y;
    }
}
