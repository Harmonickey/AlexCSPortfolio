using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JAMGameFinal
{
    public class SirEdwardBulletManager
    {
        public int numberOfZombies;
        public int numberOfZombiesKilled;

        public void Update(M4Bullet[] M4Bullets, Hero Player, int NumberOfPlayersLeft, Zombie[] Zombies, int NumberOfZombies, int NumberOfZombiesKilled, Vector2 scrollOffset)
        {
            numberOfZombies = NumberOfZombies;
            numberOfZombiesKilled = NumberOfZombiesKilled;

            foreach (M4Bullet m4Bullet in M4Bullets)
            {
                if (m4Bullet.alive)
                {
                    //this actually moves the bullet across the screen
                    m4Bullet.position += m4Bullet.velocity;

                    if (Vector2.Distance(Player.position + scrollOffset, m4Bullet.position + scrollOffset) > 1200.0f)
                    {
                        m4Bullet.alive = false;
                        continue;
                    }
                    else
                    {
                        foreach (Zombie zombie in Zombies)
                        {
                            if (zombie.alive)
                            {
                                m4Bullet.CheckForCollision(Player, zombie, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);

                                if (m4Bullet.collision)
                                {
                                    numberOfZombies = m4Bullet.numberOfZombies;
                                    numberOfZombiesKilled = m4Bullet.numberOfZombiesKilled;

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}