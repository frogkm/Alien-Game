using System.Collections;
using System.Collections.Generic;
using UnityEngine;

private class SearchNode {
    public PathNode pathNode;
    public SearchNode parent;
    public float g;
    public float h;
    public float f;

    public int x
    {
        get
        {
            return pathNode.x;
        }
    }

    public int y
    {
        get
        {
            return pathNode.y;
        }
    }

    public SearchNode(PathNode pathNode, float h) {
        this.pathNode = pathNode;
        this.h = h;
    }

    public SearchNode(PathNode pathNode, float h, SearchNode parent) : this(pathNode, h){
        this.parent = parent;
    }

}

public class AStarSearch : SearchAlgorithm
{

    private SearchNode[,] grid;

    public AStarSearch(PathNode[,] pathGrid) {
        grid = GenSearchGrid(pathGrid);
    }

    private SearchNode[,] GenSearchGrid(PathNode[,] grid) {
        SearchNode[,] searchGrid = new SearchNode[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                searchGrid[i, j] = new SearchNode(grid[i, j], 0);
            }
        }

        return searchGrid;
    }

    private SearchNode GetSearchNode(int x, int y) {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1)) {
            return null;
        }
        return grid[x][y];
    }

    private List<SearchNode> GetNeighbors(SearchNode node) {
        List<SearchNode> neighbors = new List<SearchNode>();

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                SearchNode temp = GetSearchNode(i, j);
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

        if (start.pathNode != startNode || target.pathNode != targetNode) {
            throw new InvalidOperationException("Start and/or target pathnodes are not from the grid on this a-star script"); //Pathnodes from a different 
        }

        start.h = Vector2.Distance(new Vector2(start.x, start.y), new Vector2(target.x, target.y))
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
            closedSet.Add(currentNode);

            List<SearchNode> neighbors = GetNeighbors(currentNode);

            for (SearchNode neighbor in neighbors) {
                if (neighbor == target) {
                    return MakePath(neighbor);
                }
                else {
                    int g = currentNode.g + Vector2.Distance(new Vector2(neighbor.x, neighbor.y), new Vector2(currentNode.x, currentNode.y));
                    int h = Vector2.Distance(new Vector2(neighbor.x, neighbor.y), new Vector2(target.x, target.y));
                    int f = neighbor.g + neighbor.h;

                    bool alreadySeen = frontier.Contains(neighbor);
                    bool alreadyClosed = closed.Contains(neighbor);

                    if (!alreadySeen || (alreadySeen && f < neighbor.f)) {
                        neighbor.g = g;
                        neighbor.h = h;
                        neighbor.f = f;
                    }
                }
            }



        }


        return null;
    }
}
