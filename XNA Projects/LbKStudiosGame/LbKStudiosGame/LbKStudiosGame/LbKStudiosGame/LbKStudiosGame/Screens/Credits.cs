using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LbKStudiosGame
{
    class Credits : GameScreen
    {

        ContentManager content;
        SpriteBatch spriteBatch;

        //Video stuff
        Video[] videos;
        VideoPlayer player;
        Texture2D videoTexture;

        Rectangle viewport;

        bool skipToMenu = false;
        bool creditVideoDone = false;
        bool played = false;

        string skipString = "Press A To Skip";

        public Credits()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            viewport = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            videos = new Video[1];

            videos[0] = content.Load<Video>("Videos\\54321");
   
            player = new VideoPlayer();
            
        }

        public override void HandleInput(InputState input)
        {
            
            for (int i = 0; i < InputState.MaxInputs; i++)
            {
                if (input.CurrentGamePadStates[i].Buttons.A == ButtonState.Pressed && input.PreviousGamePadStates[i].Buttons.A == ButtonState.Released ||
                    input.CurrentGamePadStates[i].Buttons.Start == ButtonState.Pressed && input.PreviousGamePadStates[i].Buttons.Start == ButtonState.Released)
                {

                    skipToMenu = true;

                    if (player != null && !player.IsDisposed)
                    {
                        player.Dispose();
                    }
                }
            }
            

            base.HandleInput(input);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (!player.IsDisposed)
            {
                if (player.State == MediaState.Stopped)
                {
                    if (!played)
                    {
                        player.IsLooped = false;
                        player.Play(videos[0]);
                        played = true;
                    }
                    else
                    {
                        player.Dispose();
                        creditVideoDone = true;
                    }

                }
            }

            if (skipToMenu || creditVideoDone)
            {
                if (!player.IsDisposed)
                {
                    player.Dispose();
                }

                LoadingScreen.Load(ScreenManager, true, null, new StartScreen());
                
            }
            

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        public override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                if (!player.IsDisposed)
                {
                    if (player.State != MediaState.Stopped)
                    {
                        videoTexture = player.GetTexture();
                    }


                    if (videoTexture != null)
                    {
                        spriteBatch.Begin();

                        spriteBatch.DrawString(ScreenManager.Font, skipString, new Vector2(viewport.Bottom - 30, viewport.Right - skipString.Length), Color.White);

                        spriteBatch.Draw(videoTexture, new Rectangle(500, 500, videoTexture.Width, videoTexture.Height), Color.White);

                        spriteBatch.End();
                    }
                }
            }



            base.Draw(gameTime);
        }
    }
}
