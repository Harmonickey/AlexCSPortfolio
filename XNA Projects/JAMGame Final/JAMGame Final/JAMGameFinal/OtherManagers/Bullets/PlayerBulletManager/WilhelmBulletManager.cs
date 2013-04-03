using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JAMGameFinal
{
    public class WilhelmBulletManager
    {
        public int numberOfZombies;
        public int numberOfZombiesKilled;
       
        public void Update(G36CBullet[] G36cBullets, Hero Player, int NumberOfPlayersLeft, Zombie[] Zombies, int NumberOfZombies, int NumberOfZombiesKilled, Vector2 scrollOffset)
        {
            numberOfZombies = NumberOfZombies;
            numberOfZombiesKilled = NumberOfZombiesKilled;

            foreach (G36CBullet g36cBullet in G36cBullets)
            {
                if (g36cBullet.alive)
                {
                    //this actually moves the bullet across the screen
                    g36cBullet.position += g36cBullet.velocity;

                    if (Vector2.Distance(Player.position + scrollOffset, g36cBullet.position + scrollOffset) > 1200.0f)
                    {
                        g36cBullet.alive = false;
                        continue;
                    }
                    else
                    {
                        foreach (Zombie zombie in Zombies)
                        {
                            if (zombie.alive)
                            {
                                g36cBullet.CheckForCollision(Player, zombie, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);

                                if (g36cBullet.collision)
                                {
                                    numberOfZombies = g36cBullet.numberOfZombies;
                                    numberOfZombiesKilled = g36cBullet.numberOfZombiesKilled;
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