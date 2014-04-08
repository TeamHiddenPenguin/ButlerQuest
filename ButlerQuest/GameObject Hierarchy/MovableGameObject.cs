// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ButlerQuest
{
    // abstract class for all GamePieceObjects that have the ability to move.
    public abstract class MovableGameObject : DrawableGameObject
    {
        // attributes
        public Vector3 velocity;// amount the object moves in a single move method
        public int direction;

        // constructor
        public MovableGameObject(Vector3 vel, Animation[] animations, string[] names, Vector3 loc, Rectangle rect)
            : base(animations, names, loc, rect)
        {
            velocity = vel;
        }

        // methods
        // moves the object based on it's velocity and the direction given.
        public void Move(Vector3 dir)
        {
            Vector3 dirUnit; // Vector2 to hold the unit vector of the direction given.
            dirUnit = Vector3.Normalize(dir); // normalizes the direction vector and stores it in dirUnit.
            location = location + (velocity * dirUnit); // changes the location by velocity as a unit vector based on direction

            // sets direction
            if (dir.X == 0) // not moving in the x
            {
                if (dir.Y > 0) direction = 2; // moving down
                else if (dir.Y < 0) direction = 0; // moving up
            }
            else if (dir.X > 0) // moving right
            {
                if (dir.Y == 0) // not moving in the y
                {
                    direction = 1; // only moving right
                }
                else if (dir.Y > 0) // moving down
                {
                    if (dir.X > dir.Y) direction = 1; // moving more right than down
                    else direction = 2; // moving more down than right
                }
                else if (dir.Y < 0) // moving up
                {
                    if (dir.X > Math.Abs(dir.Y)) direction = 1; // moving more right than up
                    else direction = 0; // moving more up than right
                }
            }
            else if (dir.X < 0) // moving left
            {
                if (dir.Y == 0) // not moving in the y
                {
                    direction = 3; // only moving left
                }
                else if (dir.Y > 0) // moving down
                {
                    if (Math.Abs(dir.X) > dir.Y) direction = 3; // moving more left than down
                    else direction = 2; // moving more down than left
                }
                else if (dir.Y < 0) // moving up
                {
                    if (dir.X < dir.Y) direction = 3; // moving more left than up
                    else direction = 0; // moving more up than left
                }
            }

            // updates the rectangle to match the new location
            rectangle.X = (int)location.X;
            rectangle.Y = (int)location.Y;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rectangle.X = (int)location.X;
            rectangle.Y = (int)location.Y;
        }
    }
}
