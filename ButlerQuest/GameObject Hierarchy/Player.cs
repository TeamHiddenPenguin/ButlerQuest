// Drew Stanton
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ButlerQuest
{
    // holds all the info for the player; textures, lives, location, etc
    public class Player : MovableGameObject
    {
        // properties
        public int lives; // the number of lives the player has remaining. if it drops to 0, game ends.
        public int moneyCollected; // the amount of money the player has collected in the level.
        int moneyNeeded; // the total amount of money the player needs to collect to beat the level.
        public Vector3 startLoc; // the stat location of the player
        public Weapon currentWeapon; // the player's current weapon
        

        // constructor
        public Player(Vector3 velocity, Animation[] animations, string[] names, Vector3 location, Rectangle rectangle, int life, int moneyGoal)
            : base (velocity, animations, names, location, rectangle)
        {
            startLoc = location;

            // not currently used for anything, but will be implemented by next milestone.
            lives = life;
            moneyNeeded = moneyGoal;
            moneyCollected = 0;

            currentWeapon = null;
        }

        public void Attack() // sets the player's animation and weapon depending on what weapon is equipped
        {
            if (currentWeapon != null)
            {
                switch (direction)
                {
                    case 0: currentWeapon.location.X = this.location.X + 10;
                        currentWeapon.location.Y = this.location.Y - 10;
                        CurrentAnimation = "UpAttack";
                        break;
                    case 1: currentWeapon.location.X = this.location.X + 20;
                        currentWeapon.location.Y = this.location.Y - 5;
                        CurrentAnimation = "RightAttack";
                        break;
                   case 2: currentWeapon.location.X = this.location.X;
                        currentWeapon.location.Y = this.location.Y - 5;
                        CurrentAnimation = "DownAttack";
                        break;
                    case 3: currentWeapon.location.X = this.location.X - 5;
                        currentWeapon.location.Y = this.location.Y - 5;
                        CurrentAnimation = "LeftAttack";
                        break;
                    default: break;
                }
                currentWeapon.visible = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (currentWeapon != null)
            {
                currentWeapon.visible = false;
                currentWeapon.Update(gameTime);
            }
        }
    }
}
