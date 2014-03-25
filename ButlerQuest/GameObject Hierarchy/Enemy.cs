using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class Enemy : MovableGameObject
    {
        //AIState can be found in AIManager
        public AI_STATE state = AI_STATE.UNAWARE;
        public Vector3 startLocation;
        public ICommand currentCommand;
        public Queue<ICommand> defaultCommands;
        public Queue<ICommand> commandQueue;

        // constructor
        public Enemy(Vector3 vel, Animation[] animations, string[] names, Vector3 loc, Rectangle rect)
            : base(vel, animations, names, loc, rect)
        {
            commandQueue = new Queue<ICommand>();
            defaultCommands = new Queue<ICommand>();
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
            //base.Update(gameTime);

            if (currentCommand == null || currentCommand.IsFinished)
                ChangeCommand();

            
            currentCommand.Update(gameTime);
        }
    }
}
