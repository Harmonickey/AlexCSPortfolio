using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LbKStudiosGame
{
    class MenuEntry
    {
        string text;

        float selectionFade;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;

        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(string text)
        {
            this.text = text;
        }

        public MenuEntry()
        {
            this.text = Text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        //can be overrided to customize appearance

        public virtual void Draw(MenuScreen screen, Vector2 position,
                                bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.Yellow : Color.White;

            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selectionFade;

            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, text, position, color, 0,
                                    origin, scale, SpriteEffects.None, 0);

        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }


    }
}