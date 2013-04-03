using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LevelCreationSoftware
{
    class InvertTest : GamePlayScreen
    {
        Player bluebox;
        bool isInverted = true;

        float scalar = 0.0f;

        public InvertTest()
        {
            LevelCreateSession = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bluebox = new Player(content.Load<Texture2D>("Sprites\\spritesheet"));

            bluebox.textureData =
                    new Color[bluebox.sprite.Width * bluebox.sprite.Height];

            bluebox.sprite.GetData(bluebox.textureData);
            
        }

        public override void HandleInput(InputState input)
        {


            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.I) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.I))
            {
                if (!isInverted)
                {
                    scalar = 0;
                    isInverted = true;
                }
                else
                {
                    scalar = 0;
                    isInverted = false;
                }
            }

            base.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(bluebox.sprite, new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height), Color.White);

            SpriteBatch.DrawString(ScreenManager.Font, "Press I to Invert Color", new Vector2(500, 900), Color.White);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {

            if (isInverted)
            {
                scalar += gameTime.ElapsedGameTime.Milliseconds * 0.05f;

                scalar = MathHelper.Clamp(scalar, 0.0f, 255.0f);

                Color[] colorList = new Color[bluebox.sprite.Width * bluebox.sprite.Height];

                int i = 0;

                foreach (Color color in bluebox.textureData)
                {
                    colorList[i] = new Color((byte)scalar - color.R, (byte)scalar - color.G, (byte)scalar - color.B);
                    i++;
                }

                bluebox.sprite.SetData(colorList);

            }

            if (!isInverted)
            {

                scalar += gameTime.ElapsedGameTime.Milliseconds * 0.05f;

                scalar = MathHelper.Clamp(scalar, 0.0f, 255.0f);

                Color[] colorList = new Color[bluebox.sprite.Width * bluebox.sprite.Height];

                int i = 0;

                foreach (Color color in bluebox.textureData)
                {
                    colorList[i] = new Color(color.R + (byte)scalar, color.G + (byte)scalar, color.B + (byte)scalar);
                    i++;
                }

                bluebox.sprite.SetData(colorList);
                
            }


            

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

    }
}
