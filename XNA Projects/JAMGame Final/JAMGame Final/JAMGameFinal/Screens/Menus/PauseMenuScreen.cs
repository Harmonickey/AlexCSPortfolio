using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JAMGameFinal
{
    class PauseMenuScreen : MenuScreen
    {
        int levelOn;

        float playerTriggerSensitivity;

        public PauseMenuScreen(float triggerSen, int levelOn)
            : base("Paused")
        {
            IsPopup = true;

            IsPauseMenuDone = false;

            this.levelOn = levelOn;

            playerTriggerSensitivity = triggerSen;
 
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry controlsGameMenuEntry = new MenuEntry("How to Play");
            MenuEntry optionsGameMenuEntry = new MenuEntry("Options");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel; 
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
            const string message = "Are you sure you want to return to the main menu?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message, true, false);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ControlsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlScreen(), e.PlayerIndex);
        }

        void OptionsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(playerTriggerSensitivity), e.PlayerIndex);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Active = false;
            
            LoadingScreen.Load(ScreenManager, true, null, true, new BackgroundScreen(),
                                                              new MainMenuScreen());

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            base.Draw(gameTime);
        }
    }
}