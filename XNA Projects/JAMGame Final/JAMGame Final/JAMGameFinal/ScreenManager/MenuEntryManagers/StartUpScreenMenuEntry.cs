using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    class StartUpScreenMenuEntry : MenuEntry
    {
        float selectionFade;

        public StartUpScreenMenuEntry(string text)
        {
            Text = text;
        }

        protected internal override void OnSelectEntry(PlayerIndex playerIndex)
        {
            base.OnSelectEntry(playerIndex);
        }

        public override void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        //can be overrided to customize appearance

        public override void Draw(MenuScreen screen, Vector2 position,
                                bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.Red : Color.White;

            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, Text, position, color, 0,
                                    origin, scale, SpriteEffects.None, 0);

        }

        public override int GetHeight(MenuScreen screen)
        {
            return base.GetHeight(screen);
        }
    }
}