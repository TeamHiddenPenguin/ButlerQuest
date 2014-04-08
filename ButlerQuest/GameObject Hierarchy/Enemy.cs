using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class Enemy : MovableGameObject
    {
        //AIState can be found in AIManager
        public AI_STATE state = AI_STATE.UNAWARE;
        public Vector3 startLocation;
        public ICommand currentCommand;
        public Queue<ICommand> defaultCommands;
        public Queue<ICommand> commandQueue;
        private double awareness = 0;
        private const float MAX_VISION_RADIUS_SQUARED = 25000;
        private const float MAX_VISION_RADIUS_SQUARED_PURSUIT = MAX_VISION_RADIUS_SQUARED + 5000;
        private const float VISION_CONE_ANGLE_DEGREES = 55;
        private const float DEG_TO_RAD = 0.0174532925f;
        public Vector3 center;

        // constructor
        public Enemy(Vector3 vel, Animation[] animations, string[] names, Vector3 loc, Rectangle rect)
            : base(vel, animations, names, loc, rect)
        {
            commandQueue = new Queue<ICommand>();
            defaultCommands = new Queue<ICommand>();
            startLocation = loc;
            state = AI_STATE.UNAWARE;
            center = new Vector3(rect.X + rect.Width, rect.Y + rect.Height, loc.Z);
        }

        public void ChangeCommand()
        {
            if (commandQueue.Count != 0 && commandQueue.Peek() != null)
            {
                currentCommand = commandQueue.Dequeue();
                currentCommand.Initialize();
            }
            else
                currentCommand = new GetNextCommandSet(this, int.MaxValue);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentCommand == null || currentCommand.IsFinished)
                ChangeCommand();

            center = new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, location.Z);

            PersonalAILogic();
            
            currentCommand.Update(gameTime);
        }

        private void PersonalAILogic()
        {
            if (state < AI_STATE.HUNTING)
            {
                if (this.center.Z == AIManager.SharedAIManager.PlayerLocation.Z)
                {
                    double dist = Vector3.DistanceSquared(this.location, AIManager.SharedAIManager.PlayerLocation);
                    if (dist < MAX_VISION_RADIUS_SQUARED)
                    {
                        if (CanSee(AIManager.SharedAIManager.PlayerLocation))
                        {
                            if (!WallInWay())
                            {
                                AIManager.SharedAIManager.lastKnownPlayerLoc = new Vector3(AIManager.SharedAIManager.PlayerLocation.X - 20, AIManager.SharedAIManager.PlayerLocation.Y - 20, AIManager.SharedAIManager.PlayerLocation.Z);

                                if (AIManager.SharedAIManager.PlayerIsSuspicious())
                                {
                                    awareness += (MAX_VISION_RADIUS_SQUARED / dist) * .001;
                                    if (awareness >= 1)
                                    {
                                        //begin a pursuit, defer to AIManager
                                        commandQueue.Clear();
                                        state = AI_STATE.PURSUIT;
                                    }
                                }
                            }
                            System.Diagnostics.Debug.WriteLine("Awareness " + awareness + "Direction " + direction);
                        }
                    }
                }
            }
            if (state == AI_STATE.PURSUIT)
            {
                double dist = Vector3.DistanceSquared(this.location, AIManager.SharedAIManager.PlayerLocation);
                if (dist < MAX_VISION_RADIUS_SQUARED_PURSUIT)
                {
                    AIManager.SharedAIManager.lastKnownPlayerLoc = AIManager.SharedAIManager.PlayerLocation;
                }
                if (commandQueue.Count < 2)
                {
                    if (!CanSee(AIManager.SharedAIManager.lastKnownPlayerLoc) || WallInWay())
                    {
                        //change state to hunting and defer to AIManager for that
                    }
                }
            }
            if (state == AI_STATE.HUNTING)
            {
                awareness -= .0005;
                if (CanSee(AIManager.SharedAIManager.PlayerLocation))
                {
                    awareness += .01;
                    if (awareness > 1)
                        //reinitialize pursuit, call functions
                        commandQueue.Clear();
                        state = AI_STATE.PURSUIT;
                }
                if (awareness <= 0)
                {
                    awareness = 0;
                    commandQueue.Clear();
                    state = AI_STATE.AWARE;
                }
            }
        }

        public bool CanSee(Vector3 point)
        {
            double minAngle = ((90 - (90 * direction)) - VISION_CONE_ANGLE_DEGREES) * DEG_TO_RAD;
            double maxAngle = ((90 - (90 * direction)) + VISION_CONE_ANGLE_DEGREES) * DEG_TO_RAD;
            double angle = -Math.Atan2((point.Y - this.center.Y), (point.X - this.center.X));
            return angle < maxAngle && angle > minAngle;
        }

        public bool WallInWay()
        {
            return false;
        }
    }
}
