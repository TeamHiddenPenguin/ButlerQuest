﻿using Microsoft.Xna.Framework;
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
        public bool alive;

        public Rectangle CloseRect
        {
            get { return new Rectangle(rectangle.X + 10, rectangle.Y, rectangle.Width / 2, rectangle.Height); }
        }

        // constructor
        public Enemy(Vector3 vel, Animation[] animations, string[] names, Vector3 loc, Rectangle rect, int value)
            : base(vel, animations, names, loc, rect)
        {
            commandQueue = new Queue<ICommand>();
            defaultCommands = new Queue<ICommand>();
            startLocation = loc;
            state = AI_STATE.UNAWARE;
            center = new Vector3(rect.X + rect.Width, rect.Y + rect.Height, loc.Z);
            moneyValue = value;
            alive = true;
        }

        public void ChangeCommand()
        {
            if (commandQueue.Count != 0 && commandQueue.Peek() != null)
            {
                currentCommand = commandQueue.Dequeue();
                currentCommand.Initialize();
            }
            else
                currentCommand = new GetNextCommandSet(this, int.MaxValue);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (alive)
            {

                AIManager.SharedAIManager.RunAI(this);

                if (currentCommand == null || currentCommand.IsFinished)
                    ChangeCommand();

                if (currentCommand is CommandWait)
                {
                    CurrentAnimation = "default";
                }

                else
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
                        default:
                            break;
                    }
                }

                currentCommand.Update(gameTime);
            }
            else
            {
                CurrentAnimation = "Dead";
            }
        }
    }
}
