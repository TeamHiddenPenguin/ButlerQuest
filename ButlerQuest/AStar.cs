/* AStar.cs
 * Written by Samuel Sternklar
 * 
 * This class contains static methods used in the A* pathfinding algorithm.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{
    static class AStar
    {
        /// <summary>
        /// Finds a path between two graph nodes using the A* algorithm
        /// </summary>
        /// <param name="start">The starting node to path from</param>
        /// <param name="end">The ending node to path from</param>
        /// <param name="distance">The function used to find the distance between two points</param>
        /// <param name="estimate">The function used to estimate the distance between the current node and another node</param>
        /// <returns>A list of nodes representing the path between the two nodes</returns>
        public static List<IGraphNode> FindPath(IGraphNode start, IGraphNode end)
        {
            //A PriorityQueue of open paths to evaluate. the priority is the total cost of the current node plus the estimated distance to the end node
            PriorityQueue<double, List<IGraphNode>> openQueue = new PriorityQueue<double, List<IGraphNode>>();
            //A hashset set of nodes we have already looked at
            //Is a hashset because it has O(1) time add and O(1) time Contains, and if we have like 30 enemies chasing us then lambda functions for List.Find(Predicate<T> predicate) are slow.
            HashSet<IGraphNode> closedSet = new HashSet<IGraphNode>();
            //The initial path, from which all other paths will be calculated
            List<IGraphNode> initialPath = new List<IGraphNode>();
            
            //Add the start node to the initial path, and the initial path to the queue of things to be evaluated
            initialPath.Add(start);
            openQueue.Enqueue(ManhattanDistance(start, end), initialPath);

            //While we stll have objects in the queue and have not exited the queue yet
            while (!openQueue.IsEmpty)
            {
                //Get the current node to be evaluated
                List<IGraphNode> currentPath = openQueue.Dequeue();
                IGraphNode currentNode = currentPath.Last();

                //If the current node has already been evaluated, then this path is definitely not the shortest path and we can skip evaluating it.
                if (!closedSet.Contains(currentNode))
                {
                    //If the current node is the end node, then we're done and can return the current path.
                    if (currentNode.Equals(end))
                        return currentPath;

                    //Add the current node to the closed set so that we know it's already been evaluated.
                    closedSet.Add(currentNode);

                    //For each of the object's neighbors, create a new path and add it to the queue
                    foreach (var node in currentNode.Neighbors)
                    {
                        //Create a new path, contiaining the old path plus this path.
                        List<IGraphNode> newPath = new List<IGraphNode>(currentPath);
                        newPath.Add(node);
                        //Add this path to the queue
                        openQueue.Enqueue(CalculatePathCost(currentPath) + ManhattanDistance(currentNode, node) + ManhattanDistance(node, end), newPath);
                    }
                }
            }
            //if no valid path exists, return null.
            return null;
        }

        /// <summary>
        /// Calculates the cost of a list of SquareGraphNodes
        /// </summary>
        /// <param name="list">The list of nodes to be evaluated</param>
        /// <returns>The total cost of the path.</returns>
        public static int CalculatePathCost(List<IGraphNode> list)
        {
            int totalCost = 0;
            foreach (var node in list)
            {
                totalCost += node.Cost;
            }
            return totalCost;
        }

        /// <summary>
        /// The manhattan distance between the two nodes.
        /// In manhattan no matter how many different paths there are, the shortest distance is defined by the x displacement + the y displacement.
        /// This is because manhattan is shaped like a grid.
        /// </summary>
        /// <param name="start">The starting node to evaluate</param>
        /// <param name="end">The ending node to evaluate</param>
        /// <returns>The manhattan distance between the two nodes</returns>
        public static double ManhattanDistance(IGraphNode start, IGraphNode end)
        {
            return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y) + Math.Abs(end.Z - start.Z);
        }

        /// <summary>
        /// Calculates the euclidean distance between two nodes.
        /// Basically does the distance formula between two points, represented by nodes.
        /// </summary>
        /// <param name="start">The starting node to evaluate</param>
        /// <param name="end">The ending node to evaluate</param>
        /// <returns>The euclidean distance between two nodes.</returns>
        public static double EuclideanDistance(IGraphNode start, IGraphNode end)
        {
            return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2) + Math.Pow(end.Z - start.Z, 2));
        }

    }
}
