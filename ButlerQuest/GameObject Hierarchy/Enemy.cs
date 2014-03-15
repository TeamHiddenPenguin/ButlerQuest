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
        public Vector2 startLocation;
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

        public void Update(GameTime gameTime)
        {
            if (currentCommand.IsFinished)
                ChangeCommand();

            //TODO: Add update code in here
        }
    }
}
