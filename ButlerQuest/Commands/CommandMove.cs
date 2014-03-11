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

        IEnemy reference;
        Vector2 speed;
        Vector2 endPosition;
        public CommandMove(Vector2 endPosition, IEnemy reference)
        {
            this.reference = reference;
            Vector2 startPosition = new Vector2(reference.Location.X, reference.Location.Y);
            this.endPosition = endPosition;
            speed = Vector2.Normalize((endPosition - startPosition)) * reference.Velocity;
        }

        public void Update(GameTime gameTime)
        {
            if (speed.X >= 0)
            {
                if (speed.Y >= 0)
                {
                    if ((reference.Location + speed).X > endPosition.X && (reference.Location + speed).Y > endPosition.Y)
                    {
                        IsFinished = true;
                        return;
                    }
                }
                else
                {
                    if ((reference.Location + speed).X > endPosition.X && (reference.Location + speed).Y < endPosition.Y)
                    {
                        IsFinished = true;
                        return;
                    }
                }
            }
            else
            {
                if (speed.Y >= 0)
                {
                    if ((reference.Location + speed).X < endPosition.X && (reference.Location + speed).Y > endPosition.Y)
                    {
                        IsFinished = true;
                        return;
                    }
                }
                else
                {
                    if ((reference.Location + speed).X < endPosition.X && (reference.Location + speed).Y < endPosition.Y)
                    {
                        IsFinished = true;
                        return;
                    }
                }
            }
            reference.Move(speed);
        }
    }
}
