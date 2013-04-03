using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    class MenuScreen : GameScreen
    {
        
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        List<MenuEntry> menuEntries2 = new List<MenuEntry>();
        List<MenuEntry> menuEntries3 = new List<MenuEntry>();
        List<MenuEntry> menuEntries4 = new List<MenuEntry>();
        List<MenuEntry> startUpMenuEntries = new List<MenuEntry>();
        
        int selectedEntry = 0;
        int selectedEntry2 = 0;
        int selectedEntry3 = 0;
        int selectedEntry4 = 0;
        int startUpSelectedEntry = 0;
        
        string menuTitle;

        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }
        protected IList<MenuEntry> MenuEntries2
        {
            get { return menuEntries2; }
        }
        protected IList<MenuEntry> MenuEntries3
        {
            get { return menuEntries3; }
        }
        protected IList<MenuEntry> MenuEntries4
        {
            get { return menuEntries4; }
        }
        protected IList<MenuEntry> StartUpMenuEntries
        {
            get { return startUpMenuEntries; }
        }
        
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }

        public override void HandleInput(InputState input)
        {
            if (OnStartUp)
            {
                if (input.IsMenuUp(ControllingPlayer))
                {
                    startUpSelectedEntry--;

                    if (startUpSelectedEntry < 0)
                        startUpSelectedEntry = startUpMenuEntries.Count - 1;
                }
                if (input.IsMenuDown(ControllingPlayer))
                {
                    startUpSelectedEntry++;

                    if (startUpSelectedEntry >= startUpMenuEntries.Count)
                        startUpSelectedEntry = 0;
                }
            }
            else
            {

                if (input.IsMenuUp(PlayerIndex.One))
                {
                    selectedEntry--;

                    if (selectedEntry < 0)
                        selectedEntry = menuEntries.Count - 1;
                }

                if (input.IsMenuUp(PlayerIndex.Two))
                {
                    selectedEntry2--;

                    if (selectedEntry2 < 0)
                        selectedEntry2 = menuEntries2.Count - 1;
                }
                if (input.IsMenuUp(PlayerIndex.Three))
                {
                    selectedEntry3--;

                    if (selectedEntry3 < 0)
                        selectedEntry3 = menuEntries3.Count - 1;
                }
                if (input.IsMenuUp(PlayerIndex.Four))
                {
                    selectedEntry4--;

                    if (selectedEntry4 < 0)
                        selectedEntry4 = menuEntries4.Count - 1;
                }

                if (input.IsMenuDown(PlayerIndex.One))
                {
                    selectedEntry++;

                    if (selectedEntry >= menuEntries.Count)
                        selectedEntry = 0;
                }
                if (input.IsMenuDown(PlayerIndex.Two))
                {
                    selectedEntry2++;

                    if (selectedEntry2 >= menuEntries2.Count)
                        selectedEntry2 = 0;
                }
                if (input.IsMenuDown(PlayerIndex.Three))
                {
                    selectedEntry3++;

                    if (selectedEntry3 >= menuEntries3.Count)
                        selectedEntry3 = 0;
                }
                if (input.IsMenuDown(PlayerIndex.Four))
                {
                    selectedEntry4++;

                    if (selectedEntry4 >= menuEntries4.Count)
                        selectedEntry4 = 0;
                }
            }
            
            PlayerIndex playerIndex;
            if (OnStartUp)
            {
                if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
                {
                    OnSelectEntry(startUpSelectedEntry, playerIndex);
                }
                else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
                {
                    OnCancel(playerIndex);
                }

            }
            else
            {
                if (input.IsMenuSelect(PlayerIndex.One, out playerIndex))
                {
                    OnSelectEntry(selectedEntry, playerIndex);
                }
                else if (input.IsMenuCancel(PlayerIndex.One, out playerIndex))
                {
                    OnCancel(playerIndex);
                }
                if (input.IsMenuSelect(PlayerIndex.Two, out playerIndex))
                {
                    OnSelectEntry(selectedEntry2, playerIndex);
                }
                else if (input.IsMenuCancel(PlayerIndex.Two, out playerIndex))
                {
                    OnCancel(playerIndex);
                }
                if (input.IsMenuSelect(PlayerIndex.Three, out playerIndex))
                {
                    OnSelectEntry(selectedEntry3, playerIndex);
                }
                else if (input.IsMenuCancel(PlayerIndex.Three, out playerIndex))
                {
                    OnCancel(playerIndex);
                }
                if (input.IsMenuSelect(PlayerIndex.Four, out playerIndex))
                {
                    OnSelectEntry(selectedEntry4, playerIndex);
                }
                else if (input.IsMenuCancel(PlayerIndex.Four, out playerIndex))
                {
                    OnCancel(playerIndex);
                }
            }
            
        }

        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            if (OnStartUp)
            {
                if (startUpSelectedEntry >= startUpMenuEntries.Count)
                {
                    startUpSelectedEntry = 0;
                }
                startUpMenuEntries[startUpSelectedEntry].OnSelectEntry(playerIndex);

            }
            else
            {

                if (playerIndex == PlayerIndex.One)
                {
                    if (selectedEntry >= menuEntries.Count)
                    {
                        selectedEntry = 0;
                    }
                    menuEntries[selectedEntry].OnSelectEntry(playerIndex);
                }
                if (playerIndex == PlayerIndex.Two)
                {
                    if (selectedEntry2 >= menuEntries2.Count)
                    {
                        selectedEntry2 = 0;
                    }
                    menuEntries2[selectedEntry2].OnSelectEntry(playerIndex);
                }
                if (playerIndex == PlayerIndex.Three)
                {
                    if (selectedEntry3 >= menuEntries3.Count)
                    {
                        selectedEntry3 = 0;
                    }
                    menuEntries3[selectedEntry3].OnSelectEntry(playerIndex);
                }
                if (playerIndex == PlayerIndex.Four)
                {
                    if (selectedEntry4 >= menuEntries4.Count)
                    {
                        selectedEntry4 = 0;
                    }
                    menuEntries4[selectedEntry4].OnSelectEntry(playerIndex);
                }
            }
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            if (!IsPauseMenuDone)
            {
                Active = true;
                IsPauseMenuDone = true;
            }
            if (OnControl)
            {
                OnControl = false;
            }
            ExitScreen();
        }

        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);

            if (OnStartUp)
            {
                for (int i = 0; i < startUpMenuEntries.Count; i++)
                {
                    bool isSelected = IsActive && (i == startUpSelectedEntry);

                    startUpMenuEntries[i].Update(this, isSelected, gameTime);
                }

            }
            else
            {

                for (int i = 0; i < menuEntries.Count; i++)
                {
                    bool isSelected = IsActive && (i == selectedEntry);

                    menuEntries[i].Update(this, isSelected, gameTime);
                }
                for (int i = 0; i < menuEntries2.Count; i++)
                {
                    bool isSelected = IsActive && (i == selectedEntry2);

                    menuEntries2[i].Update(this, isSelected, gameTime);
                }
                for (int i = 0; i < menuEntries3.Count; i++)
                {
                    bool isSelected = IsActive && (i == selectedEntry3);

                    menuEntries3[i].Update(this, isSelected, gameTime);
                }
                for (int i = 0; i < menuEntries4.Count; i++)
                {
                    bool isSelected = IsActive && (i == selectedEntry4);

                    menuEntries4[i].Update(this, isSelected, gameTime);
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Rectangle safeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            Vector2 position = new Vector2(safeArea.Left, safeArea.Top  + 50);
            Vector2 position2 = new Vector2(safeArea.Center.X + 100, safeArea.Top + 50);
            Vector2 position3 = new Vector2(safeArea.Left, safeArea.Top + 400);
            Vector2 position4 = new Vector2(safeArea.Center.X + 100, safeArea.Top + 400);
            Vector2 startUpMenuPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 + 200, ScreenManager.GraphicsDevice.Viewport.Height / 2);
            
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;

            if (ScreenState == ScreenState.TransitionOn)
                position2.X -= transitionOffset * 256;
            else
                position2.X += transitionOffset * 512;

            if (ScreenState == ScreenState.TransitionOn)
                position3.X -= transitionOffset * 256;
            else
                position3.X += transitionOffset * 512;

            if (ScreenState == ScreenState.TransitionOn)
                position4.X -= transitionOffset * 256;
            else
                position4.X += transitionOffset * 512;

            if (OnStartUp)
            {
                if (ScreenState == ScreenState.TransitionOn)
                    startUpMenuPosition.X -= transitionOffset * 256;
                else
                    startUpMenuPosition.X += transitionOffset * 512;

            }

            spriteBatch.Begin();

            if (OnStartUp)
            {
                for (int i = 0; i < startUpMenuEntries.Count; i++)
                {
                    MenuEntry startUpMenuEntry = startUpMenuEntries[i];

                    bool isSelected = IsActive && (i == startUpSelectedEntry);

                    startUpMenuEntry.Draw(this, startUpMenuPosition, isSelected, gameTime);

                    startUpMenuPosition.Y += startUpMenuEntry.GetHeight(this);
                }
                
            }
            else
            {

                for (int i = 0; i < menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = menuEntries[i];

                    bool isSelected = IsActive && (i == selectedEntry);

                    menuEntry.Draw(this, position, isSelected, gameTime);

                    position.Y += menuEntry.GetHeight(this);
                }
                for (int i = 0; i < menuEntries2.Count; i++)
                {
                    MenuEntry menuEntry2 = menuEntries2[i];

                    bool isSelected = IsActive && (i == selectedEntry2);

                    menuEntry2.Draw(this, position2, isSelected, gameTime);

                    position2.Y += menuEntry2.GetHeight(this);
                }
                for (int i = 0; i < menuEntries3.Count; i++)
                {
                    MenuEntry menuEntry3 = menuEntries3[i];

                    bool isSelected = IsActive && (i == selectedEntry3);

                    menuEntry3.Draw(this, position3, isSelected, gameTime);

                    position3.Y += menuEntry3.GetHeight(this);
                }
                for (int i = 0; i < menuEntries4.Count; i++)
                {
                    MenuEntry menuEntry4 = menuEntries4[i];

                    bool isSelected = IsActive && (i == selectedEntry4);

                    menuEntry4.Draw(this, position4, isSelected, gameTime);

                    position4.Y += menuEntry4.GetHeight(this);
                }
            }

            Vector2 titlePosition = new Vector2(400, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            float titleScale = 1.25f;
            
            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}