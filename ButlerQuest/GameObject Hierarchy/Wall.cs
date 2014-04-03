// Drew Stanton
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    public class Wall : GameObject
    {
        // representation of the walls as far as collision goes. Visuals for the walls will be handled separately, as we want other objects to overlap slightly with the wall sprites.
        public Wall(Vector3 loc, int width, int height)
            : base(loc, new Rectangle((int)loc.X, (int)loc.Y, width, height))
        {
        }
    }
}
