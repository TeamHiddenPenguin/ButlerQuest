using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class Key : DrawableGameObject
    {
        public int doorAssociation;

        public Key(Animation[] animations, string[] names, Vector3 loc, Rectangle rect, int keyNum) : base(animations, names, loc, rect)
        {
            doorAssociation = keyNum;
        }
    }
}
