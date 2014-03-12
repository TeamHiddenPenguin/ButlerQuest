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
        Vector2 speed;
        Vector2 endPosition;
        public CommandMove(Vector2 endPosition, Enemy reference)
        {
            this.reference = reference;
            Vector2 startPosition = new Vector2(reference.location.X, reference.location.Y);
            this.endPosition = endPosition;
            speed = Vector2.Normalize((endPosition - startPosition)) * reference.velocity;
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
