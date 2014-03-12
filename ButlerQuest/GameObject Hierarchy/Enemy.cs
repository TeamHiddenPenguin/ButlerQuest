using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class Enemy : MovableGameObject
    {
        public ICommand currentCommand;
        public Queue<ICommand> defaultCommands;
        public Queue<ICommand> commandQueue;
    }
}
