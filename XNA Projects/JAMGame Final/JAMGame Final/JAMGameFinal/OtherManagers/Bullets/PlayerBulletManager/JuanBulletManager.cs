using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace JAMGameFinal
{
    public class JuanBulletManager
    {
        public int numberOfZombies;
        public int numberOfZombiesKilled;
        
        public void Update(ShotgunBullet[] ShotgunBullets, Hero Player, int NumberOfPlayersLeft, Zombie[] Zombies, int NumberOfZombies, int NumberOfZombiesKilled, Vector2 scrollOffset)
        {

            numberOfZombies = NumberOfZombies;
            numberOfZombiesKilled = NumberOfZombiesKilled;
            
            foreach(ShotgunBullet shotgunBullet in ShotgunBullets)
            {
                if (shotgunBullet.alive)
                {
                    //this actually moves the bullet across the screen
                    shotgunBullet.position += shotgunBullet.velocity;

                    if (Vector2.Distance(Player.position + scrollOffset, shotgunBullet.position + scrollOffset) > Player.sprite.Width * 3)
                    {
                        shotgunBullet.alive = false;
                        continue;
                    }
                    else
                    {
                        foreach (Zombie zombie in Zombies)
                        {
                            if (zombie.alive)
                            {
                                shotgunBullet.CheckForCollision(Player, zombie, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);
                                if (shotgunBullet.collision)
                                {
                                    numberOfZombies = shotgunBullet.numberOfZombies;
                                    numberOfZombiesKilled = shotgunBullet.numberOfZombiesKilled;
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