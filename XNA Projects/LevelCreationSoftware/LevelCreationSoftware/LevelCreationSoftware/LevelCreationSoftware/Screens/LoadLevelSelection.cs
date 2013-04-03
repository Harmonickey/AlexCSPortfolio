using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace LevelCreationSoftware
{
    class LoadLevelSelection : MenuScreen
    {

        MenuEntry[] fileNameEntries;

        Viewport viewport;

        ContentManager content;

        SpriteBatch spriteBatch;

        Texture2D backgroundTextureLoadLevel;

        public LoadLevelSelection(string[] filenames)
            : base("Loaded Levels")
        {

            IsPopup = true;

            fileNameEntries = new MenuEntry[LbKStorageLevelCreation.FileNames.Count];

            for (int i = 0; i < LbKStorageLevelCreation.FileNames.Count; i++)
            {
                fileNameEntries[i] = new MenuEntry(filenames[i]);

                fileNameEntries[i].Selected += FileNameMenuEntrySelected;

                if (i <= 6)
                {
                    MenuEntries.Add(fileNameEntries[i]);
                }
                else if (i > 6 && i <= 12)
                {
                    fileNameEntries[i].CreateNewColumn = true;
                    MenuEntries.Add(fileNameEntries[i]);
                }
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

                    LoadingScreen.Load(ScreenManager, true, ControllingPlayer, new LevelCreate(fileNameEntry.Text, false));

                }
            }
        }

        public override void HandleInput(InputState input)
        {
            if (input.CurrentGamePadStates[(int)PlayerIndex.One].Buttons.Y == ButtonState.Pressed && input.PreviousGamePadStates[(int)PlayerIndex.One].Buttons.Y == ButtonState.Released)
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
            LbKStorageLevelCreation.FileNames.RemoveAt(MenuScreen.SelectedEntry);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTextureLoadLevel, new Rectangle(0,0, viewport.Width, viewport.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
