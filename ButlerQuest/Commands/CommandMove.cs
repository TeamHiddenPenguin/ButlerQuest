using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class CommandMove : ICommand
    {
        //Tells us whether the command is finished yet or not
        public bool IsFinished
        {
            get;
            set;
        }

        //A reference to the enemy we are trying to move (we need access to it's Move method)
        Enemy reference;
        //The direction we will end up moving in
        Vector3 direction;
        //The position we want to move to
        Vector3 endPosition;
        /// <summary>
        /// Constructs a CommandMove object
        /// </summary>
        /// <param name="endPosition">The position we want to move to</param>
        /// <param name="reference">A reference to the enemy we are trying to move (we need access to it's Move method)</param>
        public CommandMove(Vector3 endPosition, Enemy reference)
        {
            this.reference = reference;
            this.endPosition = endPosition;
        }

        /// <summary>
        /// Initializes this command
        /// </summary>
        public void Initialize()
        {
            Vector3 startPosition = new Vector3(reference.location.X, reference.location.Y, reference.location.Z);
            direction = endPosition - startPosition;
            if (direction == Vector3.Zero)
                IsFinished = true;
            else
                IsFinished = false;
        }

        /// <summary>
        /// Updates this command to move in a given direction
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Update(GameTime gameTime)
        {
            //If we're not done, do the following:
            if (!IsFinished)
            {
                //Move the object in our calculated direction
                reference.Move(direction);

                //The following set of nested if statements check if we have reached or overshot our target position, and corrects for it.
                if (direction.X >= 0)
                {
                    if (direction.Y >= 0)
                    {
                        if (reference.location.X >= endPosition.X && reference.location.Y >= endPosition.Y)
                        {
                            IsFinished = true;
                            reference.location = endPosition;
                            return;
                        }
                    }
                    else
                    {
                        if (reference.location.X >= endPosition.X && reference.location.Y < endPosition.Y)
                        {
                            IsFinished = true;
                            reference.location = endPosition;
                            return;
                        }
                    }
                }
                else
                {
                    if (direction.Y >= 0)
                    {
                        if (reference.location.X < endPosition.X && reference.location.Y >= endPosition.Y)
                        {
                            IsFinished = true;
                            reference.location = endPosition;
                            return;
                        }
                    }
                    else
                    {
                        if (reference.location.X < endPosition.X && reference.location.Y < endPosition.Y)
                        {
                            IsFinished = true;
                            reference.location = endPosition;
                            return;
                        }
                    }
                }
            }
        }
    }
}
