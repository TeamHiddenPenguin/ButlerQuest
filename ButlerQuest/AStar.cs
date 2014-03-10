﻿//Code from http://blogs.msdn.com/b/ericlippert/archive/2007/10/10/path-finding-using-a-in-c-3-0-part-four.aspx
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
        static public Path<Node> FindPath<Node>(
    Node start,
    Node destination,
    Func<Node, Node, double> distance,
    Func<Node, double> estimate)
    where Node : IGraphNode
        {
            if (estimate == null)
                estimate = n => distance(n, destination);

            var closed = new HashSet<Node>();
            var queue = new PriorityQueue<double, Path<Node>>();
            queue.Enqueue(0, new Path<Node>(start));
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep))
                    continue;
                if (path.LastStep.Equals(destination))
                    return path;
                closed.Add(path.LastStep);
                foreach (Node n in path.LastStep.Neighbors)
                {
                    double d = distance(path.LastStep, n);
                    var newPath = path.AddStep(n, d);
                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }
            }
            return null;
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
    //Code from http://blogs.msdn.com/b/ericlippert/archive/2007/10/08/path-finding-using-a-in-c-3-0-part-three.aspx
    class Path<Node> : IEnumerable<Node>
    {
        public Node LastStep { get; private set; }
        public Path<Node> PreviousSteps { get; private set; }
        public double TotalCost { get; private set; }
        private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }
        public Path(Node start) : this(start, null, 0) { }
        public Path<Node> AddStep(Node step, double stepCost)
        {
            return new Path<Node>(step, this, TotalCost + stepCost);
        }
        public IEnumerator<Node> GetEnumerator()
        {
            for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}