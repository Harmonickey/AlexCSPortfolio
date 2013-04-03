using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Silhouetta
{
    class ControlScreen : GameScreen
    {
        ContentManager content;
        Texture2D controlFront;
        Texture2D controlTop;

        bool scrollScreen;

        public ControlScreen()
        {
            
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void HandleInput(InputState input)
        {

            int playerIndex = (int)ControllingPlayer.Value;

            if (input.CurrentGamePadStates[playerIndex].Buttons.A == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.A == ButtonState.Released && scrollScreen)
            {
                scrollScreen = false;
            }
            else if (input.CurrentGamePadStates[playerIndex].Buttons.A == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.A == ButtonState.Released && !scrollScreen)
            {
                scrollScreen = true;
            }
            else if (input.CurrentGamePadStates[playerIndex].Buttons.B == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.B == ButtonState.Released)
            {
                this.ExitScreen();
            }

            base.HandleInput(input);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            controlFront = content.Load<Texture2D>("Background\\backgroundWithTopOrtho");
            controlTop = content.Load<Texture2D>("Background\\backgroundWithFrontOrtho");

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            
            byte fade = TransitionAlpha;
            
            spriteBatch.Begin();

            if (!scrollScreen)
            {
                spriteBatch.Draw(controlFront, fullscreen, Color.White);
            }
            else
            {
                spriteBatch.Draw(controlTop, fullscreen, Color.White);
            }

            spriteBatch.End();
        }
    }
}