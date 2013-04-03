using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JAMGameFinal
{
    class CharacterInfoScreen : GameScreen
    {

        Viewport viewPort;
        SpriteBatch spriteBatch;
        
        Texture2D image1;
        Texture2D image2;

        bool scrollScreen = false;

        public CharacterInfoScreen()
        {
            IsPopup = true;
            
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            viewPort = ScreenManager.GraphicsDevice.Viewport;
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            image1 = ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Misc\\Character_Info_Screen\\image1");
            image2 = ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Misc\\Character_Info_Screen\\image2");
        }

        public override void HandleInput(InputState input)
        {

            int playerIndex = (int)ControllingPlayer.Value;

            if (input.CurrentGamePadStates[playerIndex].Buttons.A == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.A == ButtonState.Released && scrollScreen)
            {
                scrollScreen = false;
            }
            if (input.CurrentGamePadStates[playerIndex].Buttons.A == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.A == ButtonState.Released && !scrollScreen)
            {
                scrollScreen = true;
            }
            if (input.CurrentGamePadStates[playerIndex].Buttons.B == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.B == ButtonState.Released)
            {
                this.ExitScreen();
            }
            
            base.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            if (!scrollScreen)
            {
                spriteBatch.Draw(image1, new Rectangle(0, 0,viewPort.Width, viewPort.Height), Color.White);
            }
            else
            {
                spriteBatch.Draw(image2, new Rectangle(0, 0, viewPort.Width, viewPort.Height), Color.White);
            }

            spriteBatch.End();

            //ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

        }
        
    }
}