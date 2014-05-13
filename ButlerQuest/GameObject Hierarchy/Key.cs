// Drew Stanton
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    // a key that can open a locked door
    class Key : DrawableGameObject
    {
        public int doorAssociation; // number that corresponds to an index in the list of locked doors.

        public Key(Animation[] animations, string[] names, Vector3 loc, Rectangle rect, int keyNum) : base(animations, names, loc, rect)
        {
            doorAssociation = keyNum;
        }
    }
}
