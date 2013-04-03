using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace JAMGameFinal
{
    
    public abstract class JAMZombieGameEngine : GameScreen
    {

        #region Important Game Objects

        public static Hero[] players;
        public static Zombie[] zombies;
        public static OtherObject[] playerHalos;
        public static ShotgunBullet[] shotgunBullets;
        public static MiniUziBullet[] miniUziBullets;
        public static M4Bullet[] m4Bullets;
        public static G36CBullet[] g36cBullets;

        #endregion

        #region Character and Handicap Parameters
        Character[] playerCharacters = new Character[4];
        public Character[] PlayerCharacters
        {
            get { return playerCharacters; }
            set { playerCharacters = value; }
        }

        Handicap[] playerHandicaps = new Handicap[4];
        public Handicap[] PlayerHandicaps
        {
            get { return playerHandicaps; }
            set { playerHandicaps = value; }
        }
        #endregion

        #region AudioEngine
        AudioEngine audioEngine;
        SoundBank soundBank;
        WaveBank waveBank;
        public AudioEngine AudioEngine
        {
            get { return audioEngine; }
            internal set { audioEngine = value; }
        }
        public SoundBank SoundBank
        {
            get { return soundBank; }
            internal set { soundBank = value; }
        }
        public WaveBank WaveBank
        {
            get { return waveBank; }
            internal set { waveBank = value; }
        }
        #endregion

        #region PlayerVariables
        int numberOfPlayersLeft;
        public int NumberOfPlayersLeft
        {
            get { return numberOfPlayersLeft; }
            internal set { numberOfPlayersLeft = value; }
        }

        #endregion

        #region ZombieVariables

        #region Manipulate
        
        public static float ZombieSpawnProbability = 0.01f;

        #endregion

        #region Don't Manipulate
        
        //may have to make these static but took away to try and save memory...
        int numberOfZombies;
        public int NumberOfZombies
        {
            get { return numberOfZombies; }
            set { numberOfZombies = value; }
        }
        int numberOfZombiesKilled;
        public int NumberOfZombiesKilled
        {
            get { return numberOfZombiesKilled; }
            set {numberOfZombiesKilled = value;}
        }
        #endregion
        #endregion

        #region BulletVariables
        //---------------------------------------------
        public int numberOfshotgunBullets = 5;

        public int numberOfminiUziBullets = 20;

        public int numberOfm4Bullets = 20;

        public int numberOfg36cBullets = 20;
        //---------------------------------------------
        //has to be negative for coding reasons
        public const float bulletSpeed = -20.0f;
        public const float maxFireDistance = 1468.60f;
        #endregion

        #region Misc. Variables

        //carry over sensitivity variable from previous screens
        float[] s = new float[4];

        //checks if the players are alive individually
        public bool?[] playerAlive = new bool?[4];

        //marks a flag that tells if a player has been checked for alive-ness
        //works with previous variable 'alive', above
        public bool[] playerChecked = new bool[4];

        //the timer for sayid's gun
        Timer sayidTimer;
        //boolean for specific shooting rate
        bool sayidTimerActive;
        bool sayidNewTimer = true;

        //the timer for Sir Edward's gun
        Timer juanTimer;
        //boolean for specific shooting rate
        bool juanTimerActive = false;
        bool juanNewTimer = true;
        
        Timer sir_EdwardTimer;

        bool sir_EdwardTimerActive;
        bool sir_EdwardNewTimer = true;

        Timer wilhelmTimer;

        bool wilhelmTimerActive;
        bool wilhelmNewTimer = true;

        Timer juanShootingTimer;

        Timer sir_EdwardShootingTimer;

        Timer sayidShootingTimer;

        Timer wilhelmShootingTimer;
        
        //reference values
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

        //is there a new trigger sensitivity?
        bool newTriggerSensitivity = false;

        //the camera position
        public Vector2 cameraPosition;

        //the random method
        Random random = new Random();

        //the Level Count
        static int levelCount;
        public static int LevelCount
        {
            get { return levelCount; }
            internal set
            { 
                levelCount = value;
                LevelLoading = value;
            }
        }

        //variables to use for now with the new machine gun and shot-gun implementation
        //objects to keep track of which player is using which character
        Hero sayidsPlayer;
        Hero juansPlayer;
        Hero sirEdwardsPlayer;
        Hero wilhelmsPlayer;
        
        //should the timer loop?
        AutoResetEvent autoEvent = new AutoResetEvent(true);
        AutoResetEvent unAutoEvent = new AutoResetEvent(false);

        //checks if the audio has been loaded yet
        bool isAudioLoaded = false;

        public Vector2 scrollOffset;

        #endregion

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            //allign the screen center and scroll offset each frame
            Vector2 screenCenter = new Vector2(ScreenWidth, ScreenHeight) / 2;
            scrollOffset = screenCenter - cameraPosition;

            #region Audio
            if (!isAudioLoaded)
            {
                //loads the audio engine with its wave and sound banks
                AudioEngine = new AudioEngine("Content\\Audio\\JAMAudio.xgs");
                WaveBank = new WaveBank(AudioEngine, "Content\\Audio\\Wave Bank.xwb");
                SoundBank = new SoundBank(AudioEngine, "Content\\Audio\\Sound Bank.xsb");

                //soundBank.PlayCue(string name);

                isAudioLoaded = true;

            }
            #endregion

            //if the game isn't active (paused or such) then do not Update Zombie positions
            //also do not update the camera nor the bullets
            #region Update the Methods Every 'Update'
            if (Active)
            {
                //assign alive values to the players.alive values to use later
                for (int i = 0; i < NumberOfPlayers; i++)
                {
                    playerAlive[i] = players[i].alive;
                }

                //make sure the camera follows only the alive players left
                UpdateCamera(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                
                //spawn zombies with the scroll off set implemented
                SpawnZombies(scrollOffset);
                
                #region Check if Players All Died
                
                if (GetPlayerValuesForDeaths(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]))
                {
                    //its just safe to reset these
                    sayidNewTimer = false;
                    sir_EdwardNewTimer = false;
                    Active = false;

                    //you lose
                    //make the main menu screen open with the different constructor that follows through with saved stats used in
                    //the level just played
                    LoadingScreenLost.Load(ScreenManager, true, null, new BackgroundScreen(),
                                                                      new MainMenuScreen());

                    //fade to black
                    ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

                }
                UncheckPlayers();
                #endregion

            }
            if (levelCount > 1)
            {
                ZombieSpawnProbability *= 0.02f;
            }
            #endregion

            
            if (NumberOfZombiesKilled >= 100)
            {
                Active = false;
                levelCount++;

                LoadingScreen.Load(ScreenManager, true, null, true, new BackgroundScreen(),
                                                                  new MainMenuScreen());

                ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);

        }

        #region MISC Methods

        //Uncheck the players so that they can be checked over for aliveness...
        public void UncheckPlayers()
        {
            for (int x = 0; x < 4; x++)
            {
                playerChecked[x] = false;
            }
        }

        //this returns which player is alive with a number that corresponds with the number they are in the array
        public int GetPlayerValues(bool? player1Alive, bool? player2Alive, bool? player3Alive, bool? player4Alive)
        {
            int playerNumber = 0;

            if (player1Alive == true && !playerChecked[0])
            {
                playerNumber = 0;
                playerChecked[0] = true;
            }
            else if (player2Alive == true && !playerChecked[1])
            {
                playerNumber = 1;
                playerChecked[1] = true;
            }
            else if (player3Alive == true && !playerChecked[2])
            {
                playerNumber = 2;
                playerChecked[2] = true;
            }
            else if (player4Alive == true && !playerChecked[3])
            {
                playerNumber = 3;
                playerChecked[3] = true;
            }
            
            return playerNumber;
            

        }

        public bool GetPlayerValuesForDeaths(bool? player1Alive, bool? player2Alive, bool? player3Alive, bool? player4Alive)
        {
            int playerNumber = -1;
            bool allDead = false;

            if (player1Alive == true && !playerChecked[0])
            {
                playerNumber = 0;
                playerChecked[0] = true;
            }
            else if (player2Alive == true && !playerChecked[1])
            {
                playerNumber = 1;
                playerChecked[1] = true;
            }
            else if (player3Alive == true && !playerChecked[2])
            {
                playerNumber = 2;
                playerChecked[2] = true;
            }
            else if (player4Alive == true && !playerChecked[3])
            {
                playerNumber = 3;
                playerChecked[3] = true;
            }
            if (playerNumber == -1)
            {
                allDead = true;
                return allDead;
            }
            else
            {
                return allDead;
            }

        }

        void SpawnZombies(Vector2 scrollOffset)
        {
            //keep in mind that the area to spawn in is between 0.0 and 3840x and 0.0 and 2160y

            #region Spawn Zombies
            //if a random number between 0 and 1 is less than the probability (and if there are less than the max zombies) then spawn
            
            //spawn with a random position
            for (int i = 0; i < MaxZombies; i++)
            {
                if ((random.NextDouble() < ZombieSpawnProbability) && (NumberOfZombies < MaxZombies) && !zombies[i].alive)
                {
                    
                    float randomNumber = (float)random.NextDouble();

                    //random x value
                    float x = (float)random.NextDouble() *
                        (ScreenManager.GraphicsDevice.Viewport.Width * 3);

                    //random y value
                    float y = (float)random.NextDouble() *
                        (ScreenManager.GraphicsDevice.Viewport.Height * 3);

                    #region First Spawn Point
                    if (randomNumber <= 0.25f && NumberOfZombies < MaxZombies)
                    {
                        zombies[i].position = new Vector2(x, -zombies[i].sprite.Height - 100) + scrollOffset;
                        zombies[i].health = zombies[i].fullHealth;
                        zombies[i].alive = true;
                        NumberOfZombies++;
                        
                        break;
                    }
                    #endregion
                    #region Second Spawn Point
                    if (randomNumber <= 0.50f && randomNumber > 0.25f && NumberOfZombies < MaxZombies)
                    {
                        zombies[i].position = new Vector2(x, 2160 + zombies[i].sprite.Height + 100) + scrollOffset;
                        zombies[i].health = zombies[i].fullHealth;
                        zombies[i].alive = true;
                        NumberOfZombies++;
                        
                        break;
                    }
                    #endregion
                    #region Third Spawn Point
                    if (randomNumber <= 0.75f && randomNumber > 0.50f && NumberOfZombies < MaxZombies)
                    {
                        zombies[i].position = new Vector2(-zombies[i].sprite.Width - 100, y) + scrollOffset;
                        zombies[i].health = zombies[i].fullHealth;
                        zombies[i].alive = true;
                        NumberOfZombies++;
                        
                        break;
                    }
                    #endregion
                    #region Fourth Spawn Point
                    if (randomNumber <= 1.0f && randomNumber > 0.75f && NumberOfZombies < MaxZombies)
                    {
                        zombies[i].position = new Vector2(3840 + 100, y) + scrollOffset;
                        zombies[i].health = zombies[i].fullHealth;
                        zombies[i].alive = true;
                        NumberOfZombies++;
                        
                        break;
                    }
                    #endregion
                }
            }
            #endregion
        }
        #endregion

        //Updates the bullets' direction and speed while also passing through the reference of the player who shot the specific bullet
        //   and also calls for a collison check
        //Updates the camera position while the players move across the screen
        //Updates the zombies' direction and speed while also calling to check for a collison
        #region Update Methods 

        /// <summary>
        /// Updates the camera position, scrolling the
        /// screen if the player gets too close to the edge.
        /// </summary>
        void UpdateCamera(bool? player1Alive, bool? player2Alive, bool? player3Alive, bool? player4Alive)
        {
            // How far away from the camera should we allow the player
            // to move before we scroll the camera to follow it?
            Vector2 maxScroll = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height) / 2;

            // Apply a safe area to prevent the player getting too close to the edge
            // of the screen.
            const float playerSafeArea = 0.5f;

            maxScroll *= playerSafeArea;

            maxScroll -= new Vector2(50, 50);

            // Adjust for the size of the player sprites, so we will start
            // scrolling based on the edge rather than center of the player.
            for (int i = 0; i < numberOfPlayersLeft; i++)
            {
                int j = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);

                // Make sure the camera stays within the desired distance of the player.
                Vector2 min = players[j].position - maxScroll;
                Vector2 max = players[j].position + maxScroll;

                cameraPosition.X = MathHelper.Clamp(cameraPosition.X, min.X, max.X);
                cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, min.Y, max.Y);

                players[j].position.X = MathHelper.Clamp(players[j].position.X, min.X, max.X);
                players[j].position.Y = MathHelper.Clamp(players[j].position.Y, min.Y, max.Y);

                players[j].position.X = MathHelper.Clamp(players[j].position.X, 0.0f, 3840.0f);
                players[j].position.Y = MathHelper.Clamp(players[j].position.Y, 0.0f, 2160.0f);

                if (levelCount == 2)
                {
                    players[j].position.X = MathHelper.Clamp(players[j].position.X, 1720f, 2220f);
                    players[j].position.Y = MathHelper.Clamp(players[j].position.Y, 1720f, 2220f);
                }
            }

            UncheckPlayers();
        }

        /// <summary>
        /// Each zombie should be chasing in this game, so is all that needs to be checked is
        /// where each zombie should be going
        /// This now also checks intersection between the zombie and the player
        /// </summary>
        
        
#endregion

        //Moving and Firing
        #region Input Methods
        /// <summary>
        /// This is what the player pressed while playing the game and how the game should respond to that action
        /// Controlling shooting, moving, and other player inputs
        /// </summary>
        /// <param name="input">what the player has inputted</param>
        public override void HandleInput(InputState input)
        {
            if (IsPauseMenuDone)
            {
                Active = true;
            }
            if (Active)
            {
                if (input == null)
                    throw new ArgumentNullException("input");
                
                //did you disconnect?
                #region IsPlayerControllerDisconnectedOrOutOfBatteries?

                int playerIndex = (int)ControllingPlayer.Value;

                KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
                GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

                bool gamePadDisconnected = !gamePadState.IsConnected &&
                                            input.GamePadWasConnected[playerIndex];

                //...if so then pause and wait
                if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
                {
                    #region Bring up Pause Menu

                    for (int i = 0; i < 4; i++)
                    {
                        if (ControllingPlayer == (PlayerIndex)i)
                        {
                            ScreenManager.AddScreen(new PauseMenuScreen(players[i].triggerSensitivity, levelCount), ControllingPlayer);
                        }
                    }
                    
                    #endregion
                    //assign new trigger sensitivites
                    newTriggerSensitivity = true;
                    //...and we are not 'activly playing'
                    Active = false;
                }

                #endregion
               
                   
                #region New Trigger Sensitivities
                if (newTriggerSensitivity)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (ControllingPlayer == (PlayerIndex)i)
                        {
                            players[i].triggerSensitivity = TriggerSensitivity;
                            break;
                        }
                        
                    }

                    //do not assign again
                    newTriggerSensitivity = false;
                }
                #endregion

                int j = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                int k = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                int l = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                int m = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);

                switch (numberOfPlayersLeft)
                {
                    #region 1
                    case 1:
                        j = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                        //the left stick moves the player's position in the x and y position
                        players[j].position.X += input.CurrentGamePadStates[j].ThumbSticks.Left.X * 5.0f;
                        players[j].position.Y += -input.CurrentGamePadStates[j].ThumbSticks.Left.Y * 5.0f;
                        //the right stick rotates the player in the exact direction of the right stick in referece to the controller
                        //      uses my own TurnAngle method
                        players[j].rotation = TurnAngle(input.CurrentGamePadStates[j].ThumbSticks.Right.X, input.CurrentGamePadStates[j].ThumbSticks.Right.Y, players[j].rotation);
                        UpdateWeapon(input, players[j].playerCharacter ,j);
                        
                        break;
                    #endregion
                    #region 2
                    case 2:
                        j = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                        k = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);

                        players[j].position.X += input.CurrentGamePadStates[j].ThumbSticks.Left.X * 5.0f;
                        players[j].position.Y += -input.CurrentGamePadStates[j].ThumbSticks.Left.Y * 5.0f;
                        players[j].rotation = TurnAngle(input.CurrentGamePadStates[j].ThumbSticks.Right.X, input.CurrentGamePadStates[j].ThumbSticks.Right.Y, players[j].rotation);
                        players[k].position.X += input.CurrentGamePadStates[k].ThumbSticks.Left.X * 5.0f;
                        players[k].position.Y += -input.CurrentGamePadStates[k].ThumbSticks.Left.Y * 5.0f;
                        players[k].rotation = TurnAngle(input.CurrentGamePadStates[k].ThumbSticks.Right.X, input.CurrentGamePadStates[k].ThumbSticks.Right.Y, players[k].rotation);

                        UpdateWeapon(input, players[j].playerCharacter, j);
                        UpdateWeapon(input, players[k].playerCharacter, k);

                        break;
                    #endregion
                    #region 3
                    case 3:
                        j = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                        k = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                        l = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);

                        players[j].position.X += input.CurrentGamePadStates[j].ThumbSticks.Left.X * 5.0f;
                        players[j].position.Y += -input.CurrentGamePadStates[j].ThumbSticks.Left.Y * 5.0f;
                        players[j].rotation = TurnAngle(input.CurrentGamePadStates[j].ThumbSticks.Right.X, input.CurrentGamePadStates[j].ThumbSticks.Right.Y, players[j].rotation);
                        players[k].position.X += input.CurrentGamePadStates[k].ThumbSticks.Left.X * 5.0f;
                        players[k].position.Y += -input.CurrentGamePadStates[k].ThumbSticks.Left.Y * 5.0f;
                        players[k].rotation = TurnAngle(input.CurrentGamePadStates[k].ThumbSticks.Right.X, input.CurrentGamePadStates[k].ThumbSticks.Right.Y, players[k].rotation);
                        players[l].position.X += input.CurrentGamePadStates[l].ThumbSticks.Left.X * 5.0f;
                        players[l].position.Y += -input.CurrentGamePadStates[l].ThumbSticks.Left.Y * 5.0f;
                        players[l].rotation = TurnAngle(input.CurrentGamePadStates[l].ThumbSticks.Right.X, input.CurrentGamePadStates[l].ThumbSticks.Right.Y, players[l].rotation);

                        UpdateWeapon(input, players[j].playerCharacter, j);
                        UpdateWeapon(input, players[k].playerCharacter, k);
                        UpdateWeapon(input, players[l].playerCharacter, l);

                        break;
                    #endregion
                    #region 4
                    case 4:
                        j = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                        k = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                        l = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);
                        m = GetPlayerValues(playerAlive[0], playerAlive[1], playerAlive[2], playerAlive[3]);

                        players[j].position.X += input.CurrentGamePadStates[j].ThumbSticks.Left.X * 5.0f;
                        players[j].position.Y += -input.CurrentGamePadStates[j].ThumbSticks.Left.Y * 5.0f;
                        players[j].rotation = TurnAngle(input.CurrentGamePadStates[j].ThumbSticks.Right.X, input.CurrentGamePadStates[j].ThumbSticks.Right.Y, players[j].rotation);
                        players[k].position.X += input.CurrentGamePadStates[k].ThumbSticks.Left.X * 5.0f;
                        players[k].position.Y += -input.CurrentGamePadStates[k].ThumbSticks.Left.Y * 5.0f;
                        players[k].rotation = TurnAngle(input.CurrentGamePadStates[k].ThumbSticks.Right.X, input.CurrentGamePadStates[k].ThumbSticks.Right.Y, players[k].rotation);
                        players[l].position.X += input.CurrentGamePadStates[l].ThumbSticks.Left.X * 5.0f;
                        players[l].position.Y += -input.CurrentGamePadStates[l].ThumbSticks.Left.Y * 5.0f;
                        players[l].rotation = TurnAngle(input.CurrentGamePadStates[l].ThumbSticks.Right.X, input.CurrentGamePadStates[l].ThumbSticks.Right.Y, players[l].rotation);
                        players[m].position.X += input.CurrentGamePadStates[m].ThumbSticks.Left.X * 5.0f;
                        players[m].position.Y += -input.CurrentGamePadStates[m].ThumbSticks.Left.Y * 5.0f;
                        players[m].rotation = TurnAngle(input.CurrentGamePadStates[m].ThumbSticks.Right.X, input.CurrentGamePadStates[m].ThumbSticks.Right.Y, players[m].rotation);

                        UpdateWeapon(input, players[j].playerCharacter, j);
                        UpdateWeapon(input, players[k].playerCharacter, k);
                        UpdateWeapon(input, players[l].playerCharacter, l);
                        UpdateWeapon(input, players[m].playerCharacter, m);

                        break;
                    #endregion

                }

                UncheckPlayers();
                
            }
        }

        private void SayidFireWeapon(Object stateInfo)
        {

            Hero player = sayidsPlayer;

            foreach (MiniUziBullet bullet in miniUziBullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.rotation = player.rotation;
                    bullet.position = player.position + new Vector2(
                        (float)Math.Cos(player.rotation),
                        (float)Math.Sin(player.rotation))
                        * -player.center;
                    bullet.velocity = new Vector2(
                        (float)Math.Cos(player.rotation),
                        (float)Math.Sin(player.rotation))
                        * bulletSpeed;
                    soundBank.PlayCue("m4-1bullet");

                    return;
                }
            }

            TimerCallback tcb = SayidShootingTimerCallback;

            sayidShootingTimer = new Timer(tcb, unAutoEvent, 330, 330);

        }

        private void SayidShootingTimerCallback(Object stateInfo)
        {
            sayidTimerActive = true;
            sayidShootingTimer.Dispose();
        }

        private void JuanFireWeapon(Object stateInfo)
        {
            Hero player = juansPlayer;

            //solution to biggest problem, HAIL THE SPREAD RADIANS!!!!!!!!!
            float spreadRadians = MathHelper.ToRadians(15.0f);

            //Check for First Bullet Availability
            if (!shotgunBullets[0].alive)
            {
                //make it alive
                shotgunBullets[0].alive = true;
                //Check for Second Bullet Availability
                if (!shotgunBullets[1].alive)
                {
                    //make it alive
                    shotgunBullets[1].alive = true;
                    //Check for Third Bullet Availability
                    if (!shotgunBullets[2].alive)
                    {
                        //make it alive
                        shotgunBullets[2].alive = true;
                        //Check for Fourth Bullet Availability
                        if (!shotgunBullets[3].alive)
                        {
                            //make it alive
                            shotgunBullets[3].alive = true;
                            //Check for Fifth Bullet Availability
                            if (!shotgunBullets[4].alive)
                            {
                                //make it alive
                                shotgunBullets[4].alive = true;
                                //fire!
                                #region Top Bullet
                                shotgunBullets[1].rotation = player.rotation - spreadRadians;
                                shotgunBullets[1].position = player.position + new Vector2(
                                    (float)Math.Cos(player.rotation),
                                    (float)Math.Sin(player.rotation))
                                    * -player.center;
                                shotgunBullets[1].velocity = new Vector2(
                                    (float)Math.Cos(player.rotation - spreadRadians),
                                    (float)Math.Sin(player.rotation - spreadRadians)) * bulletSpeed;
                                #endregion
                                #region MiddleTop Bullet
                                shotgunBullets[3].rotation = player.rotation - spreadRadians / 2;
                                shotgunBullets[3].position = player.position + new Vector2(
                                    (float)Math.Cos(player.rotation),
                                    (float)Math.Sin(player.rotation))
                                    * -player.center;
                                shotgunBullets[3].velocity = new Vector2(
                                    (float)Math.Cos(player.rotation - spreadRadians / 2),
                                    (float)Math.Sin(player.rotation - spreadRadians / 2)) * bulletSpeed;
                                #endregion
                                #region Middle Bullet
                                shotgunBullets[0].rotation = player.rotation;
                                shotgunBullets[0].position = player.position + new Vector2(
                                    (float)Math.Cos(player.rotation),
                                    (float)Math.Sin(player.rotation))
                                    * -player.center;
                                shotgunBullets[0].velocity = new Vector2(
                                    (float)Math.Cos(player.rotation),
                                    (float)Math.Sin(player.rotation)) * bulletSpeed;

                                #endregion
                                #region MiddleBottom Bullet
                                shotgunBullets[4].rotation = player.rotation + spreadRadians / 2;
                                shotgunBullets[4].position = player.position + new Vector2(
                                    (float)Math.Cos(player.rotation),
                                    (float)Math.Sin(player.rotation))
                                    * -player.center;
                                shotgunBullets[4].velocity = new Vector2(
                                    (float)Math.Cos(player.rotation + spreadRadians / 2),
                                    (float)Math.Sin(player.rotation + spreadRadians / 2)) * bulletSpeed;
                                #endregion
                                #region Bottom Bullet
                                shotgunBullets[2].rotation = player.rotation + spreadRadians;
                                shotgunBullets[2].position = player.position + new Vector2(
                                    (float)Math.Cos(player.rotation),
                                    (float)Math.Sin(player.rotation))
                                    * -player.center;
                                shotgunBullets[2].velocity = new Vector2(
                                    (float)Math.Cos(player.rotation + spreadRadians),
                                    (float)Math.Sin(player.rotation + spreadRadians)) * bulletSpeed;
                                #endregion
                                soundBank.PlayCue("m4-1bullet");
                                
                            }
                        }
                    }
                }
            }
            TimerCallback tcb = JuanShootingTimerCallback;

            juanShootingTimer = new Timer(tcb, unAutoEvent, 1000, 1000);

        }

        private void JuanShootingTimerCallback(Object stateInfo)
        {
            juanTimerActive = true;
            juanShootingTimer.Dispose();
        }
   
        private void SirEdwardFireWeapon(Object stateInfo)
        {
            Hero player = sirEdwardsPlayer;

            foreach (M4Bullet bullet in m4Bullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.rotation = player.rotation;
                    bullet.position = player.position + new Vector2(
                        (float)Math.Cos(player.rotation),
                        (float)Math.Sin(player.rotation))
                        * -player.center;
                    bullet.velocity = new Vector2(
                        (float)Math.Cos(player.rotation),
                        (float)Math.Sin(player.rotation))
                        * bulletSpeed;
                    soundBank.PlayCue("m4-1bullet");
                    
                    return;
                }
            }

            TimerCallback tcb = SirEdwardShootingTimerCallback;

            sir_EdwardShootingTimer = new Timer(tcb, unAutoEvent, 500, 500);

        }

        private void SirEdwardShootingTimerCallback(Object stateInfo)
        {
            sir_EdwardTimerActive = true;
            sir_EdwardShootingTimer.Dispose();
        }

        private void WilhelmFireWeapon(Object stateInfo)
        {
            Hero player = wilhelmsPlayer;

            foreach (G36CBullet bullet in g36cBullets)
            {
                if (!bullet.alive)
                {
                    bullet.alive = true;
                    bullet.rotation = player.rotation;
                    bullet.position = player.position + new Vector2(
                        (float)Math.Cos(player.rotation),
                        (float)Math.Sin(player.rotation))
                        * -player.center;
                    bullet.velocity = new Vector2(
                        (float)Math.Cos(player.rotation),
                        (float)Math.Sin(player.rotation))
                        * bulletSpeed;
                    
                    soundBank.PlayCue("p90-1bullet");
                    
                    return;
                }
            }

            TimerCallback tcb = WilhelmShootingTimerCallback;

            wilhelmShootingTimer = new Timer(tcb, unAutoEvent, 500, 500);

        }

        private void WilhelmShootingTimerCallback(Object stateInfo)
        {
            wilhelmTimerActive = true;
            wilhelmShootingTimer.Dispose();
        }

        private void UpdateWeapon(InputState input, Character playerCharacter, int playerNumber)
        {
            switch (playerCharacter)
            {
                //seems fine
                #region Sayid Weapon Timer
                case Character.Sayid:
                    //if pressed down
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right > players[playerNumber].triggerSensitivity)
                    {

                        
                        //if there is allowed for a new timer to be created
                        if (sayidNewTimer && !sayidTimerActive)
                        {
                            
                            TimerCallback tcb = SayidFireWeapon;

                            sayidsPlayer = players[playerNumber];
                             
                            sayidTimer = new Timer(tcb, autoEvent, 0, 330);
                            
                            sayidNewTimer = false;

                            sayidTimerActive = true;
                        }

                    }
                    //if let go
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right <= players[playerNumber].triggerSensitivity && sayidTimerActive)
                    {
                        if (sayidTimer != null)
                        {
                            sayidTimer.Dispose();
                        }
                        //allow for a new timer to be created
                        sayidNewTimer = true;

                        sayidTimerActive = false;
                    }
                    break;
                #endregion
                //seems fine
                #region Juan Weapon Timer
                case Character.Juan:
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right > players[playerNumber].triggerSensitivity)
                    {
                        
                        if (juanNewTimer && !juanTimerActive)
                        {

                            TimerCallback tcb = JuanFireWeapon;

                            juansPlayer = players[playerNumber];

                            juanTimer = new Timer(tcb, autoEvent, 0, 1000);

                            juanNewTimer = false;

                        }
                    }
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right <= players[playerNumber].triggerSensitivity && juanTimerActive)
                    {
                        if (juanTimer != null)
                        {
                            juanTimer.Dispose();
                        }

                        juanNewTimer = true;

                        juanTimerActive = false;
                        
                    }
                    
                    break;
                #endregion
                //seems fine
                #region Sir Edward Weapon Timer
                case Character.Sir_Edward:
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right > players[playerNumber].triggerSensitivity)
                    {
                        
                        if (sir_EdwardNewTimer && !sir_EdwardTimerActive)
                        {

                            TimerCallback tcb = SirEdwardFireWeapon;

                            sirEdwardsPlayer = players[playerNumber];
                            
                            sir_EdwardTimer = new Timer(tcb, autoEvent, 0, 500);

                            sir_EdwardNewTimer = false;

                        }
                    }
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right <= players[playerNumber].triggerSensitivity && sir_EdwardTimerActive)
                    {
                        if (sir_EdwardTimer != null)
                        {
                            sir_EdwardTimer.Dispose();
                        }

                        sir_EdwardNewTimer = true;

                        sir_EdwardTimerActive = false;
                        
                    }
                    break;
                #endregion
                //seems fine
                #region Wilhelm Weapon Timer
                case Character.Wilhelm:
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right > players[playerNumber].triggerSensitivity)
                    {
                        
                        if (wilhelmNewTimer && !wilhelmTimerActive)
                        {
                            
                            TimerCallback tcb = WilhelmFireWeapon;

                            wilhelmsPlayer = players[playerNumber];
                            
                            wilhelmTimer = new Timer(tcb, autoEvent, 0, 500);

                            wilhelmNewTimer = false;

                        }
                    }
                    if (input.CurrentGamePadStates[playerNumber].Triggers.Right <= players[playerNumber].triggerSensitivity && wilhelmTimerActive)
                    {
                        if (wilhelmTimer != null)
                        {
                            wilhelmTimer.Dispose();
                        }

                        wilhelmNewTimer = true;

                        wilhelmTimerActive = false;

                    }
                    break;
                #endregion
            }
        
        }

        #endregion

        //Methods to determine wrap angle and direction of zombie
        //Methods to determine wrap angle and direction of player
        #region Turning Objects Methods

        /// <summary>
        /// my turn angle method to give the player a rotation angle from the right stick's angle in reference to the controller
        /// </summary>
        /// <param name="x">the right stick's x value</param>
        /// <param name="y">the right stick's y value</param>
        /// <param name="previousAngle">the previous angle the stick was before this frame</param>
        /// <returns>radian value for rotation, either the previous angle because nothing changed, 
        ///          or a new value because the stick moved</returns>
        /// basically ingenious!
        private static float TurnAngle(float x, float y, float previousAngle)
        {
            
            float theta = (float)Math.Atan2(y, x);
            if (y == 0 && x == 0)
            {
                return previousAngle;
            }
            else
            {
                //check this
                //weird that this specific unit circle is more like a rose pedal
                if (Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) >= 1)
                {
                    return (float)Math.PI - theta;
                }
                else
                {
                    return previousAngle;
                }
            }
        }

        #endregion
    }
}