using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{
    class AStar
    {
        public static List<SquareGraphNode> FindPath(SquareGraphNode start, SquareGraphNode end, Func<SquareGraphNode, SquareGraphNode, double> distance, Func<SquareGraphNode, SquareGraphNode, double> estimate)
        {
            HashSet<SquareGraphNode> closedSet = new HashSet<SquareGraphNode>();
            PriorityQueue<double, List<SquareGraphNode>> queue = new PriorityQueue<double, List<SquareGraphNode>>();
            var initialPath = new List<SquareGraphNode>();
            initialPath.Add(start);
            queue.Enqueue(0, initialPath);
            while (!queue.IsEmpty)
            {
                var currentPath = queue.Dequeue();
                var currentNode = currentPath.Last();
                if (closedSet.Contains(currentNode))
                    continue;
                if (currentNode.Equals(end))
                    return currentPath;
                closedSet.Add(currentNode);
                foreach (var node in currentNode.Neighbors)
                {
                    double dist = distance(currentNode, node);
                    var newPath = new List<SquareGraphNode>(currentPath);
                    newPath.Add(node);
                    queue.Enqueue(CalculatePathCost(currentPath) + dist + estimate(node, end), newPath);
                }
            }
            return null;
        }

        public static int CalculatePathCost(List<SquareGraphNode> list)
        {
            int totalCost = 0;
            foreach (var node in list)
            {
                totalCost += node.Cost;
            }
            return totalCost;
        }

        public static double ManhattanDistance(SquareGraphNode start, SquareGraphNode end)
        {
            return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
        }

        public static double EuclideanDistance(SquareGraphNode start, SquareGraphNode end)
        {
            return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
        }

    }
}
