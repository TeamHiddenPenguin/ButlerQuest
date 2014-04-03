using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    /// <summary>
    /// This class does nothing except hold a spot in the command queue. This may not even be necessary later, as at the time of writing this the Enemy class is not completed yet
    /// </summary>
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

        public void Initialize()
        {
            IsFinished = false;
        }

        public void Update(GameTime gameTime)
        {
            if (reference.commandQueue.Peek() != null)
                IsFinished = true;
        }
    }
}
