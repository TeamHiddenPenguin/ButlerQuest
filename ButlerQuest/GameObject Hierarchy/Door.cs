// Drew Stanton
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    // locked door that temporarily blocks players from passing through a tile.
    public class Door : DrawableGameObject
    {
        public Boolean locked; // whether or not it's locked.
        public int keyAssociation; // location in an array for which key unlocks it.

        public Door(Animation[] animations, string[] names, Vector3 loc, Rectangle rect, int doorNum)
            : base(animations, names, loc, rect)
        {
            locked = true;
            keyAssociation = doorNum;
        }
    }
}
