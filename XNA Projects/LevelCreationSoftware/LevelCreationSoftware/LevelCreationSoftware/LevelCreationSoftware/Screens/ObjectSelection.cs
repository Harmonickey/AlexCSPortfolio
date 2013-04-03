using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelCreationSoftware
{
    public enum TileType
    {
        Block,
        Scenery,
        Slope
        //Four,
        //ect
    }

    class ObjectSelection : ObjectSelectionMenuScreen
    {
        public static int ObjectSelected;
        public static int LayerSelected;
        
        public static TileType type = TileType.Block;

        bool tileSelection = false;


        SpriteBatch SpriteBatch;

        //add new textures here.
        Texture2D tileSelectionBox;
        Texture2D[] fileReferences; //4
        Texture2D[] tileReferenceBlocks;  //3
        Texture2D[] tileReferenceSceneries;  //18
        Texture2D[] tileReferenceSlopes;     //2

        Texture2D fileSelectionBox;

        int tileSelected = 0;
        int fileSelected = 0;
        int layerSelected = 0;

        Color layerOneColor = Color.White;
        Color layerTwoColor = Color.White;
        Color layerThreeColor = Color.White;
        Color layerFourColor = Color.White;

        Vector2 selectionBoxPosition, fileSelectionBoxPosition;

        MenuEntry[] tileTypes;
        MenuEntry[] tiles;

        Color color = Color.White;

        bool isAssigned = false;

        public ObjectSelection(Texture2D[] referenceBlocks, 
            Texture2D[] referenceSceneries, 
            Texture2D[] referenceSlopes,
            Texture2D selectionBoxReference, 
            Texture2D[] referenceFiles, 
            SpriteBatch spriteBatch)
            : base("Object Selection")
        {
            tiles = new MenuEntry[(int)MathHelper.Max(
                MathHelper.Max(referenceBlocks.Length, referenceSceneries.Length), referenceSlopes.Length)];
            tileTypes = new MenuEntry[referenceFiles.Length];

            IsPopup = true;

            tileReferenceBlocks = referenceBlocks;
            tileReferenceSceneries = referenceSceneries;
            tileReferenceSlopes = referenceSlopes;
            fileReferences = referenceFiles;

            tileSelectionBox = selectionBoxReference;
            fileSelectionBox = selectionBoxReference;

            SpriteBatch = spriteBatch;

            for (int i = 0; i < tileTypes.Length; i++)
            {
                tileTypes[i] = new MenuEntry(string.Empty);

                tileTypes[i].Selected += TileTypeSelected;

                if (i <= 4)
                {
                    MenuEntries.Add(tileTypes[i]);
                }
                else
                {
                    tileTypes[i].CreateNewColumn = true;
                    MenuEntries.Add(tileTypes[i]);
                }
            }
            
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new MenuEntry(string.Empty);
                
                tiles[i].Selected += TileSelected;
            }
        }

        void TileTypeSelected(object sender, PlayerIndexEventArgs e)
        {
            
            tileTypes[fileSelected].isSelected = false;
            type = (TileType)fileSelected;
            MenuEntries.Clear();
            switch (type)
            {
                case TileType.Block:
                    for (int i = 0; i < tileReferenceBlocks.Length; i++)
                    {
                        //columns are more easily used when they have 4 tiles in each hence i > 3 to i = 7 below in the first new column...
                        //      that means tiles 4, 5, 6, and 7 will be in that column
                        if (i <= 3)
                        {
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 3 && i <= 7)   //first new column
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 1;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 7 && i <= 11)  //second new column
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 2;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 11 && i<= 15)  //third new column
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 3;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 15 && i <= 19)            //fourth new column
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 4;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 19)                      //fifth new column
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 5;
                            MenuEntries.Add(tiles[i]);
                        }
                                
                    }
                    break;
                case TileType.Scenery:
                    for (int i = 0; i < tileReferenceSceneries.Length; i++)
                    {
                        if (i <= 3)
                        {
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 3 && i <= 7)
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 1;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 7 && i <= 11)
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 2;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 11 && i <= 15)
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 3;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 15 && i <= 19)            //fourth new column
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 4;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 19)
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 5;
                            MenuEntries.Add(tiles[i]);
                        }      
                    }
                    break;
                case TileType.Slope:
                    for (int i = 0; i < tileReferenceSlopes.Length; i++)
                    {
                        if (i <= 3)
                        {
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 3 && i <= 7)
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 1;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 7 && i <= 11)
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 2;
                            MenuEntries.Add(tiles[i]);
                        }
                        else if (i > 11)
                        {
                            tiles[i].CreateNewColumn = true;
                            tiles[i].whichNewColumn = 3;
                            MenuEntries.Add(tiles[i]);
                        }
                                
                    }
                    break; 
                }

            tileSelection = true;
            isAssigned = false;
        }

        void TileSelected(object sender, PlayerIndexEventArgs e)
        {
            
            ObjectSelected = tileSelected;
            LayerSelected = layerSelected;
            tiles[tileSelected].isSelected = false;
            color = Color.Red;
            LevelCreate.tileChosen = true;
                   
        }

        public override void HandleInput(InputState input)
        {

            #region MenuDown
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Down) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Down))
            {
                if (tileSelection)
                {
                    tileSelected++;

                    if (type == TileType.Block)
                    {
                        if (tileSelected == tileReferenceBlocks.Length)
                        {
                            tileSelected = 0;
                        }
                    }
                    if (type == TileType.Scenery)
                    {
                        if (tileSelected == tileReferenceSceneries.Length)
                        {
                            tileSelected = 0;
                        }
                    }
                    if (type == TileType.Slope)
                    {
                        if (tileSelected == tileReferenceSlopes.Length)
                        {
                            tileSelected = 0;
                        }
                    }

                    selectionBoxPosition = new Vector2(tiles[tileSelected].menuEntryPosition.X, tiles[tileSelected].menuEntryPosition.Y);
                    
                }
                else
                {
                    
                    fileSelected++;
                    
                    if (fileSelected == tileTypes.Length)
                    {
                        fileSelected = 0;
                    }

                    fileSelectionBoxPosition = new Vector2(tileTypes[fileSelected].menuEntryPosition.X, tileTypes[fileSelected].menuEntryPosition.Y);

                    
                }

                LevelCreate.tileChosen = false;
                color = Color.White;
            }
            #endregion
            #region MenuUp
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Up) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Up))
            {
                if (tileSelection)
                {
                    tileSelected--;
                        
                    if (type == TileType.Block)
                    {
                        if (tileSelected < 0)
                        {
                            tileSelected = tileReferenceBlocks.Length - 1;
                        }
                    }
                    if (type == TileType.Scenery)
                    {
                        if (tileSelected < 0)
                        {
                            tileSelected = tileReferenceSceneries.Length - 1;
                        }
                    }
                    if (type == TileType.Slope)
                    {
                        if (tileSelected < 0)
                        {
                            tileSelected = tileReferenceSlopes.Length - 1;
                        }
                    }

                    selectionBoxPosition = new Vector2(tiles[tileSelected].menuEntryPosition.X, tiles[tileSelected].menuEntryPosition.Y);
                }
                else
                {
                    fileSelected--;
                    
                    if (fileSelected < 0)
                    {
                        fileSelected = tileTypes.Length - 1;
                    }

                    fileSelectionBoxPosition = new Vector2(tileTypes[fileSelected].menuEntryPosition.X, tileTypes[fileSelected].menuEntryPosition.Y);

                }

                LevelCreate.tileChosen = false;
                color = Color.White;
            }
            #endregion
            #region LayerSelect
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

            #endregion
            #region Back
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

                        foreach (MenuEntry tileType in tileTypes)
                        {
                            MenuEntries.Add(tileType);
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
            #endregion

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
            //basic checks if need be

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }


        public override void Draw(GameTime gameTime)
        {
            
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            SpriteBatch.Begin();

            SpriteBatch.DrawString(ScreenManager.Font, tileSelected.ToString(), new Vector2(500.0f), Color.White);

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

                SpriteBatch.DrawString(ScreenManager.Font, "0", new Vector2(800, 500), layerOneColor);
                SpriteBatch.DrawString(ScreenManager.Font, "1", new Vector2(850, 500), layerTwoColor);
                SpriteBatch.DrawString(ScreenManager.Font, "2", new Vector2(900, 500), layerThreeColor);
                SpriteBatch.DrawString(ScreenManager.Font, "3", new Vector2(950, 500), layerFourColor);
                

                if (type == TileType.Block)
                {
                    for (int i = 0; i < tileReferenceBlocks.Length; i++)
                    {
                        SpriteBatch.Draw(tileReferenceBlocks[i], new Vector2(tiles[i].menuEntryPosition.X, tiles[i].menuEntryPosition.Y), Color.White);
                    }
                    
                }
                if (type == TileType.Scenery)
                {
                    for (int i = 0; i < tileReferenceSceneries.Length; i++)
                    {
                        if (i >= 15 && i < 17)
                        {
                            SpriteBatch.Draw(tileReferenceSceneries[i],
                                new Vector2(tiles[i].menuEntryPosition.X, tiles[i].menuEntryPosition.Y),
                                null,
                                Color.White,
                                0.0f,
                                new Vector2(0, 0),
                                0.5f,
                                SpriteEffects.None, 0);
                        }
                        else
                        {

                            SpriteBatch.Draw(tileReferenceSceneries[i],
                                new Vector2(tiles[i].menuEntryPosition.X, tiles[i].menuEntryPosition.Y),
                                null,
                                Color.White,
                                0.0f,
                                new Vector2(0, 0),
                                1.0f,
                                SpriteEffects.None, 0);
                        }
                        
                    }
                    
                }
                if (type == TileType.Slope)
                {
                    for (int i = 0; i < tileReferenceSlopes.Length; i++)
                    {
                        SpriteBatch.Draw(tileReferenceSlopes[i], new Vector2(tiles[i].menuEntryPosition.X, tiles[i].menuEntryPosition.Y), Color.White);
                    }
                    
                }
                if (!IsExiting)
                {
                    SpriteBatch.Draw(tileSelectionBox, selectionBoxPosition, color);
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

                for (int i = 0; i < tileTypes.Length; i++)
                {
                    SpriteBatch.Draw(fileReferences[i], new Vector2(tileTypes[i].menuEntryPosition.X, tileTypes[i].menuEntryPosition.Y), Color.White);
                }
                
                if (!IsExiting)
                {
                    SpriteBatch.Draw(fileSelectionBox, new Rectangle((int)fileSelectionBoxPosition.X, (int)fileSelectionBoxPosition.Y, 250, 100), Color.White);
                }
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
