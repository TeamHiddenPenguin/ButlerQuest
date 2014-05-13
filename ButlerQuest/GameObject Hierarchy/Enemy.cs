using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    public class Enemy : MovableGameObject
    {
        //AIState can be found in AIManager
        public AI_STATE state = AI_STATE.UNAWARE;
        public Vector3 startLocation;
        public ICommand currentCommand;
        public Queue<ICommand> defaultCommands;
        public Queue<ICommand> commandQueue;
        public double awareness = 0;
        
        public int moneyValue; // amount of money the enemy is worth
        public bool alive; // whether or not the enemy is alive

        // used to check if objects are close enough to use pixel collision. NOT USED
        public Rectangle CloseRect
        {
            get { return new Rectangle(rectangle.X + 10, rectangle.Y, rectangle.Width / 2, rectangle.Height); }
        }

        // constructor
        public Enemy(Vector3 vel, Animation[] animations, string[] names, Vector3 loc, Rectangle rect, int value, int dir)
            : base(vel, animations, names, loc, rect)
        {
            // sets up commands
            commandQueue = new Queue<ICommand>();
            defaultCommands = new Queue<ICommand>();

            // sets starting location
            startLocation = loc;

            // sets enemy to be unaware of player
            state = AI_STATE.UNAWARE;

            center = new Vector3(rect.X + rect.Width, rect.Y + rect.Height, loc.Z);

            moneyValue = value;
            alive = true;
            direction = dir;

            switch (direction)
            {
                case 0: CurrentAnimation = "StandUp";
                    break;
                case 1: CurrentAnimation = "StandRight";
                    break;
                case 2: CurrentAnimation = "StandDown";
                    break;
                case 3: CurrentAnimation = "StandLeft";
                    break;
            }
        }

        public void ChangeCommand() // goes to the next command in the priority queue.
        {
            if (commandQueue.Count != 0 && commandQueue.Peek() != null) // if priority queue isn't empty, gets next command
            {
                currentCommand = commandQueue.Dequeue();
                currentCommand.Initialize();
            }
            else
                currentCommand = new GetNextCommandSet(this, int.MaxValue); // priority queue is empty, and needs to be re-populated.
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (alive) // doesn't bother doing anything if the enemy is dead.
            {

                AIManager.SharedAIManager.RunAI(this); // adds the enemy to the shared manager for AI updates

                if (currentCommand == null || currentCommand.IsFinished) // current command is done and needs a new one.
                    ChangeCommand();

                if (currentCommand is CommandWait) // enemy is standing still, so standing animation is played.
                {
                    switch (direction)
                    {
                        case 0: CurrentAnimation = "StandUp";
                            break;
                        case 1: CurrentAnimation = "StandRight";
                            break;
                        case 2: CurrentAnimation = "StandDown";
                            break;
                        case 3: CurrentAnimation = "StandLeft";
                            break;
                    }
                }

                else // enemy is moving somewhere, so walking animation is played.
                {
                    switch (direction)
                    {
                        case 0: CurrentAnimation = "WalkUp";
                            break;
                        case 1: CurrentAnimation = "WalkRight";
                            break;
                        case 2: CurrentAnimation = "WalkDown";
                            break;
                        case 3: CurrentAnimation = "WalkLeft";
                            break;
                    }
                }

                currentCommand.Update(gameTime); // updates current command as necessary.
            }

            else
            {
                CurrentAnimation = "Dead"; // enemy will not be drawn.
            }
        }
    }
}
