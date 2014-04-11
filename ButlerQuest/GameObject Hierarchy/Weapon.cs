using Microsoft.Xna.Framework;
// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    public class Weapon : DrawableGameObject
    {
        public int durability;
        Vector3 attackLocation;
        Rectangle attackRectangle;
        public bool taken;

        public Weapon(Animation[] animations, string[] names, Vector3 location, Rectangle rect, int durable, Vector3 attackLoc, Rectangle attackRect)
            : base(animations, names, location, rect)
        {
            durability = durable;
            attackLocation = attackLoc;
            attackRectangle = attackRect;
            taken = false;
        }
    }
}
