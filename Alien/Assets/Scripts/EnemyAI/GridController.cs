using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour
{
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;
    [SerializeField] private Transform player;

    public PathNode[,] grid;
    private float nodeDiameter;
    private int numNodesX, numNodesY;
    private Vector3 bottomLeft;

    void Start() {
        nodeDiameter = nodeRadius * 2;
        numNodesX = (int) (gridWorldSize.x / nodeDiameter);
        numNodesY = (int) (gridWorldSize.y / nodeDiameter);

        grid = new PathNode[numNodesX, numNodesY]();
        bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int i = 0; i < numNodesX; i++) {
            for (int j = 0; j < numNodesY; j++) {
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[i,j] = new PathNode(grid, walkable, i, j);
            }
        }
    }

    public PathNode NodeFromWorldPoint(Vector3 worldPosition) {
        worldPosition -= bottomLeft;

        float percentX = worldPosition.x / gridWorldSize.x;
        float percentY = worldPosition.z / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)((numNodesX ) * percentX);
        int y = (int)((numNodesY ) * percentY);

        x = Mathf.Min(x, numNodesX - 1);
        y = Mathf.Min(y, numNodesY - 1);

        return grid[x, y];
    }

    public Vector3 GetNodePosition(PathNode node) {
        return new Vector3(bottomLeft.x + node.x * nodeDiameter + nodeRadius, transform.position.y, bottomLeft.z + node.y * nodeDiameter + nodeRadius);
    }


    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null) {
            PathNode playerNode = NodeFromWorldPoint(player.position);
            //int[] gridPos = NodeToGridPos(playerNode);
            //Debug.Log(GetNodePosition(gridPos[0], gridPos[1]));
            foreach(PathNode node in grid) {
                Gizmos.color = (node.walkable)?Color.green:Color.red;
                if (playerNode == node) {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(GetNodePosition(node), new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, nodeDiameter - 0.1f));
                
            }
        }

    }
}
