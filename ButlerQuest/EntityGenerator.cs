using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    static class EntityGenerator
    {
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

        public static Wall GenerateWall(Vector3 position, int width, int height)
        {
            return new Wall(position, width, height);
        }

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
