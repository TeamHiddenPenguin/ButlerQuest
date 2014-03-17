using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{
    // abstract class used by all objects in the game
    abstract class GameObject
    {
        // attirbutes
        public Vector2 location; // position of the object in the game space
        public Rectangle rectangle; // rectangle used for the total area covered by the object

        // methods

        // sorry if this has too many if statements.
        public int CollisionSide(GameObject otherObject) // checks if the current GameObject is colliding with the GameObject given in the parameters, based on their Rectangles.
        {
            if (this.rectangle.Intersects(otherObject.rectangle)) // checks to see if the rectangles collide at all.
            {
                if (this.rectangle.X > otherObject.rectangle.X) // does not collide at this.rectangle.Top
                {
                    if (this.rectangle.Y > otherObject.rectangle.Y) // does not collide at this.rectangle.Left
                    {
                        if ((this.rectangle.X - otherObject.rectangle.X) > (this.rectangle.Y - otherObject.rectangle.Y)) return 2; // more of rectangle.Bottom is colliding than rectangle.Right

                        else return 1; // more of rectangle.Right is colliding than rectangle.Bottom.
                    }
                    else // does not collide at this.rectangle.Right
                    {
                        if ((this.rectangle.X - otherObject.rectangle.X) > (this.rectangle.Y - otherObject.rectangle.Y)) return 2; // more of rectangle.Bottom is colliding than rectangle.Left

                        return 3; // more of rectangle.Left is colliding than rectangle.Bottom.
                    }
                }
                else // does not collide at this.rectangle.Bottom
                {
                    if (this.rectangle.Y > otherObject.rectangle.Y) // does not collide at this.rectangle.Left
                    {
                        if ((this.rectangle.X - otherObject.rectangle.X) > (this.rectangle.Y - otherObject.rectangle.Y)) return 0; // more of rectangle.Top is colliding than rectangle.Right

                        return 1; // more of rectangle.Right is colliding than rectangle.Top.
                    }
                    else // does not collide at this.rectangle.Right
                    {
                        if ((this.rectangle.X - otherObject.rectangle.X) > (this.rectangle.Y - otherObject.rectangle.Y)) return 0; // more of rectangle.Top is colliding than rectangle.Left

                        return 3; // more of rectangle.Left is colliding than rectangle.Top.
                    }
                }
            }
            else // does not collide, therefore collisionSide is null.
            {
                return -1;
            }
        }
    }
}
