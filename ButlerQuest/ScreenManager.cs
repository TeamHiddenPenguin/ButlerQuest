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
using System.IO;

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
        public Game1 Game;
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

    // Main Menu to select what to do when starting game UNFINISHED make the temporary one cornflower blue, and use plain text for the selections, an arrow next to the selected one
    public class MenuScreen : Screen
    {
        // Determines which menu option is selected, 0: Start New Game, 1: Options, 2: Exit, if we make it 3: Load if it is implemented
        int selectedItem;
        Texture2D menu0 = Texture2D.FromStream(ScreenManager.SharedManager.gDevice, File.OpenRead("Content\\" + "ButlerMenu0.png"));
        Texture2D menu1 = Texture2D.FromStream(ScreenManager.SharedManager.gDevice, File.OpenRead("Content\\" + "ButlerMenu1.png"));
        Texture2D menu2 = Texture2D.FromStream(ScreenManager.SharedManager.gDevice, File.OpenRead("Content\\" + "ButlerMenu2.png"));
        private Texture2D selectionArrow;


        KeyboardState kState;
        KeyboardState oldkState;
        public MenuScreen()
        {
            oldkState = new KeyboardState();
            selectedItem = 0;
        }


        // Updates the Menu
        public override void Update(GameTime time)
        {
            HandleInput();
        }

        // Draws the Menu (Add in the images for it to draw the menu) UNFINISHED
        public override void Draw(GameTime time)
        {
            ScreenManager.SharedManager.sBatch.Begin();
            switch (selectedItem)
            {
                case 0:
                    
                    ScreenManager.SharedManager.sBatch.Draw(menu0, new Rectangle(0, 0, 1280, 720), Color.White);
                    break;
                case 1:
                    ScreenManager.SharedManager.sBatch.Draw(menu1, new Rectangle(0, 0, 1280, 720), Color.White);
                    break;
                case 2:
                    ScreenManager.SharedManager.sBatch.Draw(menu2, new Rectangle(0, 0, 1280, 720), Color.White);
                    break;
                default:
                    break;
            }
            ScreenManager.SharedManager.sBatch.End();
        }

        // Handle the menu selection input from the player
        public override void HandleInput()
        {
            oldkState = kState;
            kState = Keyboard.GetState();
            base.HandleInput();
            if (kState.IsKeyUp(Keys.W) && oldkState.IsKeyDown(Keys.W))
            {
                if (selectedItem == 0)
                {
                    selectedItem = 2;
                }
                else
                {
                    selectedItem--;
                }
            }
            if (kState.IsKeyUp(Keys.S) && oldkState.IsKeyDown(Keys.S))
            {
                if (selectedItem == 2)
                {
                    selectedItem = 0;
                }
                else
                {
                    selectedItem++;
                }
            }
            if (kState.IsKeyUp(Keys.Enter) && oldkState.IsKeyDown(Keys.Enter))
            {
                switch (selectedItem)
                {
                    case 0:
                        // Level name subject to change
                        ScreenManager.SharedManager.AddScreen(new LoadingGameScreen("Level1.tmx"));
                        break;
                    case 1:
                        ScreenManager.SharedManager.AddScreen(new OptionsScreen());
                        break;
                    case 2:
                        // Exit the game
                        ScreenManager.SharedManager.Game.Exit();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Main game Screen to be used, contains level information and the keyboard state
    public class GameScreen : Screen
    {
        public Level level;
        string levelName;
        KeyboardState kState;
        KeyboardState previousState;
        public GameScreen(string toLoad)
        {
            levelName = toLoad;
        }

        // Initializes the level
        public void Initialize()
        {
            level = new Level(levelName);
            kState = Keyboard.GetState();
            previousState = Keyboard.GetState();
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

                if (!level.player.CurrentAnimation.Contains("Attack"))
                    level.player.CurrentAnimation = "WalkUp";
            }
            if (kState.IsKeyDown(Keys.A))
            {
                level.player.Move(new Vector3(-1, 0, 0));

                if (!level.player.CurrentAnimation.Contains("Attack"))
                    level.player.CurrentAnimation = "WalkLeft";
            }
            if (kState.IsKeyDown(Keys.S))
            {
                level.player.Move(new Vector3(0, 1, 0));

                if (!level.player.CurrentAnimation.Contains("Attack"))
                    level.player.CurrentAnimation = "WalkDown";
            }
            if (kState.IsKeyDown(Keys.D))
            {
                level.player.Move(new Vector3(1, 0, 0));
                if (!level.player.CurrentAnimation.Contains("Attack"))
                    level.player.CurrentAnimation = "WalkRight";
            }
            if (kState.IsKeyDown(Keys.Space))
            {
                if (previousState.IsKeyUp(Keys.Space))
                    level.player.Attack();
            }
            if (kState.IsKeyDown(Keys.Escape))
            {
                ScreenManager.SharedManager.NextScreen();
            }
            if (!kState.IsKeyDown(Keys.W) && !kState.IsKeyDown(Keys.D) && !kState.IsKeyDown(Keys.S) && !kState.IsKeyDown(Keys.A) && !level.player.CurrentAnimation.Contains("Attack"))
            {
                switch (level.player.direction)
                {
                    case 0: level.player.CurrentAnimation = "StandUp";
                        break;
                    case 1: level.player.CurrentAnimation = "StandRight";
                        break;
                    case 2: level.player.CurrentAnimation = "StandDown";
                        break;
                    case 3: level.player.CurrentAnimation = "StandLeft";
                        break;
                    default:
                        break;
                }
            }

            previousState = kState;
        }
    }

    // Options screen for managing volume and other things
    //UNFINISHED
    public class OptionsScreen : Screen
    {
        // optionSelected 0 = Return to last screen, 1 = Quit to Menu
        int optionSelected = 0;

        Texture2D options0 = Texture2D.FromStream(ScreenManager.SharedManager.gDevice, File.OpenRead("Content\\" + "ButlerOptions0.png"));
        Texture2D options1 = Texture2D.FromStream(ScreenManager.SharedManager.gDevice, File.OpenRead("Content\\" + "ButlerOptions1.png"));

        KeyboardState kState = new KeyboardState();
        KeyboardState oldkState = new KeyboardState();

        public override void Update(GameTime time)
        {
            HandleInput();
        }

        public override void Draw(GameTime time)
        {
            // Add code to draw a visual representing which option is selected
            ScreenManager.SharedManager.sBatch.Begin();
            switch (optionSelected)
            {
                case 0:
                    // Put code to draw visual
                    ScreenManager.SharedManager.sBatch.Draw(options0, new Rectangle(0, 0, 1280, 720), Color.CornflowerBlue);
                    break;
                case 1:
                    // Put code to draw visual
                    ScreenManager.SharedManager.sBatch.Draw(options1, new Rectangle(0, 0, 1280, 720), Color.CornflowerBlue);
                    break;
                default:
                    break;
            }
            ScreenManager.SharedManager.sBatch.End();
        }

        public override void HandleInput()
        {
            oldkState = kState;
            kState = Keyboard.GetState();
            base.HandleInput();
            if (kState.IsKeyUp(Keys.W) && oldkState.IsKeyDown(Keys.W))
            {
                if (optionSelected == 0)
                {
                    optionSelected = 1;
                }
                else
                {
                    optionSelected--;
                }
            }

            if (kState.IsKeyUp(Keys.S) && oldkState.IsKeyDown(Keys.S))
            {
                if (optionSelected == 1)
                {
                    optionSelected = 0;
                }
                else
                {
                    optionSelected++;
                }
            }

            // Run the selected Option
            if (kState.IsKeyUp(Keys.Enter) && oldkState.IsKeyDown(Keys.Enter))
            {
                switch (optionSelected)
                {
                    case 0:
                        // Return to the previous screen
                        ScreenManager.SharedManager.PopScreen();
                        break;
                    case 1:
                        // Exit the Game to Main Menu
                        while (!(ScreenManager.SharedManager.CurrentScreen is MenuScreen))
                            ScreenManager.SharedManager.PopScreen();
                        break;
                    default:
                        break;
                }
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
            ScreenManager.SharedManager.sBatch.Draw(thing1, new Vector2(0, 0), Color.White);
            ScreenManager.SharedManager.sBatch.End();
        }
    }

    // Loading Screen to display while setting up the Game Screen UNFINISHED
    public class LoadingGameScreen : Screen
    {
        Texture2D loading = Texture2D.FromStream(ScreenManager.SharedManager.gDevice, File.OpenRead("Content\\LoadScreen.png"));
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
            ScreenManager.SharedManager.sBatch.Begin();
            ScreenManager.SharedManager.sBatch.Draw(loading, new Rectangle(0, 0, 1200, 720), Color.White);
            ScreenManager.SharedManager.sBatch.End();

        }

        // Goes to the next screen once the thread for loading is finished
        public override void Update(GameTime time)
        {
            if (!loadThread.IsAlive)
            {
                ScreenManager.SharedManager.NextScreen();
                AIManager.SharedAIManager.Reinitialize();
            }
        }
    }

    public class GameOverScreen : Screen
    {
        string level;
        Texture2D tex = ScreenManager.SharedManager.Content.Load<Texture2D>("GameOverScreen.png");
        public GameOverScreen(string level)
        {
            this.level = level;
        }

        public override void Draw(GameTime time)
        {
            ScreenManager.SharedManager.sBatch.Begin();
            ScreenManager.SharedManager.sBatch.Draw(tex, new Rectangle(0, 0, ScreenManager.SharedManager.gDevice.Viewport.Width, ScreenManager.SharedManager.gDevice.Viewport.Height), Color.White);
            ScreenManager.SharedManager.sBatch.End();
        }

        public override void Update(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ScreenManager.SharedManager.NextScreen();
                ScreenManager.SharedManager.PushScreen(new LoadingGameScreen(level));
            }
        }
    }

    public class VictoryScreen : Screen
    {
        Texture2D tex = ScreenManager.SharedManager.Content.Load<Texture2D>("VictoryScreen.png");

        public override void Draw(GameTime time)
        {
            ScreenManager.SharedManager.sBatch.Begin();
            ScreenManager.SharedManager.sBatch.Draw(tex, new Rectangle(0, 0, ScreenManager.SharedManager.gDevice.Viewport.Width, ScreenManager.SharedManager.gDevice.Viewport.Height), Color.White);
            ScreenManager.SharedManager.sBatch.End();
        }

        public override void Update(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ScreenManager.SharedManager.NextScreen();
            }
        }
    }
}
