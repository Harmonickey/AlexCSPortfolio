using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelCreationSoftware
{
    class HowToCreateScreen : GameScreen
    {
        ContentManager content;

        Texture2D howToCreateBackground;

        public HowToCreateScreen()
        {
            
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            howToCreateBackground = content.Load<Texture2D>("Backgrounds\\GameBackgrounds\\instructionBackground");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(howToCreateBackground, fullscreen, Color.White);

            spriteBatch.DrawString(ScreenManager.Font, "HOW TO", new Vector2(fullscreen.X + 100, fullscreen.Y + 30), Color.White);

            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Back) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Back))
            {
                this.ExitScreen();
            }
        }

    }
}
