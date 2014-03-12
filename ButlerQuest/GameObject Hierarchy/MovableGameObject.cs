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
        // moves the object based on it's velocity
        public void Move(Vector2 speed)
        {
            location += velocity;
        } 
    }
}
