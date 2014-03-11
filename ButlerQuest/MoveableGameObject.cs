using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ButlerQuest
{
    // abstract class for all GamePieceObjects that have the ability to move.
    abstract class MoveableGameObject : DrawableGameObject
    {
        // attributes
        Vector2 velocity { get; set; } // amount the object moves in a single move method


        // methods
        void Move() { } // moves the object based on it's velocity
    }
}
