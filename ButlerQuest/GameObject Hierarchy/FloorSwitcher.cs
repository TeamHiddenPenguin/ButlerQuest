using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ButlerQuest
{
    class FloorSwitcher
    {
        int z1, z2;
        Rectangle rectangle;
        int upDirection;
        int downDirection;

        public FloorSwitcher(Rectangle rect, int firstZ, int secondZ, bool horiz)
        {
            z1 = firstZ;
            z2 = secondZ;
            rectangle = rect;

            if (horiz)
            {
                upDirection = 1;
                downDirection = 3;
            }
            else
            {
                upDirection = 0;
                downDirection = 2;
            }
        }

        public bool Collides(Player player)
        {
            if (player.center.Z == z1)
                if (this.rectangle.Contains(player.rectangle))
                        if (player.direction == upDirection)
                        {
                            player.location.Z = z2;
                            return true;
                        }
            if (player.center.Z == z2)
                if (this.rectangle.Contains(player.rectangle))
                    if (player.direction == downDirection)
                    {
                        player.location.Z = z1;
                        return true;
                    }
            return false;
        }
    }
}
