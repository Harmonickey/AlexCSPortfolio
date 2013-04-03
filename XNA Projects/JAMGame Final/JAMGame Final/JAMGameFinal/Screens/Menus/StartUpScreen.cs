using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace JAMGameFinal
{
    class StartUpScreen : MenuScreen
    {

        public StartUpScreen()
            : base(string.Empty)
        {
            OnStartUp = true;

            StartUpScreenMenuEntry startGameMenuEntry = new StartUpScreenMenuEntry("Start Game");                                                   
            StartUpScreenMenuEntry controlsGameMenuEntry = new StartUpScreenMenuEntry("How to Play");
            StartUpScreenMenuEntry quitGameMenuEntry = new StartUpScreenMenuEntry("Quit Game");

            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            controlsGameMenuEntry.Selected += ControlsGameMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            StartUpMenuEntries.Add(startGameMenuEntry);
            StartUpMenuEntries.Add(controlsGameMenuEntry);
            StartUpMenuEntries.Add(quitGameMenuEntry);
        }

        void StartGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            int levelOn = 1;

            ControllingPlayer = e.PlayerIndex;

            OnStartUp = false;

            LoadingScreen.Load(ScreenManager, true, ControllingPlayer, true, new BackgroundScreen(),
                                                                              new MainMenuScreen(levelOn));

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

        }

        void ControlsGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlScreen(), e.PlayerIndex);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message, true, true);

            confirmExitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, ControllingPlayer);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}