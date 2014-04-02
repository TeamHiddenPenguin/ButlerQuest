using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna;
using System.Threading;

// code by James Borger
namespace ButlerQuest
{
    class ScreenManager
    {
        private ScreenManager()
        {
            sharedManager = this;
            screenState = new Stack<Screen>();
            screenState.Push(new LoadingGameScreen("AStarTest.tmx"));
        }
        public SpriteBatch sBatch;
        public GraphicsDevice gDevice;
        public ContentManager Content;
        static ScreenManager sharedManager;
        public Screen CurrentScreen { get { return screenState.Peek(); } }
        public static ScreenManager SharedManager
        {
            get
            {
                if (sharedManager == null)
                {
                    return new ScreenManager();
                }
                else
                {
                    return sharedManager;
                }
            }
        }
        // creates the initial stack to manage screens
        Stack<Screen> screenState;

        // Adds the new screen to the stack
        public void PushScreen(Screen newScreen)
        {
            screenState.Push(newScreen);
        }

        // Removes the top screen from the stack and returns its value
        public Screen PopScreen()
        {
            return screenState.Pop();
        }

        //Makes the current screen equal to the new screen
        public void NextScreen()
        {
            screenState.Pop();
        }

        //Adds a new screen, saving the current screen on the stack
        public void AddScreen(Screen screen)
        {
            PushScreen(screen);
        }

        public void UpdateCurrentScreen(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public void DrawCurrentScreen(GameTime gameTime)
        {
            CurrentScreen.Draw(gameTime);
        }

        public GameScreen GetCurrentGameScreen()
        {
            foreach (var screen in screenState)
            {
                if (screen is GameScreen)
                {
                    return (GameScreen)screen;
                }
            }
            return null;
        }
    }

    public abstract class Screen
    {
        public virtual void HandleInput() { }

        public abstract void Draw(GameTime time);

        public abstract void Update(GameTime time);
    }

    public class GameScreen : Screen
    {
        public Level level;
        string levelName;
        KeyboardState kState;
        public GameScreen(string toLoad)
        {
            levelName = toLoad;
        }

        public void Initialize()
        {
            level = new Level(levelName);
            kState = Keyboard.GetState();
        }

        public override void Update(GameTime time)
        {
            level.Update(time);
            HandleInput();
        }

        public override void Draw(GameTime time)
        {
            level.Draw(time);
        }

        public override void HandleInput()
        {
            kState = Keyboard.GetState();
            base.HandleInput();
            if (kState.IsKeyDown(Keys.W))
            {
                level.player.Move(new Vector3(0, -1, 0));
            }
            if (kState.IsKeyDown(Keys.A))
            {
                level.player.Move(new Vector3(-1, 0, 0));
            }
            if (kState.IsKeyDown(Keys.S))
            {
                level.player.Move(new Vector3(0, 1, 0));
            }
            if (kState.IsKeyDown(Keys.D))
            {
                level.player.Move(new Vector3(1, 0, 0));
            }
        }
    }

    public class DebugScreen : Screen
    {
        Texture2D thing1;
        public DebugScreen()
        {
            
        }

        public void Initialize()
        {
            thing1 = ScreenManager.SharedManager.Content.Load<Texture2D>("Map in Editor.png");
            Thread.Sleep(2000);
        }

        public override void Update(GameTime time)
        {
            
        }

        public override void Draw(GameTime time)
        {
            ScreenManager.SharedManager.sBatch.Begin();
            ScreenManager.SharedManager.sBatch.Draw(thing1, new Vector2(0,0), Color.White);
            ScreenManager.SharedManager.sBatch.End();
        }
    }

    public class LoadingGameScreen : Screen
    {
        Thread loadThread;
        GameScreen gameScreen;
        public LoadingGameScreen(string level)
        {
            gameScreen = new GameScreen(level);
            ScreenManager.SharedManager.PushScreen(gameScreen);
            loadThread = new Thread(gameScreen.Initialize);
            loadThread.Start();
        }


        public override void Draw(GameTime time)
        {
            //do stuff
        }

        public override void Update(GameTime time)
        {
            if (!loadThread.IsAlive)
            {
                ScreenManager.SharedManager.NextScreen();
            }
        }
    }
}
