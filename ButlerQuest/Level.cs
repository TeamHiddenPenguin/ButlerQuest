// Drew Stanton
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
    public class Level
    {
        // properties
        public Player player; // the player. It's public so we can transfer info from one level to another (lives remaining, possibly score) without having to make the player in the gamestate management.
        public List<Enemy> basicEnemies; // a list to hold all of the normal butlers for the level
        public List<Wall> walls; // a list to hold all of the walls for the level
        List<Weapon> weapons; // a list to hold all of the weapons for the level
        List<Coin> coins; // a list to hold all of the coins for the level
        public Map levelMap; // parses the map and tiles used for the specific level
        public Rectangle windowSpace; // window space used for drawing the map
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;

        // constructor
        public Level(string mapFile)
        {
            basicEnemies = new List<Enemy>();
            walls = new List<Wall>();
            weapons = new List<Weapon>();
            coins = new List<Coin>();

            graphics = ScreenManager.SharedManager.gDevice;
            spriteBatch = ScreenManager.SharedManager.sBatch;

            levelMap = new Map(mapFile, new int[2] { 1, int.MaxValue });

            
            foreach (var groupname in levelMap.ObjectGroups.Keys) // takes each entity in the map file and creates objects based on their type.
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
                        else if (entity.Type == "Weapon")
                        {
                            weapons.Add(EntityGenerator.GenerateWeapon(new Vector3(entity.X, entity.Y, currentFloor), 2, entity.Properties.Find(x => x.Item1 == "type").Item2));
                        }
                        else if (entity.Type == "Coin")
                        {
                            coins.Add(EntityGenerator.GenerateCoin(new Vector3(entity.X, entity.Y, currentFloor), int.Parse(entity.Properties.Find(x => x.Item1 == "value").Item2)));
                        }
                    }
                }
                //Otherwise it's a floor graph and sam will write this code later when it's relevant
            }

            windowSpace = new Rectangle((int)(player.location.X + (player.rectangle.Width / 2)) - (graphics.Viewport.Width / 2), (int)(player.location.Y + (player.rectangle.Height / 2)) - (graphics.Viewport.Height / 2), graphics.Viewport.Width, graphics.Viewport.Height);

            //AIManager.DebugInitialize(this);

        }

        // methods
        // calls the draw method of everything that is drawn
        public void Draw(GameTime gameTime)
        {
            Matrix translation = Matrix.CreateTranslation(-windowSpace.X, -windowSpace.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, translation);

            levelMap.Draw(windowSpace, (int)player.location.Z);

            if (basicEnemies != null)
                foreach (Enemy enemy in basicEnemies)
                    if (enemy.alive)
                        enemy.Draw(spriteBatch);


            if (weapons != null)
                foreach (Weapon weapon in weapons) 
                    weapon.Draw(spriteBatch);


            if (coins != null)
                foreach (Coin coin in coins)
                    if (coin.active)
                        coin.Draw(spriteBatch);


            if (player.direction == 1 || player.direction == 2)
            {
                player.Draw(spriteBatch);
                if (player.currentWeapon != null)
                    player.currentWeapon.Draw(spriteBatch);
            }
            else
            {
                if (player.currentWeapon != null)
                    player.currentWeapon.Draw(spriteBatch);

                player.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        // calls the update methods for every object that has one. Checks player's collison with all objects.
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            if (weapons.Count != 0) foreach (Weapon weapon in weapons) weapon.Update(gameTime);
            if (coins.Count != 0) foreach (Coin coin in coins) coin.Update(gameTime);

            AIManager.SharedAIManager.MakePaths();

            //Enemy updates and collision
            foreach (Enemy enemy in basicEnemies) // updates enemies and checks for collision with player if on the same floor.
            {
                if (enemy.alive)
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
                                ForceGlobalAIStateChange(AI_STATE.UNAWARE);
                                break;
                        }
                    }

                    // only worry about collision if the player is attacking
                    if (player.CurrentAnimation.Contains("Attack"))
                    {
                        int collision = player.currentWeapon.CollisionSide(enemy);
                        if (collision > -1)
                        {
                            enemy.alive = false;
                            coins.Add(EntityGenerator.GenerateCoin(enemy.location, enemy.moneyValue));
                        }
                    }
                }
            }
            // wall collision
            foreach (Wall block in walls)
            {
                int collision = player.CollisionSide(block);
                switch (collision)
                {
                    case 0: player.location.Y += player.velocity.Y;
                        break;

                    case 1: player.location.X -= player.velocity.X;
                        break;

                    case 2: player.location.Y -= player.velocity.Y;
                        break;

                    case 3: player.location.X += player.velocity.X;
                        break;

                    default: break;
                }
            }

            // coin collision
            for (int i = 0; i < coins.Count; i++)
            {
                int collision = player.CollisionSide(coins[i]);
                if (collision > -1)
                {
                    player.moneyCollected += coins[i].InteractWith();
                }
            }

            // weapon collision
            if (player.currentWeapon == null)
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    int collision = player.CollisionSide(weapons[i]);
                    if (collision > -1)
                    {
                        player.currentWeapon = weapons[i];
                        weapons.Remove(weapons[i]);

                        player.currentWeapon.rectangle.Width = (int)(player.currentWeapon.rectangle.Width / 1.2);
                        player.currentWeapon.rectangle.Height = (int)(player.currentWeapon.rectangle.Height / 1.2);
                    }
                }
            }

            windowSpace.X = (int)(player.location.X + (player.rectangle.Width / 2)) - (windowSpace.Width / 2);
            windowSpace.Y = (int)(player.location.Y + (player.rectangle.Height / 2)) - (windowSpace.Height / 2);

            AIManager.SharedAIManager.playerLoc = player.center;
        }

        public void ForceGlobalAIStateChange(AI_STATE newState)
        {
            foreach (var enemy in basicEnemies)
            {
                enemy.state = newState;
                enemy.commandQueue.Clear();
                enemy.currentCommand = null;
            }
        }
    }
}
