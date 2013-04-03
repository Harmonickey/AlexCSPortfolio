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

namespace LbKStudiosGame
{

    
    public abstract class GamePlayScreen : GameScreen
    {
        
        #region Variables, Objects, Fields

        //the content manager
        public ContentManager content;

        //the storage device with is saved in memory so we don't have to make another one when saving
        public static StorageDevice storageDevice;
        public IAsyncResult resultCheck;        //this just returns if you selected the device

        //the spritebatch, able to be used by other classes
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        SpriteBatch spriteBatch;

        //game save flag
        bool gameSaveRequested = false;
        
        //audio stuff
        AudioEngine audioEngine;
        SoundBank soundBank;
        WaveBank waveBank;
        
        //the player object
        public Player player;

        //the tile object, and its bounding rectangle
        public Tile[] tiles;
        Rectangle tileRectangle;

        //the background texture
        protected Texture2D gameBackgroundTexture;

        //the viewport of the screen
        public Viewport viewport;

        //vectors to scroll the screen
        public Vector2 cameraPosition;
        public Vector2 screenCenter;
        public Vector2 scrollOffset;
        
        //our gamer signed in, able to be used by other classes
        public SignedInGamer GamerOne
        {
            get { return gamerOne; }
            set { gamerOne = value; }
        }

        SignedInGamer gamerOne;

        //acceleration values
        public const float gravityAcceleration = 2.0f;
        public const float movementAcceleration = 5.0f;
        
        //some values
        public int ScreenWidth = 1980;
        public int ScreenHeight = 1080;

        public static  int LevelOn = 0;

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

            screenCenter = new Vector2(ScreenWidth / 2, ScreenHeight / 2);

            audioEngine = new AudioEngine("Content\\Audio\\LbKAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            gameBackgroundTexture = content.Load<Texture2D>("Backgrounds\\background");

        }

        #endregion

        #region Update Methods
        /// <summary>
        /// Check for input
        /// </summary>
        /// <param name="input">grabs the values from InputState</param>
        public override void HandleInput(InputState input)
        {

            if (input == null)
                throw new ArgumentNullException("input");

            //did you disconnect? //Switch this to two players if we want both players to be able to pause
            #region IsPlayerControllerDisconnectedOrOutOfBatteries?

            KeyboardState keyboardState = input.CurrentKeyboardStates[(int)gamerOne.PlayerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[(int)gamerOne.PlayerIndex];

            bool playerOneGamePadDisconnected = !gamePadState.IsConnected &&
                                        input.GamePadWasConnected[(int)gamerOne.PlayerIndex];

            //...if so then pause and wait
            if (input.IsPauseGame(gamerOne.PlayerIndex) || playerOneGamePadDisconnected)
            {
                
                ScreenManager.AddScreen(new PauseMenuScreen(), gamerOne.PlayerIndex);

            }
            
            #endregion

            //the left stick and some movement accerlation are used to make the player move left and right
            player.velocity.X = input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].ThumbSticks.Left.X * movementAcceleration;
            
            //if we press and hold A then velocity is applied, or its just 0
            if (input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].Buttons.A == ButtonState.Pressed)
            {
                player.velocity.Y = -5.0f;
            }
            else
            {
                player.velocity.Y = 0.0f;
            }
            //this happens if we press and let go of the Y button
            if (input.CurrentGamePadStates[(int)gamerOne.PlayerIndex].Buttons.Y == ButtonState.Pressed && input.PreviousGamePadStates[(int)gamerOne.PlayerIndex].Buttons.Y == ButtonState.Released)
            {
                
            }

        }
        /// <summary>
        /// Call this when you need to save data in-game
        /// </summary>
        private void SaveGame()
        {
            //set flag to true
            gameSaveRequested = true;
            
        }

        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (IsActive)
            {
                //the scroll off set of the background position (position of the sprites on screen in relation to the background itself)
                scrollOffset = screenCenter - cameraPosition;

                //for some reason the tiles float off when I update the camera, something to do with the player.position messing with the scrollOffset variable
                //UpdateCamera();

                //add on the gravity acceleration
                player.position.Y += gravityAcceleration;

                //add the velocity to the position (could add gravity as a new vector but it really doesnt matter)
                player.position += player.velocity;  // + new Vector2(0, gravityAcceleration);

                //then after the new vectors have been applied to position, then check if it's colliding with anything.
                HandleTileCollision();
            }
            //To save stuff we need to assign the LbkStorage.(value) values to a known value, then call the save method
            //examples below
            #region Game Save Requested

            if (gameSaveRequested)
            {
                //no need to reassign a storage device here because we already have a device from the automatic load at the beginning.

                if (storageDevice != null && storageDevice.IsConnected)
                {
                    LbKStorage.Position = player.position;
                    LbKStorage.Score = player.score;
                    LbKStorage.Level = LevelOn;

                    LbKStorage.SaveGame(storageDevice, gamerOne);
                }

                //reset flag
                gameSaveRequested = false;

            }
            #endregion

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
            
        }

        private void HandleTileCollision()
        {
            //the player bounds
            Rectangle playerRectangle = new Rectangle((int)(player.position.X), (int)(player.position.Y), player.sprite.Width, player.sprite.Height);

            //iterate through all tiles
            for (int i = 0; i < tiles.Length; i++)
            {
                //the tile bounds
                tileRectangle = new Rectangle((int)(tiles[i].position.X), (int)(tiles[i].position.Y), tiles[i].sprite.Width, tiles[i].sprite.Height);

                //only check if it's close  && only check if it has collided with a tile
                if ((Vector2.Distance(player.position + player.center, tiles[i].position + tiles[i].center) < player.sprite.Width * 2) && player.Collision(playerRectangle, tileRectangle))
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
                            //simply give it a new vector with its X staying the same but correcting the Y position the exact amount it collided inwards with the other rectangle
                            player.position = new Vector2(player.position.X, player.position.Y + depth.Y);
                            //reassign the rectangle a value (probably redundant since it checks again when the method is called
                            playerRectangle = new Rectangle((int)(player.position.X), (int)(player.position.Y), player.sprite.Width, player.sprite.Height);

                        }
                        // or Resolve the collision along the x-axis
                        else
                        {
                            //same here but in the side-wards direction
                            player.position = new Vector2(player.position.X + depth.X, player.position.Y);
                            //reassign the rectangle a value (probably redundant since it checks again when the method is called
                            playerRectangle = new Rectangle((int)(player.position.X), (int)(player.position.Y), player.sprite.Width, player.sprite.Height);
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

        #endregion

        //This just includes the Turn Angle Method
        #region Extra Private Methods
        /// <summary>
        /// This is my own turn angle method but really it would be useful if we actually wanted to rotate
        /// </summary>
        /// <param name="x">some x parameter given by a thumbstick</param>
        /// <param name="y">some y parameter given by a thumbstick</param>
        /// <param name="previousAngle">the possible return value that comes when nothing is applied to y or x</param>
        /// <returns></returns>
        private float TurnAngle(float x, float y, float previousAngle)
        {
            //the angle of rotation
            float theta = (float)Math.Atan2(y, x);
            //if there is no rotation at all then we just return the previous angle it already was
            if (y == 0 && x == 0)
            {
                return previousAngle;
            }
            else
            {
                //check this
                //weird that this specific unit circle is more like a rose pedal
                //find distance to the edge of the thumbstick parameter, if its all the way to the edge, then apply the angle to turn
                if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) >= 1)
                {
                    return (float)Math.PI - theta;
                }
                //or just return the previous angle, to prevent touchy movements
                else
                {
                    return previousAngle;
                }
            }
        }

        #endregion

    }
}
