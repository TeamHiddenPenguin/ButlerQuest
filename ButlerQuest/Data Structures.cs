/* 
 * The PriorityQueue is a well-known data structure. It's essentially a weighted Queue, where instead of being strictly "First In, First Out", it is "First Highest-Priority In, First Highest-Priority Out", if that makes any sense.
 * Basically what that means is that no matter what order it is enqueued in, the highest priority value will be dequeued before anything else (unless the priority is specified). If two things of the same priority exist, then the
 * first value to be added to the priority queue would be dequeued. If a priority is specified when dequeueing, then the first value to enqueued at that priority is dequeued.
 * 
 * This particular implementation of the PriorityQueue data structure was written quickly, and is horribly inefficient. Unfortunately I don't know enough about data structures to calculate how inefficient it is, or figure out
 * how to make it more efficient.
 * 
 * Written by Samuel Sternklar on 3/1/2014
 * 
 */ 
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{
    class PriorityQueue<TPriority, TValue>
    {
        //How we're storing the PriorityQueue. Horribly inefficient in comparison to some other ways of doing things, but this is still fairly quick for our purposes
        SortedDictionary<TPriority, Queue<TValue>> storage;
        
        /// <summary>
        /// Initializes the PriorityQueue
        /// </summary>
        public PriorityQueue()
        {
            storage = new SortedDictionary<TPriority, Queue<TValue>>();
        }

        /// <summary>
        /// Basic Enqueue function
        /// </summary>
        /// <param name="priority">The priority at which to enqueue a value</param>
        /// <param name="value">The value to enqueue</param>
        public void Enqueue(TPriority priority, TValue value)
        {
            //If the priority doesn't exist in our storage, add it in.
            if (!storage.ContainsKey(priority))
            {
                //Create a new queue and enqueue our value
                Queue<TValue> valQueue = new Queue<TValue>();
                valQueue.Enqueue(value);

                //add our new queue into storage
                storage.Add(priority, valQueue);
            }
            //Otherwise, enqueue our value at the priority
            else
            {
                storage[priority].Enqueue(value);
            }
        }

        /// <summary>
        /// Dequeues the value at a given priority
        /// </summary>
        /// <param name="priority">The priority to dequeue a value at</param>
        /// <returns>The dequeued value</returns>
        public TValue Dequeue(TPriority priority)
        {
            //If we have the priority stored, dequeue the value at the priority
            if (storage.ContainsKey(priority))
            {
                TValue returnable = storage[priority].Dequeue();

                //If the priority is now empty, remove it from storage.
                if (storage[priority].Count == 0)
                {
                    storage.Remove(priority);
                }

                return returnable;
            }
            //Otherwise throw an Exception
            else
                throw new KeyNotFoundException("No values exist at this priority");
        }

        /// <summary>
        /// Dequeues the value at the highest priority
        /// </summary>
        /// <returns>The dequeued value</returns>
        public TValue Dequeue()
        {
            //Figuring out how to get the first instance in the SortedDictionary was harder than I thought it would be. However, Google prevailed. There are lots of angle-braces here.

            //If there is something inside our storage, proceed with the dequeueing
            if (storage.Count != 0)
            {
                //Get the first KeyValuePair from storage
                KeyValuePair<TPriority, Queue<TValue>> pair = storage.First<KeyValuePair<TPriority, Queue<TValue>>>();

                //Dequeue the first value
                TValue returnable = pair.Value.Dequeue();

                //If there's nothing left at the highest priority, remove it from storage
                if (pair.Value.Count == 0)
                {
                    storage.Remove(pair.Key);
                }

                return returnable;
            }
            //Otherwise throw errors
            else
                throw new WTFException("The PriorityQueue is empty");
        }

        /// <summary>
        /// Gets the next value
        /// </summary>
        /// <returns>The next value</returns>
        public TValue Peek()
        {
            //If there is something inside our storage, proceed with the peeking
            if (storage.Count != 0)
            {
                KeyValuePair<TPriority, Queue<TValue>> pair = storage.First<KeyValuePair<TPriority, Queue<TValue>>>();
                return pair.Value.Peek();
            }
            else
                throw new WTFException("The PriorityQueue is empty");
        }

        /// <summary>
        /// Gets the next value at a given priority
        /// </summary>
        /// <returns>The next value at a given priority</returns>
        public TValue Peek(TPriority priority)
        {
            //If there is something stored at this priority, proceed with the peeking
            if (storage.ContainsKey(priority))
                return storage[priority].Peek();
            else
                throw new KeyNotFoundException("No values exist at this priority");
        }

        /// <summary>
        /// Dequeues all values at a given priority
        /// </summary>
        /// <param name="priority">The priority to dequeue</param>
        /// <returns>A queue representing all values at a given priority</returns>
        public Queue<TValue> DequeueEntirePriority(TPriority priority)
        {
            //If we have the priority in storage, delete it and return everything at that priority
            if (storage.ContainsKey(priority))
            {
                Queue<TValue> returnable = storage[priority];
                storage.Remove(priority);
                return returnable;
            }
            //Otherwise, exceptions!
            else
                throw new KeyNotFoundException("No values exist at this priority");
        }

        /// <summary>
        /// Tells whether or not our PriorityQueue is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return storage.Count == 0; }
        }
    }


    /*
     * A graph node interface meant to work with a pathfinding algorithm
     */
    interface IGraphNode
    {
        int Cost { get; set; }
        List<IGraphNode> Neighbors { get; set; }
    }
    class SquareGraphNode : IGraphNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool HasConnectionUpwards { get; set; }
        public bool HasConnectionDownwards { get; set; }

        public int Cost { get; set; }
        public List<IGraphNode> Neighbors { get; set; }

        public SquareGraphNode(int x, int y, int z, int cost)
        {
            X = x;
            Y = y;
            Z = z;
            Cost = cost;
            HasConnectionDownwards = false;
            HasConnectionUpwards = false;
            Neighbors = new List<IGraphNode>();
        }
    }
    class SquareGraph
    {
        int Width;
        int Height;
        int xSpace;
        int ySpace;
        List<SquareGraphNode> nodes;

        public SquareGraph(int width, int height, int xSpace, int ySpace)
        {
            Width = width;
            Height = height;
            this.xSpace = xSpace;
            this.ySpace = ySpace;
        }

        public SquareGraph(int width, int height, int xSpace, int ySpace, List<SquareGraphNode> nodes) : this(width, height, xSpace, ySpace)
        {
            this.nodes = nodes;
            CreateNeighborLists();
        }

        public void CreateNeighborLists()
        {
            foreach (SquareGraphNode node in nodes)
            {
                SquareGraphNode tempNode;
                SquareGraphNode left = nodes.Find(x => x.X == node.X - 1 && x.Y == node.Y && x.Z == node.Z);
                SquareGraphNode right = nodes.Find(x => x.X == node.X + 1 && x.Y == node.Y && x.Z == node.Z);
                SquareGraphNode up = nodes.Find(x => x.X == node.X && x.Y == node.Y - 1 && x.Z == node.Z);
                SquareGraphNode down = nodes.Find(x => x.X == node.X && x.Y == node.Y + 1 && x.Z == node.Z);
                if (left != null)
                {
                    //Add left to the list
                    node.Neighbors.Add(left);
                    if (up != null)
                    {
                        //Add the upper left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X - 1 && x.Y == node.Y - 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                    if (down != null)
                    {
                        //Add the lower left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X - 1 && x.Y == node.Y + 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                }
                if (right != null)
                {
                    //Add right to the list
                    node.Neighbors.Add(right);
                    if (up != null)
                    {
                        //Add the upper left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X + 1 && x.Y == node.Y - 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                    if (down != null)
                    {
                        //Add the lower left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X + 1 && x.Y == node.Y + 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                }
                //check up and down
                if (up != null)
                    node.Neighbors.Add(up);

                if (down != null)
                    node.Neighbors.Add(down);
            }
        }

        public SquareGraphNode GetNodeFromGraphCoords(int x, int y, int z)
        {
            SquareGraphNode tempNode = nodes.Find(n => n.X == x && n.Y == y && n.Z == z);
            if (tempNode != null)
                return tempNode;
            else
                throw new KeyNotFoundException("Could not find a node at this point");
        }

        public SquareGraphNode GetNode(int x, int y, int z)
        {
            return GetNodeFromGraphCoords(x / xSpace, y / ySpace, z);
        }
    }

    /// <summary>
    /// Well That Failed Exception
    /// </summary>
    class WTFException : Exception
    {
        public WTFException()
            : base("Something failed and there was no better default exception for it")
        {
        }

        public WTFException(string msg)
            : base(msg)
        {
        }
    }
}
