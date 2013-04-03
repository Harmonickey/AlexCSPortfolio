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

namespace LevelCreationSoftware
{
    class StartScreen : GameScreen
    {
        SignedInGamer gamerOne;

        bool gamerSelected = false;

        Texture2D pressStartBackground;

        ContentManager content;
        SpriteBatch spriteBatch;

        Viewport viewport;

        ParticleSystem pSystem;

        public StartScreen(ParticleSystem system)
        {
            pSystem = system;
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            viewport = ScreenManager.GraphicsDevice.Viewport;

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            EmitterSystem = pSystem;

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            pressStartBackground = content.Load<Texture2D>("Backgrounds\\GameBackgrounds\\PressStartBackground");
        }

        public override void HandleInput(InputState input)
        {
            if (!gamerSelected)
            {
                for (int i = 0; i < InputState.MaxInputs; i++)
                {
                    if (input.CurrentGamePadStates[i].IsButtonDown(Buttons.Start) == true && input.PreviousGamePadStates[i].IsButtonUp(Buttons.Start) == true ||
                        input.CurrentKeyboardStates[i].IsKeyDown(Keys.Enter) == true && input.PreviousKeyboardStates[i].IsKeyUp(Keys.Enter) ||
                        input.CurrentKeyboardStates[i].IsKeyDown(Keys.Space) == true && input.PreviousKeyboardStates[i].IsKeyUp(Keys.Space))
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
                GamerOne = gamerOne;

                LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new MainMenuScreen());
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(pressStartBackground, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

            spriteBatch.End();
        }
    }
}
