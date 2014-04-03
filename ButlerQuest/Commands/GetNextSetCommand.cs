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
        //Get a reference to the enemy to hand to the AIManager
        Enemy reference;
        //the current ditance from the enemy to the player
        int lastDistance;
        /// <summary>
        /// Constructs a new GetNextCommandSet command
        /// </summary>
        /// <param name="reference">the object to be handed to the AIManager</param>
        /// <param name="lastDistance">The last distance from the enemy to the player</param>
        public GetNextCommandSet(Enemy reference, int lastDistance)
        {
            this.reference = reference;
            this.lastDistance = lastDistance;
            Initialize();
        }

        /// <summary>
        /// Initialize the command
        /// </summary>
        public void Initialize()
        {
            IsFinished = false;
        }

        /// <summary>
        /// Update the command
        /// </summary>
        /// <param name="gameTime">a time component that we don't need for THIS command</param>
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //If the AIManager isn't already trying to path this enemy, add it to the list.
            if (!AIManager.SharedAIManager.enemiesToPath.Contains(reference))
            {
                AIManager.SharedAIManager.enemiesToPath.Enqueue(lastDistance, reference);
            }
            //If we have another command, finish this command.
            if (reference.commandQueue.Count != 0 && reference.commandQueue.Peek() != null)
                IsFinished = true;
        }
    }
}
