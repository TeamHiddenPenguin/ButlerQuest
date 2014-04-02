using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{
    // holds everything needed for a single level.
    // Player class does not yet exist, so that will be added in later.
    class Level
    {
        // properties
        public Player player; // the player. It's public so we can transfer info from one level to another (lives remaining, possibly score) without having to make the player in the gamestate management.
        List<Enemy> basicEnemies; // a list to hold all of the normal butlers for the level
        List<Wall> walls; // a list to hold all of the walls for the level
        public Map levelMap; // parses the map and tiles used for the specific level
        public Rectangle windowSpace; // window space used for drawing the map
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;

        // constructor
        public Level(string mapFile)
        {
            basicEnemies = new List<Enemy>();
            walls = new List<Wall>();
            graphics = ScreenManager.SharedManager.gDevice;
            spriteBatch = ScreenManager.SharedManager.sBatch;

            levelMap = new Map(mapFile, graphics, spriteBatch);

            foreach (var groupname in levelMap.ObjectGroups.Keys)
            {
                int currentFloor = int.Parse(groupname[5].ToString()) - 1;
                if (groupname.Substring(6, 8) == "Entities")
                {
                    foreach (var entity in levelMap.ObjectGroups[groupname])
                    {
                        if (entity.Type == "Enemy")
                        {
                            basicEnemies.Add(EntityGenerator.GenerateEnemy(new Vector3(entity.X, entity.Y, currentFloor), entity.Properties));
                        }
                        else if (entity.Type == "Player")
                        {
                            player = EntityGenerator.GeneratePlayer(new Vector3(entity.X, entity.Y, currentFloor), 5, 500);
                        }
                        else if (entity.Type == "Wall")
                        {
                            walls.Add(EntityGenerator.GenerateWall(new Vector3(entity.X, entity.Y, currentFloor), entity.Width, entity.Height));
                        }
                    }
                }
                //Otherwise it's a floor graph and sam will write this code later when it's relevant
            }

            windowSpace = new Rectangle((int)player.location.X, (int)player.location.Y, graphics.Viewport.Width, graphics.Viewport.Height);

            windowSpace.X = (int)(player.location.X + (player.rectangle.Width / 2)) - (windowSpace.Width / 2);
            windowSpace.Y = (int)(player.location.Y + (player.rectangle.Height / 2)) - (windowSpace.Height / 2);

            AIManager.DebugInitialize(this);

        }

        // methods
        // calls the draw method of everything that is drawn
        public void Draw(GameTime gameTime)
        {
            Matrix translation = Matrix.CreateTranslation(-windowSpace.X, -windowSpace.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, translation);

            levelMap.Draw(windowSpace, (int)player.location.Z);

            foreach (Enemy enemy in basicEnemies) enemy.Draw(spriteBatch);

            player.Draw(spriteBatch);

            spriteBatch.End();
        }

        // calls the update methods for every object that has one. Checks player's collison with all objects.
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            AIManager.SharedAIManager.MakePaths();

            //Enemy updates and collision
            foreach (Enemy enemy in basicEnemies) // updates enemies and checks for collision with player if on the same floor.
            {
                enemy.Update(gameTime);
                //Only worry about collision if they are hunting the player, otherwise don't worry about it
                if (enemy.state >= AI_STATE.HUNTING)
                {
                        int collision = player.CollisionSide(enemy);
                        switch (collision)
                        {
                            case -1: break;
                            default: player.lives--;
                                player.location = player.startLoc;
                                break;
                        }
                }
            }
            // wall collision
            foreach (Wall block in walls)
            {
                int collision = player.CollisionSide(block);
                switch (collision)
                {
                    case 0: player.location.Y -= player.velocity.Y;
                        break;

                    case 1: player.location.X -= player.velocity.X;
                        break;

                    case 2: player.location.Y += player.velocity.Y;
                        break;

                    case 3: player.location.X += player.velocity.X;
                        break;

                    default: break;
                }
            }
            windowSpace.X = (int)(player.location.X + (player.rectangle.Width / 2)) - (windowSpace.Width / 2);
            windowSpace.Y = (int)(player.location.Y + (player.rectangle.Height / 2)) - (windowSpace.Height / 2);

            AIManager.SharedAIManager.lastKnownPlayerLoc = player.location;
        }
    }
}
