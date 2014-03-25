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
        int lastDistance;
        public GetNextCommandSet(Enemy reference, int lastDistance)
        {
            this.reference = reference;
            this.lastDistance = lastDistance;
            Initialize();
        }

        public void Initialize()
        {
            IsFinished = false;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!AIManager.SharedAIManager.enemiesToPath.Contains(reference))
            {
                AIManager.SharedAIManager.enemiesToPath.Enqueue(lastDistance, reference);
            }
            if (reference.commandQueue.Count != 0 && reference.commandQueue.Peek() != null)
                IsFinished = true;
        }
    }
}
