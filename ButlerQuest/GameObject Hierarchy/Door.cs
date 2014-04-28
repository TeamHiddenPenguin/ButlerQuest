using Microsoft.Xna.Framework;
// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    public class Door : DrawableGameObject
    {
        public Boolean locked;
        public int keyAssociation;

        public Door(Animation[] animations, string[] names, Vector3 loc, Rectangle rect, int doorNum)
            : base(animations, names, loc, rect)
        {
            locked = true; ;
            keyAssociation = doorNum;
        }
    }
}
