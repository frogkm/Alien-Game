using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch : SearchAlgorithm
{
    private struct SearchNode {
        public int x;
        public int y;
        public float g;
        public float h;

        public SearchNode(int x, int y, float h) {
            this.x = x;
            this.y = y;
            this.g = g;
            this.h = h;
        }
        
    }

    private SearchNode[,] GenSearchGrid(PathNode[,] grid, SearchNode target) {
        SearchNode[,] searchGrid = new SearchNode[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++) {
            for (int j = 0; j < grid.GetLength(1); j++) {
                searchGrid[i, j] = new SearchNode(i, j, Vector2.Distance(new Vector2(target.x, target.y), new Vector2(i, j)));
            }
        }

        return searchGrid;
    }

    public override PathNode[] GetPath(PathNode[,] grid, int startX, int startY, int targetX, int targetY) {
        List<SearchNode> frontier = new List<SearchNode>();
        HashSet<SearchNode> closedSet = new HashSet<SearchNode>();

        SearchNode start = new SearchNode(startX, startY, Vector2.Distance(new Vector2(targetX, targetY), new Vector2(startX, startY)));
        SearchNode target = new SearchNode(targetX, targetY, 0f);

        SearchNode[,] searchGrid = GenSearchGrid(grid, target);

        frontier.Add(start);

        while (frontier.Count > 0) {
            SearchNode currentNode = frontier[0];
            for (int i = 1; i < frontier.Count; i++) {
                float frontF = frontier[i].g + frontier[i].h;
                float currF = currentNode.g + currentNode.h;
                if (frontF < currF || frontF == currF && frontier[i].h < currentNode.h) {
                    currentNode = frontier[i];
                }
            }

            frontier.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == target) {
                return;
            }


        }


        return null;
    }
}
