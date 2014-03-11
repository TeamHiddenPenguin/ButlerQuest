using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    interface ICommand
    {
        //Showing people how to use git stuff
        bool IsFinished
        {
            get;
            set;
        }
        void Update(GameTime gameTime);
    }
}
