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

        // constructor
        public MovableGameObject(Vector3 vel, Animation[] animations, string[] names, Vector3 loc, Rectangle rect)
            : base(animations, names, loc, rect)
        {
            velocity = vel;
        }

        // methods
        // moves the object based on it's velocity and the direction given.
        public void Move(Vector3 direction)
        {
            Vector3 dirUnit; // Vector2 to hold the unit vector of the direction given.
            dirUnit = Vector3.Normalize(direction); // normalizes the direction vector and stores it in dirUnit.
            location = location + (velocity * dirUnit); // changes the location by velocity as a unit vector based on direction

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
