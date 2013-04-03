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
    class PauseMenuScreen : MenuScreen
    {
        MenuEntry resumeGameMenuEntry;
        MenuEntry returnGameMenuEntry;
        MenuEntry returnToMainMenu;
        MenuEntry controlsGameMenuEntry;
        MenuEntry optionsGameMenuEntry;
        MenuEntry quitGameMenuEntry;

        //MenuEntry saveGameMenuEntry;
        MenuEntry saveGameTextMenuEntry;
        MenuEntry playTestGameMenuEntry;

        static List<Tile> Tiles;
        static List<Background> Backgrounds;

        IAsyncResult KeyboardResult;

        string input;

        string LevelName;

        Vector2 currentOffSet;

        public static string SavedName;

        
        public PauseMenuScreen(List<Tile> tilesReference, List<Background> backgroundsReference, string levelName, Vector2 scrollOffset)
            : base("Paused")
        {
            
            IsPopup = true;
            
            currentOffSet = scrollOffset;
            LevelName = levelName;
            Tiles = tilesReference;
            Backgrounds = backgroundsReference;
            
            resumeGameMenuEntry = new MenuEntry("Resume Level Create");
            playTestGameMenuEntry = new MenuEntry("Play Test");
            controlsGameMenuEntry = new MenuEntry("How to Create");
            //saveGameMenuEntry = new MenuEntry("Save");
            saveGameTextMenuEntry = new MenuEntry("Save to Text");
            returnToMainMenu = new MenuEntry("Return To Main Menu");
            quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += ResumeGameMenuEntrySelected;
            
            controlsGameMenuEntry.Selected += HowToCreateMenuEntrySelected;

            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            
            returnToMainMenu.Selected += ReturnToMainMenuSelected;
                
            playTestGameMenuEntry.Selected += PlayTestGameMenuEntrySelected;

            if (levelName != String.Empty)
            {
                SavedName = levelName;
                saveGameTextMenuEntry.Selected += SaveGameToTextMenuEntrySelected_Named;
            }
            else
            {
                saveGameTextMenuEntry.Selected += SaveGameToTextMenuEntrySelected_Unnamed;
            }

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(playTestGameMenuEntry);
            MenuEntries.Add(controlsGameMenuEntry);
            //MenuEntries.Add(saveGameMenuEntry);
            MenuEntries.Add(saveGameTextMenuEntry);
            MenuEntries.Add(returnToMainMenu);
            MenuEntries.Add(quitGameMenuEntry);

        }

        public PauseMenuScreen()
            : base("Paused")
        {
            IsPopup = true;
            
            resumeGameMenuEntry = new MenuEntry("Resume Game");
            controlsGameMenuEntry = new MenuEntry("How to Play");
            optionsGameMenuEntry = new MenuEntry("Options");
            returnGameMenuEntry = new MenuEntry("Return to Level Create");
            quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += ResumeGameMenuEntrySelected;
            controlsGameMenuEntry.Selected += ControlsGameMenuEntrySelected;
            optionsGameMenuEntry.Selected += OptionsGameMenuEntrySelected;
            returnGameMenuEntry.Selected += ReturnGameMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(controlsGameMenuEntry);
            MenuEntries.Add(optionsGameMenuEntry);
            MenuEntries.Add(returnGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Don't forget to save before exiting session";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ResumeGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            this.ExitScreen();
        }

        void ReturnToMainMenuSelected(object sender, PlayerIndexEventArgs e)
        {
            LevelCreateSession = false;
            

            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new MainMenuScreen());
        }

        void ControlsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlScreen(), e.PlayerIndex);
        }

        void HowToCreateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HowToCreateScreen(), e.PlayerIndex);
        }

        void OptionsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        void ReturnGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LevelCreateSession = true;
            //for debug
            string path = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\PlayTestInfo.txt";
            //string path = "Content\\PlayTestInfo.txt";
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new LevelCreate(true, path));
        }

        void SaveGameToTextMenuEntrySelected_Unnamed(object sender, PlayerIndexEventArgs e)
        {

            if (KeyboardResult == null && !Guide.IsVisible)
            {
                string title = "Level Name";
                string description = "Name This Level (No Repeat Names)";
                string defaultText = "'Level Name'";

                KeyboardResult = Guide.BeginShowKeyboardInput(PlayerIndex.One, title,
                                                          description, defaultText,
                                                          null, null);

                

            }
            
        }

        void SaveGameToTextMenuEntrySelected_Named(object sender, PlayerIndexEventArgs e)
        {
            //for debug
            string path = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\" + SavedName + ".txt";
            //string path = "Content\\" + SavedName + ".txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < Tiles.Count; i++)
                {                            //0                               //1                                     //2                                          //3               //4
                    sw.WriteLine(Tiles[i].position.X.ToString() + "," + Tiles[i].position.Y.ToString() + "," + Tiles[i].type.ToString() + "," + Tiles[i].objectNumber.ToString() + "," + Tiles[i].layerNumber.ToString() + ",Tile");
                }
                for (int i = 0; i < Backgrounds.Count; i++)
                {
                    sw.WriteLine(Backgrounds[i].position.X.ToString() + "," + Backgrounds[i].position.Y.ToString() + "," + Backgrounds[i].type.ToString() + "," + Backgrounds[i].objectNumber.ToString() + "," + Backgrounds[i].layerNumber.ToString() + ",Background");
                }

                sw.Close();
            }
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            
            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new MainMenuScreen());
        }

        void PlayTestGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //for debug
            string path = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\PlayTestInfo.txt";
            //string path = "Content\\PlayTestInfo.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < Tiles.Count; i++)
                {
                    sw.WriteLine(Tiles[i].position.X.ToString() + "," + Tiles[i].position.Y.ToString() + "," + Tiles[i].type.ToString() + "," + Tiles[i].objectNumber.ToString() + "," + Tiles[i].layerNumber.ToString() + ",Tile");
                }
                for (int i = 0; i < Backgrounds.Count; i++)
                {
                    sw.WriteLine(Backgrounds[i].position.X.ToString() + "," + Backgrounds[i].position.Y.ToString() + "," + Backgrounds[i].type.ToString() + "," + Backgrounds[i].objectNumber.ToString() + "," + Backgrounds[i].layerNumber.ToString() + ",Background");
                }
                sw.Close();
            }

            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new PlayTestScreen(path));
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }

        void ConfirmNameChangeMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            if (KeyboardResult == null && !Guide.IsVisible)
            {
                string title = "Level Name";
                string description = "Name This Level (No Repeat Names)";
                string defaultText = "'Level Name'";

                KeyboardResult = Guide.BeginShowKeyboardInput(PlayerIndex.One, title,
                                                          description, defaultText,
                                                          null, null);

            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (KeyboardResult != null && KeyboardResult.IsCompleted)
            {
                input = Guide.EndShowKeyboardInput(KeyboardResult);
                //for debug
                string path = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\" + input + ".txt";
                string listPath = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\LevelLayouts.txt";
                //string path = "Content\\" + input + ".txt";
                //string listPath = "Content\\LevelLayouts.txt";
                List<string> levelList = new List<string>();

                if (File.Exists(listPath))
                {
                    using (StreamReader sr = new StreamReader(listPath))
                    {

                        while (sr.Peek() > 0)
                        {
                            string line = sr.ReadLine();

                            if (line == input)
                            {
                                const string message = "Needs a Different Name!";

                                MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

                                confirmQuitMessageBox.Accepted += ConfirmNameChangeMessageBoxAccepted;

                                ScreenManager.AddScreen(confirmQuitMessageBox, PlayerIndex.One);

                                KeyboardResult = null;

                                break;

                            }
                        }
                        sr.Close();
                    }
                }

                if (KeyboardResult != null && KeyboardResult.IsCompleted)
                {

                    SavedName = input;

                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        for (int i = 0; i < Tiles.Count; i++)
                        {
                            sw.WriteLine(Tiles[i].position.X.ToString() + "," + Tiles[i].position.Y.ToString() + "," + Tiles[i].type.ToString() + "," + Tiles[i].objectNumber.ToString() + "," + Tiles[i].layerNumber.ToString() + ",Tile");
                        }
                        for (int i = 0; i < Backgrounds.Count; i++)
                        {
                            sw.WriteLine(Backgrounds[i].position.X.ToString() + "," + Backgrounds[i].position.Y.ToString() + "," + Backgrounds[i].type.ToString() + "," + Backgrounds[i].objectNumber.ToString() + "," + Backgrounds[i].layerNumber.ToString() + ",Background");
                        }
                        sw.Close();
                    }

                    levelList.Clear();

                    if (File.Exists(listPath))
                    {
                        using (StreamReader sr = new StreamReader(listPath))
                        {

                            while (sr.Peek() > 0)
                            {
                                levelList.Add(sr.ReadLine());
                            }
                            sr.Close();
                        }

                        File.Delete(listPath);
                    }



                    using (StreamWriter sw = new StreamWriter(listPath))
                    {
                        for (int i = 0; i < levelList.Count; i++)
                        {
                            sw.WriteLine(levelList[i]);
                        }

                        sw.WriteLine(input);
                    }


                    KeyboardResult = null;
                }
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }
    }
}