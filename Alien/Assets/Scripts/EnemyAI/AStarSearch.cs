using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : SearchAlgorithm
{
    private class SearchNode {
        public PathNode pathNode;
        public SearchNode parent;
        public float g;
        public float h;
        public float f;

        public SearchNode(PathNode pathNode, float h) {
            this.pathNode = pathNode;
            this.h = h;
        }

        public SearchNode(PathNode pathNode, float h, SearchNode parent){
            this.pathNode = pathNode;
            this.h = h;
            this.parent = parent;
        }
    }

    private SearchNode[,] grid;

    public AStarSearch(PathNode[,] pathGrid) {
        grid = GenSearchGrid(pathGrid);
    }

    private SearchNode[,] GenSearchGrid(PathNode[,] grid) {
        SearchNode[,] searchGrid = new SearchNode[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                //Debug.Log(grid[i, j]);
                searchGrid[i, j] = new SearchNode(grid[i, j], 0);
            }
        }

        //Debug.Log("GEN");

        return searchGrid;
    }

    private SearchNode GetSearchNode(int x, int y) {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) {
            return null;
        }
        return grid[x, y];
    }

    private List<SearchNode> GetNeighbors(SearchNode node) {
        List<SearchNode> neighbors = new List<SearchNode>();

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                SearchNode temp = GetSearchNode(node.pathNode.x + i, node.pathNode.y + j);
                if (temp != null) {
                    neighbors.Add(temp);
                }
            }
        }

        return neighbors;
    }   

    private PathNode[] MakePath(SearchNode node) {
        List<PathNode> path = new List<PathNode>();

        while (node != null) {
            path.Insert(0, node.pathNode);
            node = node.parent;
        }

        return path.ToArray();
    }

    public override PathNode[] GetPath(PathNode startNode, PathNode targetNode) {
        List<SearchNode> frontier = new List<SearchNode>();
        HashSet<SearchNode> closedSet = new HashSet<SearchNode>();

        SearchNode start = grid[startNode.x, startNode.y];
        SearchNode target = grid[targetNode.x, targetNode.y];
        //Debug.Log(start.pathNode);
        //Debug.Log(target.pathNode);

        //if (start.pathNode != startNode || target.pathNode != targetNode) {
            //throw new InvalidOperationException("Start and/or target pathnodes are not from the grid on this a-star script"); //Pathnodes from a different 
        //    return null;
        //}

        start.h = Vector2.Distance(new Vector2(start.pathNode.x, start.pathNode.y), new Vector2(target.pathNode.x, target.pathNode.y));
        start.f = 0;
        start.g = 0;
        
        frontier.Add(start);

        while (frontier.Count > 0) {
            SearchNode currentNode = frontier[0];
            for (int i = 1; i < frontier.Count; i++) {
                if (frontier[i].f < currentNode.f || frontier[i].f == currentNode.f && frontier[i].h < currentNode.h) {
                    currentNode = frontier[i];
                }
            }

            frontier.Remove(currentNode);
            

            List<SearchNode> neighbors = GetNeighbors(currentNode);

            foreach (SearchNode neighbor in neighbors) {
                //Debug.Log(neighbor.pathNode.x + " " + neighbor.pathNode.y);
                if (neighbor == target) {
                    neighbor.parent = currentNode;
                    return MakePath(neighbor);
                
                }

                float g = currentNode.g + Vector2.Distance(new Vector2(neighbor.pathNode.x, neighbor.pathNode.y), new Vector2(currentNode.pathNode.x, currentNode.pathNode.y));
                
                if (neighbor.g == 0 || g < neighbor.g) {
                    neighbor.parent = currentNode;
                    neighbor.g = g;
                    neighbor.h = Vector2.Distance(new Vector2(neighbor.pathNode.x, neighbor.pathNode.y), new Vector2(target.pathNode.x, target.pathNode.y));
                    neighbor.f = neighbor.g + neighbor.h;
                }

                bool inFrontier = frontier.Contains(neighbor);
                bool inClosed = closedSet.Contains(neighbor);

                if (!inFrontier && !inClosed) {
                    frontier.Add(neighbor);
                }
                
            }

            closedSet.Add(currentNode);

        }

        //Debug.Log("FAILURE");
        return null;
    }
}
