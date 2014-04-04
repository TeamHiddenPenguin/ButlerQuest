/* AIManager.cs
 * Written by Samuel Sternklar
 * 
 * This class is a singleton that controls AI behavior when it requests new commands.
 */ 
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ButlerQuest;

namespace ButlerQuest
{
    /// <summary>
    /// Global enum representing the state of the AI
    /// </summary>
    public enum AI_STATE
    {
        UNAWARE = 0,
        AWARE,
        HUNTING,
        PURSUIT
    }

    class AIManager
    {
        int CommandsToCopy = 3;
        //The shared manager to be returned by the SharedManager property
        static private AIManager sharedManager;
        //The stored map that this AIManager is working from
        public Map map;
        //The level which this AIManager is working on
        public Level level;
        //The graph which this AIManager is working on
        public SquareGraph graph;
        //The priority queue of enemies to path. It would be a list, but if we run out of time per frame then we have to keep the ones that still need pathing on the next frame.
        public PriorityQueue<int, Enemy> enemiesToPath;
        //The last known location of the player. This is known to all enemies
        public Vector3 lastKnownPlayerLoc;

        //The shared AI manager to be used for all AI Management operations. If one doesn't exist, create it and return it.
        public static AIManager SharedAIManager
        {
            get
            {
                if (sharedManager != null)
                    return sharedManager;
                else
                    return new AIManager();
            }
        }

        public static void DebugInitialize(Level level)
        {
            SharedAIManager.level = level;
            SharedAIManager.map = SharedAIManager.level.levelMap;
            SharedAIManager.graph = SharedAIManager.map.Graph;
        }

        /// <summary>
        /// Constructor for the AIManager class
        /// </summary>
        private AIManager()
        {
            //If an AIManager doesn't exist, create one.
            if (sharedManager == null)
                sharedManager = this;
            //If one does exist, something went so horribly, horribly wrong, that a "Well That Failed" exception may not truly convey the severity of what just happened.
            else
                throw new WTFException("An instance of AIManager already exists. This should never happen. If it does, congrats you just called a private method outside of the class it exists in.");

            //Get our level, map, and graph, otherwise throw exceptions because we're doing something wrong
            if ((level = ScreenManager.SharedManager.GetCurrentGameScreen().level) == null)
                throw new WTFException("We are currently not inside a GameScreen and cannot create an AIManager, or the level has not been initiated yet. Stop trying to get the AIManager before creating the level!");
            if((map = level.levelMap) == null)
                throw new WTFException("The current map has not been initialized yet. Stop trying to get the AIManager before finishing building the level.");
            if((graph = map.Graph) == null)
                throw new WTFException("The graph for the current map has not been built yet. Stop trying to get the AIManager before finishing building the level.");
             
            //Create the PriorityQueue
            enemiesToPath = new PriorityQueue<int, Enemy>();
        }

        /// <summary>
        /// Makes or resets one path for an enemy in the PriorityQueue
        /// </summary>
        private void MakePath()
        {
            Enemy currentEnemy = enemiesToPath.Dequeue();
            if (currentEnemy != null)
            {
                switch (currentEnemy.state)
                {
                    //If the enemy is not actively doing something to the player, run the normal path
                    case AI_STATE.UNAWARE:
                    case AI_STATE.AWARE:
                        if (currentEnemy.location == currentEnemy.startLocation)
                        {
                            currentEnemy.commandQueue = new Queue<ICommand>(currentEnemy.defaultCommands);
                        }
                        else
                        {
                            currentEnemy.commandQueue = BuildPath(currentEnemy.location, currentEnemy.startLocation, currentEnemy, -1);
                            //copy all of the default commands into the command queue
                            foreach (var i in currentEnemy.defaultCommands)
                            {
                                currentEnemy.commandQueue.Enqueue(i);
                            }
                        }
                        break;
                    case AI_STATE.HUNTING:
                        //DO LATER THIS IS GONNA BE HARD
                    case AI_STATE.PURSUIT:
                        //build the path, factoring in the distance between the current location and the last known player location.
                        currentEnemy.commandQueue = BuildPath(currentEnemy.location, lastKnownPlayerLoc, currentEnemy, CommandsToCopy);
                        break;
                }
            }
        }

        /// <summary>
        /// Makes or resets all paths for any enemy in the PriorityQueue
        /// </summary>
        public void MakePaths()
        {
            //While we still have enemies to path, keep pathing!
            while (!enemiesToPath.IsEmpty)
            {
                MakePath();
            }
        }

        /// <summary>
        /// Builds a path between two nodes, and converts it into a queue of movement commands
        /// </summary>
        /// <param name="start">The starting node</param>
        /// <param name="end">The ending node</param>
        /// <param name="reference">The enemy we are pathfinding for</param>
        /// <param name="commandsToCopy">The number of commands to copy to the queue before requesting new commands</param>
        /// <returns></returns>
        private Queue<ICommand> BuildPath(Vector3 start, Vector3 end, Enemy reference, int commandsToCopy)
        {
            //Build the path.
            List<IGraphNode> path = AStar.FindPath(graph.GetNode((int)start.X, (int)start.Y, (int)start.Z), graph.GetNode((int)end.X, (int)end.Y, (int)end.Z));
            //create a list of commands to translate between a list of nodes and a queue of commands
            List<ICommand> translationList = new List<ICommand>();
            //Populate the translation list with commands from the graph nodes.
            foreach (var item in path)
            {
                translationList.Add(new CommandMove(new Vector3(item.X * graph.nodeWidth, item.Y * graph.nodeHeight, item.Z), reference));
            }
            
            //if commandsToCopy is less than or equal to 0, then we don't want to add any extra commands in because it will be gotten elsewhere.
            if (commandsToCopy > -1)
            {
                //If the number of commands we are trying to input before requesting new commands is greater than the total number of commands, just stick it at the end of the list.
                if (commandsToCopy >= path.Count)
                    translationList.Insert(path.Count, new GetNextCommandSet(reference, path[path.Count - 1].Cost));
                //Otherwise, place it after the number of commands it's trying to copy
                else
                    translationList.Insert(commandsToCopy + 1, new GetNextCommandSet(reference, path[commandsToCopy].Cost));

                //Copy a wait command to top off the list
                translationList.Add(new WaitForNextCommand(reference));
            }

            //Return a new queue of commands, gotten from the translation list
            return new Queue<ICommand>(translationList);
        }
    }
}
