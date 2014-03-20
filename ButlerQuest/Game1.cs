﻿#region Using Statements
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
        Enemy target;
        Enemy player;
        Enemy colTest;
        Vector3 lastNodeLoc;
        Texture2D playertx;
        Texture2D targettx;
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
            player = new Enemy(Vector3.Zero, null, null, new Vector3(test.ObjectGroups["Start"][0].X, test.ObjectGroups["Start"][0].Y, 0), new Rectangle(0, 0, 32, 32));
            lastNodeLoc = player.location;
            target = new Enemy(new Vector3(6,6,1), null, null, new Vector3(test.ObjectGroups["Target"][0].X, test.ObjectGroups["Target"][0].Y, 1), new Rectangle(0, 0, 32, 32));
            colTest = new Enemy(new Vector3(6,6,1), null, null, new Vector3(100, 100, 1), new Rectangle(100, 100, 32, 32));
            path = AStar.FindPath<SquareGraphNode>(test.Graph.GetNode((int)target.location.X, (int)target.location.Y, (int)target.location.Z), test.Graph.GetNode((int)player.location.X, (int)player.location.Y, (int)player.location.Z), AStar.ManhattanDistance, null);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            oldState = kState;
            kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.W))
            {
                target.Move(new Vector3(0, -6, 0));
            }
            if (kState.IsKeyDown(Keys.A))
            {
                target.Move(new Vector3(-6, 0, 0));
            }
            if (kState.IsKeyDown(Keys.S))
            {
                target.Move(new Vector3(0, 6, 0));
            }
            if (kState.IsKeyDown(Keys.D))
            {
                target.Move(new Vector3(6, 0, 0));
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
                Vector3 pathLoc = new Vector3(path.LastStep.X * 32, path.LastStep.Y * 32, path.LastStep.Z);
                player.location.X = player.location.X + .05f * (pathLoc.X - lastNodeLoc.X);
                player.location.Y = player.location.Y + .05f * (pathLoc.Y - lastNodeLoc.Y);
                player.location.Z = player.location.Z + .05f * (pathLoc.Z - lastNodeLoc.Z);

                if (Math.Abs(player.location.X - pathLoc.X) < 1 && Math.Abs(player.location.Y - pathLoc.Y) < 1 && Math.Abs(player.location.Z - pathLoc.Z) < .1f)
                {
                    player.location = pathLoc;
                    try
                    {
                        path = AStar.FindPath<SquareGraphNode>(test.Graph.GetNode((int)target.location.X, (int)target.location.Y, (int)target.location.Z), test.Graph.GetNode((int)player.location.X, (int)player.location.Y, (int)target.location.Z), AStar.ManhattanDistance, null);
                    }
                    catch(Exception e)
                    {
                        //If for some reason the pathfinding fails, just continue along the current path until we can path again
                    }
                    path = path.PreviousSteps;
                    lastNodeLoc = player.location;
                    
                }
            }
            else
                //If we are out of path, try to path again
                try
                {
                    path = AStar.FindPath<SquareGraphNode>(test.Graph.GetNode((int)target.location.X, (int)target.location.Y, (int)target.location.Z), test.Graph.GetNode((int)player.location.X, (int)player.location.Y, (int)player.location.Z), AStar.ManhattanDistance, null);
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
            if(player.location.Z < .9f)
                spriteBatch.Draw(playertx, new Vector2(player.location.X, player.location.Y), Color.White);
            test.Draw(viewport, 1);
            if(player.location.Z >.89f)
                spriteBatch.Draw(playertx, new Vector2(player.location.X, player.location.Y), Color.White);
            spriteBatch.Draw(targettx, new Vector2(target.location.X, target.location.Y), Color.White);
            spriteBatch.Draw(playertx, colTest.rectangle, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
