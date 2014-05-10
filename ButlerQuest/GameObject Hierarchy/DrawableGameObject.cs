//Written by Drew Stanton and Jesse Florio
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ButlerQuest
{
    // abstract class for all GamePiece objects with a visual component
    public abstract class DrawableGameObject : GameObject
    {
        // attributes
        // this is a dictionary of all the animations with the key being the name of the animations
        public Dictionary<string, Animation> anims;
        // the name of the animation currently being displayed
        string currentAnimation;


        // properties
        // property for current animation
        public string CurrentAnimation
        {
            get { return currentAnimation; }

            set // if the given value exists in the animation dictionary, changes the current animation, otherwise sets the animation to the default
            {
                if (value != currentAnimation)
                {
                    if (anims.ContainsKey(value))
                    {
                        currentAnimation = value;
                    }
                    else currentAnimation = "default";
                }
            }
        }

        // constructor
        // takes the names and animations and adds them to the anims dictionary
        public DrawableGameObject(Animation[] animations, string[] names, Vector3 loc, Rectangle rect)
            : base(loc, rect)
        {
            anims = new Dictionary<string, Animation>();

            for (int i = 0; i < names.Length; i++)
            {
                anims.Add(names[i], animations[i]);
            }
            CurrentAnimation = "default";
        }

        // methods
        // draws the current animation from the anim dictionary
        public void Draw(SpriteBatch spriteBatch) // draws the current animation
        {
            if(currentAnimation != null && anims.ContainsKey(currentAnimation))
                anims[currentAnimation].Draw(spriteBatch, rectangle);
        }

        public virtual void Update(GameTime gameTime) // updates the current animation
        {
            base.Update();

            if (currentAnimation != null && anims.ContainsKey(currentAnimation))
                anims[currentAnimation].Update();
        }

        [Obsolete]
        public bool PixelCollide(DrawableGameObject other)
        {
            Texture2D otherTex = other.anims[other.CurrentAnimation].GetTexture(ScreenManager.SharedManager.gDevice, ScreenManager.SharedManager.sBatch);
            Texture2D thisTex = this.anims[this.CurrentAnimation].GetTexture(ScreenManager.SharedManager.gDevice, ScreenManager.SharedManager.sBatch);
            Color[] otherPixels = new Color[otherTex.Width * otherTex.Height];
            Color[] thisPixels = new Color[thisTex.Width * thisTex.Height];
            otherTex.GetData(otherPixels);
            thisTex.GetData(thisPixels);

            int minX = Math.Max(this.rectangle.X, other.rectangle.X);
            int maxX = Math.Min(this.rectangle.X + this.rectangle.Width, other.rectangle.X + other.rectangle.Width);
            int minY = Math.Max(this.rectangle.Y, other.rectangle.Y);
            int maxY = Math.Min(this.rectangle.Y + this.rectangle.Height, other.rectangle.Y + other.rectangle.Height);

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    Color otherPixel = otherPixels[(x - other.rectangle.X) + (y - other.rectangle.Y) * other.rectangle.Width];
                    Color thisPixel = thisPixels[(x - this.rectangle.X) + (y - this.rectangle.Y) * this.rectangle.Width];

                    if (otherPixel.A != 0 && thisPixel.A != 0)
                        return true;
                }
            }
            return false;
        }

        public void ResetAllAnimations()
        {
            foreach (var anim in anims.Values)
            {
                anim.Reset();
            }
        }
    }
}
