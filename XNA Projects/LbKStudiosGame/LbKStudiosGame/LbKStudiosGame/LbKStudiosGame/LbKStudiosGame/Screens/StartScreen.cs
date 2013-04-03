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
    class StartScreen : GameScreen
    {
        SignedInGamer gamerOne;

        bool gamerSelected = false;

        Texture2D pressStartBackground;

        ContentManager content;
        SpriteBatch spriteBatch;

        Rectangle viewport;

        public StartScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            viewport = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            pressStartBackground = content.Load<Texture2D>("Backgrounds\\PressStartBackground");
        }

        public override void HandleInput(InputState input)
        {
            if (!gamerSelected)
            {
                for (int i = 0; i < InputState.MaxInputs; i++)
                {
                    if (input.CurrentGamePadStates[i].IsButtonDown(Buttons.Start) == true && input.PreviousGamePadStates[i].IsButtonUp(Buttons.Start) == true)
                    {
                        gamerOne = Gamer.SignedInGamers[(PlayerIndex)i];

                        gamerSelected = true;

                        if (gamerOne == null)
                        {
                            if (!Guide.IsVisible)
                            {
                                Guide.ShowSignIn(1, false);
                            }
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (!Guide.IsVisible && gamerSelected)
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new MainMenuScreen(gamerOne));
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(pressStartBackground, viewport, Color.White);

            spriteBatch.End();
        }
    }
}
