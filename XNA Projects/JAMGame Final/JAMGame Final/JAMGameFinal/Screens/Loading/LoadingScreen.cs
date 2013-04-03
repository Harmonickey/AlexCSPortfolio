using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace JAMGameFinal
{
    class LoadingScreen : GameScreen
    {
        bool loadingIsSlow;
        bool otherScreensAreGone;

        /*
        Video videos;
        VideoPlayer player;
        Texture2D videoTexture;
        */
        private bool toMainMenu;

        Rectangle viewportRect;

        GameScreen[] screensToLoad;

        //is it loaded?
        private bool readyToLoad;

        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, bool toMainMenu,
                              GameScreen[] screensToLoad)
        {

            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;
            this.toMainMenu = toMainMenu;
            this.readyToLoad = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer,
                                bool toMainMenu,
                                params GameScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                                            loadingIsSlow,
                                                            toMainMenu,
                                                            screensToLoad);
               
            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);

            if (otherScreensAreGone)
            {
                if (toMainMenu)
                {
                    ScreenManager.RemoveScreen(this);

                    foreach (GameScreen screen in screensToLoad)
                    {
                        if (screen != null)
                        {
                            ScreenManager.AddScreen(screen, ControllingPlayer);
                        }
                    }

                    ScreenManager.Game.ResetElapsedTime();
                }
                else
                {
                    readyToLoad = true;
                }
            }
        }

        public override void HandleInput(InputState input)
        {
            if (readyToLoad)
            {
                int playerIndex = (int)ControllingPlayer.Value;
                if (input.CurrentGamePadStates[playerIndex].Buttons.A == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.A == ButtonState.Released)
                {
                    ScreenManager.RemoveScreen(this);
                    
                    foreach (GameScreen screen in screensToLoad)
                    {
                        if (screen != null)
                        {
                            ScreenManager.AddScreen(screen, ControllingPlayer);
                        }
                    }

                    ScreenManager.Game.ResetElapsedTime();

                }
            }

            base.HandleInput(input);
        }

        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == ScreenState.Active) &&
               (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            if (loadingIsSlow)
            {

                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";

                if (LevelLoading == 0)
                    LevelLoading = 1;

                string levelOn = "     LEVEL " + LevelLoading;

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                viewportRect = new Rectangle(0, 0,
                viewport.Width,
                viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                ContentManager content;
                content = new ContentManager(ScreenManager.Game.Services, "Content");

                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = new Color(255, 255, 255, TransitionAlpha);

                spriteBatch.Begin();

                if (readyToLoad)
                {
                    spriteBatch.Draw(content.Load<Texture2D>("Background\\background"), viewportRect, Color.White);
                    spriteBatch.Draw(content.Load<Texture2D>("Sprites\\Misc\\ButtonTexture"), new Vector2(0, (viewport.Height / 2) - 50), Color.White);
                }
                else
                {
                    spriteBatch.Draw(content.Load<Texture2D>("Background\\background"), viewportRect, Color.White);
                    spriteBatch.DrawString(font, message, textPosition, color);
                    spriteBatch.DrawString(font, levelOn, textPosition + new Vector2(100, 0), Color.White);
                }
                spriteBatch.End();
            }
        }
    }
}