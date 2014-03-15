using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class GetNextCommandSet : ICommand
    {
        public bool IsFinished
        {
            get;
            set;
        }
        Enemy reference;
        public GetNextCommandSet(Enemy reference, int lastDistance)
        {
            this.reference = reference;
            AIManager.SharedAIManager.enemiesToPath.Enqueue(lastDistance, reference);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (reference.commandQueue.Peek() != null)
                IsFinished = true;
        }
    }
}
