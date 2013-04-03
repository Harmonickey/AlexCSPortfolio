using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    class MessageBoxScreen : GameScreen
    {

        string message;
        Texture2D gradientTexture;
        private bool exiting;
        private bool returning;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        public MessageBoxScreen(string message, bool includeUsageText, bool includeExitingUsageText)
        {
            const string usageText = "\n A Button = OK" +
                                     "\n B button = Cancel";

            const string exitingUsageText = "\n A Button = Exit" +
                                            "\n B Button = Cancel";

            const string continueText = "\n A Button = Continue";

            if (includeUsageText)
                this.message = message + usageText;
            else
            {
                if (includeExitingUsageText)
                    this.message = message + exitingUsageText;
                else
                    this.message = message + continueText;
            }
            

            IsPopup = true;

            exiting = includeExitingUsageText;
            returning = includeUsageText;
            
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("Font\\gradient");

        }

        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {

                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));

                if (returning)
                {
                    LoadingScreen.Load(ScreenManager, true, playerIndex, false, new BackgroundScreen(), 
                                                                                new MainMenuScreen());
                }
                else if (exiting)
                {
                    ScreenManager.Game.Exit();
                }

                ExitScreen();
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            Color color = new Color(255, 255, 255, TransitionAlpha);

            spriteBatch.Begin();

            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }
    }
}