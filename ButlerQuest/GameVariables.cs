using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Jesse Florio

namespace ButlerQuest
{
    //  contains various variables used throughout the game
    static class GameVariables
    {
        // sets the width and height of the tiles in the game
        public static int tileWidth = 40;
        public static int tileHeight = 40;

        //  ENEMY VARIABLES
        // enemy money 
        public static int normalEnemyCash = 100;  // indicaters how much money is dropped by normal enemies.  Will be altered for special enemies.

        // sets the file for the enemy's spritesheet
        static string enemyFile = "SpriteSheetEnemies.png";
        public static Texture2D enemyTex = ScreenManager.SharedManager.Content.Load<Texture2D>(enemyFile);

        // populate animation array for enemies
        public static Animation[] enemyAnimations = new Animation[5]{
                    new Animation(0,0,40,40,4,4,1, enemyTex), //default
                    new Animation(4,5,40,40,4,4,15, enemyTex), //WalkUp
                    new Animation(11,15,40,40,4,4,5,enemyTex), //WalkRight
                    new Animation(1,2,40,40,4,4,15, enemyTex), //WalkDown
                    new Animation(6,10,40,40,4,4,5,enemyTex)};//WalkLeft;

        // add in names of enemy animation frames
        public static string[] enemyAnimationNames = new string[5] { "default", "WalkUp", "WalkRight", "WalkDown", "WalkLeft" };
        
        // sets how fast the enemies move
        public static Vector3 enemyMoveSpeed = new Vector3(2.3f, 2.3f, 1);

        //  PLAYER VARIABLES
        // sets files for player's spritesheet
        static string playerFile = "SpriteSheetAlfredJeevesIII.png";
        public static Texture2D playerTex = ScreenManager.SharedManager.Content.Load<Texture2D>(playerFile);

        // populate animation array for player
        public static Animation[] playerAnimations = new Animation[9] {
                    new Animation(0,0,40,40,4,4,1, playerTex), //default
                    new Animation(4,5,40,40,4,4,15, playerTex), //WalkUp
                    new Animation(11,15,40,40,4,4,5,playerTex), //WalkRight
                    new Animation(1,2,40,40,4,4,15, playerTex), //WalkDown
                    new Animation(6,10,40,40,4,4,5,playerTex), //WalkLeft

                    // placeholders until attack animation is a thing.
                    new Animation(0,0,40,40,4,4,1, playerTex),
                    new Animation(0,0,40,40,4,4,1, playerTex),
                    new Animation(0,0,40,40,4,4,1, playerTex),
                    new Animation(0,0,40,40,4,4,1, playerTex),
                };

        // add in the names of the player's animation frames
        public static string[] playerAnimationNames = new string[9] { "default", "WalkUp", "WalkRight", "WalkDown", "WalkLeft", "UpAttack", "RightAttack", "DownAttack", "LeftAttack" };

        // sets how fast the player moves
        public static Vector3 playerSpeed = new Vector3(2, 2, 1);

        // WEAPON VARIABLES 
        // sets the file for the weapon spritesheet / file
        static string weaponFileCandleStick = "CandleStick.png";
        static string weaponFileTray = "Tray.png";
        static string weaponFileVase = "Vase.png";

        public static Texture2D weaponTex = ScreenManager.SharedManager.Content.Load<Texture2D>(weaponFileCandleStick);
        public static Texture2D weaponTex2 = ScreenManager.SharedManager.Content.Load<Texture2D>(weaponFileTray);
        public static Texture2D weaponTex3 = ScreenManager.SharedManager.Content.Load<Texture2D>(weaponFileVase);

        // populate animation arrays for three weapons
        public static Animation[] weaponCandleStickAnimation = new Animation[1] {
                    new Animation(0, 0, 40, 40, 1, 1, 1, weaponTex)
                };
        public static Animation[] weaponTrayAnimaton = new Animation[1] {
                    new Animation(0, 0, 40, 40, 1, 1, 1, weaponTex2)
                };
        public static Animation[] weaponVaseAnimation = new Animation[1] {
                    new Animation(0, 0, 40, 40, 1, 1, 1, weaponTex3)
                };

        // switch statement to determine which animation will be used, depending on the weapon's type
        public static Animation[] GetWeaponAnimations(string weaponType)
        {
            switch (weaponType)
            {
                case "CandleStick":
                    return weaponCandleStickAnimation;

                case "Tray":
                    return weaponTrayAnimaton;

                case "Vase":
                    return weaponVaseAnimation;

                default:
                    return null;
            }
        }

        //  COIN VARIABLES
        // sets the file for the coin's image file
        static string coinFile = "Coin.png";
        public static Texture2D coinTex = ScreenManager.SharedManager.Content.Load<Texture2D>(coinFile);

        // populate animation array for coin
        public static Animation[] coinAnimation = new Animation[1] {
                    new Animation(0, 0, 40, 40, 1, 1, 1, coinTex)
                };


        // KEY VARIABLES
        // sets the file for the key's image
        static string keyFile = "target.png";
        public static Texture2D keyTex = ScreenManager.SharedManager.Content.Load<Texture2D>(keyFile);

        // populates animation array for key
        public static Animation[] keyAnimation = new Animation[1] {
                    new Animation(0, 0, 40, 40, 1, 1, 1, keyTex)
                };


        // DOOR VARIABLES
        // sets the file for the door's image
        static string doorFile = "Wall&Floor.png";
        public static Texture2D doorTex = ScreenManager.SharedManager.Content.Load<Texture2D>(doorFile);

        // populates animation array for door
        public static Animation[] doorAnimation = new Animation[1] {
                    new Animation(1, 1, 40, 40, 1, 2, 1, doorTex)
                };
    }
}
