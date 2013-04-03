using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JAMGameFinal
{
    public class SayidBulletManager 
    {
        public int numberOfZombies;
        public int numberOfZombiesKilled;

        public void Update(MiniUziBullet[] MiniUziBullets, Hero Player, int NumberOfPlayersLeft, Zombie[] Zombies, int NumberOfZombies, int NumberOfZombiesKilled, Vector2 scrollOffset)
        {
            numberOfZombies = NumberOfZombies;
            numberOfZombiesKilled = NumberOfZombiesKilled;

            foreach (MiniUziBullet miniUziBullet in MiniUziBullets)
            {
                if (miniUziBullet.alive)
                {
                    //this actually moves the bullet across the screen
                    miniUziBullet.position += miniUziBullet.velocity;

                    if (Vector2.Distance(Player.position + scrollOffset, miniUziBullet.position + scrollOffset) > 1200.0f)
                    {
                        miniUziBullet.alive = false;
                        continue;
                    }
                    else
                    {
                        foreach (Zombie zombie in Zombies)
                        {
                            if (zombie.alive)
                            {
                                miniUziBullet.CheckForCollision(Player, zombie, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);

                                if (miniUziBullet.collision)
                                {
                                    numberOfZombies = miniUziBullet.numberOfZombies;
                                    numberOfZombiesKilled = miniUziBullet.numberOfZombiesKilled;

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