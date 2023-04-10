using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private SearchAlgorithm searchAlg;
    [SerializeField] private GridController gridController;
    [SerializeField] private Transform targetTransform;
    private bool drawn = false;


    void Start() {
        //Debug.Log(gridController.grid[0,0]);
        
    }

    void Update() {
        if (searchAlg == null) {
            searchAlg = new AStarSearch(gridController.grid);
        }
    }

    public PathNode[] GetPath(Vector3 target) {
        PathNode startNode = gridController.NodeFromWorldPoint(transform.position);
        PathNode targetNode = gridController.NodeFromWorldPoint(target);
        return searchAlg.GetPath(startNode, targetNode);
    }

    void OnDrawGizmos() {
        //Debug.Log("ondraw");

        if (searchAlg != null && !drawn) {
            drawn = true;
            //Debug.Log(targetTransform);
            
            PathNode[] path = GetPath(targetTransform.position);
            

            foreach(PathNode node in path) {
                Debug.Log(node.x + " " + node.y);
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(gridController.GetNodePosition(node), new Vector3(gridController.nodeRadius*2, gridController.nodeRadius*2, gridController.nodeRadius*2));
            }
        }

    }
    
}
