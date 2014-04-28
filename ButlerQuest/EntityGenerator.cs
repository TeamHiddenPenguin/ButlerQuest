//Written by Sam Sternklar (mostly), and Jesse Florio
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    static class EntityGenerator
    {
        /// <summary>
        /// Makes an enemy object
        /// </summary>
        /// <param name="position">position to make the enemy at</param>
        /// <param name="commands">default commands</param>
        /// <returns>An enemy</returns>
        public static Enemy GenerateEnemy(Vector3 position, List<Tuple<string, string>> commands)
        {
            Enemy temp = new Enemy(
                GameVariables.enemyMoveSpeed,
                GameVariables.enemyAnimations,
                GameVariables.enemyAnimationNames,
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    GameVariables.tileWidth, GameVariables.tileHeight),
                    GameVariables.normalEnemyCash);

            foreach (var parseable in commands)
            {
                switch (parseable.Item1.ToUpper().Trim('0', '1', '2', '3', '4', '5', '6', '7', '8', '9'))
                {
                    case "MOVE":
                        //format is "X,Y,Z"
                        Vector3 moveTo = new Vector3();
                            string[] coords = parseable.Item2.Split(',');
                            float.TryParse(coords[0], out moveTo.X);
                            float.TryParse(coords[1], out moveTo.Y);
                            float.TryParse(coords[2], out moveTo.Z);
                            temp.defaultCommands.Enqueue(new CommandMove(moveTo, temp));
                        break;
                    case "WAIT":
                        int time = 0;
                        int.TryParse(parseable.Item2, out time);
                        temp.defaultCommands.Enqueue(new CommandWait(time));
                        break;
                    case "END ":
                        temp.defaultCommands.Enqueue(new GetNextCommandSet(temp, int.MaxValue));
                        temp.defaultCommands.Enqueue(new WaitForNextCommand(temp));
                        break;
                    default:
                        break;
                }
            }

            return temp;
        }

        /// <summary>
        /// Makes a wall with a given width and height
        /// </summary>
        /// <param name="position">position of the wall</param>
        /// <param name="width">width of the wall</param>
        /// <param name="height">height of the wall</param>
        /// <returns>A wall</returns>
        public static Wall GenerateWall(Vector3 position, int width, int height)
        {
            return new Wall(position, width, height);
        }

        public static Door GenerateDoor(Vector3 position, int doorNum)
        {
            Door door = new Door(
                GameVariables.doorAnimation,
                new String[1] { "door" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    GameVariables.tileWidth, GameVariables.tileHeight),
                    doorNum);

            return door;
        }

        public static Key GenerateKey(Vector3 position, int keyNum)
        {
            Key key = new Key(
                GameVariables.keyAnimation,
                new String[1] { "key" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    GameVariables.tileWidth, GameVariables.tileHeight),
                    keyNum);

            return key;
        }

        /// <summary>
        /// Generates a Player object
        /// </summary>
        /// <param name="position">The position of the player</param>
        /// <param name="lives">the number of lives the player has</param>
        /// <param name="goal">The final goal of the player</param>
        /// <returns>A player object</returns>
        public static Player GeneratePlayer(Vector3 position, int lives, int goal)
        {
            Player player = new Player(
                GameVariables.playerSpeed,
                GameVariables.playerAnimations,
                GameVariables.playerAnimationNames,
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    GameVariables.tileWidth, GameVariables.tileHeight),
                lives,
                goal);

            

            return player;
        }


        public static Weapon GenerateWeapon(Vector3 position, int durability, string weaponType)
        {
            

            Weapon weapon = new Weapon(
                GameVariables.GetWeaponAnimations(weaponType),
                new String[1] { weaponType },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    GameVariables.tileWidth, GameVariables.tileHeight),
                durability);

            weapon.CurrentAnimation = weaponType;



            return weapon;
        }


        public static Coin GenerateCoin(Vector3 position, int value)
        {

            Coin coin = new Coin(
                GameVariables.coinAnimation,
                new String[1] { "coin" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    GameVariables.tileWidth, GameVariables.tileHeight),
                value);

            coin.CurrentAnimation = "coin";

            return coin;
        }
    }
}
