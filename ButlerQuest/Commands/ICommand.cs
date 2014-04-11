//Written by Samuel Sternklar
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    public interface ICommand
    {
        //Tells whether or not the command is finished executing
        bool IsFinished
        {
            get;
            set;
        }
        //Update the command
        void Update(GameTime gameTime);
        //Initialize the command, replaces the constructor because we need to finish constructing the command when it activates first
        void Initialize();
    }
}
