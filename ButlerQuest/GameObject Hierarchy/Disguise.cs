// Drew Stanton
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    // disguise objects
    public class Disguise : DrawableGameObject
    {
        public string disguiseType; // the type of disguise, either Chef, Mechanic, or Box

        public Disguise(Animation[] animations, string[] names, Vector3 loc, Rectangle rect, string disguise)
            : base(animations, names, loc, rect)
        {
            disguiseType = disguise;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // updates the drawing rectangle's location, as it has the potential to be moved, whereas other drawable game objects don't.
            rectangle.X = (int)location.X;
            rectangle.Y = (int)location.Y;
        }
    }
}
