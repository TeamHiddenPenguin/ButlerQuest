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
        public static Animation[] enemyAnimations = new Animation[8]{
                    new Animation(13,13,40,40,4,4,1, enemyTex), //StandUp
                    new Animation(10,10,40,40,4,4,1, enemyTex), //StandRight
                    new Animation(0,0,40,40,4,4,1, enemyTex), //StandDown
                    new Animation(5,5,40,40,4,4,1, enemyTex), //StandLeft
                    new Animation(13,15,40,40,4,4,15, enemyTex), //WalkUp
                    new Animation(8,12,40,40,4,4,15,enemyTex), //WalkRight
                    new Animation(0,2,40,40,4,4,15, enemyTex), //WalkDown
                    new Animation(3,7,40,40,4,4,15,enemyTex)};//WalkLeft;

        // add in names of enemy animation frames
        public static string[] enemyAnimationNames = new string[8] { "StandUp", "StandRight", "StandDown", "StandLeft", "WalkUp", "WalkRight", "WalkDown", "WalkLeft" };
        
        // sets how fast the enemies move
        public static Vector3 enemyMoveSpeed = new Vector3(2, 2, 1);

        //  PLAYER VARIABLES
        // sets files for player's spritesheet
        static string playerFile = "SpriteSheetAlfredJeevesIII.png";
        public static Texture2D playerTex = ScreenManager.SharedManager.Content.Load<Texture2D>(playerFile);

        // populate animation array for player
        public static Animation[] playerAnimations = new Animation[12] {
                    new Animation(17, 17, 40, 40, 5, 4, 1, playerTex), // StandUp
                    new Animation(12, 12, 40, 40, 5, 4, 1, playerTex), // StandRight
                    new Animation(4, 4, 40 ,40, 5, 4, 1, playerTex), //StandDown
                    new Animation(7, 7, 40, 40, 5, 4, 1, playerTex), // StandLeft
                    new Animation(17, 19, 40, 40, 5, 4, 15, playerTex), //WalkUp
                    new Animation(12, 16, 40, 40, 5, 4, 5, playerTex), //WalkRight
                    new Animation(4, 6, 40, 40, 5, 4, 15, playerTex), //WalkDown
                    new Animation(7, 11, 40, 40, 5, 4, 5, playerTex), //WalkLeft
                    new Animation(3, 3, 40, 40, 5, 4, 1, playerTex), // AttackUp
                    new Animation(2, 2, 40, 40, 5, 4, 1, playerTex), // AttackRight
                    new Animation(0, 0, 40, 40, 5, 4, 1, playerTex), // AttackDown
                    new Animation(1, 1, 40, 40, 5, 4, 1, playerTex), // AttackLeft
                };

        // add in the names of the player's animation frames
        public static string[] playerAnimationNames = new string[12] { "StandUp", "StandRight", "StandDown", "StandLeft", "WalkUp", "WalkRight", "WalkDown", "WalkLeft", "UpAttack", "RightAttack", "DownAttack", "LeftAttack" };

        // sets how fast the player moves
        public static Vector3 playerSpeed = new Vector3(2, 2, 1);

        //ITEM VARIABLES
        static string itemFile = "ObjectSpritesheet.png";
        public static Texture2D itemTex = ScreenManager.SharedManager.Content.Load<Texture2D>(itemFile);

        // WEAPON VARIABLES 
        // populate animation arrays for three weapons
        public static Animation[] weaponCandleStickAnimation = new Animation[1] {
                    new Animation(0, 0, 40, 40, 2, 3, 1, itemTex)
                };
        public static Animation[] weaponTrayAnimaton = new Animation[1] {
                    new Animation(1, 1, 40, 40, 2, 3, 1, itemTex)
                };
        public static Animation[] weaponVaseAnimation = new Animation[1] {
                    new Animation(2, 2, 40, 40, 2, 3, 1, itemTex)
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
        // populate animation array for coin
        public static Animation[] coinAnimation = new Animation[1] {
                    new Animation(3, 3, 40, 40, 2, 3, 1, itemTex)
                };


        // KEY VARIABLES
        // populates animation array for key
        public static Animation[] keyAnimation = new Animation[1] {
                    new Animation(4, 4, 40, 40, 2, 3, 1, itemTex)
                };


        // DOOR VARIABLES
        // populates animation array for door
        public static Animation[] doorAnimation = new Animation[1] {
                    new Animation(5, 5, 40, 40, 2, 3, 1, itemTex)
                };


        // DISGUISE VARIABLES
        // sets the file for a disguise's image
        static string chefDisguiseFile = "ChefSpritesheet.png";
        static string mechanicDisguiseFile = "MechanicSpritesheet.png";
        static string boxDisguiseFile = "BoxSpritesheet.png";

        public static Texture2D chefTex = ScreenManager.SharedManager.Content.Load<Texture2D>(chefDisguiseFile);
        public static Texture2D mechTex = ScreenManager.SharedManager.Content.Load<Texture2D>(mechanicDisguiseFile);
        public static Texture2D boxTex = ScreenManager.SharedManager.Content.Load<Texture2D>(boxDisguiseFile);

        // CHEF ANIMATION
        public static Animation[] chefAnimations = new Animation[12] {
                    new Animation(17, 17, 40, 40, 5, 4, 1, chefTex), // StandUp
                    new Animation(12, 12, 40, 40, 5, 4, 1, chefTex), // StandRight
                    new Animation(4, 4, 40 ,40, 5, 4, 1, chefTex), //StandDown
                    new Animation(7, 7, 40, 40, 5, 4, 1, chefTex), // StandLeft
                    new Animation(17, 19, 40, 40, 5, 4, 15, chefTex), //WalkUp
                    new Animation(12, 16, 40, 40, 5, 4, 5, chefTex), //WalkRight
                    new Animation(4, 6, 40, 40, 5, 4, 15, chefTex), //WalkDown
                    new Animation(7, 11, 40, 40, 5, 4, 5, chefTex), //WalkLeft
                    new Animation(3, 3, 40, 40, 5, 4, 1, chefTex), // AttackUp
                    new Animation(2, 2, 40, 40, 5, 4, 1, chefTex), // AttackRight
                    new Animation(0, 0, 40, 40, 5, 4, 1, chefTex), // AttackDown
                    new Animation(1, 1, 40, 40, 5, 4, 1, chefTex), // AttackLeft
                };
        public static Animation[] mechanicAnimations = new Animation[12] {
                    new Animation(17, 17, 40, 40, 5, 4, 1, mechTex), // StandUp
                    new Animation(12, 12, 40, 40, 5, 4, 1, mechTex), // StandRight
                    new Animation(4, 4, 40 ,40, 5, 4, 1, mechTex), //StandDown
                    new Animation(7, 7, 40, 40, 5, 4, 1, mechTex), // StandLeft
                    new Animation(17, 19, 40, 40, 5, 4, 15, mechTex), //WalkUp
                    new Animation(12, 16, 40, 40, 5, 4, 5, mechTex), //WalkRight
                    new Animation(4, 6, 40, 40, 5, 4, 15, mechTex), //WalkDown
                    new Animation(7, 11, 40, 40, 5, 4, 5, mechTex), //WalkLeft
                    new Animation(3, 3, 40, 40, 5, 4, 1, mechTex), // AttackUp
                    new Animation(2, 2, 40, 40, 5, 4, 1, mechTex), // AttackRight
                    new Animation(0, 0, 40, 40, 5, 4, 1, mechTex), // AttackDown
                    new Animation(1, 1, 40, 40, 5, 4, 1, mechTex), // AttackLeft
                };
        public static Animation[] boxAnimations = new Animation[12] {
                    new Animation(17, 17, 40, 40, 5, 4, 1, boxTex), // StandUp
                    new Animation(12, 12, 40, 40, 5, 4, 1, boxTex), // StandRight
                    new Animation(4, 4, 40 ,40, 5, 4, 1, boxTex), //StandDown
                    new Animation(7, 7, 40, 40, 5, 4, 1, boxTex), // StandLeft
                    new Animation(17, 19, 40, 40, 5, 4, 15, boxTex), //WalkUp
                    new Animation(12, 16, 40, 40, 5, 4, 5, boxTex), //WalkRight
                    new Animation(4, 6, 40, 40, 5, 4, 15, boxTex), //WalkDown
                    new Animation(7, 11, 40, 40, 5, 4, 5, boxTex), //WalkLeft
                    new Animation(3, 3, 40, 40, 5, 4, 1, boxTex), // AttackUp
                    new Animation(2, 2, 40, 40, 5, 4, 1, boxTex), // AttackRight
                    new Animation(0, 0, 40, 40, 5, 4, 1, boxTex), // AttackDown
                    new Animation(1, 1, 40, 40, 5, 4, 1, boxTex), // AttackLeft
                };

        // returns the correct animation depending on what disguise it is
        public static Animation[] GetDisguiseAnimations(string disguiseType)
        {
            switch (disguiseType)
            {
                case "Chef": return chefAnimations;

                case "Mechanic": return mechanicAnimations;

                case "Box": return boxAnimations;

                default: return null;
            }
        }
    }
}
