using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ButlerQuest;

namespace ButlerQuest
{
    enum AI_STATE
    {
        UNAWARE = 0,
        AWARE,
        HUNTING,
        PURSUIT
    }

    class AIManager
    {
        static private AIManager sharedManager;
        private Map map;
        //private Level level;
        private SquareGraph graph;
        public PriorityQueue<int, Enemy> enemiesToPath;
        private Vector2 lastKnownPlayerLoc;
        int frequencyFactor;
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

        private AIManager()
        {
            if (sharedManager == null)
                sharedManager = this;
            else
                throw new WTFException("An instance of AIManager already exists. This should never happen");

            /*if ((level = (ScreenManager.GetCurrentScreen() as GameScreen).level;) == null)
                throw new WTFException("We are currently not inside a GameScreen and cannot create an AIManager, or the level has not been initiated yet. Stop trying to get the AIManager before creating the level!");
            if((map = level.map) == null)
                throw new WTFException("The current map has not been initialized yet. Stop trying to get the AIManager before finishing building the level.");
            if((graph = map.Graph) == null)
                throw new WTFException("The graph for the current map has not been built yet. Stop trying to get the AIManager before finishing building the level.");
             
             */

            enemiesToPath = new PriorityQueue<int, Enemy>();
        }

        //If we run it async, use this
        public void ExecuteCommandsAsync()
        {
            Enemy currentEnemy = enemiesToPath.Dequeue();
            if (currentEnemy != null)
            {
                switch (currentEnemy.state)
                {
                    case AI_STATE.UNAWARE:
                    case AI_STATE.AWARE:
                        if (currentEnemy.location == currentEnemy.startLocation)
                        {
                            currentEnemy.commandQueue = new Queue<ICommand>(currentEnemy.defaultCommands);
                        }
                        else
                        {
                            currentEnemy.commandQueue = BuildPath(currentEnemy.location, currentEnemy.startLocation, currentEnemy, currentEnemy.defaultCommands.Count);
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
                        //build the path, factoring in the distance between the current location and the last known player location. FrequencyFactor determines how quickly we should poll for input.
                        currentEnemy.commandQueue = BuildPath(currentEnemy.location, lastKnownPlayerLoc, currentEnemy, ((int)(Math.Abs(lastKnownPlayerLoc.X - currentEnemy.location.X) + Math.Abs(lastKnownPlayerLoc.Y - currentEnemy.location.Y)) / frequencyFactor) + 1);
                        break;
                }
            }
        }

        private Queue<ICommand> BuildPath(Vector2 start, Vector2 end, Enemy reference, int commandsToCopy)
        {
            Queue<ICommand> retQueue = new Queue<ICommand>();
            Path<SquareGraphNode> path = AStar.FindPath<SquareGraphNode>(graph.GetNode((int)end.X, (int)end.Y, 0), graph.GetNode((int)start.X, (int)start.Y, 0), AStar.ManhattanDistance, null);
            while (path != null || commandsToCopy > 0)
            {
                retQueue.Enqueue(new CommandMove(new Vector2(path.LastStep.X, path.LastStep.Y), reference));
                path = path.PreviousSteps;
                commandsToCopy--;
            }
            retQueue.Enqueue(new GetNextCommandSet(reference, path.LastStep.Cost));
            while (path != null)
            {
                retQueue.Enqueue(new CommandMove(new Vector2(path.LastStep.X, path.LastStep.Y), reference));
                path = path.PreviousSteps;
            }
            retQueue.Enqueue(new WaitForNextCommand(reference));
            return retQueue;
        }
    }
}
