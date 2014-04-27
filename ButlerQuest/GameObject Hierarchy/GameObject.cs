// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{
    // abstract class used by all objects in the game
    public abstract class GameObject
    {
        // attirbutes
        public Vector3 location; // position of the object in the game space
        public Rectangle rectangle; // rectangle used for the total area covered by the object
        public Vector3 center;

        // constructor
        protected GameObject(Vector3 loc, Rectangle rect)
        {
            location = loc;
            rectangle = rect;
        }

        // method
        // sorry if this has too many if statements.
        public int CollisionSide(GameObject otherObject) // checks if the current GameObject is colliding with the GameObject given in the parameters, based on their Rectangles.
        {
            if (this.location.Z == otherObject.location.Z)
            {
                if (this.rectangle.Intersects(otherObject.rectangle)) // checks to see if the rectangles collide at all.
                {
                    if (this.rectangle.X > otherObject.rectangle.X) // does not collide at this.rectangle.Right
                    {
                        if (this.rectangle.Y > otherObject.rectangle.Y) // does not collide at this.rectangle.Bottom
                        {
                            if (Math.Abs(this.rectangle.X - otherObject.rectangle.Right) > Math.Abs(this.rectangle.Y - otherObject.rectangle.Bottom)) return 0; // more of rectangle.Top is colliding than rectangle.Left

                            else return 3; // more of rectangle.Left is colliding than rectangle.Bottom.
                        }
                        else // does not collide at this.rectangle.Top
                        {
                            if (Math.Abs(this.rectangle.X - otherObject.rectangle.Right) > Math.Abs(this.rectangle.Bottom - otherObject.rectangle.Y)) return 2; // more of rectangle.Bottom is colliding than rectangle.Left

                            return 3; // more of rectangle.Left is colliding than rectangle.Bottom.
                        }
                    }
                    else // does not collide at this.rectangle.Left
                    {
                        if (this.rectangle.Y > otherObject.rectangle.Y) // does not collide at this.rectangle.Bottom
                        {
                            if (Math.Abs(this.rectangle.Right - otherObject.rectangle.X) > Math.Abs(this.rectangle.Y - otherObject.rectangle.Bottom)) return 0; // more of rectangle.Top is colliding than rectangle.Right

                            return 1; // more of rectangle.Right is colliding than rectangle.Top.
                        }
                        else // does not collide at this.rectangle.Top
                        {
                            if (Math.Abs(this.rectangle.Right - otherObject.rectangle.X) > Math.Abs(this.rectangle.Bottom - otherObject.rectangle.Y)) return 2; // more of rectangle.Bottom is colliding than rectangle.Right

                            return 1; // more of rectangle.Right is colliding than rectangle.Bottom.
                        }
                    }
                }
            }
            return -1; // if the method makes it this far, then no collison has occured
        }

        public virtual void Update()
        {
            center = new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, location.Z);
        }
    }
}
