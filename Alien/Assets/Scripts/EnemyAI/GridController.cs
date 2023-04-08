using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grid {
    public PathNode[,] g;

    public Grid(int xCount, int yCount) {
        g = new PathNode[xCount, yCount];
    }

    public int[] NodeToGridPos(PathNode node) {
        for (int i = 0; i < g.GetLength(0); i++) {
            for (int j = 0; j < g.GetLength(1); j++) {
                if (g[i,j] == node) {
                    return (new int[2]{i, j});
                }
            }
        }
        return null;
    }
}


public class GridController : MonoBehaviour
{
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;
    [SerializeField] private Transform player;

    private Grid grid;
    private float nodeDiameter;
    private int numNodesX, numNodesY;
    private Vector3 bottomLeft;

    public Grid GetGrid() {
        return grid;
    }

    void Start() {
        nodeDiameter = nodeRadius * 2;
        numNodesX = (int) (gridWorldSize.x / nodeDiameter);
        numNodesY = (int) (gridWorldSize.y / nodeDiameter);

        grid = new Grid(numNodesX, numNodesY);
        bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int i = 0; i < numNodesX; i++) {
            for (int j = 0; j < numNodesY; j++) {
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid.g[i,j] = new PathNode(walkable);
            }
        }
    }

    public PathNode NodeFromWorldPoint(Vector3 worldPosition) {
        int[] gridPos = GridPosFromWorldPoint(worldPosition);

        return grid.g[gridPos[0], gridPos[1]];
    }

    public int[] GridPosFromWorldPoint(Vector3 worldPosition) {
        worldPosition -= bottomLeft;

        float percentX = worldPosition.x / gridWorldSize.x;
        float percentY = worldPosition.z / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)((numNodesX ) * percentX);
        int y = (int)((numNodesY ) * percentY);

        x = Mathf.Min(x, numNodesX - 1);
        y = Mathf.Min(y, numNodesY - 1);

        return (new int[2]{x, y});
    }

    public Vector3 GetNodePosition(int x, int y) {
        return new Vector3(bottomLeft.x + x * nodeDiameter + nodeRadius, transform.position.y, bottomLeft.z + y * nodeDiameter + nodeRadius);
    }

    public Vector3 GetNodePosition(PathNode node) {
        int[] gridPos = grid.NodeToGridPos(node);
        return GetNodePosition(gridPos[0], gridPos[1]);
    }


    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null) {
            PathNode playerNode = NodeFromWorldPoint(player.position);
            //int[] gridPos = NodeToGridPos(playerNode);
            //Debug.Log(GetNodePosition(gridPos[0], gridPos[1]));
            foreach(PathNode node in grid.g) {
                Gizmos.color = (node.walkable)?Color.green:Color.red;
                if (playerNode == node) {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(GetNodePosition(node), new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, nodeDiameter - 0.1f));
                
            }
        }

    }
}
