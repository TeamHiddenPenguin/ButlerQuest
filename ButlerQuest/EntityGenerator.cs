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
            Texture2D tex = ScreenManager.SharedManager.Content.Load<Texture2D>("SpriteSheetEnemies.png");

            Enemy temp = new Enemy(
                new Vector3(2.3f, 2.3f, 1),
                new Animation[5]{
                    new Animation(0,0,40,40,4,4,1, tex), //default
                    new Animation(4,5,40,40,4,4,15, tex), //WalkUp
                    new Animation(11,15,40,40,4,4,5,tex), //WalkRight
                    new Animation(1,2,40,40,4,4,15, tex), //WalkDown
                    new Animation(6,10,40,40,4,4,5,tex) //WalkLeft
                },
                new string[5] { "default", "WalkUp", "WalkRight", "WalkDown", "WalkLeft" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    40, 40),
                    100);

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

        /// <summary>
        /// Generates a Player object
        /// </summary>
        /// <param name="position">The position of the player</param>
        /// <param name="lives">the number of lives the player has</param>
        /// <param name="goal">The final goal of the player</param>
        /// <returns>A player object</returns>
        public static Player GeneratePlayer(Vector3 position, int lives, int goal)
        {
            Texture2D tex = ScreenManager.SharedManager.Content.Load<Texture2D>("SpriteSheetAlfredJeevesIII.png");

            Player player = new Player(
                new Vector3(2, 2, 1),
                new Animation[5] {
                    new Animation(0,0,40,40,4,4,1, tex), //default
                    new Animation(4,5,40,40,4,4,15, tex), //WalkUp
                    new Animation(11,15,40,40,4,4,5,tex), //WalkRight
                    new Animation(1,2,40,40,4,4,15, tex), //WalkDown
                    new Animation(6,10,40,40,4,4,5,tex) //WalkLeft
                },
                new string[5] { "default", "WalkUp", "WalkRight", "WalkDown", "WalkLeft" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    40, 40),
                lives,
                goal);

            

            return player;
        }


        public static Weapon GenerateWeapon(Vector3 position, int durability, string weaponType)
        {
            Texture2D tex = ScreenManager.SharedManager.Content.Load<Texture2D>("player.png");

            Weapon weapon = new Weapon(
                new Animation[1] {
                    new Animation(0, 0, 40, 40, 1, 1, 1, tex)
                },
                new String[1] { weaponType },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    40, 40),
                durability,
                new Vector3(),
                new Rectangle());

            weapon.CurrentAnimation = weaponType;



            return weapon;
        }


        public static Coin GenerateCoin(Vector3 position, int value)
        {
            Texture2D tex = ScreenManager.SharedManager.Content.Load<Texture2D>("Target.png");

            Coin coin = new Coin(
                new Animation[1] {
                    new Animation(0, 0, 40, 40, 1, 1, 1, tex)
                },
                new String[1] { "coin" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    40, 40),
                value);

            coin.CurrentAnimation = "coin";


            return coin;
        }
    }
}
