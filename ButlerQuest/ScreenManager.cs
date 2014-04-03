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
        // Constructor for screenmanager
        private ScreenManager()
        {
            sharedManager = this;
            screenState = new Stack<Screen>();
        }
        public SpriteBatch sBatch;
        public GraphicsDevice gDevice;
        public ContentManager Content;
        static ScreenManager sharedManager;
        public Screen CurrentScreen { get { return screenState.Peek(); } }

        // Create the SharedManager as a singleton using a get set
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

        // Updates the screen
        public void UpdateCurrentScreen(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        // Draws the current screen
        public void DrawCurrentScreen(GameTime gameTime)
        {
            CurrentScreen.Draw(gameTime);
        }

        // Checks for the GameScreen
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

    // Structural class to be inherited by screens
    public abstract class Screen
    {
        public virtual void HandleInput() { }

        public abstract void Draw(GameTime time);

        public abstract void Update(GameTime time);
    }

    // Main game Screen to be used, contains level information and the keyboard state
    public class GameScreen : Screen
    {
        public Level level;
        string levelName;
        KeyboardState kState;
        public GameScreen(string toLoad)
        {
            levelName = toLoad;
        }

        // Initializes the level
        public void Initialize()
        {
            level = new Level(levelName);
            kState = Keyboard.GetState();
        }

        // Updates the level
        public override void Update(GameTime time)
        {
            level.Update(time);
            HandleInput();
        }

        // Draws the level
        public override void Draw(GameTime time)
        {
            level.Draw(time);
        }

        // Handle the movement input from the keyboard for controlling the player
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

    // Debug Screen for testing purposes
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

    // Loading Screen to display while setting up the Game Screen
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

        // Goes to the next screen once the thread for loading is finished
        public override void Update(GameTime time)
        {
            if (!loadThread.IsAlive)
            {
                ScreenManager.SharedManager.NextScreen();
            }
        }
    }
}
