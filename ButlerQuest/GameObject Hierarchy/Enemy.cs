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

        public void ChangeCommand()
        {
            if (commandQueue.Peek() != null)
                currentCommand = commandQueue.Dequeue();
            else
                currentCommand = new GetNextCommandSet(this, int.MaxValue);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentCommand.IsFinished)
                ChangeCommand();

            
            currentCommand.Update(gameTime);
        }
    }
}
