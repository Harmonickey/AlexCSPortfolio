using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace Silhouetta
{
    
    class MainMenuScreen : MenuScreen
    {
        
        #region Variables and Objects

        ContentManager content;
        SpriteBatch spriteBatch;

        MenuEntry helpMenuEntry, exitMenuEntry, startMenuEntry;
        MenuEntry continueMenuEntry, newGameMenuEntry;


        Texture2D background;

        Viewport viewport;

        bool gameLoadRequested = false;

        static IAsyncResult resultCheck;

        static StorageDevice storageDevice;

        #endregion

        #region Initialization
        public MainMenuScreen(bool isContinue, bool isNewGame)
            : base("Main Menu Screen")
        {
            if (!isContinue && !isNewGame)
            {
                startMenuEntry = new MenuEntry("Start");
                helpMenuEntry = new MenuEntry("Help");
                exitMenuEntry = new MenuEntry("Exit");

                startMenuEntry.Selected += StartMenuEntrySelected;
                helpMenuEntry.Selected += HelpMenuEntrySelected;
                exitMenuEntry.Selected += ExitMenuEntrySelected;

                MenuEntries.Add(startMenuEntry);
                MenuEntries.Add(helpMenuEntry);
                MenuEntries.Add(exitMenuEntry);
            }
            else if (isContinue && isNewGame)
            {
                continueMenuEntry = new MenuEntry("Continue");
                newGameMenuEntry = new MenuEntry("New Game");

                continueMenuEntry.Selected += ContinueMenuEntrySelected;
                newGameMenuEntry.Selected += NewGameMenuEntrySelected;

                MenuEntries.Add(continueMenuEntry);
                MenuEntries.Add(newGameMenuEntry);
            }
            else if (!isContinue && isNewGame)
            {
                newGameMenuEntry = new MenuEntry("New Game");

                newGameMenuEntry.Selected += NewGameMenuEntrySelected;

                MenuEntries.Add(newGameMenuEntry);
            }

        }

        #endregion
        

        #region Methods

        public override void LoadContent()
        {

            viewport = ScreenManager.GraphicsDevice.Viewport;

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            background = content.Load<Texture2D>("Backgrounds\\GameBackgrounds\\background");

            

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {

            #region Load Game Requested
            if (gameLoadRequested && resultCheck.IsCompleted)
            {
                storageDevice = StorageDevice.EndShowSelector(resultCheck);

                if (storageDevice != null && storageDevice.IsConnected)
                {

                    LbKStorage.LoadGame(storageDevice, GamerOne);

                    if (LbKStorage.Level > 1 || LbKStorage.Level == 1 && LbKStorage.CheckPoint > 1)
                    {
                        ScreenManager.AddScreen(new MainMenuScreen(true, true), GamerOne.PlayerIndex);
                    }
                    else
                    {
                        ScreenManager.AddScreen(new MainMenuScreen(false, true), GamerOne.PlayerIndex);
                    }
                }

                //reset flag
                gameLoadRequested = false;
            }

            #endregion

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        void HelpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //ScreenManager.AddScreen(new HelpMenuScreen(), GamerOne.PlayerIndex);
        }

        void ExitMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, GamerOne.PlayerIndex);

        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        void StartMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            resultCheck = StorageDevice.BeginShowSelector(
                            GamerOne.PlayerIndex, null, null);

            gameLoadRequested = true;

        }

        void ContinueMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (LbKStorage.Level == 1)
            {
                LoadingScreen.Load(ScreenManager, true, GamerOne.PlayerIndex, new Level());
            }
            //LoadingScreen.Load(ScreenManager, true, GamerOne.PlayerIndex, new ?"Level")
        }

        void NewGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            //LoadingScreen.Load(ScreenManager, true, GamerOne.PlayerIndex, new LevelOne());
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

    }
}
