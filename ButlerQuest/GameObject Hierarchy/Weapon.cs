// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ButlerQuest
{
    public class Weapon : DrawableGameObject
    {
        public int durability;
        public bool visible;

        public Weapon(Animation[] animations, string[] names, Vector3 location, Rectangle rect, int durable)
            : base(animations, names, location, rect)
        {
            durability = durable;
            visible = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            rectangle.X = (int)location.X;
            rectangle.Y = (int)location.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
                base.Draw(spriteBatch);
        }
    }
}
