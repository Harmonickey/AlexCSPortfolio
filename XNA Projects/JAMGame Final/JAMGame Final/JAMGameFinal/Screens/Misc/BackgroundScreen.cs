using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace JAMGameFinal
{
    class BackgroundScreen : GameScreen
    {
        ContentManager content;
        Texture2D menuBackgroundTexture, startUpScreenBackgroundTexture;
        //Video stuff
        Video[] videos;
        VideoPlayer player;
        Texture2D videoTexture;

        int videoCount = 1;
        int counter = 0;

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (OnStartUp && !player.IsDisposed && counter == 0)
            {
                counter++;
                player.IsLooped = false;
                player.Play(videos[0]);
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            videos = new Video[videoCount];

            startUpScreenBackgroundTexture = content.Load<Texture2D>("Background\\StartupScreen");
            videos[0] = content.Load<Video>("Video\\complete part 1_0001");
            player = new VideoPlayer();

            menuBackgroundTexture = content.Load<Texture2D>("Background\\menu screen background");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Rectangle startUpTitlePosition = new Rectangle(600, 100, startUpScreenBackgroundTexture.Width, startUpScreenBackgroundTexture.Height);
            byte fade = TransitionAlpha;

            DrawVideo(spriteBatch, fullscreen);

            spriteBatch.Begin();

            if (!OnStartUp)
            {
                spriteBatch.Draw(menuBackgroundTexture, fullscreen,
                                new Color(fade, fade, fade));
            }
            else
            {
                spriteBatch.Draw(startUpScreenBackgroundTexture, startUpTitlePosition,
                                Color.White);
            }
            
            spriteBatch.End();
        }

        private void DrawVideo(SpriteBatch spriteBatch, Rectangle size)
        {

            if (player.State != MediaState.Stopped)
            {
                videoTexture = player.GetTexture();
            }

            if (videoTexture != null)
            {
                spriteBatch.Begin();
                       
                spriteBatch.Draw(videoTexture, size, Color.White);

                spriteBatch.End();
            }
        }


    }
}