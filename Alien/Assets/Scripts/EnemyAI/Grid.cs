using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
    public Vector3 position;
    public bool walkable;

    public PathNode(bool walkable, Vector3 position) {
        this.walkable = walkable;
        this.position = position;
    }

}

public class Grid : MonoBehaviour
{
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;

    private PathNode[,] grid;
    private float nodeDiameter;
    private int numNodesX, numNodesY;

    void Start() {
        nodeDiameter = nodeRadius * 2;
        numNodesX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        numNodesY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        grid = new PathNode[numNodesX, numNodesY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int i = 0; i < numNodesX; i++) {
            for (int j = 0; j < numNodesY; j++) {
                Vector3 worldPoint = bottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[i,j] = new PathNode(walkable, worldPoint);
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));


        if (grid != null) {
            foreach(PathNode node in grid) {
                Gizmos.color = (node.walkable)?Color.green:Color.red;
                Gizmos.DrawSphere(node.position, nodeRadius-0.1f);
            }
        }

    }
}
