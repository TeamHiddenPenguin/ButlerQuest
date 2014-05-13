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
        List<Door> doors; // a list to hold all of the locked doors for a level.
        List<Key> keys; // a list to hold all of the keys for a level.
        List<Disguise> disguises; // a list to hold all of the disguises for a level.
        List<FloorSwitcher> floorSwitchers; // a list to hold all of the floor switchers
        public Map levelMap; // parses the map and tiles used for the specific level.
        public Rectangle windowSpace; // window space used for drawing the map.
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        public RoomGraph roomGraph;
        public string mapFile;
        SpriteFont font;

        // constructor
        public Level(string mapFile)
        {
            this.mapFile = mapFile;
            basicEnemies = new List<Enemy>();
            walls = new List<Wall>();
            weapons = new List<Weapon>();
            coins = new List<Coin>();
            doors = new List<Door>();
            keys = new List<Key>();
            disguises = new List<Disguise>();
            floorSwitchers = new List<FloorSwitcher>();
            font = ScreenManager.SharedManager.Content.Load<SpriteFont>("Arial");

            graphics = ScreenManager.SharedManager.gDevice;
            spriteBatch = ScreenManager.SharedManager.sBatch;

            levelMap = new Map(mapFile, new int[8] { 1, int.MaxValue, 1, 1, 1, 1, 0, 0 });


            roomGraph = new RoomGraph();
            foreach (var groupname in levelMap.ObjectGroups.Keys) // Sam did this loop and everything inside it.
            {
                int currentFloor = int.Parse(groupname[5].ToString()) - 1;
                if (groupname.Substring(6, 8) == "Entities")
                {
                    foreach (var entity in levelMap.ObjectGroups[groupname])
                    {
                        if (entity.Type == "Enemy")
                        {
                            basicEnemies.Add(EntityGenerator.GenerateEnemy(new Vector3(entity.X, entity.Y, currentFloor), entity.Properties, int.Parse(entity.Properties.Find(x => x.Item1 == "startDirection").Item2)));
                        }
                        else if (entity.Type == "Player")
                        {
                            player = EntityGenerator.GeneratePlayer(new Vector3(entity.X, entity.Y, currentFloor), 5, 400);
                        }
                        else if (entity.Type == "Wall")
                        {
                            walls.Add(EntityGenerator.GenerateWall(new Vector3(entity.X, entity.Y, currentFloor), entity.Width, entity.Height));
                        }
                        else if (entity.Type == "LockedDoor")
                        {
                            doors.Add(EntityGenerator.GenerateDoor(new Vector3(entity.X, entity.Y, currentFloor), doors.Count));
                        }
                        else if (entity.Type == "Key")
                        {
                            keys.Add(EntityGenerator.GenerateKey(new Vector3(entity.X, entity.Y, currentFloor), keys.Count));
                        }
                        else if (entity.Type == "Weapon")
                        {
                            weapons.Add(EntityGenerator.GenerateWeapon(new Vector3(entity.X, entity.Y, currentFloor), int.Parse(entity.Properties.Find(x => x.Item1 == "durability").Item2), entity.Properties.Find(x => x.Item1 == "type").Item2));
                        }
                        else if (entity.Type == "Coin")
                        {
                            coins.Add(EntityGenerator.GenerateCoin(new Vector3(entity.X, entity.Y, currentFloor), int.Parse(entity.Properties.Find(x => x.Item1 == "value").Item2)));
                        }
                        else if (entity.Type == "Disguise")
                        {
                            disguises.Add(EntityGenerator.GenerateDisguise(new Vector3(entity.X, entity.Y, currentFloor), entity.Properties.Find(x => x.Item1 == "disguiseType").Item2));
                        }
                        else if (entity.Type == "FloorSwitcher")
                        {
                            floorSwitchers.Add(new FloorSwitcher(new Rectangle(entity.X, entity.Y, entity.Width, entity.Height), currentFloor, currentFloor + 1, bool.Parse(entity.Properties.Find(x => x.Item1 == "Horizontal").Item2)));
                        }
                    }
                }
                if (groupname.Contains("Room"))
                {
                    foreach (var entity in levelMap.ObjectGroups[groupname])
                    {
                        RoomGraphNode node = new RoomGraphNode(new Rectangle(entity.X, entity.Y, entity.Width, entity.Height), currentFloor, entity.Name);
                        List<string> connections = new List<string>();
                        foreach (var t in entity.Properties)
                        {
                            if (t.Item1 != null)
                                connections.Add(t.Item1);
                            if (t.Item2 != null)
                                node.validDisguises.Add(t.Item2);
                        }
                        roomGraph.AddNode(node, connections);
                    }
                }
                player.moneyNeeded = (basicEnemies.Count + coins.Count) * 70; 
            }

            foreach (Door lockedDoor in doors)
            {
                if (keys[lockedDoor.keyAssociation] == null)
                    lockedDoor.locked = false;
            }

            windowSpace = new Rectangle((int)(player.location.X + (player.rectangle.Width / 2)) - (graphics.Viewport.Width / 2), (int)(player.location.Y + (player.rectangle.Height / 2)) - (graphics.Viewport.Height / 2), graphics.Viewport.Width, graphics.Viewport.Height);

        }

        // methods
        // calls the draw method of everything that is drawn
        public void Draw(GameTime gameTime)
        {
            Matrix translation = Matrix.CreateTranslation(-windowSpace.X, -windowSpace.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, translation);

            levelMap.Draw(windowSpace, (int)player.location.Z);

            if (weapons != null)
                foreach (Weapon weapon in weapons) 
                    if (weapon.location.Z == player.location.Z)
                        weapon.Draw(spriteBatch);

            if (coins != null)
                foreach (Coin coin in coins)
                    if (coin.active && coin.location.Z == player.location.Z)
                        coin.Draw(spriteBatch);

            if (keys != null)
                foreach (Key key in keys)
                    if (key.location.Z == player.location.Z)
                        key.Draw(spriteBatch);

            if (disguises != null)
                foreach (Disguise disguise in disguises)
                    if (disguise.location.Z == player.location.Z)
                        disguise.Draw(spriteBatch);

            if (basicEnemies != null)
                foreach (Enemy enemy in basicEnemies)
                    if (enemy.alive && enemy.location.Z == player.location.Z)
                        enemy.Draw(spriteBatch);

            if (doors != null)
                foreach (Door door in doors)
                    if (door.locked && door.location.Z == player.location.Z)
                        door.Draw(spriteBatch);

            if (player.direction == 1 || player.direction == 2)
            {
                player.Draw(spriteBatch);

                if (player.currentDisguise != null)
                    player.currentDisguise.Draw(spriteBatch);

                if (player.currentWeapon != null)
                    player.currentWeapon.Draw(spriteBatch);
            }
            else
            {
                if (player.currentWeapon != null)
                    player.currentWeapon.Draw(spriteBatch);

                player.Draw(spriteBatch);

                if (player.currentDisguise != null)
                    player.currentDisguise.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, "Money: " + player.moneyCollected + " / " + player.moneyNeeded, new Vector2(windowSpace.X, windowSpace.Y), Color.FromNonPremultiplied(255,114,0,255));
            spriteBatch.End();
        }

        // calls the update methods for every object that has one. Checks player's collison with all objects.
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            if (weapons.Count != 0) foreach (Weapon weapon in weapons) weapon.Update(gameTime);
            if (coins.Count != 0) foreach (Coin coin in coins) coin.Update(gameTime);
            if (keys.Count != 0) foreach (Key key in keys) key.Update(gameTime);
            if (doors.Count != 0) foreach (Door door in doors) door.Update(gameTime);
            if (disguises.Count != 0) foreach (Disguise disguise in disguises) disguise.Update(gameTime);

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
                            default:
                                if(enemy.CloseRect.Intersects(player.CloseRect))
                                {
                                    ScreenManager.SharedManager.PopScreen();
                                    ScreenManager.SharedManager.AddScreen(new GameOverScreen(mapFile));
                                }
                                break;
                        }
                    }

                    // only worry about collision if the player is attacking
                    if (player.CurrentAnimation.Contains("Attack") && player.currentWeapon != null)
                    {
                        player.currentWeapon.location.Z = player.location.Z;
                        int collision = player.currentWeapon.CollisionSide(enemy);
                        if (collision > -1)
                        {
                            enemy.alive = false;
                            coins.Add(EntityGenerator.GenerateCoin(enemy.location, enemy.moneyValue));
                            player.currentWeapon.durability--;
                            if (player.currentWeapon.durability == 0)
                                player.currentWeapon = null;
                            if (enemy.state > AI_STATE.AWARE)
                            {
                                AIManager.SharedAIManager.RemoveFromPursuit(enemy);
                            }
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

            // door collision
            foreach (Door block in doors)
            {
                if (block.locked)
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
            }

            // key collision
            for (int i = 0; i < keys.Count; i++)
            {
                int collision = player.CollisionSide(keys[i]);
                if (collision > -1)
                {
                    doors.Remove(doors[keys[i].doorAssociation]);
                    keys.Remove(keys[i]);
                }
            }

            // coin collision
            for (int i = 0; i < coins.Count; i++)
            {
                if (coins[i].active == true)
                {
                    int collision = player.CollisionSide(coins[i]);
                    if (collision > -1)
                    {
                        int collected = coins[i].InteractWith();
                        player.moneyCollected += collected;
                        coins[i].active = false;
                    }
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
                        player.currentWeapon.visible = false;

                        player.currentWeapon.rectangle.Width = (int)(player.currentWeapon.rectangle.Width / 1.3);
                        player.currentWeapon.rectangle.Height = (int)(player.currentWeapon.rectangle.Height / 1.3);
                    }
                }
            }

            // disguise collision
                for (int i = 0; i < disguises.Count; i++)
                {
                    int collision = player.CollisionSide(disguises[i]);
                    if (collision > -1)
                    {
                        if (player.currentDisguise == null)
                        {
                            player.currentDisguise = disguises[i];
                            disguises.Remove(disguises[i]);
                            player.ResetAllAnimations();
                            player.currentDisguise.ResetAllAnimations();
                        }
                        else
                        {
                            switch (collision)
                            {
                                case 0:
                                    disguises.Add(
                                        EntityGenerator.GenerateDisguise(
                                        new Vector3(player.location.X, player.location.Y + 40, player.location.Z),
                                        player.currentDisguise.disguiseType)
                                        );

                                    player.currentDisguise = disguises[i];
                                    disguises.Remove(disguises[i]);
                                    player.ResetAllAnimations();
                                     player.currentDisguise.ResetAllAnimations();
                                    break;

                                case 1:
                                    disguises.Add(
                                        EntityGenerator.GenerateDisguise(
                                        new Vector3(player.location.X - 40, player.location.Y, player.location.Z),
                                        player.currentDisguise.disguiseType)
                                        );

                                    player.currentDisguise = disguises[i];
                                    disguises.Remove(disguises[i]);
                                    player.ResetAllAnimations();
                                    player.currentDisguise.ResetAllAnimations();
                                    break;

                                case 2:
                                    disguises.Add(
                                        EntityGenerator.GenerateDisguise(
                                        new Vector3(player.location.X, player.location.Y - 40, player.location.Z),
                                        player.currentDisguise.disguiseType)
                                        );

                                    player.currentDisguise = disguises[i];
                                    disguises.Remove(disguises[i]);
                                    player.ResetAllAnimations();
                                    player.currentDisguise.ResetAllAnimations();
                                    break;

                                case 3:
                                    disguises.Add(
                                        EntityGenerator.GenerateDisguise(
                                        new Vector3(player.location.X + 40, player.location.Y, player.location.Z),
                                        player.currentDisguise.disguiseType)
                                        );

                                    player.currentDisguise = disguises[i];
                                    disguises.Remove(disguises[i]);
                                    player.ResetAllAnimations();
                                    player.currentDisguise.ResetAllAnimations();
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            foreach (var fs in floorSwitchers)
            {
                if (fs.Collides(player))
                    System.Diagnostics.Debug.WriteLine("Switched Floor");
            }

            windowSpace.X = (int)(player.location.X + (player.rectangle.Width / 2)) - (windowSpace.Width / 2);
            windowSpace.Y = (int)(player.location.Y + (player.rectangle.Height / 2)) - (windowSpace.Height / 2);

            AIManager.SharedAIManager.playerLoc = player.center;

            if (player.moneyCollected >= player.moneyNeeded)
            {
                ScreenManager.SharedManager.NextScreen();
                ScreenManager.SharedManager.AddScreen(new VictoryScreen());
            }
        }
    }
}
