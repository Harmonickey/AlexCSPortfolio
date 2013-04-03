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

namespace LevelCreationSoftware
{
    
    class MainMenuScreen : MenuScreen
    {
        
        string[] fileNames;

        IAsyncResult KeyboardResult;


        #region Variables and Objects

        ContentManager content;
        SpriteBatch spriteBatch;

        MenuEntry helpMenuEntry, exitMenuEntry, levelCreateMenuEntry, loadTextLevelMenuEntry;
        //MenuEntry levelLoadMenuEntry;
        MenuEntry invertTest;
        MenuEntry voidTest;

        Texture2D background;

        Viewport viewport;

        bool gameLoadRequested = false;

        static IAsyncResult resultCheck;

        static StorageDevice storageDevice;

        #endregion

        #region Initialization
        public MainMenuScreen()
            : base("Main Menu Screen")
        {
            
            levelCreateMenuEntry = new MenuEntry("Create Level");
            //levelLoadMenuEntry = new MenuEntry("Load Level");
            loadTextLevelMenuEntry = new MenuEntry("Load Text Level");
            invertTest = new MenuEntry("Invert Test");
            voidTest = new MenuEntry("Void Test");
            helpMenuEntry = new MenuEntry("Help");
            exitMenuEntry = new MenuEntry("Exit");

            levelCreateMenuEntry.Selected += LevelCreateMenuEntrySelected;
            //levelLoadMenuEntry.Selected += LevelLoadMenuEntrySelected;
            
            loadTextLevelMenuEntry.Selected += LoadTextLevelMenuEntrySelected;

            invertTest.Selected += InvertTestMenuEntrySelected;
            voidTest.Selected += VoidTestMenuEntrySelected;

            helpMenuEntry.Selected += HelpMenuEntrySelected;
            exitMenuEntry.Selected += ExitMenuEntrySelected;

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

            MenuEntries.Add(levelCreateMenuEntry);
            //MenuEntries.Add(levelLoadMenuEntry);
            MenuEntries.Add(loadTextLevelMenuEntry);
            MenuEntries.Add(invertTest);
            MenuEntries.Add(voidTest);
            MenuEntries.Add(helpMenuEntry);
            MenuEntries.Add(exitMenuEntry);

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {

            #region Load Game Requested
            if (gameLoadRequested && resultCheck.IsCompleted)
            {
                storageDevice = StorageDevice.EndShowSelector(resultCheck);

                if (storageDevice != null && storageDevice.IsConnected)
                {
                    LbKStorageLevelCreation.LoadGame(storageDevice, GamerOne, true);

                    if (LbKStorageLevelCreation.FileNames != null)
                    {
                        //assign the filenames to the filename list from the save file with the correct count
                        fileNames = new string[LbKStorageLevelCreation.FileNames.Count];

                        for (int i = 0; i < LbKStorageLevelCreation.FileNames.Count; i++)
                        {
                            fileNames[i] = LbKStorageLevelCreation.FileNames[i];
                        }

                        ScreenManager.AddScreen(new LoadLevelSelection(fileNames), PlayerIndex.One);
                        //LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new LoadLevelSelection(fileNames));
                    }
                }

                //reset flag
                gameLoadRequested = false;
            }

            #endregion

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        void LevelCreateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            GamePlayScreen.storageDevice = storageDevice;
            GamePlayScreen.LevelCreateSession = true;

            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new LevelCreate(false, String.Empty));
        }

        void HelpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HowToCreateScreen(), PlayerIndex.One);
        }

        void ExitMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        void LevelLoadMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            resultCheck = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);

            gameLoadRequested = true;

        }

        void LoadTextLevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //for debug
            string listPath = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\LevelLayouts.txt";
            //string listPath = "Content\\LevelLayouts.txt";
            List<string> theList = new List<string>();

            if (File.Exists(listPath))
            {
                using (StreamReader sr = new StreamReader(listPath))
                {
                    while (sr.Peek() > 0)
                    {
                        theList.Add(sr.ReadLine());
                    }

                    sr.Close();
                }
            }

            
            //add more
            //theList[1] = "'LevelName'";
            ScreenManager.AddScreen(new LoadTextLevelSelection(theList), PlayerIndex.One);

            //LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new LoadTextLevelSelection(theList));
            //ScreenManager.AddScreen(new LoadTextLevelSelection(theList), PlayerIndex.One);
        }

        void InvertTestMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (KeyboardResult == null && !Guide.IsVisible)
            {
                string title = "PASSWORD";
                string description = "Enter Password to Enter Debug Session";
                string defaultText = "";

                AsyncCallback callback = new AsyncCallback(ConfirmQuitMessageBoxAccepted_Invert);

                KeyboardResult = Guide.BeginShowKeyboardInput(PlayerIndex.One, title,
                                                          description, defaultText,
                                                          callback, null, true);

            }
        }

        void ConfirmQuitMessageBoxAccepted_Invert(IAsyncResult result)
        {
            if (Guide.EndShowKeyboardInput(KeyboardResult) == "CumBucket")
            {
                LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new InvertTest());
            }
            else
            {
                const string message = "Wrong Password!!!";

                MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

                confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted_Invert;

                ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
            }
        }

        void ConfirmQuitMessageBoxAccepted_Invert(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new MainMenuScreen());
        }
        void VoidTestMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new VoidTest());
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
