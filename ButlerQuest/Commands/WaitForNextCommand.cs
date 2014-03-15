using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class WaitForNextCommand : ICommand
    {

        public bool IsFinished
        {
            get;
            set;
        }
        Enemy reference;
        public WaitForNextCommand(Enemy reference)
        {
            this.reference = reference;
        }

        public void Update(GameTime gameTime)
        {
            if (reference.commandQueue.Peek() != null)
                IsFinished = true;
        }
    }
}
