﻿//Written by Samuel Sternklar
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    //This class initializes an update-based timer, and when it is finished it ends.
    class CommandWait : ICommand
    {
        public bool IsFinished
        {
            get;
            set;
        }

        private int timer;
        private int frames;

        public CommandWait(int frames)
        {
            this.frames = frames;
            Initialize();
        }

        public void Initialize()
        {
            IsFinished = false;
            timer = 0;
        }

        public void Update(GameTime gameTime)
        {
            timer++;
            if (timer >= frames)
                IsFinished = true;
        }
    }
}
