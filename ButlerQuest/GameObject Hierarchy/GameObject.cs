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
        public Vector2 location; // position of the object in the game space
        public Rectangle rectangle; // rectangle used for the total area covered by the object


    }
}
