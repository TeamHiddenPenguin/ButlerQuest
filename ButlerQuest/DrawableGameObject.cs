using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ButlerQuest
{
    // abstract class for all GamePiece objects with a visual component
    abstract class DrawableGameObject : GameObject
    {
        // attributes
        Dictionary<string, Animation> sprites; // a dictionary of all of the animations for a single DGO.
        string currentAnimation; // represents the animation to be drawn on the screen. Used as a key in the sprites Dictionary.


        // properties
        string CurrentAnimation
        {
            get { return currentAnimation; }

            set // if the given value exists in the animation dictionary, changes the current animation, otherwise sets the animation to the default
            {
                if (sprites.ContainsKey(value))
                {
                    currentAnimation = value;
                    sprites[currentAnimation].Reset();
                }
                else currentAnimation = "default";
            }
        }

        // constructor
        //DrawableGameObject(Animation[] animations, string[] names);

        // methods
        void Draw(SpriteBatch spriteBatch) // draws the current animation
        {
            sprites[currentAnimation].Draw(spriteBatch, objectRect);
        }

        void Update() // updates the current animation
        {

        }
    }
}
