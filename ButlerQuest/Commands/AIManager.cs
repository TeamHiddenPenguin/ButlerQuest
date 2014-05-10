﻿/* AIManager.cs
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
        //The priority queue of enemies to path. It would be a list, but if we run out of time per frame then we have to keep the ones that still need pathing on the next frame.
        public PriorityQueue<int, Enemy> enemiesToPath;
        //The last known location of the player. This is known to all enemies
        public Vector3 lastKnownPlayerLoc;
        //The current player location, used to check visibility against
        public Vector3 playerLoc;
        //The enemies involved in the current pursuit
        private HashSet<Enemy> currentPursuit;
        private bool isPursuitActive;

        private Random rand;

        public const float MAX_VISION_RADIUS_SQUARED = 50000;
        public const float MAX_VISION_RADIUS_SQUARED_PURSUIT = MAX_VISION_RADIUS_SQUARED;// + 5000;
        public const float VISION_CONE_ANGLE_DEGREES = 55;
        public const float DEG_TO_RAD = 0.0174532925f;
        public const double PERCENTAGE_ROOMS_TO_SEARCH = 2.0 / 3.0;

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
             
            //Create the PriorityQueue
            enemiesToPath = new PriorityQueue<int, Enemy>();

            currentPursuit = new HashSet<Enemy>();
            isPursuitActive = false;

            rand = new Random();
        }

        public void Reinitialize()
        {
            if ((level = ScreenManager.SharedManager.GetCurrentGameScreen().level) == null)
                throw new WTFException("We are currently not inside a GameScreen and cannot create an AIManager, or the level has not been initiated yet. Stop trying to get the AIManager before creating the level!");
            if ((map = level.levelMap) == null)
                throw new WTFException("The current map has not been initialized yet. Stop trying to get the AIManager before finishing building the level.");

            //Create the PriorityQueue
            enemiesToPath = new PriorityQueue<int, Enemy>();

            currentPursuit = new HashSet<Enemy>();
            isPursuitActive = false;
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
                        //If we ever hit this case, we simply switch the AI state back to Aware. This is because if we ever finish searching spots when hunting, we haven't found the player,
                        //so we want the enemy to go back to his path.
                        currentEnemy.state = AI_STATE.AWARE;
                        enemiesToPath.Enqueue(int.MaxValue, currentEnemy);
                        break;
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
            List<IGraphNode> path = AStar.FindPath(map.Graph.GetNode((int)start.X, (int)start.Y, (int)start.Z), map.Graph.GetNode((int)end.X, (int)end.Y, (int)end.Z));
            //create a list of commands to translate between a list of nodes and a queue of commands
            List<ICommand> translationList = new List<ICommand>();
            //Populate the translation list with commands from the graph nodes.
            for (int i = 1; i < path.Count; i++)
            {
                var item = path[i];
                translationList.Add(new CommandMove(new Vector3(item.X * map.Graph.nodeWidth, item.Y * map.Graph.nodeHeight, item.Z), reference));
            }
            
            //if commandsToCopy is less than or equal to 0, then we don't want to add any extra commands in because it will be gotten elsewhere.
            if (commandsToCopy > -1)
            {
                //If the number of commands we are trying to input before requesting new commands is greater than the total number of commands, just stick it at the end of the list.
                if (commandsToCopy + 1 >= path.Count)
                    translationList.Insert(path.Count - 1, new GetNextCommandSet(reference, path[path.Count - 1].Cost));
                //Otherwise, place it after the number of commands it's trying to copy
                else
                    translationList.Insert(commandsToCopy + 1, new GetNextCommandSet(reference, path[commandsToCopy].Cost));

                //Copy a wait command to top off the list
                translationList.Add(new WaitForNextCommand(reference));
            }

            //Return a new queue of commands, gotten from the translation list
            return new Queue<ICommand>(translationList);
        }
        
        /// <summary>
        /// Runs the AI for the enemy passed in
        /// </summary>
        /// <param name="enemy">Enemy to run the AI on</param>
        public void RunAI(Enemy enemy)
        {
            if (enemy.state < AI_STATE.HUNTING)
            {
                if (enemy.center.Z == playerLoc.Z)
                {
                    double dist = Vector3.DistanceSquared(enemy.location, playerLoc);
                    if (dist < MAX_VISION_RADIUS_SQUARED && CanSee(enemy, playerLoc) && !WallInWay(enemy, (int)dist))
                    {
                        lastKnownPlayerLoc = playerLoc;

                        if (AIManager.SharedAIManager.PlayerIsSuspicious())
                        {
                            enemy.awareness += (MAX_VISION_RADIUS_SQUARED / dist) * .02;
                            if (enemy.awareness >= 1)
                            {
                                AddToPursuit(enemy);
                                SwitchPursuitState(AI_STATE.PURSUIT);
                            }
                        }
                        else
                        {
                            enemy.awareness += (MAX_VISION_RADIUS_SQUARED / dist) * .0009;
                            if (enemy.awareness >= 1)
                            {
                                AddToPursuit(enemy);
                                SwitchPursuitState(AI_STATE.PURSUIT);
                            }
                        }
                    }
                }
            }
            if (enemy.state == AI_STATE.PURSUIT)
            {
                double dist = Vector3.DistanceSquared(enemy.location, playerLoc);
                foreach (var other in level.basicEnemies.Where(x => x.location.Z == enemy.location.Z && Vector3.DistanceSquared(enemy.location, x.location) < MAX_VISION_RADIUS_SQUARED_PURSUIT))
                {
                    AddToPursuit(other);
                }
                if (dist < MAX_VISION_RADIUS_SQUARED_PURSUIT)
                {
                    lastKnownPlayerLoc = playerLoc;
                }
                else if (map.Graph.GetNode((int)enemy.center.X, (int)enemy.center.Y, (int) enemy.center.Z) == map.Graph.GetNode((int)lastKnownPlayerLoc.X, (int)lastKnownPlayerLoc.Y, (int)lastKnownPlayerLoc.Z))
                {
                    SwitchPursuitState(AI_STATE.HUNTING);
                }
            }
            if (enemy.state == AI_STATE.HUNTING)
            {
                enemy.awareness -= .0005;
                if (Vector3.DistanceSquared(enemy.location, playerLoc) < MAX_VISION_RADIUS_SQUARED_PURSUIT && CanSee(enemy, playerLoc) && !WallInWay(enemy, (int)Vector3.DistanceSquared(enemy.location, playerLoc)))
                {
                    enemy.awareness += .1;
                    if (enemy.awareness > 1)
                    {
                        lastKnownPlayerLoc = playerLoc;
                        SwitchPursuitState(AI_STATE.PURSUIT);
                    }
                }
                if (enemy.awareness <= 0)
                {
                    enemy.awareness = 0;
                    SwitchPursuitState(AI_STATE.AWARE);
                }
            }
        }

        public bool PlayerIsSuspicious()
        {
            if (level.roomGraph.GetNode(level.player.rectangle).validDisguises.Contains(level.player.currentDisguise.disguiseType) || (level.player.currentWeapon != null && level.player.currentWeapon.visible))
                return false;
            return true;
        }

        public void AddToPursuit(Enemy e)
        {
            if (!isPursuitActive)
            {
                currentPursuit = new HashSet<Enemy>();
                isPursuitActive = true;
            }
            if(!currentPursuit.Contains(e))
                currentPursuit.Add(e);
        }

        public void RemoveFromPursuit(Enemy e)
        {
            if(currentPursuit.Contains(e))
            {
                currentPursuit.Remove(e);
                if (currentPursuit.Count == 0)
                    isPursuitActive = false;
            }
        }

        public void SwitchPursuitState(AI_STATE state)
        {
            foreach (Enemy enemy in currentPursuit)
            {
                enemy.commandQueue.Clear();
                enemy.currentCommand = null;
                enemy.state = state;
            }
            if (state == AI_STATE.HUNTING)
            {
                System.Diagnostics.Debug.WriteLine("Began Hunt");
                BeginHunt();
            }
            if (state < AI_STATE.HUNTING)
                isPursuitActive = false;
        }

        public void BeginHunt()
        {
            RoomGraphNode currentRoom = level.roomGraph.GetNode(new Rectangle((int)(lastKnownPlayerLoc.X), (int)(lastKnownPlayerLoc.Y), 1, 1));

            int roomsToSearch = (int)Math.Ceiling(currentRoom.Neighbors.Count * PERCENTAGE_ROOMS_TO_SEARCH);
            Queue<RoomGraphNode> rooms = new Queue<RoomGraphNode>();

            for (int i = 0; i < roomsToSearch; i++)
            {
                rooms.Enqueue((RoomGraphNode)currentRoom.Neighbors[rand.Next(currentRoom.Neighbors.Count)]);
            }
            int enemiesSearching = Math.Min(currentPursuit.Count, 4);
            enemiesSearching = Math.Min(enemiesSearching, roomsToSearch);

            double roomsPerEnemy = roomsToSearch / enemiesSearching;

            //MAKE MORE EFFICIENT

            int count = 0;
            foreach(Enemy enemy in currentPursuit)
            {
                if (count >= enemiesSearching)
                    break;

                Vector3 loc = enemy.location;
                for(int j = 0; j < roomsPerEnemy; j++)
                {
                    RoomGraphNode next = rooms.Dequeue();
                    Vector3 nextLoc = new Vector3(next.X, next.Y, next.Z);
                    Queue<ICommand> toNextRoom = BuildPath(loc, nextLoc, enemy, -1);
                    loc = nextLoc;

                    foreach (var element in toNextRoom)
                    {
                        enemy.commandQueue.Enqueue(element);
                    }
                    enemy.commandQueue.Enqueue(new CommandWait(120));
                }
                count++;
            }
        }

        public bool CanSee(Enemy enemy, Vector3 point)
        {
            double minAngle = ((90 - (90 * enemy.direction)) - VISION_CONE_ANGLE_DEGREES) * DEG_TO_RAD;
            double maxAngle = ((90 - (90 * enemy.direction)) + VISION_CONE_ANGLE_DEGREES) * DEG_TO_RAD;
            double angle = -Math.Atan2((point.Y - enemy.center.Y), (point.X - enemy.center.X));
            return angle < maxAngle && angle > minAngle;
        }

        public bool WallInWay(Enemy enemy,int dist)
        {
            foreach (var wall in level.walls.Where(x => x.location.Z == playerLoc.Z /*&& Vector3.DistanceSquared(enemy.center, x.center) < dist*/))
            {
                if ((enemy.center.X > wall.rectangle.Right && playerLoc.X > wall.rectangle.Right) ||
                    (enemy.center.X < wall.rectangle.Left && playerLoc.X < wall.rectangle.Left) ||
                    (enemy.center.Y > wall.rectangle.Top && playerLoc.Y > wall.rectangle.Top) ||
                    (enemy.center.Y < wall.rectangle.Bottom && playerLoc.Y < wall.rectangle.Bottom))
                {
                    Vector2 v = new Vector2(playerLoc.X - enemy.center.X, playerLoc.Y - enemy.center.Y);
                    Vector2[] wallVecs = new Vector2[4];
                    wallVecs[0] = new Vector2(wall.rectangle.Left, wall.rectangle.Bottom - wall.rectangle.Top);
                    wallVecs[1] = new Vector2(wall.rectangle.Right, wall.rectangle.Bottom - wall.rectangle.Top);
                    wallVecs[2] = new Vector2(wall.rectangle.Right - wall.rectangle.Left, wall.rectangle.Top);
                    wallVecs[3] = new Vector2(wall.rectangle.Right - wall.rectangle.Left, wall.rectangle.Bottom);
                    bool checker = false;
                    foreach (var wallVec in wallVecs)
                    {
                        if (wallVec.X / v.X == wallVec.Y / v.Y)
                        {
                            checker = true;
                            break;
                        }
                    }
                    if (!checker)
                        continue;
                }
                return true;
            }
            return false;
        }
    }
}
