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
        public bool alive;

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


            AIManager.SharedAIManager.RunAI(this);

            if (currentCommand == null || currentCommand.IsFinished)
                ChangeCommand();

            
            currentCommand.Update(gameTime);
            if (alive)
            {
                if (currentCommand == null || currentCommand.IsFinished)
                    ChangeCommand();

                AIManager.SharedAIManager.RunAI(this);

                currentCommand.Update(gameTime);
            }
            else
            {
                CurrentAnimation = "Dead";
            }
        }
    }
}
