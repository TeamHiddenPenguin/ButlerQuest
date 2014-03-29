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
        Rectangle windowSpace; // window space used for drawing the map
        GameTime gameTime;
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;

        // constructor
        public Level(string mapFile)
        {
            gameTime = new GameTime();

            graphics = new GraphicsDevice();
            spriteBatch = new SpriteBatch(graphics);

            levelMap = new Map(mapFile, graphics, spriteBatch);

            player = EntityGenerator.GeneratePlayer(new Vector3(levelMap.ObjectGroups["Player"][0].X, levelMap.ObjectGroups["Player"][0].Y, 0), 5, 500);

            windowSpace = new Rectangle((int)player.location.X, (int)player.location.Y, 1200, 720);

            for (int i = 0; i < levelMap.ObjectGroups["Enemy"].Count; i++)
            {
                basicEnemies.Add(EntityGenerator.GenerateEnemy(new Vector3(levelMap.ObjectGroups["Enemy"][i].X, levelMap.ObjectGroups["Enemy"][i].Y, 0), levelMap.ObjectGroups["Enemy"][i].Properties));
            }

            for (int i = 0; i < levelMap.ObjectGroups["Wall"].Count; i++)
            {
                walls.Add(EntityGenerator.GenerateWall(new Vector3(levelMap.ObjectGroups["Wall"][i].X, levelMap.ObjectGroups["Wall"][i].Y, 0), levelMap.ObjectGroups["Wall"][i].Width, levelMap.ObjectGroups["Wall"][i].Height));
            }

            AIManager.DebugInitialize(this);

        }

        // methods
        // calls the draw method of everything that is drawn
        public void Draw(SpriteBatch spritebatch)
        {
            levelMap.DrawToTexture(windowSpace, (int)player.location.Z);

            spritebatch.Begin();

            foreach (Enemy enemy in basicEnemies) enemy.Draw(spritebatch);

            player.Draw(spritebatch);

            spritebatch.End();
        }

        // calls the update methods for every object that has one. Checks player's collison with all objects.
        public void Update()
        {
            player.Update(gameTime);

            AIManager.SharedAIManager.MakePaths();

            foreach (Enemy enemy in basicEnemies) // updates enemies and checks for collision with player if on the same floor.
            {
                enemy.Update(gameTime);

                if (player.location.Z == enemy.location.Z)
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

            foreach (Wall block in walls)
            {
                int collision = player.CollisionSide(block);
                switch (collision)
                {
                    case 0: player.location.Y -= player.velocity.Y;
                        windowSpace.Y -= (int)player.velocity.Y;
                        break;

                    case 1: player.location.X -= player.velocity.X;
                        windowSpace.X -= (int)player.velocity.X;
                        break;

                    case 2: player.location.Y += player.velocity.Y;
                        windowSpace.Y += (int)player.velocity.Y;
                        break;

                    case 3: player.location.X += player.velocity.X;
                        windowSpace.X += (int)player.velocity.X;
                        break;

                    default: break;
                }
            }
        }
    }
}
