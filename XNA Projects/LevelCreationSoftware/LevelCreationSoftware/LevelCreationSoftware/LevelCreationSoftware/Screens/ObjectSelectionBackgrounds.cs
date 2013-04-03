using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelCreationSoftware
{
    public enum BackgroundType
    {
        Normal,
        Other
    }

    class ObjectSelectionBackgrounds : ObjectSelectionMenuScreen
    {

        public static int ObjectSelected;
        public static int LayerSelected;

        SpriteBatch spriteBatch;

        Texture2D[] normalBackgroundReferences;
        Texture2D[] otherBackgroundReferences;
        Texture2D[] fileReferences;


        public static BackgroundType type = BackgroundType.Normal;

        bool tileSelection = false;

        Texture2D fileSelectionBox;

        int tileSelected = 0;
        int fileSelected = 0;
        int layerSelected = 0;

        Color layerOneColor = Color.White;
        Color layerTwoColor = Color.White;
        Color layerThreeColor = Color.White;
        Color layerFourColor = Color.White;

        Vector2 selectionBoxPosition, fileSelectionBoxPosition;

        Texture2D tileSelectionBox;

        MenuEntry[] backgroundTypes;
        MenuEntry[] backgrounds;

        Color color = Color.White;

        bool isAssigned = false;

        public ObjectSelectionBackgrounds(Texture2D[] referenceBackgroundsNormal, Texture2D[] referenceBackgroundsOther, Texture2D[] referenceFiles, Texture2D selectionBoxReference, SpriteBatch spriteBatch)
            :base("ObjectSelection")
        {
            IsPopup = true;
            //do Max and Mins to get the ultimate if there are more types later
            backgrounds = new MenuEntry[(int)MathHelper.Max(referenceBackgroundsNormal.Length, referenceBackgroundsOther.Length)];
            backgroundTypes = new MenuEntry[referenceFiles.Length];

            normalBackgroundReferences = referenceBackgroundsNormal;
            otherBackgroundReferences = referenceBackgroundsOther;

            fileReferences = referenceFiles;

            fileSelectionBox = selectionBoxReference;
            tileSelectionBox = selectionBoxReference;

            this.spriteBatch = spriteBatch;

            for (int i = 0; i < backgroundTypes.Length; i++)
            {
                backgroundTypes[i] = new MenuEntry(string.Empty);

                backgroundTypes[i].Selected += BackgroundTypeSelected;

                if (i <= 4)
                {
                    MenuEntries.Add(backgroundTypes[i]);
                }
                else
                {
                    backgroundTypes[i].CreateNewColumn = true;
                    MenuEntries.Add(backgroundTypes[i]);
                }
            }

            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i] = new MenuEntry(string.Empty);

                backgrounds[i].Selected += BackgroundSelected;
            }
        }

        void BackgroundTypeSelected(object sender, PlayerIndexEventArgs e)
        {
            
            backgroundTypes[fileSelected].isSelected = false;
            type = (BackgroundType)fileSelected;
            MenuEntries.Clear();
            switch (type)
            {
                case BackgroundType.Normal:
                    for (int j = 0; j < normalBackgroundReferences.Length; j++)
                    {
                        if (j <= 3)
                        {
                            MenuEntries.Add(backgrounds[j]);
                        }
                        else if (j > 3 && j <= 7)
                        {
                            backgrounds[j].CreateNewColumn = true;
                            backgrounds[j].whichNewColumn = 1;
                            MenuEntries.Add(backgrounds[j]);
                        }
                        else if (j > 7 && j <= 11)
                        {
                            backgrounds[j].CreateNewColumn = true;
                            backgrounds[j].whichNewColumn = 2;
                            MenuEntries.Add(backgrounds[j]);
                        }
                        else if (j > 11)
                        {
                            backgrounds[j].CreateNewColumn = true;
                            backgrounds[j].whichNewColumn = 3;
                            MenuEntries.Add(backgrounds[j]);
                        }

                    }
                    break;
                case BackgroundType.Other:
                    for (int j = 0; j < otherBackgroundReferences.Length; j++)
                    {
                        if (j <= 3)
                        {
                            MenuEntries.Add(backgrounds[j]);
                        }
                        else if (j > 3 && j <= 7)
                        {
                            backgrounds[j].CreateNewColumn = true;
                            backgrounds[j].whichNewColumn = 1;
                            MenuEntries.Add(backgrounds[j]);
                        }
                        else if (j > 7 && j <= 11)
                        {
                            backgrounds[j].CreateNewColumn = true;
                            backgrounds[j].whichNewColumn = 2;
                            MenuEntries.Add(backgrounds[j]);
                        }
                        else if (j > 11)
                        {
                            backgrounds[j].CreateNewColumn = true;
                            backgrounds[j].whichNewColumn = 3;
                            MenuEntries.Add(backgrounds[j]);
                        }

                    }
                    break;
            }

            tileSelection = true;
            isAssigned = false;
        }

        void BackgroundSelected(object sender, PlayerIndexEventArgs e)
        {
            
            ObjectSelected = tileSelected;
            LayerSelected = layerSelected;
            backgrounds[tileSelected].isSelected = false;
            color = Color.Red;
            LevelCreate.backgroundChosen = true;
                  
        }

        public override void HandleInput(InputState input)
        {
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Down) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Down))
            {
                if (tileSelection)
                {
                    tileSelected++;

                    if (type == BackgroundType.Normal)
                    {
                        if (tileSelected == normalBackgroundReferences.Length)
                        {
                            tileSelected = 0;
                        }
                    }
                    if (type == BackgroundType.Other)
                    {
                        if (tileSelected == otherBackgroundReferences.Length)
                        {
                            tileSelected = 0;
                        }
                    }
                    

                    selectionBoxPosition = new Vector2(backgrounds[tileSelected].menuEntryPosition.X, backgrounds[tileSelected].menuEntryPosition.Y);

                }
                else
                {

                    fileSelected++;

                    if (fileSelected == backgroundTypes.Length)
                    {
                        fileSelected = 0;
                    }

                    fileSelectionBoxPosition = new Vector2(backgroundTypes[fileSelected].menuEntryPosition.X, backgroundTypes[fileSelected].menuEntryPosition.Y);


                }

                LevelCreate.tileChosen = false;
                color = Color.White;
            }
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Up) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Up))
            {
                if (tileSelection)
                {
                    tileSelected--;

                    if (type == BackgroundType.Normal)
                    {
                        if (tileSelected < 0)
                        {
                            tileSelected = normalBackgroundReferences.Length - 1;
                        }
                    }
                    if (type == BackgroundType.Other)
                    {
                        if (tileSelected < 0)
                        {
                            tileSelected = otherBackgroundReferences.Length - 1;
                        }
                    }
                    
                    selectionBoxPosition = new Vector2(backgrounds[tileSelected].menuEntryPosition.X, backgrounds[tileSelected].menuEntryPosition.Y);
                }
                else
                {
                    fileSelected--;

                    if (fileSelected < 0)
                    {
                        fileSelected = backgroundTypes.Length - 1;
                    }

                    fileSelectionBoxPosition = new Vector2(backgroundTypes[fileSelected].menuEntryPosition.X, backgroundTypes[fileSelected].menuEntryPosition.Y);

                }

                LevelCreate.tileChosen = false;
                color = Color.White;
            }
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Back) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Back))
            {
                if (tileSelection)
                {
                    if (LevelCreate.tileChosen)
                    {

                        this.ExitScreen();

                    }
                    else
                    {
                        MenuEntries.Clear();

                        foreach (MenuEntry backgroundType in backgroundTypes)
                        {
                            MenuEntries.Add(backgroundType);
                        }

                        tileSelection = false;
                        //isAssigned = false;
                    }
                }
                else
                {
                    this.ExitScreen();
                }
            }

            if (tileSelection)
            {
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D0) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D0))
                {
                    layerSelected = 0;
                    layerOneColor = Color.Red;
                    layerTwoColor = Color.White;
                    layerThreeColor = Color.White;
                    layerFourColor = Color.White;
                }
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D1) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D1))
                {
                    layerSelected = 1;
                    layerOneColor = Color.White;
                    layerTwoColor = Color.Red;
                    layerThreeColor = Color.White;
                    layerFourColor = Color.White;
                }
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D2) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D2))
                {
                    layerSelected = 2;
                    layerOneColor = Color.White;
                    layerTwoColor = Color.White;
                    layerThreeColor = Color.Red;
                    layerFourColor = Color.White;
                }
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D3) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D3))
                {
                    layerSelected = 3;
                    layerOneColor = Color.White;
                    layerTwoColor = Color.White;
                    layerThreeColor = Color.White;
                    layerFourColor = Color.Red;
                }
                if (layerSelected == 0)
                {
                    
                    layerOneColor = Color.Red;
                    layerTwoColor = Color.White;
                    layerThreeColor = Color.White;
                    layerFourColor = Color.White;

                }
            }

            base.HandleInput(input);
        }

        private bool CheckExit()
        {
            if (LevelCreate.tileChosen)
            {
                return true;
            }
            else
                return false;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            spriteBatch.Begin();

            if (tileSelection)
            {

                if (!isAssigned)
                {
                    Rectangle safeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

                    selectionBoxPosition = new Vector2(safeArea.Left + 100, safeArea.Top + 150);
                    //fileSelected = 0;
                    tileSelected = 0;
                    isAssigned = true;
                }

                spriteBatch.DrawString(ScreenManager.Font, "0", new Vector2(800, 500), layerOneColor);
                spriteBatch.DrawString(ScreenManager.Font, "1", new Vector2(850, 500), layerTwoColor);
                spriteBatch.DrawString(ScreenManager.Font, "2", new Vector2(900, 500), layerThreeColor);
                spriteBatch.DrawString(ScreenManager.Font, "3", new Vector2(950, 500), layerFourColor);
                
                if (type == BackgroundType.Normal)
                {
                    for (int i = 0; i < normalBackgroundReferences.Length; i++)
                    {
                        spriteBatch.Draw(normalBackgroundReferences[i],
                            new Rectangle((int)backgrounds[i].menuEntryPosition.X, (int)backgrounds[i].menuEntryPosition.Y, 250, 100),
                            null,
                            Color.White,
                            0.0f,
                            new Vector2(0, 0),
                            SpriteEffects.None, 0);
                    }
                    
                }
                if (type == BackgroundType.Other)
                {
                    for (int i = 0; i < otherBackgroundReferences.Length; i++)
                    {
                        spriteBatch.Draw(otherBackgroundReferences[i],
                            new Rectangle((int)backgrounds[i].menuEntryPosition.X, (int)backgrounds[i].menuEntryPosition.Y, 250, 100),
                            null,
                            Color.White,
                            0.0f,
                            new Vector2(0, 0),
                            SpriteEffects.None, 0);
                    }

                }
                
                if (!IsExiting)
                {
                    spriteBatch.Draw(tileSelectionBox, new Rectangle((int)selectionBoxPosition.X, (int)selectionBoxPosition.Y, 250, 100), Color.White);
                }
            }
            else
            {
                if (!isAssigned)
                {
                    Rectangle safeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

                    fileSelectionBoxPosition = new Vector2(safeArea.Left + 100, safeArea.Top + 150);

                    isAssigned = true;
                }

                for (int i = 0; i < backgroundTypes.Length; i++)
                {
                    spriteBatch.Draw(fileReferences[i], new Vector2(backgroundTypes[i].menuEntryPosition.X, backgroundTypes[i].menuEntryPosition.Y), Color.White);
                }

                if (!IsExiting)
                {
                    spriteBatch.Draw(fileSelectionBox, new Rectangle((int)fileSelectionBoxPosition.X, (int)fileSelectionBoxPosition.Y, 250, 100), Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
