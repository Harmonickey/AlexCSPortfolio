using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace JAMGameFinal
{

    public enum Collision
    {
        Bullets,
        Players
    }

    class Level1 : JAMZombieGameEngine
    {
        float scale;
        float[] sen = new float[4];

        //the health bar and background texture
        Texture2D healthBar, gameBackgroundTexture, redRectangle;

        Random random = new Random();

        int xSignedPosition = -1;
        int ySignedPosition = -1;

        int totalFieldWidth = 3840;
        int totalFieldHeight = 2160;

        public Level1(Character p1, Character p2, Character p3, Character p4, Handicap h1, Handicap h2, Handicap h3, Handicap h4, int numberPlaying,
                        float s1, float s2, float s3, float s4)
        {
          
            PlayerCharacters[0] = p1;
            PlayerCharacters[1] = p2;
            PlayerCharacters[2] = p3;
            PlayerCharacters[3] = p4;
            PlayerHandicaps[0] = h1;
            PlayerHandicaps[1] = h2;
            PlayerHandicaps[2] = h3;
            PlayerHandicaps[3] = h4;

            sen[0] = s1;
            sen[1] = s2;
            sen[2] = s3;
            sen[3] = s4;

            NumberOfPlayers = numberPlaying;
            NumberOfPlayersLeft = NumberOfPlayers;
            //the transition time to fade in...
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            //and off the screen...
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }

        public override void LoadContent()
        {
            if (ScreenManager.Game.Content == null)
                ScreenManager.Game.Content = new ContentManager(ScreenManager.Game.Services, "Content");

            redRectangle = ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Misc\\redRectangle");

            players = new Hero[NumberOfPlayers];
            playerHalos = new OtherObject[NumberOfPlayers];

            shotgunBullets = new ShotgunBullet[numberOfshotgunBullets];
            miniUziBullets = new MiniUziBullet[numberOfminiUziBullets];
            m4Bullets = new M4Bullet[numberOfm4Bullets];
            g36cBullets = new G36CBullet[numberOfg36cBullets];

            zombies = new Zombie[MaxZombies];
            

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                
                //Load the texture data from the Sprite folder depending on who the player is
                #region Texture Data
                if (PlayerCharacters[i] == Character.Sayid)
                {
                    players[i] = new Hero(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Sayid\\Images\\Sayid"));
                    playerHalos[i] = new OtherObject(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Sayid\\Halo\\Sayid-Halo"));
                }
                if (PlayerCharacters[i] == Character.Sir_Edward)
                {
                    players[i] = new Hero(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Sir_Edward\\Images\\Sir_Edward"));
                    playerHalos[i] = new OtherObject(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                }
                if (PlayerCharacters[i] == Character.Wilhelm)
                {
                    players[i] = new Hero(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Wilhelm\\Images\\Wilhelm"));
                    playerHalos[i] = new OtherObject(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Wilhelm\\Halo\\Wilhelm-Halo"));
                }
                if (PlayerCharacters[i] == Character.Juan)
                {
                    players[i] = new Hero(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Juan\\Images\\Juan"));
                    playerHalos[i] = new OtherObject(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Juan\\Halo\\Juan-Halo"));
                }
                
                #endregion
                //give a position for the player to start out at
                #region Position
                //bottom left first               
                players[i].position = new Vector2(players[i].sprite.Width * xSignedPosition + (totalFieldWidth / 2), players[i].sprite.Height * ySignedPosition + (totalFieldHeight / 2));
                if (xSignedPosition == -1 && ySignedPosition == -1)
                {
                    //then bottom right
                    xSignedPosition *= -1;
                }
                else if (xSignedPosition == 1 && ySignedPosition == -1)
                {
                    //then top right
                    ySignedPosition *= -1;
                }
                else if (xSignedPosition == 1 && ySignedPosition == 1)
                {
                    //then top left
                    xSignedPosition *= -1;
                }
                #endregion
                //give the correct health for the player depending on the handicap
                #region Health
                if (players[i].playerHandicap == Handicap.Easy)
                {
                    players[i].health = 20;
                }
                else if (players[i].playerHandicap == Handicap.Medium)
                {
                    players[i].health = 15;
                }
                else if (players[i].playerHandicap == Handicap.Hard)
                {
                    players[i].health = 10;
                }
                else if (players[i].playerHandicap == Handicap.VeryHard)
                {
                    players[i].health = 5;
                }
                //reference giving origina2 health for sca2e factoring 2ater
                players[i].fullHealth = players[i].health;
                #endregion

                players[i].playerCharacter = PlayerCharacters[i];
                players[i].playerHandicap = PlayerHandicaps[i];
                players[i].triggerSensitivity = sen[i];
                LoadBullets(players[i].playerCharacter);
                players[i].LoadUpBulletManager();
                
            }

            #region Zombies
            for (int i = 0; i < MaxZombies; i++)
            {
                zombies[i] = new Zombie(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Zombies\\Images\\zombie1(1)"));
            }
            #endregion

            healthBar = ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Misc\\HealthBar");

            gameBackgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("Background\\GrassBackground");

            Active = true;

            //sleep for a little so the game can load
            Thread.Sleep(1000);
            //resets so xbox doesnt try to catch up with lost time in thread
            ScreenManager.Game.ResetElapsedTime();
        }

        private void LoadBullets(Character playerCharacter)
        {
            #region Sayid Bullets
            if (playerCharacter == Character.Sayid)
            {
                for (int i = 0; i < numberOfminiUziBullets; i++)
                {
                    miniUziBullets[i] = new MiniUziBullet(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Sayid\\Bullet\\miniUziBullet"));
                }
            }
            #endregion
            #region Sir Edward Bullets
            if (playerCharacter == Character.Sir_Edward)
            {
                for (int i = 0; i < numberOfm4Bullets; i++)
                {
                    m4Bullets[i] = new M4Bullet(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Sir_Edward\\Bullet\\m4Bullet"));
                }
            }
            #endregion
            #region Wilhelm Bullets
            if (playerCharacter == Character.Wilhelm)
            {
                for (int i = 0; i < numberOfg36cBullets; i++)
                {
                    g36cBullets[i] = new G36CBullet(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Players\\Wilhelm\\Bullet\\g36cBullet"));
                }
            }
            #endregion
            #region Juan Bullets
            if (playerCharacter == Character.Juan)
            {
                for (int i = 0; i < numberOfshotgunBullets; i++)
                {
                    shotgunBullets[i] = new ShotgunBullet(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Misc\\shotgunEffectBullet"));
                }
            }
            #endregion
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            Vector2 screenCenter = new Vector2(ScreenWidth, ScreenHeight) / 2;

            UpdateBulletMethods();

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                 playerAlive[i] = players[i].alive;
            }

            foreach (Zombie zombie in zombies)
            {
                if (zombie.alive)
                {
                    foreach (Hero player in players)
                    {
                        if (player.alive)
                        {
                            if (Vector2.Distance(player.position + scrollOffset, zombie.position + scrollOffset) < player.sprite.Width * 3 + (zombie.sprite.Width / 2))
                            {
                                zombie.CheckForCollision(player, NumberOfPlayersLeft, NumberOfZombies, scrollOffset);
                                NumberOfZombies = zombie.numberOfZombies;
                                NumberOfPlayersLeft = zombie.numberOfPlayersLeft;
                            }
                        }
                    }
                }
                if (Active)
                {
                    zombie.UpdateZombies(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3], screenCenter, scrollOffset, players, NumberOfPlayersLeft);
                }
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        void UpdateBulletMethods()
        {
            for (int j = 0; j < NumberOfPlayers; j++)
            {
                int i = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                if (players[i].playerCharacter == Character.Juan)
                {
                    players[i].juanBulletManager.Update(shotgunBullets, players[i], NumberOfPlayersLeft, zombies, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);
                    NumberOfZombies = players[i].juanBulletManager.numberOfZombies;
                    NumberOfZombiesKilled = players[i].juanBulletManager.numberOfZombiesKilled;
                }
                else if (players[i].playerCharacter == Character.Sir_Edward)
                {

                    players[i].sirEdwardBulletManager.Update(m4Bullets, players[i], NumberOfPlayersLeft, zombies, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);
                    NumberOfZombies = players[i].sirEdwardBulletManager.numberOfZombies;
                    NumberOfZombiesKilled = players[i].sirEdwardBulletManager.numberOfZombiesKilled;
                    
                }
                else if (players[i].playerCharacter == Character.Wilhelm)
                {

                    players[i].wilhelmBulletManager.Update(g36cBullets, players[i], NumberOfPlayersLeft, zombies, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);
                    NumberOfZombies = players[i].wilhelmBulletManager.numberOfZombies;
                    NumberOfZombiesKilled = players[i].wilhelmBulletManager.numberOfZombiesKilled;
                    
                }
                else if (players[i].playerCharacter == Character.Sayid)
                {

                    players[i].sayidBulletManager.Update(miniUziBullets, players[i], NumberOfPlayersLeft, zombies, NumberOfZombies, NumberOfZombiesKilled, scrollOffset);
                    NumberOfZombies = players[i].sayidBulletManager.numberOfZombies;
                    NumberOfZombiesKilled = players[i].sayidBulletManager.numberOfZombiesKilled;
                    
                }
            }
            UncheckPlayers();
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 screenCenter = new Vector2(ScreenWidth, ScreenHeight) / 2;
            Vector2 scrollOffset = screenCenter - cameraPosition;

            //begin a new sprite batch
            ScreenManager.SpriteBatch.Begin();

            DrawBackground(scrollOffset);

            #region BulletDraw
            //Draw the bullets
            DrawBullets(scrollOffset);
            
            #endregion
            #region PlayerDraw
            //draw the players
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                //only if it's alive...
                if (players[i].alive)
                {
                    Vector2 position = players[i].position + scrollOffset;
                    //uses the 7th overload to draw the player and halos
                    ScreenManager.SpriteBatch.Draw(players[i].sprite, position, null, Color.White, players[i].rotation, players[i].center, 1.0f, SpriteEffects.None, 0);
                    ScreenManager.SpriteBatch.Draw(playerHalos[i].sprite, position, null, Color.White, 0.0f, players[i].center, 1.0f, SpriteEffects.None, 0);
                    
                }
            }
            #endregion
            #region ZombieDraw
            //draw the zombies
            //for (int i = 0; i < MaxZombies; i++)
            //{
                //only if it's alive... (and if the game is activly playing:  i.e. not paused)
            foreach (Zombie zombie in zombies)
            {
                if (zombie.alive)
                {
                    Vector2 position = zombie.position + scrollOffset;
                    //uses the 7th overload to draw the zombies and halo
                    ScreenManager.SpriteBatch.Draw(zombie.sprite,        //the loaded texture
                        position,                              //the specified position
                        null,                                  //this is not a sprite sheet
                        Color.White,                           //no additional colors need to be added
                        zombie.zombieOrientation,                  //the rotation of the texture-on-screen
                        zombie.center,                     //the origin of the sprite: i.e. to say where to rotate around, or to say from where to make the position
                        1.0f,                                  //the scale of the sprite from its original size
                        SpriteEffects.None, 0);    //the sprite is flipped horizontally to correct the view
                                                   //the sprite is in the back layer

                }
            }
            #endregion

            DrawHUD();

            ScreenManager.SpriteBatch.End();
        }

        void DrawBullets(Vector2 scrollOffset)
        {
            byte fade = TransitionAlpha;

            foreach (Hero player in players)
            {
                if (player.alive)
                {
                    switch (player.playerCharacter)
                    {
                        #region Juan
                        case Character.Juan:
                            {
                                foreach (ShotgunBullet bullet in shotgunBullets)
                                {

                                    if (bullet.alive)
                                    {
                                        Vector2 position = bullet.position + scrollOffset;

                                        ScreenManager.SpriteBatch.Draw(bullet.sprite,
                                            position,
                                            null,
                                            new Color(fade, fade, fade),
                                            bullet.rotation,
                                            bullet.center,
                                            1.0f,
                                            SpriteEffects.None, 0);
                                    }
                                }
                                break;
                            }
                        #endregion
                        #region Sayid
                        case Character.Sayid:
                            {
                                foreach (MiniUziBullet bullet in miniUziBullets)
                                {
                                    if (bullet.alive)
                                    {
                                        Vector2 position = bullet.position + scrollOffset;

                                        ScreenManager.SpriteBatch.Draw(bullet.sprite,
                                            position,
                                            null,
                                            Color.White,
                                            bullet.rotation,
                                            bullet.center,
                                            1.0f,
                                            SpriteEffects.None, 0);
                                    }
                                }
                                break;
                            }
                        #endregion
                        #region Sir Edward
                        case Character.Sir_Edward:
                            {
                                foreach (M4Bullet bullet in m4Bullets)
                                {
                                    if (bullet.alive)
                                    {
                                        Vector2 position = bullet.position + scrollOffset;

                                        ScreenManager.SpriteBatch.Draw(bullet.sprite,
                                            position,
                                            null,
                                            Color.White,
                                            bullet.rotation,
                                            bullet.center,
                                            1.0f,
                                            SpriteEffects.None, 0);
                                    }
                                }
                                break;
                            }
                        #endregion
                        #region Wilhelm
                        case Character.Wilhelm:
                            {
                                foreach (G36CBullet bullet in g36cBullets)
                                {
                                    if (bullet.alive)
                                    {
                                        Vector2 position = bullet.position + scrollOffset;

                                        ScreenManager.SpriteBatch.Draw(bullet.sprite,
                                            position,
                                            null,
                                            Color.White,
                                            bullet.rotation,
                                            bullet.center,
                                            1.0f,
                                            SpriteEffects.None, 0);
                                    }
                                }
                                break;
                            }
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Draws the repeating background texture.
        /// </summary>
        void DrawBackground(Vector2 scrollOffset)
        {
            // Work out the position of the top left visible tile.
            int tileX = (int)scrollOffset.X % gameBackgroundTexture.Width;
            int tileY = (int)scrollOffset.Y % gameBackgroundTexture.Height;
            
            if (tileX > 0)
                tileX -= gameBackgroundTexture.Width;

            if (tileY > 0)
                tileY -= gameBackgroundTexture.Height;

            // Draw however many repeating tiles are needed to cover the screen.
            for (int x = tileX; x < ScreenWidth; x += gameBackgroundTexture.Width)
            {
                for (int y = tileY; y < ScreenHeight; y += gameBackgroundTexture.Height)
                {
                    ScreenManager.SpriteBatch.Draw(gameBackgroundTexture, new Vector2(x, y), Color.White);
                }
            }
        }
        /// <summary>
        /// May be used for other Heads up Display items other than just health like bullet count and status
        /// </summary>
        void DrawHUD()
        {   
            int j = 0;

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            for (int i = 0; i < NumberOfPlayersLeft; i++)
            {

                j = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);

                switch (j)
                {
                    //HUD SpriteBatch.Draw codes (Health Display)
                    #region players 1
                    case 0:
                        //creates a scale for the health bar based on the original health and the current health
                        scale = players[j].health / (float)players[j].fullHealth;
                        //Draws the Health bar
                        //Take into note that the scale is being applied only to the x value of the vector below because we do not want to scale it vertically
                        spriteBatch.Draw(healthBar, new Vector2(52, 17), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(scale, 1), SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "Score: " + players[j].score, new Vector2(52, 25), Color.White);
                        spriteBatch.DrawString(font, "Kills: " + NumberOfZombiesKilled, new Vector2(200, 25), Color.White);
                        spriteBatch.DrawString(font, "X: " + GameScreen.NewVariableX.ToString(), players[j].position, Color.White);
                        spriteBatch.DrawString(font, "Y: " + GameScreen.NewVariableY.ToString(), players[j].position + new Vector2(50, 50), Color.White);
                        break;
                    #endregion
                    #region players 2
                    case 1:
                        scale = players[j].health / (float)players[j].fullHealth;
                        spriteBatch.Draw(healthBar, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 352, 17), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(scale, 1), SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "Score: " + players[j].score, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 352, 25), Color.White);
                        break;
                    #endregion
                    #region players 3
                    case 2:
                        scale = players[j].health / (float)players[j].fullHealth;
                        spriteBatch.Draw(healthBar, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 352, ScreenManager.GraphicsDevice.Viewport.Height - 110), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(scale, 1), SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "Score: " + players[j].score, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 352, ScreenManager.GraphicsDevice.Viewport.Height - 118), Color.White);
                        break;
                    #endregion
                    #region players 4
                    case 3:
                        scale = players[j].health / (float)players[j].fullHealth;
                        spriteBatch.Draw(healthBar, new Vector2(52, ScreenManager.GraphicsDevice.Viewport.Height - 110), null, Color.White, 0.0f, new Vector2(0, 0), new Vector2(scale, 1), SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "Score: " + players[j].score, new Vector2(52, ScreenManager.GraphicsDevice.Viewport.Height - 118), Color.White);
                        break;
                    #endregion
                }
            }

            UncheckPlayers();
        }
    }
}