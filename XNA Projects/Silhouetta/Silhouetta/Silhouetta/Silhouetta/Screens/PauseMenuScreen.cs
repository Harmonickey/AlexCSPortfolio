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

namespace Silhouetta
{
    class PauseMenuScreen : MenuScreen
    {
        MenuEntry resumeGameMenuEntry;
        MenuEntry controlsGameMenuEntry;
        MenuEntry optionsGameMenuEntry;
        MenuEntry quitGameMenuEntry;

        int score;
        int level;
        int checkPoint;
        
        IAsyncResult KeyboardResult;

        string input;

        public static string SavedName;

        public PauseMenuScreen(int currentScore, int currentLevel, int currentCheckPoint, string levelName)
            : base("Paused: " + levelName)
        {
            score = currentScore;
            level = currentLevel;
            checkPoint = currentCheckPoint;

            IsPopup = true;
            
            resumeGameMenuEntry = new MenuEntry("Resume Game");
            controlsGameMenuEntry = new MenuEntry("How to Play");
            optionsGameMenuEntry = new MenuEntry("Options");
            quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += ResumeGameMenuEntrySelected;
            controlsGameMenuEntry.Selected += ControlsGameMenuEntrySelected;
            optionsGameMenuEntry.Selected += OptionsGameMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(controlsGameMenuEntry);
            MenuEntries.Add(optionsGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Don't forget to save before exiting session";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, GamerOne.PlayerIndex);
        }

        void ResumeGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ExitScreen();
        }

        void ControlsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlScreen(), e.PlayerIndex);
        }

        void OptionsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            
            LoadingScreen.Load(ScreenManager, true, GamerOne.PlayerIndex, new MainMenuScreen(false, false));
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (KeyboardResult != null && KeyboardResult.IsCompleted)
            {
                input = Guide.EndShowKeyboardInput(KeyboardResult);

                SavedName = input;

                KeyboardResult = null;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }
    }
}