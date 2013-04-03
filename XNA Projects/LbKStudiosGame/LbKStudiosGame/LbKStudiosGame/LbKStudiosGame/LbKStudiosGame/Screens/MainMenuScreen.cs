using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace LbKStudiosGame
{

    enum MenuOption
    {
        Start,
        Gallery,
        Credits
    }

    enum StartOption
    {
        NewGame,
        Continue
    }

    class MainMenuScreen : GameScreen
    {
        #region Variables and Objects

        ContentManager content;
        SpriteBatch spriteBatch;

        Rectangle viewport;

        Texture2D menuSelectRectangle, background;

        Texture2D[] menuTemplate = new Texture2D[3];

        Vector2 menuSelectPosition;

        MenuOption menuOption = MenuOption.Start;

        StartOption startOption = StartOption.NewGame;

        SignedInGamer gamerOne;

        //which template for the background menu should be used
        //note that it might be easier to make an enumeration out of this for readability in the code
        private int whichOne = 0;

        bool gameLoadRequested = false;

        static IAsyncResult resultCheck;

        static StorageDevice storageDevice;

        bool allowContinue = true;

        #endregion

        #region Initialization

        public MainMenuScreen(SignedInGamer gamerOne)
        {

            this.gamerOne = gamerOne;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }

        #endregion

        #region Methods

        public override void LoadContent()
        {

            viewport = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            menuSelectRectangle = content.Load<Texture2D>("Backgrounds\\MenuSelectRectangle");

            menuTemplate[0] = content.Load<Texture2D>("Backgrounds\\MenuTemplate");
            menuTemplate[1] = content.Load<Texture2D>("Backgrounds\\MenuTemplate2");
            menuTemplate[2] = content.Load<Texture2D>("Backgrounds\\MenuTemplate3");

            background = content.Load<Texture2D>("Backgrounds\\background");

            menuSelectPosition = new Vector2(100.0f);

            //just load up the gamer right away, but select from the indicated device by the user
            //it will show every time if you have the MU plugged in, I think just in case the user wants to load from the MU obviously
            //if you unplug the MU then it doesn't even show the screen
            if (gamerOne != null)
            {
                gameLoadRequested = true;
                resultCheck = StorageDevice.BeginShowSelector(
                            gamerOne.PlayerIndex, null, null);
            }
        }



        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {

            #region Load Game Requested
            if (gameLoadRequested && resultCheck.IsCompleted)
            {
                storageDevice = StorageDevice.EndShowSelector(resultCheck);

                if (storageDevice != null && storageDevice.IsConnected)
                {
                    //Try this call, and if doesn't work use commented section here
                    //LoadGame(storageDevice, gamerOne);
                    LbKStorage.LoadGame(storageDevice, gamerOne);

                    if (LbKStorage.NothingLoaded)
                    {
                        allowContinue = false;
                    }
                }

                //reset flag
                gameLoadRequested = false;
            }

            #endregion


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        public override void HandleInput(InputState input)
        {
            //This needs to be check for values in whichOne and allowContinue because for some reason the menutemplate is not what it's suppossed to be.
            //The menu template should have a blocked out continue and the menu should not allow for continue selection if there was nothing loaded

            if (input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].IsButtonDown(Buttons.DPadDown) == true && input.PreviousGamePadStates[(int)gamerOne.PlayerIndex].IsButtonUp(Buttons.DPadDown) == true ||
                input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].IsButtonDown(Buttons.LeftThumbstickDown) == true && input.PreviousGamePadStates[(int)gamerOne.PlayerIndex].IsButtonUp(Buttons.LeftThumbstickDown) == true)
            {
                if (whichOne == 0)
                {
                    menuOption++;
                    menuSelectPosition += new Vector2(0, 180);

                    if (menuOption > MenuOption.Credits)
                    {
                        menuOption = 0;
                        menuSelectPosition = new Vector2(100.0f);
                    }
                }
                else
                {
                    if (allowContinue)
                    {
                        startOption++;
                        menuSelectPosition += new Vector2(0, 180);

                        if (startOption > StartOption.Continue)
                        {
                            startOption = 0;
                            menuSelectPosition = new Vector2(100.0f);
                        }
                    }
                }
            }
            if (input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].IsButtonDown(Buttons.DPadUp) == true && input.PreviousGamePadStates[(int)gamerOne.PlayerIndex].IsButtonUp(Buttons.DPadUp) == true ||
                input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].IsButtonDown(Buttons.LeftThumbstickUp) == true && input.PreviousGamePadStates[(int)gamerOne.PlayerIndex].IsButtonUp(Buttons.LeftThumbstickUp) == true)
            {
                if (whichOne == 0)
                {
                    menuOption--;
                    menuSelectPosition -= new Vector2(0, 180);

                    if (menuOption < 0)
                    {
                        menuOption = MenuOption.Credits;
                        menuSelectPosition = new Vector2(0, 540);
                    }
                }
                else
                {
                    if (allowContinue)
                    {
                        startOption--;
                        menuSelectPosition -= new Vector2(0, 180);

                        if (startOption < 0)
                        {
                            startOption = StartOption.Continue;
                            menuSelectPosition = new Vector2(0, 540);
                        }
                    }
                }

                
            }
            if (input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].IsButtonDown(Buttons.A) == true && input.PreviousGamePadStates[(int)gamerOne.PlayerIndex].IsButtonUp(Buttons.A) == true)
            {
                if (whichOne == 0)
                {
                    if (menuOption == MenuOption.Start)
                    {
                        
                        //LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new DifferentMenuScreen());
                        //reset the position of the rectangle
                        menuSelectPosition = new Vector2(100.0f);

                        //which menuTemplate to use
                        whichOne++;
                        if (allowContinue)
                        {
                            whichOne++;
                        }
                        return;
                    }

                    else if (menuOption == MenuOption.Gallery)
                    {

                        //TODO:
                    }
                    else if (menuOption == MenuOption.Credits)
                    {
                        //TODO:
                    }
                }
                if (whichOne == 1)
                {
                    if (startOption == StartOption.NewGame)
                    {
                        GamePlayScreen.storageDevice = storageDevice;
                        
                        LoadingScreen.Load(ScreenManager, true, gamerOne.PlayerIndex, new LevelOne(gamerOne));
                        //load up a new game
                        
                    }
                    
                    if (startOption == StartOption.Continue)
                    {
                        if (gamerOne != null)
                        {
                            gameLoadRequested = true;
                            resultCheck = StorageDevice.BeginShowSelector(gamerOne.PlayerIndex, null, null);
                        }
                        
                    }
                }
                

            }
            
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);

            spriteBatch.Draw(menuTemplate[whichOne],
                new Vector2(100.0f),
                null,
                Color.White,
                0.0f,
                new Vector2(0, 0),
                0.5f,
                SpriteEffects.None, 0);
             
            spriteBatch.Draw(menuSelectRectangle,
                    menuSelectPosition,
                    null,
                    Color.White,
                    0.0f,
                    new Vector2(0, 0),
                    new Vector2(0.5f, 0.2f),
                    SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion

    }
}
