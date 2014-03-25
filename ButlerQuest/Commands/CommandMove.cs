using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class CommandMove : ICommand
    {
        public bool IsFinished
        {
            get;
            set;
        }

        Enemy reference;
        Vector3 direction;
        Vector3 endPosition;
        public CommandMove(Vector3 endPosition, Enemy reference)
        {
            this.reference = reference;
            Vector3 startPosition = new Vector3(reference.location.X, reference.location.Y, reference.location.Z);
            this.endPosition = endPosition;
            direction = endPosition - startPosition;
        }

        public void Update(GameTime gameTime)
        {
            if (direction == Vector3.Zero)
            {
                IsFinished = true;
                return;
            }
            reference.Move(direction);
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
