using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class Wall : GameObject
    {
        public Wall(Vector3 loc, int width, int height)
            : base(loc, new Rectangle((int)loc.X, (int)loc.Y, width, height))
        {
        }
    }
}
