// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ButlerQuest
{
    // a weapon that can be used by the player.
    public class Weapon : DrawableGameObject
    {
        public int durability; // how many times an enemy can be hit with the weapon before the weapon breaks.
        public bool visible; // whether or not to draw the weapon.

        public Weapon(Animation[] animations, string[] names, Vector3 location, Rectangle rect, int durable)
            : base(animations, names, location, rect)
        {
            durability = durable;
            visible = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // updates the drawing rectangle, as drawable game objects do not automatically do this.
            rectangle.X = (int)location.X;
            rectangle.Y = (int)location.Y;
        }

        public new void Draw(SpriteBatch spriteBatch) // only draws if it is visible.
        {
            if (visible)
                base.Draw(spriteBatch);
        }
    }
}
