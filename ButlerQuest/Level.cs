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
        Map levelMap; // parses the map and tiles used for the specific level
        Rectangle windowSpace; // window space used for drawing the map
        int floor; // the floor the player is on.
        GameTime gameTime;

        // constructor
        public Level(Player plyr, List<Enemy> enemies, Map map, GameTime gt)
        {
            player = plyr;
            basicEnemies = enemies;
            levelMap = map;
            gameTime = gt;
        }

        // methods
        // calls the draw method of everything that is drawn
        public void Draw(SpriteBatch spritebatch)
        {
            levelMap.DrawToTexture(windowSpace, floor);

            spritebatch.Begin();

            foreach (Enemy enemy in basicEnemies) enemy.Draw(spritebatch);

            player.Draw(spritebatch);

            spritebatch.End();
        }

        // calls the update methods for every object that has one
        public void Update()
        {
            player.Update(gameTime);

            foreach (Enemy enemy in basicEnemies) enemy.Update(gameTime);
        }
    }
}
