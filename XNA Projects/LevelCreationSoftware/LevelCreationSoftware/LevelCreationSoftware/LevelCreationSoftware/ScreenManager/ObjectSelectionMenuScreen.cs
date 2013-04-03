using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelCreationSoftware
{
    class ObjectSelectionMenuScreen : GameScreen
    {

        List<MenuEntry> menuEntries = new List<MenuEntry>();

        int selectedEntry = 0;

        string menuTitle;

        bool doReset = false;

        int previousColumnNumber = 1;

        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        public ObjectSelectionMenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

        }

        public override void HandleInput(InputState input)
        {


            if (input.IsMenuUp(PlayerIndex.One))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }


            if (input.IsMenuDown(PlayerIndex.One))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            PlayerIndex playerIndex;

            if (input.IsMenuSelect(PlayerIndex.One, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);

            }
            else if (input.IsMenuCancel(PlayerIndex.One, out playerIndex))
            {

                OnCancel(playerIndex);

            }

        }

        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {

            if (playerIndex == PlayerIndex.One)
            {
                if (selectedEntry >= menuEntries.Count)
                {
                    selectedEntry = 0;
                }
                menuEntries[selectedEntry].OnSelectEntry(playerIndex);
                
            }

        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            //this is the only difference
        }

        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }


        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Rectangle safeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            Vector2 position = new Vector2(safeArea.Left + 100, safeArea.Top + 150);
            
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;

            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                if (menuEntry.CreateNewColumn)
                {
                    if (menuEntry.whichNewColumn != previousColumnNumber)
                    {
                        doReset = true;
                        position = NewColumn(menuEntry.whichNewColumn, position);
                    }

                    

                    menuEntry.Draw(this, position, isSelected, gameTime);
                    
                }
                else
                {
                    menuEntry.Draw(this, position, isSelected, gameTime);
                }

                if (!LevelCreateSession)
                {
                    position.Y += menuEntry.GetHeight(this);
                }
                else
                {
                    position.Y += menuEntry.GetHeight(this) + 100;
                }

                previousColumnNumber = menuEntry.whichNewColumn;
            }

            previousColumnNumber = 1;

            Vector2 titlePosition = new Vector2(safeArea.Left + 400, safeArea.Top + 100);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        private Vector2 NewColumn(int whichColumn, Vector2 referencePosition)
        {
            Rectangle safeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            referencePosition = CheckNewColumnHeight(referencePosition);

            referencePosition += new Vector2(whichColumn * 100, 0);
            
            return referencePosition;
        }

        private Vector2 CheckNewColumnHeight(Vector2 referencePosition)
        {
            Rectangle safeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            if (doReset)
            {
                doReset = false;
                referencePosition = new Vector2(safeArea.Left + 100, safeArea.Top + 150);
            }
            

            return referencePosition;
        }
    }
}
