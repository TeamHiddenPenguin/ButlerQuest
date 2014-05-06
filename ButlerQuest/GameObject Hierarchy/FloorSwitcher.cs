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
        public FloorSwitcher(Rectangle rect, int firstZ, int secondZ)
        {
            z1 = firstZ;
            z2 = secondZ;
            rectangle = rect;
        }

        public bool Collides(Player player)
        {
            if (player.center.Z == z1)
                if (this.rectangle.Contains(player.rectangle))
                    if (player.direction == 1)
                    {
                        player.location.Z = z2;
                        return true;
                    }
            if (player.center.Z == z2)
                if (this.rectangle.Contains(player.rectangle))
                    if (player.direction == 3)
                    {
                        player.location.Z = z1;
                        return true;
                    }
            return false;
        }
    }
}
