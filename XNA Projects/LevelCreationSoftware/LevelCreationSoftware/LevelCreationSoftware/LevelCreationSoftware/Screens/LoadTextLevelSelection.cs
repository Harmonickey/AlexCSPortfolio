using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace LevelCreationSoftware
{
    class LoadTextLevelSelection : MenuScreen
    {
        string[] fileNames;

        MenuEntry[] fileNameEntries;

        Viewport viewport;

        ContentManager content;

        SpriteBatch spriteBatch;

        Texture2D backgroundTextureLoadLevel;

        
        public LoadTextLevelSelection(List<string> names)
            :base("Text Levels")
        {
            IsPopup = true;

            
            fileNames = new string[names.Count];

            for (int i = 0; i < names.Count; i++)
            {
                fileNames[i] = names[i];
            }

            fileNameEntries = new MenuEntry[fileNames.Length];

            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNameEntries[i] = new MenuEntry(fileNames[i]);

                fileNameEntries[i].Selected += FileNameMenuEntrySelected;

                MenuEntries.Add(fileNameEntries[i]);
            }

            

        }

        public override void LoadContent()
        {
            viewport = ScreenManager.GraphicsDevice.Viewport;

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            backgroundTextureLoadLevel = content.Load<Texture2D>("Backgrounds\\GameBackgrounds\\backgroundLoadLevel");
        }

        void FileNameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //nothing
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            //give it the .isSelected attribute first
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);

            //then check for it
            foreach (MenuEntry fileNameEntry in fileNameEntries)
            {
                if (fileNameEntry.isSelected)
                {
                    LevelCreateSession = true;

                    fileNameEntry.isSelected = false;

                    LoadingScreen.Load(ScreenManager, true, ControllingPlayer, new LevelCreate(fileNameEntry.Text, true));

                }
            }
        }

        public override void HandleInput(InputState input)
        {
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Back) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Back))
            {
                this.ExitScreen();
            }
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Delete) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Delete))
            {
                const string message = "Are you sure you want to delete this file?";

                MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

                confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

                ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
            }

            base.HandleInput(input);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            MenuEntries.RemoveAt(MenuScreen.SelectedEntry);
            //for debug
            string path = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\" + fileNames[MenuScreen.SelectedEntry] + ".txt";
            string listPath = @"c:\Users\Alex\Desktop\LevelCreationSoftware\LevelCreationSoftware\LevelCreationSoftware\bin\x86\Debug\Content\LevelLayouts.txt";
            //string path = "Content\\" + fileNames[MenuScreen.SelectedEntry] + ".txt";
            //string listPath = "Content\\LevelLayouts.txt";
            
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            List<string> levelList = new List<string>();

            if (File.Exists(listPath))
            {
                using (StreamReader sr = new StreamReader(listPath))
                {

                    while (sr.Peek() > 0)
                    {
                        string line = sr.ReadLine();
                        if (line != fileNames[MenuScreen.SelectedEntry])
                        {
                            levelList.Add(line);
                        }
                        
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

            } 


        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTextureLoadLevel, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
