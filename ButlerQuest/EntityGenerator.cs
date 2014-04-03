using Microsoft.Xna.Framework;
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
                new Vector3(1, 1, 1),
                null,
                new string[4] { "WalkUp", "WalkRight", "WalkDown", "WalkLeft" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    32, 32));

            foreach (var parseable in commands)
            {
                switch (parseable.Item1.ToUpper())
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
                    case "END":
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
            return new Player(
                new Vector3(1, 1, 1),
                null,
                new string[8] { "WalkUp", "WalkRight", "WalkDown", "WalkLeft", "AttackUp", "AttackRight", "AttackDown", "AttackLeft" },
                position,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    32, 32),
                lives,
                goal);
        }

    }
}
