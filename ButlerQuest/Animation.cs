using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    // 
    class Animation
    {
        // attributes
        Texture2D image;
        public int currentFrame;
        public int startFrame;
        public int endFrame;
        public int frameWidth;
        public int frameHeight;
        public int rows;
        public int columns;
        int counter;
        public int latency;

        // constructor
        public Animation(int firstFrame, int lastFrame, int frameW, int frameH, int row, int cols, int maxCounter, Texture2D spriteSheet)
        {
            startFrame = firstFrame;
            endFrame = lastFrame;
            currentFrame = startFrame;
            frameWidth = frameW;
            frameHeight = frameH;
            rows = row;
            columns = cols;
            image = spriteSheet;
            counter = 0;
            latency = maxCounter;
        }

        // methods
        public void Reset() // resets to the first frame of the animation
        {
            currentFrame = startFrame;
        }

        public void Update()
        {
            counter++;
            if (counter > latency)
            {
                counter = 0;
                currentFrame++;
                if (currentFrame > endFrame)
                {
                    currentFrame = startFrame;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRect)
        {
            Rectangle source = new Rectangle((currentFrame % columns) * frameWidth, (currentFrame / columns) * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(image, destinationRect, source, Color.White);
        }

        //Sam told me to do this
        public Texture2D GetTexture(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, frameWidth, frameHeight);
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(new Color(0, 0, 0, 0));
            spriteBatch.Begin();
            Draw(spriteBatch, new Rectangle(0, 0, frameWidth, frameHeight));
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
            return renderTarget;
        }
    }
}
