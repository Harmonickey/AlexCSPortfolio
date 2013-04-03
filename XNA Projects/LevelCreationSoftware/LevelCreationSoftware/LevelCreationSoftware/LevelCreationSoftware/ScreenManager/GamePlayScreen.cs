#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace LevelCreationSoftware
{

    
    public abstract class GamePlayScreen : GameScreen
    {

        #region Variables, Objects, Fields

        //the content manager
        public ContentManager content;
        //the spritebatch, able to be used by other classes
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        SpriteBatch spriteBatch;

        //audio stuff
        AudioEngine audioEngine;
        SoundBank soundBank;
        WaveBank waveBank;

        InputState refInput = new InputState();


        float refDepthY = 0.0f;
        Rectangle reftileRectangle = Rectangle.Empty;
        
        //the storage device with is saved in memory so we don't have to make another one when saving
        public static StorageDevice storageDevice;
        public static IAsyncResult resultCheck;        //this just returns if you selected the device
        
        //game save flag
        public static bool gameSaveRequested;
        private static bool gameLoadRequested;
        private static bool resultCheckRequested;

        //the player object
        public Player player;
        public Matrix playerTransform;
        public Rectangle playerRectangle;
       
        public Player playerCollisionReference;
        //acceleration values
        public float gravityAcceleration = .1f;
        public const float movementAcceleration = 5;
        public int flyingBarScalar = 0;
        public bool flyingAllowed = true;
        
        //the tile object, and its bounding rectangle
        public Tile[] tiles;
        public Rectangle tileRectangle;
        public Matrix tileTransform;
        public Rectangle debugRectangle;

        public Background[] backgrounds;
        
        public Texture2D collisionTile;
        public Rectangle collisionRectangle;

        //the background texture
        protected Texture2D gameBackgroundTexture;

        //the viewport of the screen
        public Viewport viewport;
        public Rectangle titleSafeArea;
        //vectors to scroll the screen
        public Vector2 cameraPosition;
        public Vector2 screenCenter;
        public Vector2 scrollOffset;
        public Vector2 layerOneScrollOffset;
        public Vector2 layerTwoScrollOffset;
        public Vector2 layerThreeScrollOffset;

        //some values
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public Rectangle hudPosition, hudPosition2;
        public Texture2D barOutline, flyingBar;

        public static int LevelOn = 0;
        private static string LevelName;

        float collisionThreshold = 150.0f;

        #endregion

        #region Load The Content
        /// <summary>
        /// This loads all necessary content that include the usable componenets.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            viewport = ScreenManager.GraphicsDevice.Viewport;
            titleSafeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            screenCenter = new Vector2(viewport.Width, viewport.Height) / 2;

            audioEngine = new AudioEngine("Content\\Audio\\LbKAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            gameBackgroundTexture = content.Load<Texture2D>("Backgrounds\\GameBackgrounds\\gameBackground");

            barOutline = content.Load<Texture2D>("Sprites\\barOutline");
            flyingBar = content.Load<Texture2D>("Sprites\\flyingBar");
            
            collisionTile = content.Load<Texture2D>("Sprites\\boundingRectangle");

            hudPosition = new Rectangle(titleSafeArea.X, titleSafeArea.Height - 100, barOutline.Width, barOutline.Height);
            hudPosition2 = new Rectangle(hudPosition.X, hudPosition.Y + 80, barOutline.Width, barOutline.Height);

            

        }


        #endregion

        #region Update Methods
        /// <summary>
        /// Check for input
        /// </summary>
        /// <param name="input">grabs the values from InputState</param>
        public override void HandleInput(InputState input)
        {
            if (!LevelCreateSession)
            {

                if (input == null)
                    throw new ArgumentNullException("input");

                //did you disconnect? //Switch this to two players if we want both players to be able to pause
                #region IsPlayerControllerDisconnectedOrOutOfBatteries?

                KeyboardState keyboardState = input.CurrentKeyboardStates[(int)PlayerIndex.One];
                GamePadState gamePadState = input.CurrentGamePadStates[(int)PlayerIndex.One];

                bool playerOneGamePadDisconnected = !gamePadState.IsConnected &&
                                            input.GamePadWasConnected[(int)PlayerIndex.One];

                //...if so then pause and wait
                if (input.IsPauseGame(PlayerIndex.One) || playerOneGamePadDisconnected)
                {

                    ScreenManager.AddScreen(new PauseMenuScreen(), PlayerIndex.One);

                }

                #endregion

                #region Move Left and Right

                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Right))
                {
                    player.velocity.X = 3.0f;
                }
                else if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Left))
                {
                    player.velocity.X = -3.0f;
                }
                else
                {
                    player.velocity.X = 0;
                }


                #endregion

                #region Fly/Jump
                //if we press and hold A then velocity is applied, or its just 0
                //if we press and hold A and still flying energy then we fly, or we just float on down due to gravity but the flying energy doesn't replenish unless we've hit something
                if (input.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(Keys.Space) && flyingAllowed)
                {
                    player.velocity.Y = -2;
                    flyingBarScalar = -2;
                    gravityAcceleration = 0;
                }
                else
                {
                    flyingBarScalar = 0;
                    gravityAcceleration = .1f;

                }
                //CheckImaginaryRectangles();
                HandleTileCollision();                
                #endregion

            }

            refInput = input;

        }
        /// <summary>
        /// Call this when you need to save data in-game
        /// </summary>
        public static void SaveGame()
        {
            //set flag to true
            gameSaveRequested = true;

            if (storageDevice != null && storageDevice.IsConnected && LevelCreateSession)
            {
                LbKStorageLevelCreation.SaveGame(storageDevice, GamerOne, true);
                gameSaveRequested = false;
            }
            
        }

        public static void SaveGame(string levelName)
        {
            LevelName = levelName;
            //set flag to true
            gameSaveRequested = true;

            if (storageDevice != null && storageDevice.IsConnected && LevelCreateSession)
            {
                //load up the filenames specifically and hopefully a list will come up to FileNames
                LbKStorageLevelCreation.LoadGame(storageDevice, GamerOne, true);
                //if filenames comes back null due to this being the first time saving
                if (LbKStorageLevelCreation.FileNames == null)
                {
                    LbKStorageLevelCreation.FileNames = new List<string>();
                }
                //then add on a new level name to the list
                LbKStorageLevelCreation.FileNames.Add(levelName);
                //save it to both the lbktiledata to get from later in a list, and to the specific filename so that it can be retreived specifically later
                LbKStorageLevelCreation.SaveGame(storageDevice, GamerOne, false);
                //we do not need to save again
                gameSaveRequested = false;
            }
            if (storageDevice == null)
            {
                resultCheckRequested = true;
                gameLoadRequested = true;
            }

        }

        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (IsActive)
            {
                
                UpdateCamera();

                //the scroll off set of the background position (position of the sprites on screen in relation to the background itself)
                scrollOffset = screenCenter - cameraPosition;
                layerOneScrollOffset = screenCenter - (cameraPosition / 2);
                layerTwoScrollOffset = screenCenter - (cameraPosition / (5/2));
                layerThreeScrollOffset = screenCenter - (cameraPosition / 3);

                if (!LevelCreateSession)
                {
                    //add on the gravity acceleration
                    player.velocity.Y += gravityAcceleration;

                    player.velocity.Y = MathHelper.Clamp(player.velocity.Y, -2, 8);
                    //add the velocity to the position (could add gravity as a new vector but it really doesnt matter)
                    player.position += player.velocity;

                    //then after the new vectors have been applied to position, then check if it's colliding with anything.
                    //HandleTileCollision();

                    DoHudPosition();
                     
                }

            }
            //To save stuff we need to assign the LbkStorage.(value) values to a known value, then call the save method
            //examples below
            #region Game Save Requested

            if (gameSaveRequested)
            {
                //no need to reassign a storage device here because we already have a device from the automatic load at the beginning.
                if (!LevelCreateSession)
                {
                    if (storageDevice != null && storageDevice.IsConnected)
                    {
                        LbKStorage.Position = player.position;
                        LbKStorage.Score = player.score;
                        LbKStorage.Level = LevelOn;

                        LbKStorage.SaveGame(storageDevice, GamerOne);
                    }

                    //reset flag
                    gameSaveRequested = false;
                }
                else
                {
                    if (storageDevice != null && storageDevice.IsConnected)
                    {
                        LbKStorageLevelCreation.SaveGame(storageDevice, GamerOne, true);
                    }

                    gameSaveRequested = false;
                    
                }

            }

            if (gameLoadRequested && resultCheckRequested)
            {
                if (!Guide.IsVisible)
                {
                    resultCheck = StorageDevice.BeginShowSelector(
                        PlayerIndex.One, null, null);

                    resultCheckRequested = false;
                }

            }
            else if (gameLoadRequested && resultCheck.IsCompleted)
            {
                
                storageDevice = StorageDevice.EndShowSelector(resultCheck);
                gameLoadRequested = false;

                if (storageDevice != null && storageDevice.IsConnected && LevelCreateSession)
                {
                    //load up the filenames specifically and hopefully a list will come up to FileNames
                    LbKStorageLevelCreation.LoadGame(storageDevice, GamerOne, true);
                    //if filenames comes back null due to this being the first time saving
                    if (LbKStorageLevelCreation.FileNames == null)
                    {
                        LbKStorageLevelCreation.FileNames = new List<string>();
                    }
                    //then add on a new level name to the list
                    LbKStorageLevelCreation.FileNames.Add(LevelName);
                    //save it to both the lbktiledata to get from later in a list, and to the specific filename so that it can be retreived specifically later
                    LbKStorageLevelCreation.SaveGame(storageDevice, GamerOne, false);
                    //we do not need to save again
                    gameSaveRequested = false;
                }
            }
            #endregion

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
            
        }

        private void DoHudPosition()
        {
            hudPosition.Width += flyingBarScalar;

            if (hudPosition.Width <= 0)
            {
                flyingAllowed = false;
            }
            else
            {
                flyingAllowed = true;
            }

            hudPosition.Width = (int)MathHelper.Clamp((float)hudPosition.Width, 0.0f, (float)barOutline.Width);
        }

        private void HandleTileCollision()
        {
            //the player bounds
            #region PlayerBounds
            playerTransform =
                Matrix.CreateTranslation(new Vector3(-player.center, 0.0f)) *
                Matrix.CreateRotationZ(player.rotation) *
                Matrix.CreateTranslation(new Vector3(player.position + scrollOffset, 0.0f));

            playerRectangle = GameObject.CalculateBoundingRectangle(
                new Rectangle(0, 0, player.sprite.Width, player.sprite.Height),
                playerTransform);
            #endregion
            //iterate through all tiles

            foreach (Tile tile in tiles)
            {
                if (Vector2.Distance(tile.position - tile.center, player.position- player.center) < collisionThreshold)
                {
                    if (tile.type != TileType.Scenery)
                    {
                        //the tile bounds
                        #region TileBounds
                        
                        tileRectangle = new Rectangle((int)(tile.position.X - tile.center.X + scrollOffset.X), (int)(tile.position.Y - tile.center.Y + scrollOffset.Y), tile.sprite.Width, tile.sprite.Height);
                        #endregion

                        if (player.Collision(playerRectangle, tileRectangle))
                        {
                            ResolveCollisions(tile, player);

                        }
                        else
                        {
                            ResolveCollisionErrors(tile, player);

                        }
                    }
                }
            }
        }

        #endregion

        #region Collisions

        private void ResolveCollisions(Tile tile, Player player)
        {
            if (tile.type == TileType.Slope)
            {

                tileTransform =
                        Matrix.CreateTranslation(new Vector3(-tile.center, 0.0f)) *
                        Matrix.CreateRotationZ(tile.rotation) *
                        Matrix.CreateTranslation(new Vector3(tile.position + scrollOffset, 0.0f));

                tileRectangle = GameObject.CalculateBoundingRectangle(
                    new Rectangle(0, 0, tile.sprite.Width, tile.sprite.Height),
                    tileTransform);

                if (GameObject.IntersectPixels(playerTransform, player.sprite.Width, player.sprite.Height, playerCollisionReference.textureData,
                                               tileTransform, tile.sprite.Width, tile.sprite.Height, tile.textureData))
                {
                    player.velocity.Y = 0;

                    if (player.velocity.X > 0)
                    {
                        player.position.Y -= player.velocity.X;
                    }
                    else if (player.velocity.X < 0 && (playerRectangle.Right) <= (tileRectangle.Right))
                    {
                        player.position.Y += player.velocity.X;
                    }
                }

            }
            else
            {
                

                //get the signed depth of the intersection to account for the correction vector
                Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerRectangle, tileRectangle);

                //if the depth isn't zero...probably just a redundant check because it is assumed that the depth is not zero due to the check above on collision
                if (depth != Vector2.Zero)
                {
                    //get the absolute values to check if its a side-on collision or a bottom-top collision
                    float absDepthX = Math.Abs(depth.X);
                    float absDepthY = Math.Abs(depth.Y);

                    // Resolve the collision along the y-axis.
                    if (absDepthY < absDepthX)
                    {
                        //no matter what the velocity should go to 0 when y-axis colliding
                        player.velocity.Y = 0;
                        //CheckImaginaryRectangles();
                        if (depth.Y < 0)
                        {
                            gravityAcceleration = 0;
                            refDepthY = depth.Y;
                            reftileRectangle = tileRectangle;
                            CheckImaginaryRectangles();
                        }
                        
                        //simply give it a new vector with its X staying the same but correcting the Y position the exact amount it collided inwards with the other rectangle
                        player.position = new Vector2(player.position.X, player.position.Y + depth.Y);
                        //reassign the rectangle a value (probably redundant since it checks again when the method is called
                        playerRectangle = new Rectangle((int)(player.position.X - player.center.X + scrollOffset.X), (int)(player.position.Y - player.center.Y + scrollOffset.Y), player.sprite.Width, player.sprite.Height);

                    }
                    // or Resolve the collision along the x-axis
                    else
                    {
                        //on x-axis collision the player.velocity.Y needs to be the same so he doesn't "stick" to the wall(s)

                        //same here but in the side-wards direction
                        player.position = new Vector2(player.position.X + depth.X, player.position.Y);

                        //reassign the rectangle a value (probably redundant since it checks again when the method is called
                        playerRectangle = new Rectangle((int)(player.position.X - player.center.X + scrollOffset.X), (int)(player.position.Y - player.center.Y + scrollOffset.Y), player.sprite.Width, player.sprite.Height);

                    }

                }


            }
            
        }

        private void CheckImaginaryRectangles()
        {
            
            Rectangle rectA = new Rectangle((int)playerRectangle.X, (int)(playerRectangle.Y - refDepthY), playerRectangle.Width, playerRectangle.Height);

            if (rectA.Intersects(reftileRectangle))
            {
                if (refInput.CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyUp(Keys.Space))
                    flyingBarScalar = 5;
            }
            
        }

        private void ResolveCollisionErrors(Tile tile, Player player)
        {
            if (player.velocity.X < 0)
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    if (tiles[i].type == TileType.Slope)
                    {
                        tileRectangle = new Rectangle((int)(tiles[i].position.X + scrollOffset.X), (int)(tiles[i].position.Y + scrollOffset.Y), tiles[i].sprite.Width, tiles[i].sprite.Height);
                        //where would we be five frames in the future
                        player.position.Y += 10;
                        playerRectangle = new Rectangle((int)(player.position.X - player.center.X + scrollOffset.X), (int)(player.position.Y - player.center.Y + scrollOffset.Y), player.sprite.Width, player.sprite.Height);

                        if (player.Collision(playerRectangle, tileRectangle))
                        {

                            player.position.Y -= 10;
                            gravityAcceleration = .1f;
                           
                            tileRectangle = new Rectangle((int)(tile.position.X + scrollOffset.X), (int)(tile.position.Y + scrollOffset.Y), tile.sprite.Width, tile.sprite.Height);
                            playerRectangle = new Rectangle((int)(player.position.X - player.center.X + scrollOffset.X), (int)(player.position.Y - player.center.Y + scrollOffset.Y), player.sprite.Width, player.sprite.Height);

                        }
                        else
                        {
                            player.position.Y -= 10;
                            
                            tileRectangle = new Rectangle((int)(tile.position.X + scrollOffset.X), (int)(tile.position.Y + scrollOffset.Y), tile.sprite.Width, tile.sprite.Height);
                            playerRectangle = new Rectangle((int)(player.position.X - player.center.X + scrollOffset.X), (int)(player.position.Y - player.center.Y + scrollOffset.Y), player.sprite.Width, player.sprite.Height);

                        }
                    }
                }
            }
        }

        #endregion

        #region Camera Movement
        /// <summary>
        /// Updates the camera position, scrolling the
        /// screen if the player gets too close to the edge.
        /// </summary>
        void UpdateCamera()
        {
            
            if (!LevelCreateSession)
            {
                // How far away from the camera should we allow the player
                // to move before we scroll the camera to follow it?
                Vector2 maxScroll = new Vector2(ScreenWidth, ScreenHeight) / 2;

                // Apply a safe area to prevent the player getting too close to the edge
                // of the screen.
                const float playerSafeArea = 0.5f;

                maxScroll *= playerSafeArea;

                maxScroll -= new Vector2(100, 100);

                // Adjust for the size of the player sprites, so we will start
                // scrolling based on the edge rather than center of the player.

                // Make sure the camera stays within the desired distance of the player.
                Vector2 min = player.position - maxScroll;
                Vector2 max = player.position + maxScroll;

                cameraPosition.X = MathHelper.Clamp(cameraPosition.X, min.X, max.X);
                cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, min.Y, max.Y);

                //these are the screen boundaries so we can still see the character and move the camera
                player.position.X = MathHelper.Clamp(player.position.X, min.X, max.X);
                player.position.Y = MathHelper.Clamp(player.position.Y, min.Y, max.Y);

                //this can be set to do the overall boundaries.
                //players[i].position.X = MathHelper.Clamp(players[i].position.X, 0.0f, 3840.0f);
                //players[i].position.Y = MathHelper.Clamp(players[i].position.Y, 0.0f, 2160.0f);
            }
            
        }

        #endregion

        #region Draw Methods
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            if (!LevelCreateSession)
            {
                DrawHud();

                SpriteBatch.Draw(barOutline, new Rectangle((int)playerRectangle.X, (int)(playerRectangle.Y - refDepthY), playerRectangle.Width, playerRectangle.Height), Color.White);
                SpriteBatch.Draw(barOutline, reftileRectangle, Color.White);
            }
            
            SpriteBatch.End();
        }

        void DrawHud()
        {
            SpriteBatch.Draw(flyingBar,
                hudPosition,
                Color.White);
            SpriteBatch.Draw(barOutline,
                hudPosition,
                Color.White);
            SpriteBatch.Draw(barOutline,
                hudPosition2,
                Color.White);
            
        }

        #endregion
    }
}
