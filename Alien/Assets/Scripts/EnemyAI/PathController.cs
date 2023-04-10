using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private SearchAlgorithm searchAlg;
    [SerializeField] private GridController gridController;


    void Awake() {
        searchAlg = new AStarSearch(gridController.grid);
    }

    public PathNode[] GetPath(Vector3 target) {
        int[] startPos = gridController.GridPosFromWorldPoint(transform.position);
        int[] targetPos = gridController.GridPosFromWorldPoint(target);
        return searchAlg.GetPath(gridWrapper.g, startPos[0], startPos[1], targetPos[0], targetPos[1]);
    }
    
}
