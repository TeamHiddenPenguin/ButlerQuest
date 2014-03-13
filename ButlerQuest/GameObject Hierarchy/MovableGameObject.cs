using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ButlerQuest
{
    // abstract class for all GamePieceObjects that have the ability to move.
    abstract class MovableGameObject : DrawableGameObject
    {
        // attributes
        public Vector2 velocity;// amount the object moves in a single move method


        // methods
        // moves the object based on it's velocity and the direction given.
        public void Move(Vector2 direction)
        {
            Vector2 dirUnit; // Vector2 to hold the unit vector of the direction given.
            Vector2.Normalize(ref direction, out dirUnit); // normalizes the direction vector and stores it in dirUnit.
            location = location + (velocity * dirUnit);
        } 
    }
}
