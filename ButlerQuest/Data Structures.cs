//Written by Samuel Sternklar
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{

    /// <summary>
    /// The PriorityQueue is a well-known data structure. It's essentially a weighted Queue, where instead of being strictly "First In, First Out", it is "First Highest-Priority In,
    /// First Highest-Priority Out", if that makes any sense. Basically what that means is that no matter what order it is enqueued in, the highest priority value will be dequeued
    /// before anything else (unless the priority is specified). If two things of the same priority exist, then the first value to be added to the priority queue would be dequeued.
    /// If a priority is specified when dequeueing, then the first value to enqueued at that priority is dequeued.
    /// </summary>
    /// <typeparam name="TPriority">The type that will be used to determine the priority a value will be placed at</typeparam>
    /// <typeparam name="TValue">The type of the value that will be stored</typeparam>
    public class PriorityQueue<TPriority, TValue>
    {
        //How we're storing the PriorityQueue. SortedDictionary has worst case O(log(n)) add, remove, and lookup, the same as a Heap, which is normally used for PriorityQueues
        //As such, it's easier to use a SortedDictionary than it would be to write my own Heap implementation
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
            //Otherwise return the default value of our generic value type
            else
                return default(TValue);
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
            //Otherwise return the default value
            else
                return default(TValue);
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
                return default(TValue);
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
                return default(TValue);
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
            //Otherwise, return null
            else
                return default(Queue<TValue>);
        }

        /// <summary>
        /// Tells whether or not our PriorityQueue is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return storage.Count == 0; }
        }

        /// <summary>
        /// Tells us the number of elements in the PriorityQueue
        /// </summary>
        public int Count
        {
            get { return storage.Count; }
        }

        /// <summary>
        /// Tells us whether or not the PriorityQueue contains a thing at ANY priority
        /// </summary>
        /// <param name="thing">Thing to see if we have it or not</param>
        /// <returns>Whether or not we have the thing</returns>
        public bool Contains(TValue thing)
        {
            foreach (var prio in storage.Values)
            {
                if (prio.Contains(thing))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// A graph nod interface so that we can assure certain standard pieces between all types of graph nodes that we create
    /// so that we can re-use code (such as the A* implementation, if necessary)
    /// </summary>
    public interface IGraphNode
    {
        int X { get; set; }
        int Y { get; set; }
        int Z { get; set; }
        int Cost { get; set; }
        List<IGraphNode> Neighbors { get; set; }
    }

    /// <summary>
    /// A node to be used in a SquareGraph. It itself has no operations, but it contains all of the information required of it
    /// </summary>
    public class SquareGraphNode : IGraphNode
    {
        //The X coordinate of the node
        public int X { get; set; }
        //The Y coordinate
        public int Y { get; set; }
        //The Z coordinate
        public int Z { get; set; }

        //Tells whether or not there is a node at a Z coordinate directly above this node
        public bool HasConnectionUpwards { get; set; }
        //Tells whether or not there is a node at a Z coordinate directly below this node
        public bool HasConnectionDownwards { get; set; }

        //The cost of traversing this node
        public int Cost { get; set; }
        //This node's neighbors
        public List<IGraphNode> Neighbors { get; set; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="z">The Z coordinate</param>
        /// <param name="cost">The cost of the node</param>
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

    /// <summary>
    /// A "square graph", basically a grid
    /// </summary>
    public class SquareGraph
    {
        //The width of this graph (in nodes)
        int Width;
        //The height of this graph (in nodes)
        int Height;
        //The width of a node (in pixels)
        public int nodeWidth;
        //The height of a node (in pixels)
        public int nodeHeight;
        List<SquareGraphNode> nodes { get; set; }

        /// <summary>
        /// Create a square graph with no nodes in it
        /// </summary>
        /// <param name="width">the width of this graph</param>
        /// <param name="height">the heght of this graph</param>
        /// <param name="nodeWidth">the width of a node</param>
        /// <param name="nodeHeight">the height of a node</param>
        public SquareGraph(int width, int height, int nodeWidth, int nodeHeight)
        {
            Width = width;
            Height = height;
            this.nodeWidth = nodeWidth;
            this.nodeHeight = nodeHeight;
        }

        /// <summary>
        /// Create a square graph with nodes in it
        /// </summary>
        /// <param name="width">the width of this graph</param>
        /// <param name="height">the heght of this graph</param>
        /// <param name="nodeWidth">the width of a node</param>
        /// <param name="nodeHeight">the height of a node</param>
        /// <param name="nodes">An existing list of nodes to form connections between</param>
        public SquareGraph(int width, int height, int nodeWidth, int nodeHeight, List<SquareGraphNode> nodes) : this(width, height, nodeWidth, nodeHeight)
        {
            this.nodes = nodes;
            CreateNeighborLists();
        }

        /// <summary>
        /// Create connections between nodes
        /// </summary>
        public void CreateNeighborLists()
        {
            //Loop through every node
            foreach (SquareGraphNode node in nodes)
            {
                //Find the nodes directly left, up, and down from this node.
                SquareGraphNode tempNode;
                SquareGraphNode left = nodes.Find(x => x.X == node.X - 1 && x.Y == node.Y && x.Z == node.Z);
                SquareGraphNode right = nodes.Find(x => x.X == node.X + 1 && x.Y == node.Y && x.Z == node.Z);
                SquareGraphNode up = nodes.Find(x => x.X == node.X && x.Y == node.Y - 1 && x.Z == node.Z);
                SquareGraphNode down = nodes.Find(x => x.X == node.X && x.Y == node.Y + 1 && x.Z == node.Z);
                
                //If the node to the left of this one is not equal to null,
                if (left != null)
                {
                    //Add left to the list
                    node.Neighbors.Add(left);
                    //And if up is not equal to null, 
                    if (up != null)
                    {
                        //Add the upper left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X - 1 && x.Y == node.Y - 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                    //And if down is not equal to null,
                    if (down != null)
                    {
                        //Add the lower left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X - 1 && x.Y == node.Y + 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                }
                //If the right node is not equal to null,
                if (right != null)
                {
                    //Add right to the list
                    node.Neighbors.Add(right);
                    //And if up is not equal to null,
                    if (up != null)
                    {
                        //Add the upper left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X + 1 && x.Y == node.Y - 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                    //And if down is not equal to null,
                    if (down != null)
                    {
                        //Add the lower left node if it exists
                        if ((tempNode = nodes.Find(x => x.X == node.X + 1 && x.Y == node.Y + 1 && x.Z == node.Z)) != null)
                            node.Neighbors.Add(tempNode);
                    }
                }
                //Actually add up and down, did not want to before because it would be more checks.
                if (up != null)
                    node.Neighbors.Add(up);

                if (down != null)
                    node.Neighbors.Add(down);

                //If we have marked this node as having a z-axis connection, try to find it and add it to the list.
                if (node.HasConnectionDownwards)
                    if ((tempNode = nodes.Find(x => x.X == node.X && x.Y == node.Y && x.Z == node.Z + 1)) != null)
                        node.Neighbors.Add(tempNode);

                if (node.HasConnectionUpwards)
                    if ((tempNode = nodes.Find(x => x.X == node.X && x.Y == node.Y && x.Z == node.Z - 1)) != null)
                        node.Neighbors.Add(tempNode);
            }
        }

        /// <summary>
        /// Attempts to get a graph node from the list from node space, throws an error if one cannot be found
        /// </summary>
        /// <param name="x">X position in node space</param>
        /// <param name="y">Y position in node space</param>
        /// <param name="z">Z position in node space</param>
        /// <returns>The node at a given space, or an error</returns>
        public SquareGraphNode GetNodeFromGraphCoords(int x, int y, int z)
        {
            SquareGraphNode tempNode = nodes.Find(n => n.X == x && n.Y == y && n.Z == z);
            if (tempNode != null)
                return tempNode;
            else
                throw new KeyNotFoundException("Could not find a node at this point");
        }

        /// <summary>
        /// Attempts to get a graph world from the list from world space, throws an error if one cannot
        /// </summary>
        /// <param name="x">X position in world space</param>
        /// <param name="y">Y position in world space</param>
        /// <param name="z">Z position in world space</param>
        /// <returns>The node at a given space, or an error</returns>
        public SquareGraphNode GetNode(int x, int y, int z)
        {
            return GetNodeFromGraphCoords(x / nodeWidth, y / nodeHeight, z);
        }
    }

    public class RoomGraphNode : IGraphNode
    {
        //The X coordinate of the node(center)
        public int X { get; set; }
        //The Y coordinate(center)
        public int Y { get; set; }
        //The Z coordinate(center)
        public int Z { get; set; }
        //This node's neighbors
        public List<IGraphNode> Neighbors { get; set; }
        public HashSet<string> validDisguises;
        public Rectangle rect;
        public int Cost { get { return 0; } set { } }
        public string name;

        public RoomGraphNode(Rectangle rect, int z, string name)
        {
            this.rect = rect;
            X = rect.Center.X;
            Y = rect.Center.Y;
            Z = z;
            this.name = name;
            Neighbors = new List<IGraphNode>();
            validDisguises = new HashSet<string>();
        }

        public void AddNode(IGraphNode node)
        {
            if (node != null)
            {
                if (!node.Neighbors.Contains(this))
                    node.Neighbors.Add(this);
                if (!this.Neighbors.Contains(node))
                    this.Neighbors.Add(node);
            }
        }
        public void AddDisguise(string disguiseName)
        {
            if (!validDisguises.Contains(disguiseName))
                validDisguises.Add(disguiseName);
        }
    }

    public class RoomGraph
    {
        List<IGraphNode> nodes;
        public RoomGraph(params RoomGraphNode[] nodes)
        {
            this.nodes = nodes.ToList<IGraphNode>();
        }

        public RoomGraphNode GetNode(Rectangle rect)
        {
            return (RoomGraphNode)(nodes.Find(x => ((RoomGraphNode)x).rect.Intersects(rect)));
        }

        public RoomGraphNode GetNode(string name)
        {
            return (RoomGraphNode)(nodes.Find(x => ((RoomGraphNode)x).name == name));
        }

        public void AddNode(RoomGraphNode node)
        {
            nodes.Add(node);
        }

        public void AddNode(RoomGraphNode node, List<string> connections)
        {
            foreach (var s in connections)
            {
                node.AddNode(GetNode(s));
            }
            nodes.Add(node);
        }
    }

    /// <summary>
    /// Well That Failed Exception, used anywhere in this project where a better exception
    /// to throw could not be found, and when the writer is too lazy to make another exception
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
