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
        Vector3 speed;
        Vector3 endPosition;
        public CommandMove(Vector3 endPosition, Enemy reference)
        {
            this.reference = reference;
            Vector3 startPosition = new Vector3(reference.location.X, reference.location.Y, reference.location.Z);
            this.endPosition = endPosition;
            speed = Vector3.Normalize((endPosition - startPosition)) * reference.velocity;
        }

        public void Update(GameTime gameTime)
        {
            reference.Move(speed);
            if (speed.X >= 0)
            {
                if (speed.Y >= 0)
                {
                    if (reference.location.X > endPosition.X && reference.location.Y > endPosition.Y)
                    {
                        IsFinished = true;
                        reference.location = endPosition;
                    }
                }
                else
                {
                    if (reference.location.X > endPosition.X && reference.location.Y < endPosition.Y)
                    {
                        IsFinished = true;
                        reference.location = endPosition;
                    }
                }
            }
            else
            {
                if (speed.Y >= 0)
                {
                    if (reference.location.X < endPosition.X && reference.location.Y > endPosition.Y)
                    {
                        IsFinished = true;
                        reference.location = endPosition;
                    }
                }
                else
                {
                    if (reference.location.X < endPosition.X && reference.location.Y < endPosition.Y)
                    {
                        IsFinished = true;
                        reference.location = endPosition;
                    }
                }
            }
        }
    }
}
