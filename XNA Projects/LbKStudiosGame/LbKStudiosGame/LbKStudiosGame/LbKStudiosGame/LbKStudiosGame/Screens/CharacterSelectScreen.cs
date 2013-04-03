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
    class CharacterSelectScreen : MenuScreen
    {
        
        ContentManager content;
        SpriteBatch spriteBatch;

        //private int numberOfPlayers;

        Rectangle viewport;

        SignedInGamer gamerOne;

        Texture2D grid;

        public CharacterSelectScreen(SignedInGamer gamerOne)
            :base ("Character Select")
        {
            this.gamerOne = gamerOne;

            TransitionOnTime = TimeSpan.FromSeconds(0.3);
            TransitionOffTime = TimeSpan.FromSeconds(0.3);
        }

        public override void LoadContent()
        {

            
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            viewport = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            grid = content.Load<Texture2D>("Backgrounds\\backgroundGrid");
        }

        public override void HandleInput(InputState input)
        {
            if (input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].Buttons.A == ButtonState.Pressed && input.PreviousGamePadStates[(int)gamerOne.PlayerIndex].Buttons.A == ButtonState.Released)
            {
                LoadingScreen.Load(ScreenManager, true, gamerOne.PlayerIndex, new LevelOne(gamerOne));
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(grid, viewport, Color.White);
            spriteBatch.End();
        }

    }
}
