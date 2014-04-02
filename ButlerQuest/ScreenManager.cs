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
            currentScreen = new LoadingGameScreen("AStarTest.tmx");
        }
        public SpriteBatch sBatch;
        public GraphicsDevice gDevice;
        public ContentManager Content;
        static ScreenManager sharedManager;
        private Screen currentScreen;
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
            currentScreen = screenState.Pop();
        }

        //Adds a new screen, saving the current screen on the stack
        public void AddScreen(Screen screen)
        {
            PushScreen(currentScreen);
            currentScreen = screen;
        }

        public void UpdateCurrentScreen(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public void DrawCurrentScreen(GameTime gameTime)
        {
            currentScreen.Draw(gameTime);
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
        Level level;
        string levelName;
        public GameScreen(string toLoad)
        {
            levelName = toLoad;
        }

        public void Initialize()
        {
            level = new Level(levelName);
        }

        public override void Update(GameTime time)
        {
            level.Update(time);
        }

        public override void Draw(GameTime time)
        {
            level.Draw(time);
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
