using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    interface IEnemy
    {
        Vector2 Velocity { get; set; }
        Vector2 Location { get; set; }

        ICommand CurrentCommand { get; set; }
        Queue<ICommand> DefaultCommands { get; set; }
        Queue<ICommand> CommandQueue { get; set; }

        void Move(Vector2 speed);
    }
}
