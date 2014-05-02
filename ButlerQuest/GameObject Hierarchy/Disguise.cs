using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    public class Disguise : DrawableGameObject
    {
        public string disguiseType;

        public Disguise(Animation[] animations, string[] names, Vector3 loc, Rectangle rect, string disguise)
            : base(animations, names, loc, rect)
        {
            disguiseType = disguise;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            rectangle.X = (int)location.X;
            rectangle.Y = (int)location.Y;
        }
    }
}
