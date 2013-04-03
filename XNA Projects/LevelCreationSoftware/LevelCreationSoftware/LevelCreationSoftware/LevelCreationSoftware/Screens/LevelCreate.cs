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
    public enum AllowedObject
    {
        Tile,
        Background,
        Scenery
    }

    class LevelCreate : GamePlayScreen
    {
        #region Variables
       
        bool toggleCameraMove = false;

        Vector2 mouseCoor = Vector2.Zero;

        AllowedObject allowedObject = AllowedObject.Tile;

        bool tileSelected = false;
        bool backgroundSelected = false;

        public static bool tileChosen = false;
        public static bool backgroundChosen = false;

        int tileNumber = 0;
        string LevelName;

        bool loadingSavedGame = false;
        bool loadingSavedGameFromText = false;

        //add new textures here.
        Texture2D tileSelectionBox;

        int numberOfFileReferences = 3;
        Texture2D[] fileReferences;

        int numberOfBlockTiles = 15;
        Texture2D[] tileReferenceBlocks;  

        int numberOfSceneryTiles = 18;
        Texture2D[] tileReferenceSceneries;  

        int numberOfSlopeTiles = 2;
        Texture2D[] tileReferenceSlopes;

        int numberOfNormalBackgroundReferences = 2;
        Texture2D[] normalBackgroundReferences;

        int numberOfOtherBackgroundReferences = 1;
        Texture2D[] otherBackgroundReferences;

        int numberOfBackgroundFileReferences = 2;
        Texture2D[] backgroundFileReferences;

        List<Rectangle> tileRectangles = new List<Rectangle>();
        List<Rectangle> backgroundRectangles = new List<Rectangle>();

        List<Tile> Tiles = new List<Tile>();
        Tile tempLoadedTile;

        List<Background> Backgrounds = new List<Background>();
        Background tempLoadedBackground;

        bool returningToLevelCreate;

        string tempPath;

        Color numberOneColor = Color.Red;
        Color numberTwoColor = Color.White;
        Color numberThreeColor = Color.White;

        float zoomScale = 1.0f;

        Point mousePoint;

        
        #endregion

        public LevelCreate(bool returning, string path)
        {
            
            returningToLevelCreate = returning;

            tempPath = path;
        }

        public LevelCreate(string levelName, bool textFile)
        {
            
            LevelName = levelName;
            
            loadingSavedGame = true;

            if (textFile)
            {
                loadingSavedGameFromText = true;
            }

        }

        public override void LoadContent()
        {
            base.LoadContent();

            fileReferences = new Texture2D[numberOfFileReferences];
            //load new tiles here
            for (int i = 0; i < fileReferences.Length; i++)
            {
                fileReferences[i] = content.Load<Texture2D>("Sprites\\Files\\Tiles\\File" + i.ToString());
                //0 is block
                //1 is scenery
                //2 is slope
            }

            backgroundFileReferences = new Texture2D[numberOfBackgroundFileReferences];

            for (int i = 0; i < backgroundFileReferences.Length; i++)
            {
                backgroundFileReferences[i] = content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\BackgroundFile" + i.ToString());
                //0 is Normal
                //1 is Other
               
            }

            tileReferenceBlocks = new Texture2D[numberOfBlockTiles];

            for (int i = 0; i < tileReferenceBlocks.Length; i++)
            {
                tileReferenceBlocks[i] = content.Load<Texture2D>("Sprites\\Block\\Block" + i.ToString());
            }
            
            tileReferenceSceneries = new Texture2D[numberOfSceneryTiles];

            for (int i = 0; i < tileReferenceSceneries.Length; i++)
            {
                tileReferenceSceneries[i] = content.Load<Texture2D>("Sprites\\Scenery\\Scenery" + i.ToString());
            }

            tileReferenceSlopes = new Texture2D[numberOfSlopeTiles];

            for (int i = 0; i < tileReferenceSlopes.Length; i++)
            {
                tileReferenceSlopes[i] = content.Load<Texture2D>("Sprites\\Slope\\Slope" + i.ToString());
            }

            normalBackgroundReferences = new Texture2D[numberOfNormalBackgroundReferences];

            for (int i = 0; i < normalBackgroundReferences.Length; i++)
            {
                normalBackgroundReferences[i] = content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\Normal\\NormalBackground" + i.ToString());
            }

            otherBackgroundReferences = new Texture2D[numberOfOtherBackgroundReferences];

            for (int i = 0; i < otherBackgroundReferences.Length; i++)
            {
                otherBackgroundReferences[i] = content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\Other\\OtherBackground" + i.ToString());
            }
            
            tileSelectionBox = content.Load<Texture2D>("Sprites\\tileSelectionBox");
            
            if (loadingSavedGame)
            {
                if (loadingSavedGameFromText)
                {
                    string path = "Content\\" + LevelName + ".txt";

                    using (StreamReader sr = new StreamReader(path))
                    {
                        //Taken from a text file formated with 4 pieces of info.  coor1,coor2,type,style,layer,object
                        //                                                       (float,float,TileType,int32,int32,string)
                        while (sr.Peek() >= 0)  //apparently this keep going until the stream reader peeks and sees nothing on the line
                        {

                            string line = sr.ReadLine();  //read the current line

                            string[] parts = line.Split(',');  //split anything that has commas to separate parts of a string array

                            float X = (float)Convert.ToInt32(parts[0]);  //This is the first coordinate of the tile/background
                            float Y = (float)Convert.ToInt32(parts[1]);  //This is the second coordinate of the tile/background
                            int objectNumber = Convert.ToInt32(parts[3]);   //This is which style of the tile/background
                            int layerNumber = Convert.ToInt32(parts[4]);  //This is the layer of the tile/background
                            TileType type = TileType.Block;
                            BackgroundType bType = BackgroundType.Normal;
                            if (parts[5] == "Tile")
                            {
                                if (parts[2] == "Block")
                                    type = TileType.Block;
                                if (parts[2] == "Slope")
                                    type = TileType.Slope;
                                if (parts[2] == "Scenery")
                                    type = TileType.Scenery;
                                
                                tempLoadedTile = new Tile(content.Load<Texture2D>("Sprites\\" + parts[2] + "\\" + parts[2] + objectNumber.ToString()));
                                tempLoadedTile.position = new Vector2(X, Y);
                                tempLoadedTile.type = type;
                                tempLoadedTile.objectNumber = objectNumber;
                                tempLoadedTile.layerNumber = layerNumber;

                                if (parts[2] == "Scenery")
                                {
                                    tempLoadedTile.textureData =
                                        new Color[tempLoadedTile.sprite.Width * tempLoadedTile.sprite.Height];
                                    tempLoadedTile.sprite.GetData(tempLoadedTile.textureData);
                                }
                                Tiles.Add(tempLoadedTile);

                                tileRectangles.Add(new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), 100, 100));
                                tileRectangles[tileRectangles.Count - 1] = new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), 100, 100);

                            }
                            else if (parts[5] == "Background")
                            {
                                if (parts[2] == "Normal")
                                    bType = BackgroundType.Normal;
                                if (parts[2] == "Other")
                                    bType = BackgroundType.Other;

                                tempLoadedBackground = new Background(content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\" + parts[2] + "\\" + parts[2] + "Background" + objectNumber.ToString()));
                                tempLoadedBackground.position = new Vector2(X, Y);
                                tempLoadedBackground.type = bType;
                                tempLoadedBackground.objectNumber = objectNumber;
                                tempLoadedBackground.layerNumber = layerNumber;
                                Backgrounds.Add(tempLoadedBackground);

                                backgroundRectangles.Add(new Rectangle((int)(Backgrounds[Backgrounds.Count - 1].position.X + scrollOffset.X), (int)(Backgrounds[Backgrounds.Count - 1].position.Y + scrollOffset.Y), Backgrounds[Backgrounds.Count - 1].sprite.Width, Backgrounds[Backgrounds.Count - 1].sprite.Height));
                                backgroundRectangles[backgroundRectangles.Count - 1] = new Rectangle((int)(Backgrounds[Backgrounds.Count - 1].position.X + scrollOffset.X), (int)(Backgrounds[Backgrounds.Count - 1].position.Y + scrollOffset.Y), Backgrounds[Backgrounds.Count - 1].sprite.Width, Backgrounds[Backgrounds.Count - 1].sprite.Height);
                            }
                        }
                        sr.Close();
                    }
                    
                }
            }

            if (returningToLevelCreate)
            {
                using (StreamReader sr = new StreamReader(tempPath))
                {
                    
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();

                        string[] parts = line.Split(',');

                        float X = (float)Convert.ToInt32(parts[0]);  //This is the first coordinate
                        float Y = (float)Convert.ToInt32(parts[1]);  //This is the second coordinate
                        int objectNumber = Convert.ToInt32(parts[3]);   //This is which style of the tile
                        int layerNumber = Convert.ToInt32(parts[4]);
                        TileType type = TileType.Block;
                        BackgroundType bType = BackgroundType.Normal;

                        if (parts[5] == "Tile")
                        {
                            if (parts[2] == "Block")
                                type = TileType.Block;
                            if (parts[2] == "Slope")
                                type = TileType.Slope;
                            if (parts[2] == "Scenery")
                                type = TileType.Scenery;
                            
                            tempLoadedTile = new Tile(content.Load<Texture2D>("Sprites\\" + parts[2] + "\\" + parts[2] + objectNumber.ToString()));
                            tempLoadedTile.position = new Vector2(X, Y);
                            tempLoadedTile.type = type;
                            tempLoadedTile.objectNumber = objectNumber;
                            tempLoadedTile.layerNumber = layerNumber;
                            if (parts[2] == "Scenery")
                            {
                                tempLoadedTile.textureData =
                                    new Color[tempLoadedTile.sprite.Width * tempLoadedTile.sprite.Height];
                                tempLoadedTile.sprite.GetData(tempLoadedTile.textureData);
                            }

                            Tiles.Add(tempLoadedTile);

                            tileRectangles.Add(new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), 100, 100));
                            tileRectangles[tileRectangles.Count - 1] = new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), 100, 100);

                        }
                        else if (parts[5] == "Background")
                        {

                            if (parts[2] == "Normal")
                                bType = BackgroundType.Normal;
                            if (parts[2] == "Other")
                                bType = BackgroundType.Other;

                            tempLoadedBackground = new Background(content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\" + parts[2] + "\\" + parts[2] + "Background" + objectNumber.ToString()));
                            tempLoadedBackground.position = new Vector2(X, Y);
                            tempLoadedBackground.type = bType;
                            tempLoadedBackground.objectNumber = objectNumber;
                            tempLoadedBackground.layerNumber = layerNumber;

                            Backgrounds.Add(tempLoadedBackground);

                            backgroundRectangles.Add(new Rectangle((int)(Backgrounds[Backgrounds.Count - 1].position.X + scrollOffset.X), (int)(Backgrounds[Backgrounds.Count - 1].position.Y + scrollOffset.Y), 100, 100));
                            backgroundRectangles[backgroundRectangles.Count - 1] = new Rectangle((int)(Backgrounds[Backgrounds.Count - 1].position.X + scrollOffset.X), (int)(Backgrounds[Backgrounds.Count - 1].position.Y + scrollOffset.Y), 100, 100);

                        }
                    }
                    sr.Close();
                }

                File.Delete(tempPath);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();

            foreach (Background background in Backgrounds)
            {

                switch (background.layerNumber)
                {
                    case 0:
                        background.scrollOffset = scrollOffset;
                        break;
                    case 1:
                        background.scrollOffset = layerOneScrollOffset;
                        break;
                    case 2:
                        background.scrollOffset = layerTwoScrollOffset;
                        break;
                    case 3:
                        background.scrollOffset = layerThreeScrollOffset;
                        break;
                }
                
                SpriteBatch.Draw(background.sprite,
                    (background.position + background.scrollOffset) + GetZoomVector(background.position),
                    null,
                    Color.White,
                    background.rotation,
                    new Vector2(0.0f),
                    zoomScale,
                    SpriteEffects.None, 0);
            }

            foreach(Tile tile in Tiles)
            {

                switch (tile.layerNumber)
                {
                    case 0:
                        tile.scrollOffset = scrollOffset;
                        break;
                    case 1:
                        tile.scrollOffset = layerOneScrollOffset;
                        break;
                    case 2:
                        tile.scrollOffset = layerTwoScrollOffset;
                        break;
                    case 3:
                        tile.scrollOffset = layerThreeScrollOffset;
                        break;
                }

                if (tile.type == TileType.Scenery && tile.objectNumber >= 15 && tile.objectNumber <= 16)
                {

                    SpriteBatch.Draw(tile.sprite,
                        (tile.position + tile.scrollOffset) + GetZoomVector(tile.position),
                        null,
                        Color.White,
                        tile.rotation,
                        new Vector2(0.0f),
                        zoomScale * 0.1f,
                        SpriteEffects.None, 0);

                }
                else
                {
                    SpriteBatch.Draw(tile.sprite,
                        (tile.position + tile.scrollOffset) + GetZoomVector(tile.position),
                        null,
                        Color.White,
                        tile.rotation,
                        new Vector2(0.0f),
                        zoomScale,
                        SpriteEffects.None, 0);
                }


            }

            if (allowedObject == AllowedObject.Scenery || allowedObject == AllowedObject.Tile)
            {
                if (tileSelected)
                {
                    SpriteBatch.Draw(tileSelectionBox, new Rectangle((int)(Tiles[tileNumber].position.X + scrollOffset.X), (int)(Tiles[tileNumber].position.Y + scrollOffset.Y), Tiles[tileNumber].sprite.Width, Tiles[tileNumber].sprite.Height), Color.Yellow);
                }
            }
            if (allowedObject == AllowedObject.Background)
            {
                if (backgroundSelected)
                {
                    SpriteBatch.Draw(tileSelectionBox, new Rectangle((int)(Backgrounds[tileNumber].position.X + scrollOffset.X), (int)(Backgrounds[tileNumber].position.Y + scrollOffset.Y), Backgrounds[tileNumber].sprite.Width, Backgrounds[tileNumber].sprite.Height), Color.Yellow);
                }

            }
            if (zoomScale == 1.0f)
            {
                
                SpriteBatch.DrawString(ScreenManager.Font, "( 1 ) Tile", new Vector2(mousePoint.X, mousePoint.Y) + new Vector2(100, 0), numberOneColor, 0.0f, new Vector2(0.0f), zoomScale, SpriteEffects.None, 0);
                SpriteBatch.DrawString(ScreenManager.Font, "( 2 ) Background", new Vector2(mousePoint.X, mousePoint.Y) + new Vector2(100, 50), numberTwoColor, 0.0f, new Vector2(0.0f), zoomScale, SpriteEffects.None, 0);
                SpriteBatch.DrawString(ScreenManager.Font, "( 3 ) Scenery", new Vector2(mousePoint.X, mousePoint.Y) + new Vector2(100, 100), numberThreeColor, 0.0f, new Vector2(0.0f), zoomScale, SpriteEffects.None, 0);
            }

            SpriteBatch.DrawString(ScreenManager.Font, "Zoom: " + ((int)(zoomScale * 100.0f)).ToString() + "%", new Vector2(viewport.X + viewport.Width - 200, viewport.Y + 50), Color.White);
            SpriteBatch.DrawString(ScreenManager.Font, "'0' - + ", new Vector2(viewport.X + viewport.Width - 200, viewport.Y + 80), Color.White);
            
            SpriteBatch.End();
        }

        private Vector2 GetZoomVector(Vector2 originalPosition)
        {
            Vector2 directionVector = Vector2.Normalize(screenCenter - (originalPosition + scrollOffset));

            Vector2 magVector = originalPosition - (originalPosition * zoomScale);

            float magnitude = Vector2.Distance(originalPosition, originalPosition * zoomScale);

            Vector2 signedVector = Vector2.Zero;

            if (zoomScale > 1.0f)
                signedVector = new Vector2(-1, -1);
            else if (zoomScale < 1.0f)
                signedVector = new Vector2(1, 1);
            else
                signedVector = Vector2.Zero;

            return directionVector * (signedVector * magnitude);

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
            //small point on the pointer

            if (tileChosen == true)
            {
                LoadTile(ObjectSelection.type, ObjectSelection.ObjectSelected, ObjectSelection.LayerSelected);

                tileChosen = false;
            }
            if (backgroundChosen == true)
            {
                LoadBackground(ObjectSelectionBackgrounds.type, ObjectSelectionBackgrounds.ObjectSelected, ObjectSelectionBackgrounds.LayerSelected);

                backgroundChosen = false;
            }
            /*
            if (PauseMenuScreen.SavedName != null)
            {
                LevelName = PauseMenuScreen.SavedName;
                PauseMenuScreen.SavedName = null;
            }
            */ 
        }

        public override void HandleInput(InputState input)
        {
            mousePoint = new Point((int)(input.CurrentMouseStates[(int)PlayerIndex.One].X), (int)(input.CurrentMouseStates[(int)PlayerIndex.One].Y));
            
            if (zoomScale > 1.0f || zoomScale < 1.0f)
            {
                #region Move Zoomed Screen
                if (input.CurrentMouseStates[(int)PlayerIndex.One].LeftButton == ButtonState.Pressed)
                {

                    if (input.CurrentMouseStates[(int)PlayerIndex.One].X != input.PreviousMouseStates[(int)PlayerIndex.One].X)
                    {

                        cameraPosition -= new Vector2(input.CurrentMouseStates[(int)PlayerIndex.One].X - input.PreviousMouseStates[(int)PlayerIndex.One].X, input.CurrentMouseStates[(int)PlayerIndex.One].Y - input.PreviousMouseStates[(int)PlayerIndex.One].Y);

                    }
                }
                #endregion
            }
            else
            {
                #region Pause
                KeyboardState keyboardState = input.CurrentKeyboardStates[(int)PlayerIndex.One];
                GamePadState gamePadState = input.CurrentGamePadStates[(int)PlayerIndex.One];

                bool playerOneGamePadDisconnected = !gamePadState.IsConnected &&
                                            input.GamePadWasConnected[(int)PlayerIndex.One];

                //...if so then pause and wait
                if (input.IsPauseGame(PlayerIndex.One) || playerOneGamePadDisconnected)
                {
                    if (LevelName != null)
                    {
                        ScreenManager.AddScreen(new PauseMenuScreen(Tiles, Backgrounds, LevelName, scrollOffset), PlayerIndex.One);

                    }
                    else
                    {
                        ScreenManager.AddScreen(new PauseMenuScreen(Tiles, Backgrounds, String.Empty, scrollOffset), PlayerIndex.One);
                    }

                }
                #endregion

                #region GetTiles
                //add references here
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.LeftShift) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.LeftShift))
                {
                    ScreenManager.AddScreen(new ObjectSelection(tileReferenceBlocks, tileReferenceSceneries, tileReferenceSlopes, tileSelectionBox, fileReferences, SpriteBatch), PlayerIndex.One);
                }

                #endregion

                #region GetBackgrounds

                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.RightShift) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.RightShift))
                {
                    //do this for background creation
                    ScreenManager.AddScreen(new ObjectSelectionBackgrounds(normalBackgroundReferences, otherBackgroundReferences, backgroundFileReferences, tileSelectionBox, SpriteBatch), PlayerIndex.One);
                }

                #endregion

                #region SelectAllowedObject
                if (!tileSelected && !backgroundSelected)
                {
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D1) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D1))
                    {
                        allowedObject = AllowedObject.Tile;
                        
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D2) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D2))
                    {
                        allowedObject = AllowedObject.Background;
                        
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D3) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D3))
                    {
                        allowedObject = AllowedObject.Scenery;
                        
                    }
                    if (input.CurrentMouseStates[(int)PlayerIndex.One].ScrollWheelValue > input.PreviousMouseStates[(int)PlayerIndex.One].ScrollWheelValue)
                    {
                        allowedObject--;

                        if (allowedObject < 0)
                            allowedObject = AllowedObject.Scenery;
                    }
                    if (input.CurrentMouseStates[(int)PlayerIndex.One].ScrollWheelValue < input.PreviousMouseStates[(int)PlayerIndex.One].ScrollWheelValue)
                    {
                        allowedObject++;

                        if (allowedObject > AllowedObject.Scenery)
                            allowedObject = 0;
                    }

                    if (allowedObject == AllowedObject.Tile)
                    {
                        numberOneColor = Color.Red;
                        numberTwoColor = Color.White;
                        numberThreeColor = Color.White;
                    }
                    else if (allowedObject == AllowedObject.Background)
                    {
                        numberOneColor = Color.White;
                        numberTwoColor = Color.Red;
                        numberThreeColor = Color.White;
                    }
                    else if (allowedObject == AllowedObject.Scenery)
                    {
                        numberOneColor = Color.White;
                        numberTwoColor = Color.White;
                        numberThreeColor = Color.Red;
                    }
                }

                #endregion

                if (!toggleCameraMove)
                {
                    #region SelectObjects Mouse
                    if (input.CurrentMouseStates[(int)PlayerIndex.One].LeftButton == ButtonState.Pressed && input.PreviousMouseStates[(int)PlayerIndex.One].LeftButton == ButtonState.Released ||
                        input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Space) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Space))
                    {
                        if (allowedObject == AllowedObject.Tile || allowedObject == AllowedObject.Scenery)
                        {
                            if (!tileSelected)
                            {
                                for (int i = Tiles.Count - 1; i >= 0; i--)
                                {
                                    if (Tiles[i].type == TileType.Scenery && allowedObject == AllowedObject.Scenery)
                                    {
                                        if (Tiles[i].objectNumber == 15 || Tiles[i].objectNumber == 16)
                                        {
                                            tileRectangles[i] = new Rectangle((int)(Tiles[i].position.X + scrollOffset.X), (int)(Tiles[i].position.Y + scrollOffset.Y), 10, 10);
                                        }
                                        else
                                        {
                                            tileRectangles[i] = new Rectangle((int)(Tiles[i].position.X + scrollOffset.X), (int)(Tiles[i].position.Y + scrollOffset.Y), Tiles[i].sprite.Width, Tiles[i].sprite.Height);
                                        }

                                        if (tileRectangles[i].Contains(mousePoint))
                                        {
                                            tileSelected = true;
                                            tileNumber = i;
                                            break;
                                        }
                                    }
                                    if (Tiles[i].type != TileType.Scenery && allowedObject == AllowedObject.Tile)
                                    {

                                        tileRectangles[i] = new Rectangle((int)(Tiles[i].position.X + scrollOffset.X), (int)(Tiles[i].position.Y + scrollOffset.Y), Tiles[i].sprite.Width, Tiles[i].sprite.Height);

                                        if (tileRectangles[i].Contains(mousePoint))
                                        {
                                            tileSelected = true;
                                            tileNumber = i;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                tileSelected = false;
                                tileRectangles[tileNumber] = new Rectangle((int)(Tiles[tileNumber].position.X + scrollOffset.X), (int)(Tiles[tileNumber].position.Y + scrollOffset.Y), Tiles[tileNumber].sprite.Width, Tiles[tileNumber].sprite.Height);
                                return;
                            }
                        }
                        if (allowedObject == AllowedObject.Background)
                        {
                            if (!backgroundSelected)
                            {
                                for (int i = Backgrounds.Count - 1; i >= 0; i--)
                                {

                                    backgroundRectangles[i] = new Rectangle((int)(Backgrounds[i].position.X + scrollOffset.X), (int)(Backgrounds[i].position.Y + scrollOffset.Y), Backgrounds[i].sprite.Width, Backgrounds[i].sprite.Height);

                                    if (backgroundRectangles[i].Contains(mousePoint))
                                    {
                                        backgroundSelected = true;
                                        tileNumber = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {

                                backgroundSelected = false;
                                backgroundRectangles[tileNumber] = new Rectangle((int)(Backgrounds[tileNumber].position.X + scrollOffset.X), (int)(Backgrounds[tileNumber].position.Y + scrollOffset.Y), Backgrounds[tileNumber].sprite.Width, Backgrounds[tileNumber].sprite.Height);
                                return;
                            }
                        }
                    }
                    #endregion

                }
                else
                {
                    #region MoveCamera
                    if (input.CurrentMouseStates[(int)PlayerIndex.One].LeftButton == ButtonState.Pressed)
                    {
                        
                        if (input.CurrentMouseStates[(int)PlayerIndex.One].X != input.PreviousMouseStates[(int)PlayerIndex.One].X)
                        {
                            
                            cameraPosition -= new Vector2(input.CurrentMouseStates[(int)PlayerIndex.One].X - input.PreviousMouseStates[(int)PlayerIndex.One].X, input.CurrentMouseStates[(int)PlayerIndex.One].Y - input.PreviousMouseStates[(int)PlayerIndex.One].Y);
                            
                        }
                    }
                    #endregion

                }

                #region Move Tile
                if (tileSelected)
                {
                    
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.A) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.A))
                    {
                        Tiles[tileNumber].position.X -= 1;
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D))
                    {
                        Tiles[tileNumber].position.X += 1;
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.W) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.W))
                    {
                        Tiles[tileNumber].position.Y -= 1;
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.S) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.S))
                    {
                        Tiles[tileNumber].position.Y += 1;
                    }

                    Tiles[tileNumber].position += new Vector2(input.CurrentMouseStates[(int)PlayerIndex.One].X - input.PreviousMouseStates[(int)PlayerIndex.One].X, input.CurrentMouseStates[(int)PlayerIndex.One].Y - input.PreviousMouseStates[(int)PlayerIndex.One].Y);

                    
                }
                #endregion

                #region MoveBackground

                if (backgroundSelected)
                {

                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.A) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.A))
                    {
                        Backgrounds[tileNumber].position.X -= 1;
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D))
                    {
                        Backgrounds[tileNumber].position.X += 1;
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.W) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.W))
                    {
                        Backgrounds[tileNumber].position.Y -= 1;
                    }
                    if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.S) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.S))
                    {
                        Backgrounds[tileNumber].position.Y += 1;
                    }

                    Backgrounds[tileNumber].position += new Vector2(input.CurrentMouseStates[(int)PlayerIndex.One].X - input.PreviousMouseStates[(int)PlayerIndex.One].X, input.CurrentMouseStates[(int)PlayerIndex.One].Y - input.PreviousMouseStates[(int)PlayerIndex.One].Y);
                }

                #endregion

                #region CopyTile

                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.C) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.C))
                {
                    if (tileSelected)
                    {
                        tileSelected = false;
                        LoadTile(Tiles[tileNumber].type, Tiles[tileNumber].objectNumber, Tiles[tileNumber].layerNumber);
                    }
                    if (backgroundSelected)
                    {
                        backgroundSelected = false;
                        LoadBackground(Backgrounds[tileNumber].type, Backgrounds[tileNumber].objectNumber, Backgrounds[tileNumber].layerNumber);
                    }
                    
                }

                #endregion

                #region DeleteTile

                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Delete) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Delete))
                {
                    if (tileSelected)
                    {
                        tileSelected = false;
                        Tiles.RemoveAt(tileNumber);
                    }
                    if (backgroundSelected)
                    {
                        backgroundSelected = false;
                        Backgrounds.RemoveAt(tileNumber);
                    }
                    
                }

                #endregion
            }

            #region Zoom Screen
            if (!tileSelected && !backgroundSelected)
            {
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.OemPlus) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.OemPlus))
                {
                    zoomScale += 0.1f;
                    if (zoomScale > 2.0f)
                        zoomScale -= 0.1f;
                }
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.OemMinus) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.OemMinus))
                {
                    zoomScale -= 0.1f;
                    if (zoomScale < 0.0f)
                        zoomScale += 0.1f;
                }
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.D0) && input.PreviousKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.D0))
                {
                    zoomScale = 1.0f;
                }
            }

            
            #endregion

            #region ToggleCamera
            if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.LeftControl))
            {
                toggleCameraMove = true;
            }
            else
            {
                toggleCameraMove = false;
            }
            #endregion
        }

        void LoadTile(TileType type, int objectSelected, int layerSelected)
        {
            if (type == TileType.Block)
            {
                Tiles.Add(new Tile(content.Load<Texture2D>("Sprites\\Block\\Block" + objectSelected.ToString())));
            }
            if (type == TileType.Scenery)
            {
                Tiles.Add(new Tile(content.Load<Texture2D>("Sprites\\Scenery\\Scenery" + objectSelected.ToString())));
            }
            if (type == TileType.Slope)
            {
                Tiles.Add(new Tile(content.Load<Texture2D>("Sprites\\Slope\\Slope" + objectSelected.ToString())));
            }
            Tiles[Tiles.Count - 1].position = new Vector2(mousePoint.X, mousePoint.Y) - scrollOffset;  
            Tiles[Tiles.Count - 1].type = type;
            Tiles[Tiles.Count - 1].objectNumber = objectSelected;
            Tiles[Tiles.Count - 1].layerNumber = layerSelected;

            if (Tiles[Tiles.Count - 1].type == TileType.Scenery)
            {
                Tiles[Tiles.Count - 1].textureData =
                                        new Color[Tiles[Tiles.Count - 1].sprite.Width * Tiles[Tiles.Count - 1].sprite.Height];
                Tiles[Tiles.Count - 1].sprite.GetData(Tiles[Tiles.Count - 1].textureData);
            }
            if (Tiles[Tiles.Count - 1].type == TileType.Scenery && (objectSelected == 15 || objectSelected == 16))
            {

                tileRectangles.Add(new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), 10, 10));
                tileRectangles[tileRectangles.Count - 1] = new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), 10, 10);
            }
            else
            {
                tileRectangles.Add(new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), Tiles[Tiles.Count - 1].sprite.Width, Tiles[Tiles.Count - 1].sprite.Height));
                tileRectangles[tileRectangles.Count - 1] = new Rectangle((int)(Tiles[Tiles.Count - 1].position.X + scrollOffset.X), (int)(Tiles[Tiles.Count - 1].position.Y + scrollOffset.Y), Tiles[Tiles.Count - 1].sprite.Width, Tiles[Tiles.Count - 1].sprite.Height);
            }
        }

        void LoadBackground(BackgroundType type, int objectSelected, int layerSelected)
        {
            if (type == BackgroundType.Normal)
            {
                Backgrounds.Add(new Background(content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\Normal\\NormalBackground" + objectSelected.ToString())));
            }
            if (type == BackgroundType.Other)
            {
                Backgrounds.Add(new Background(content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\Other\\OtherBackground" + objectSelected.ToString())));
            }

            Backgrounds[Backgrounds.Count - 1].position = new Vector2(mousePoint.X, mousePoint.Y) - scrollOffset; 
            Backgrounds[Backgrounds.Count - 1].type = type;
            Backgrounds[Backgrounds.Count - 1].objectNumber = objectSelected;
            Backgrounds[Backgrounds.Count - 1].layerNumber = layerSelected;
            backgroundRectangles.Add(new Rectangle((int)(Backgrounds[Backgrounds.Count - 1].position.X + scrollOffset.X), (int)(Backgrounds[Backgrounds.Count - 1].position.Y + scrollOffset.Y), Backgrounds[Backgrounds.Count - 1].sprite.Width, Backgrounds[Backgrounds.Count - 1].sprite.Height));
            backgroundRectangles[backgroundRectangles.Count - 1] = new Rectangle((int)(Backgrounds[Backgrounds.Count - 1].position.X + scrollOffset.X), (int)(Backgrounds[Backgrounds.Count - 1].position.Y + scrollOffset.Y), Backgrounds[Backgrounds.Count - 1].sprite.Width, Backgrounds[Backgrounds.Count - 1].sprite.Height);
        } 
    }
}
