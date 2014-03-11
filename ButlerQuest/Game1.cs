#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace ButlerQuest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState kState = new KeyboardState();
        KeyboardState oldState = new KeyboardState();
        Map test;
        Rectangle viewport;
        Vector2 target;
        Vector2 player;
        Vector2 lastNodeLoc;
        Texture2D playertx;
        Texture2D targettx;
        Animation anim;
        Path<SquareGraphNode> path;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1200;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            viewport = new Rectangle(0, 0, 1200, 720);
            test = new Map("Content\\AstarTest.tmx", GraphicsDevice, spriteBatch);
            player = new Vector2(test.ObjectGroups["Start"][0].X, test.ObjectGroups["Start"][0].Y);
            lastNodeLoc = player;
            target = new Vector2(test.ObjectGroups["Target"][0].X, test.ObjectGroups["Target"][0].Y);
            path = AStar.FindPath<SquareGraphNode>(test.Graph.GetNode((int)target.X, (int)target.Y, 0), test.Graph.GetNode((int)player.X, (int)player.Y, 0), AStar.ManhattanDistance, null);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            playertx = Content.Load<Texture2D>("player.png");
            targettx = Content.Load<Texture2D>("target.png");
            anim = new Animation(0, 2, 32, 32, 1, 3, 10, Content.Load<Texture2D>("tiles.png"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            anim.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            oldState = kState;
            kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.W))
            {
                target.Y -= 6;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                target.X -= 6;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                target.Y += 6;
            }
            if (kState.IsKeyDown(Keys.D))
            {
                target.X += 6;
            }
            if(kState.IsKeyDown(Keys.Up))
            {
                viewport.Y -= 6;
            }
            if(kState.IsKeyDown(Keys.Left))
            {
                viewport.X -= 6;
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                viewport.Y += 6;
            }
            if (kState.IsKeyDown(Keys.Right))
            {
                viewport.X += 6;
            }


            if (path != null)
            {
                Vector2 pathLoc = new Vector2(path.LastStep.X * 32, path.LastStep.Y * 32);
                player.X = player.X + .05f * (pathLoc.X - lastNodeLoc.X);
                player.Y = player.Y + .05f * (pathLoc.Y - lastNodeLoc.Y);

                if (Math.Abs(player.X - pathLoc.X) < 1 && Math.Abs(player.Y - pathLoc.Y) < 1)
                {
                    player = pathLoc;
                    try
                    {
                        path = AStar.FindPath<SquareGraphNode>(test.Graph.GetNode((int)target.X, (int)target.Y, 0), test.Graph.GetNode((int)player.X, (int)player.Y, 0), AStar.ManhattanDistance, null);
                    }
                    catch(Exception e)
                    {
                        //If for some reason the pathfinding fails, just continue along the current path until we can path again
                    }
                    path = path.PreviousSteps;
                    lastNodeLoc = player;
                    
                }
            }
            else
                //If we are out of path, try to path again
                try
                {
                    path = AStar.FindPath<SquareGraphNode>(test.Graph.GetNode((int)target.X, (int)target.Y, 0), test.Graph.GetNode((int)player.X, (int)player.Y, 0), AStar.ManhattanDistance, null);
                }
                catch (Exception e)
                {
                    //If we are out of path and can't path, then just sit in place until we can path again.
                }

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(test.BackgroundColor);

            // TODO: Add your drawing code here
            Matrix cameraMatrix = Matrix.CreateTranslation(-(int)viewport.X, -(int)viewport.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cameraMatrix);
            test.Draw(viewport, 0);
            anim.Draw(spriteBatch, new Rectangle(0, 0, 32, 32));
            spriteBatch.Draw(targettx, target, Color.White);
            spriteBatch.Draw(playertx, player, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
