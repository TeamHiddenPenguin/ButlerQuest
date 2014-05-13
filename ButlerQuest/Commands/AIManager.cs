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
            if (path == null)
            {
                return new Queue<ICommand>();
            }

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
            //If our enemy state is unaware or aware but not actively pursuing, do this set of stuff.
            if (enemy.state < AI_STATE.HUNTING)
            {
                //If the enemy is on the same floor as the player, do stuff. Otherwise there's no way to see the player so don't bother
                if (enemy.center.Z == playerLoc.Z)
                {
                    //Find the distance between the player and the enemy
                    double dist = Vector3.DistanceSquared(enemy.location, playerLoc);
                    //If we are within vision radius, do stuff. Otherwise we can't see the player and the enemy goes on his merry way.
                    if (dist < MAX_VISION_RADIUS_SQUARED && CanSee(enemy, playerLoc) && !WallInWay(enemy))
                    {
                        //Update the player's last known location
                        lastKnownPlayerLoc = playerLoc;

                        //If the player is suspicious, rapidly increase his awareness based on the distance between him and us.
                        if (AIManager.SharedAIManager.PlayerIsSuspicious())
                        {
                            enemy.awareness += (MAX_VISION_RADIUS_SQUARED / dist) * .02;
                        }
                        //If the player isn't suspicious, slowly increase his awareness.
                        else
                        {
                            enemy.awareness += (MAX_VISION_RADIUS_SQUARED / dist) * .0009;   
                        }
                        //If our enemy's awareness is greater than 1, begin pursuing the player.
                        if (enemy.awareness >= 1)
                        {
                            AddToPursuit(enemy);
                            SwitchPursuitState(AI_STATE.PURSUIT);
                        }
                    }
                }
            }
            //If our enemy state is currently pursuing the player
            if (enemy.state == AI_STATE.PURSUIT)
            {
                //Find the distance between the enemy in the player
                double dist = Vector3.DistanceSquared(enemy.location, playerLoc);
                //Find every enemy on the same floor as the player and within the current enemy's vision radius and add them to the current pursuit
                foreach (var other in level.basicEnemies.Where(x => x.location.Z == enemy.location.Z && Vector3.DistanceSquared(enemy.location, x.location) < MAX_VISION_RADIUS_SQUARED_PURSUIT))
                {
                    AddToPursuit(other);
                }
                //If the player is within the current vision radius and not behind any walls, update his current known location
                if (dist < MAX_VISION_RADIUS_SQUARED_PURSUIT)
                {
                    lastKnownPlayerLoc = playerLoc;
                }
                //If we can't see the player and are at the end of the path, switch the pursuit to hunting state because something went wrong.
                else if (map.Graph.GetNode((int)enemy.center.X, (int)enemy.center.Y, (int) enemy.center.Z) == map.Graph.GetNode((int)lastKnownPlayerLoc.X, (int)lastKnownPlayerLoc.Y, (int)lastKnownPlayerLoc.Z))
                {
                    SwitchPursuitState(AI_STATE.HUNTING);
                }
            }
            //If the enemy is currently hunting the player
            if (enemy.state == AI_STATE.HUNTING)
            {
                //slowly decrease the enemy's awareness
                enemy.awareness -= .0005;
                //If we are within vision radius, can see the player, and do not have a wall in the way...
                if (Vector3.DistanceSquared(enemy.location, playerLoc) < MAX_VISION_RADIUS_SQUARED_PURSUIT && CanSee(enemy, playerLoc) && !WallInWay(enemy))
                {
                    //give the player 1/6 of a second or less to get out of view range, and if he fails to do so re-enter pursuit mode.
                    enemy.awareness += .1;
                    if (enemy.awareness > 1)
                    {
                        lastKnownPlayerLoc = playerLoc;
                        SwitchPursuitState(AI_STATE.PURSUIT);
                    }
                }
                //If our awareness hits 0, exit hunting mode and resume normal pathfinding
                if (enemy.awareness <= 0)
                {
                    enemy.awareness = 0;
                    SwitchPursuitState(AI_STATE.AWARE);
                }
            }
        }

        /// <summary>
        /// Checks if the player is currently suspicious.
        /// The player is suspicious if he is currently wearing a disguise that is not available in the current room, or if the player is currently swinging a weapon
        /// </summary>
        /// <returns>Whether or not the player is suspicious</returns>
        private bool PlayerIsSuspicious()
        {
            if ((level.player.currentDisguise != null && level.roomGraph.GetNode(level.player.rectangle).validDisguises.Contains(level.player.currentDisguise.disguiseType)) || (level.player.currentWeapon != null && level.player.currentWeapon.visible))
                return false;
            return true;
        }

        /// <summary>
        /// Adds an enemy to the pursuit
        /// </summary>
        /// <param name="e">enemy to add to the pursuit</param>
        private void AddToPursuit(Enemy e)
        {
            //If a pursuit is not currently active, activate one
            if (!isPursuitActive)
            {
                currentPursuit = new HashSet<Enemy>();
                isPursuitActive = true;
            }
            //If the current enemy is already in the pursuit, don't add him in.
            if(!currentPursuit.Contains(e))
                currentPursuit.Add(e);
        }

        /// <summary>
        /// Remove an enemy from the pursuit
        /// </summary>
        /// <param name="e">enemy to remove from the pursuit</param>
        public void RemoveFromPursuit(Enemy e)
        {
            //If the current pursuit doesn't have the enemy, don't bother removing it. Otherwise remove it.
            if(currentPursuit.Contains(e))
            {
                currentPursuit.Remove(e);
                //If the current pursuit is empty, end it.
                if (currentPursuit.Count == 0)
                    isPursuitActive = false;
            }
        }

        /// <summary>
        /// Switches the state of the current pursuit to a given state
        /// </summary>
        /// <param name="state">state to switch the pursuit to</param>
        private void SwitchPursuitState(AI_STATE state)
        {
            //For every enemy in the pursuit, change their state to the new state and have them get new commands.
            foreach (Enemy enemy in currentPursuit)
            {
                enemy.commandQueue.Clear();
                enemy.currentCommand = null;
                enemy.state = state;
            }
            //If we are switching to hunting, begin the hunt.
            if (state == AI_STATE.HUNTING)
            {
                BeginHunt();
            }
            //If the enemy state is no longer hunting, end the current pursuit.
            if (state < AI_STATE.HUNTING)
                isPursuitActive = false;
        }

        /// <summary>
        /// Begins a hunt
        /// </summary>
        private void BeginHunt()
        {
            //Get the current room that the enemy is in.
            RoomGraphNode currentRoom = level.roomGraph.GetNode(new Rectangle((int)(lastKnownPlayerLoc.X), (int)(lastKnownPlayerLoc.Y), 1, 1));

            //Find the number of rooms to search
            int roomsToSearch = (int)Math.Ceiling(currentRoom.Neighbors.Count * PERCENTAGE_ROOMS_TO_SEARCH);
            //Create a queue of rooms to search
            Queue<RoomGraphNode> rooms = new Queue<RoomGraphNode>();
            //Fill the room queue randomly with the rooms to search.
            for (int i = 0; i < roomsToSearch; i++)
            {
                rooms.Enqueue((RoomGraphNode)currentRoom.Neighbors[rand.Next(currentRoom.Neighbors.Count)]);
            }
            //Find the number of enemies to search. It should be no greater than 4, and the number of enemies searching should not be larger than the number of rooms to search.
            int enemiesSearching = Math.Min(currentPursuit.Count, 4);
            enemiesSearching = Math.Min(enemiesSearching, roomsToSearch);

            //If there are no enemies searching, return. Enemies will default to the "UNAWARE" state.
            if (enemiesSearching < 1)
                return;

            //Find the number of rooms that each enemy should search.
            double roomsPerEnemy = roomsToSearch / enemiesSearching;

            //Keep track of how many enemies have rooms to search.
            int count = 0;
            //Give enemies their commands.
            foreach(Enemy enemy in currentPursuit)
            {
                //If we have sent as many enemies to rooms as we should be, exit the loop.
                if (count >= enemiesSearching)
                    break;

                //Get the current enemy location
                Vector3 loc = enemy.location;
                //For each room that this enemy must search...
                for(int j = 0; j < roomsPerEnemy; j++)
                {
                    //Get the next room to search
                    RoomGraphNode next = rooms.Dequeue();
                    //Build a path from the enemy's current location to the next room.
                    Vector3 nextLoc = new Vector3(next.X, next.Y, next.Z);
                    Queue<ICommand> toNextRoom = BuildPath(loc, nextLoc, enemy, -1);
                    //update the current location.
                    loc = nextLoc;

                    //Enqueue the current path from the current room to the next room into the enemy's commands
                    foreach (var element in toNextRoom)
                    {
                        enemy.commandQueue.Enqueue(element);
                    }
                    //Wait 2 seconds before moving on.
                    enemy.commandQueue.Enqueue(new CommandWait(120));
                }
                //Increment the counter
                count++;
            }
        }

        /// <summary>
        /// Checks if a point lies within an enemy's vision cone
        /// </summary>
        /// <param name="enemy">enemy that needs to know if he can see</param>
        /// <param name="point">point that the enemy is trying to see</param>
        /// <returns>whether or not the point lies within the enemy</returns>
        private bool CanSee(Enemy enemy, Vector3 point)
        {
            //find the minimum angle in radians of the vision cone
            double minAngle = ((90 - (90 * enemy.direction)) - VISION_CONE_ANGLE_DEGREES) * DEG_TO_RAD;
            //find the maximum angle in radians of the vision cone
            double maxAngle = ((90 - (90 * enemy.direction)) + VISION_CONE_ANGLE_DEGREES) * DEG_TO_RAD;
            //find the angle between the enemy and the point
            double angle = -Math.Atan2((point.Y - enemy.center.Y), (point.X - enemy.center.X));
            //return if the angle is between the minimum angle and the maximum angle
            return angle < maxAngle && angle > minAngle;
        }

        /// <summary>
        /// Checks if a wall is between the enemy and the current player
        /// </summary>
        /// <param name="enemy">the enemy to check against</param>
        /// <returns>Whether or not the enemy can see the player</returns>
        private bool WallInWay(Enemy enemy)
        {
            //For each wall on the same floor as the enemy...
            foreach (var wall in level.walls.Where(x => x.location.Z == playerLoc.Z))
            {
                //If a collision is possible (ie the enemy and the player are not both on one side of the wall)...
                if ((enemy.center.X > wall.rectangle.Right && playerLoc.X > wall.rectangle.Right) ||
                    (enemy.center.X < wall.rectangle.Left && playerLoc.X < wall.rectangle.Left) ||
                    (enemy.center.Y > wall.rectangle.Top && playerLoc.Y > wall.rectangle.Top) ||
                    (enemy.center.Y < wall.rectangle.Bottom && playerLoc.Y < wall.rectangle.Bottom))
                {
                    //Create a vector between the player and the enemy
                    Vector2 v = new Vector2(playerLoc.X - enemy.center.X, playerLoc.Y - enemy.center.Y);
                    //Create vectors representing each of the wall's rectangle's sides
                    Vector2[] wallVecs = new Vector2[4];
                    wallVecs[0] = new Vector2(wall.rectangle.Left, wall.rectangle.Bottom - wall.rectangle.Top);
                    wallVecs[1] = new Vector2(wall.rectangle.Right, wall.rectangle.Bottom - wall.rectangle.Top);
                    wallVecs[2] = new Vector2(wall.rectangle.Right - wall.rectangle.Left, wall.rectangle.Top);
                    wallVecs[3] = new Vector2(wall.rectangle.Right - wall.rectangle.Left, wall.rectangle.Bottom);
                    bool checker = false;
                    //For each wall side, if the sight line does not intersect the wall, return false.
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
