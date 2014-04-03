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
        Dictionary<string, Animation> anims;
        // the name of the animation currently being displayed
        string currentAnimation;


        // properties
        // property for current animation
        public string CurrentAnimation
        {
            get { return currentAnimation; }

            set // if the given value exists in the animation dictionary, changes the current animation, otherwise sets the animation to the default
            {
                if (anims.ContainsKey(value))
                {
                    currentAnimation = value;
                    anims[currentAnimation].Reset();
                }
                else currentAnimation = "default";
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
            if(currentAnimation != null && anims[currentAnimation] != null)
                anims[currentAnimation].Draw(spriteBatch, rectangle);
        }

        public virtual void Update(GameTime gameTime) // updates the current animation
        {
            if (currentAnimation != null && anims[currentAnimation] != null)
                anims[currentAnimation].Update();
        }
    }
}
