using Microsoft.Xna.Framework;
// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    // represents a single amount of money on the map.
    class Coin : DrawableGameObject
    {
        private int value; // the amount of money the coin is worth
        public bool active; // whether or not the coin is active

        public Coin(Animation[] animations, string[] names, Vector3 loc, Rectangle rect, int val)
            : base(animations, names, loc, rect)
        {
            value = val;
            active = true;
        }

        public int InteractWith() // when the player collides with an active coin
        {
            active = false; // deactivates the coin so it can't be obtained again.
            return value; // returns the amount of money to increase the player's score by.
        }
    }
}
