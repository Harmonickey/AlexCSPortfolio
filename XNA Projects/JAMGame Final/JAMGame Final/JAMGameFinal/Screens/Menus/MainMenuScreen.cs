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
    #region MenuOptionEnumerations
    public enum Character
    {
        Juan,
        Wilhelm,
        Sayid,
        Sir_Edward,
        Null
    }

    public enum Handicap
    {
        Easy,
        Medium,
        Hard,
        VeryHard
    }

    public enum Joined
    {
        NotJoined,
        Joined
    }

    public enum Ready
    {
        ReadyUp,
        Ready
    }

    #endregion

    class MainMenuScreen : MenuScreen
    {
        
        #region MenuEntries
        MenuEntry playerMenuEntryP1;
        MenuEntry playerMenuEntryP2;
        MenuEntry playerMenuEntryP3;
        MenuEntry playerMenuEntryP4;

        static Character playerP1 = Character.Null;
        static Character playerP2 = Character.Null;
        static Character playerP3 = Character.Null;
        static Character playerP4 = Character.Null;

        MenuEntry handicapMenuEntryP1;
        MenuEntry handicapMenuEntryP2;
        MenuEntry handicapMenuEntryP3;
        MenuEntry handicapMenuEntryP4;

        static Handicap handicapP1 = Handicap.Easy;
        static Handicap handicapP2 = Handicap.Easy;
        static Handicap handicapP3 = Handicap.Easy;
        static Handicap handicapP4 = Handicap.Easy;

        MenuEntry joinGameMenuEntryP1;
        MenuEntry joinGameMenuEntryP2;
        MenuEntry joinGameMenuEntryP3;
        MenuEntry joinGameMenuEntryP4;

        static Joined joinedP1 = Joined.NotJoined;
        static Joined joinedP2 = Joined.NotJoined;
        static Joined joinedP3 = Joined.NotJoined;
        static Joined joinedP4 = Joined.NotJoined;

        MenuEntry readyGameMenuEntryP1;
        MenuEntry readyGameMenuEntryP2;
        MenuEntry readyGameMenuEntryP3;
        MenuEntry readyGameMenuEntryP4;

        static Ready readyP1 = Ready.ReadyUp;
        static Ready readyP2 = Ready.ReadyUp;
        static Ready readyP3 = Ready.ReadyUp;
        static Ready readyP4 = Ready.ReadyUp;



        #endregion

        //Content and SpriteBatch
        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont font;

        //Audio stuff
        AudioEngine audioEngine;
        SoundBank soundBank;
        WaveBank waveBank;

        //Video stuff
        Video[] videos;
        VideoPlayer player;
        Texture2D videoTexture;

        //Player Textures
        Texture2D Sayid;
        Texture2D Sir_Edward;
        Texture2D Wilhelm;
        Texture2D Juan;

        //Character Picture Positions
        Vector2 texturePosition = new Vector2(375, 100);        //Player1 character picture position
        Vector2 texturePosition2 = new Vector2(1025, 100);      //Player2 character picture position
        Vector2 texturePosition3 = new Vector2(375, 450);       //Player3 character picture position 
        Vector2 texturePosition4 = new Vector2(1025, 450);      //Player4 character picture position

        //you'll be able to modify this in the UpdateSensitivity method
        Rectangle[] sourceRectangleSen;              //Sensitivity Bar Rectangle from sprite sheet

        //you'll be able to modify this in the Load Content Method
        Rectangle sourceRectangleLeft;               //Left Bunker Rectangle from sprite sheet
        Rectangle sourceRectangleRight;              //Right Bunker Rectangle from sprite sheet

        //a few misc. game objects being used in the menu screen
        OtherObject[] sensitivityDisplays;          //The actual sensitivity Heads Up Display
        OtherObject[] leftBunkers;                  //The actual left bunker object
        OtherObject[] rightBunkers;                 //The actual right bunker object

        Rectangle safeBounds;

        const float SafeAreaPortion = 0.05f;

        #region XboxGuideButtonTextures & Booleans
        //the xbox guide button without colors
        Texture2D center;
        //player 1's individual textures and tracking variables (see further down)
        Texture2D p1NotJoined;        //texture for being not joined yet (red color)
        Texture2D p1Ready;            //texture for being ready (green color)
        Texture2D p1JoinedNotReady;   //texture for being joined but not ready (yellow color)

        bool p1NJ = true;        //Not Joined
        bool p1R = false;        //Ready
        bool p1JNR = false;      //Joined but Not Ready

        #region Other Players Idividual textures and tracking variables
        Texture2D p2NotJoined;
        Texture2D p2Ready;
        Texture2D p2JoinedNotReady;

        bool p2NJ = true;
        bool p2R = false;
        bool p2JNR = false;

        Texture2D p3NotJoined;
        Texture2D p3Ready;
        Texture2D p3JoinedNotReady;

        bool p3NJ = true;
        bool p3R = false;
        bool p3JNR = false;

        Texture2D p4NotJoined;
        Texture2D p4Ready;
        Texture2D p4JoinedNotReady;

        bool p4NJ = true;
        bool p4R = false;
        bool p4JNR = false;
        #endregion

        #endregion

        #region Variables

        int textureLength;           //the xbox guide button texture length
        bool play = false;           //is game ready to play yet?
        int counter = 0;             //prevents, when counter is 1, from anymore videos to be played, and then the next screen has to load
        int videoCount = 1;          //how many videos we have total
        int numberOfPlayers = 1;     //sets the number of players playing
        bool active = true;          //another screen.active component to keep track of pausing or such
        float left;                  //the .left component of a position  (change in the load content method)
        float top;                   //the .top compontent of a position  (change in the load content method)

        //value is from 0 to 0.99; 0 being very sensitive
        float[] playerTriggerSensitivitys;

        //the number of players that have joined but aren't ready yet
        int numberOfPlayersJoined = 0;
        int numberOfPlayersReady = 0;
        #endregion

        public MainMenuScreen(int startingLevel)
            : base("J|A|M Studios  " + "Level: " + startingLevel)
        {

            #region GameMenuEntries
            playerMenuEntryP1 = new MenuEntry(string.Empty);
            playerMenuEntryP2 = new MenuEntry(string.Empty);
            playerMenuEntryP3 = new MenuEntry(string.Empty);
            playerMenuEntryP4 = new MenuEntry(string.Empty);

            handicapMenuEntryP1 = new MenuEntry(string.Empty);
            handicapMenuEntryP2 = new MenuEntry(string.Empty);
            handicapMenuEntryP3 = new MenuEntry(string.Empty);
            handicapMenuEntryP4 = new MenuEntry(string.Empty);

            joinGameMenuEntryP1 = new MenuEntry(string.Empty);
            joinGameMenuEntryP2 = new MenuEntry(string.Empty);
            joinGameMenuEntryP3 = new MenuEntry(string.Empty);
            joinGameMenuEntryP4 = new MenuEntry(string.Empty);

            readyGameMenuEntryP1 = new MenuEntry(string.Empty);
            readyGameMenuEntryP2 = new MenuEntry(string.Empty);
            readyGameMenuEntryP3 = new MenuEntry(string.Empty);
            readyGameMenuEntryP4 = new MenuEntry(string.Empty);


            #endregion

            #region GameMenuEntries Selected?

            playerMenuEntryP1.Selected += PlayerMenuEntrySelected;
            playerMenuEntryP2.Selected += PlayerMenuEntrySelected;
            playerMenuEntryP3.Selected += PlayerMenuEntrySelected;
            playerMenuEntryP4.Selected += PlayerMenuEntrySelected;

            handicapMenuEntryP1.Selected += HandicapMenuEntrySelected;
            handicapMenuEntryP2.Selected += HandicapMenuEntrySelected;
            handicapMenuEntryP3.Selected += HandicapMenuEntrySelected;
            handicapMenuEntryP4.Selected += HandicapMenuEntrySelected;

            joinGameMenuEntryP1.Selected += JoinGameMenuEntrySelected;
            joinGameMenuEntryP2.Selected += JoinGameMenuEntrySelected;
            joinGameMenuEntryP3.Selected += JoinGameMenuEntrySelected;
            joinGameMenuEntryP4.Selected += JoinGameMenuEntrySelected;

            readyGameMenuEntryP1.Selected += ReadyGameMenuEntrySelected;
            readyGameMenuEntryP2.Selected += ReadyGameMenuEntrySelected;
            readyGameMenuEntryP3.Selected += ReadyGameMenuEntrySelected;
            readyGameMenuEntryP4.Selected += ReadyGameMenuEntrySelected;


            #endregion

            #region GameMenuEntries AddedToList

            MenuEntries.Add(joinGameMenuEntryP1);
            MenuEntries2.Add(joinGameMenuEntryP2);
            MenuEntries3.Add(joinGameMenuEntryP3);
            MenuEntries4.Add(joinGameMenuEntryP4);

            MenuEntries.Add(playerMenuEntryP1);
            MenuEntries2.Add(playerMenuEntryP2);
            MenuEntries3.Add(playerMenuEntryP3);
            MenuEntries4.Add(playerMenuEntryP4);

            MenuEntries.Add(handicapMenuEntryP1);
            MenuEntries2.Add(handicapMenuEntryP2);
            MenuEntries3.Add(handicapMenuEntryP3);
            MenuEntries4.Add(handicapMenuEntryP4);

            MenuEntries.Add(readyGameMenuEntryP1);
            MenuEntries2.Add(readyGameMenuEntryP2);
            MenuEntries3.Add(readyGameMenuEntryP3);
            MenuEntries4.Add(readyGameMenuEntryP4);

            #endregion

        }

        public MainMenuScreen()
            : base("J|A|M Studios  " + "Level:  " + LevelLoading)
        {
            #region GameMenuEntries
            playerMenuEntryP1 = new MenuEntry(string.Empty);
            playerMenuEntryP2 = new MenuEntry(string.Empty);
            playerMenuEntryP3 = new MenuEntry(string.Empty);
            playerMenuEntryP4 = new MenuEntry(string.Empty);

            handicapMenuEntryP1 = new MenuEntry(string.Empty);
            handicapMenuEntryP2 = new MenuEntry(string.Empty);
            handicapMenuEntryP3 = new MenuEntry(string.Empty);
            handicapMenuEntryP4 = new MenuEntry(string.Empty);

            joinGameMenuEntryP1 = new MenuEntry(string.Empty);
            joinGameMenuEntryP2 = new MenuEntry(string.Empty);
            joinGameMenuEntryP3 = new MenuEntry(string.Empty);
            joinGameMenuEntryP4 = new MenuEntry(string.Empty);

            readyGameMenuEntryP1 = new MenuEntry(string.Empty);
            readyGameMenuEntryP2 = new MenuEntry(string.Empty);
            readyGameMenuEntryP3 = new MenuEntry(string.Empty);
            readyGameMenuEntryP4 = new MenuEntry(string.Empty);


            #endregion

            #region GameMenuEntries Selected?

            playerMenuEntryP1.Selected += PlayerMenuEntrySelected;
            playerMenuEntryP2.Selected += PlayerMenuEntrySelected;
            playerMenuEntryP3.Selected += PlayerMenuEntrySelected;
            playerMenuEntryP4.Selected += PlayerMenuEntrySelected;

            handicapMenuEntryP1.Selected += HandicapMenuEntrySelected;
            handicapMenuEntryP2.Selected += HandicapMenuEntrySelected;
            handicapMenuEntryP3.Selected += HandicapMenuEntrySelected;
            handicapMenuEntryP4.Selected += HandicapMenuEntrySelected;

            joinGameMenuEntryP1.Selected += JoinGameMenuEntrySelected;
            joinGameMenuEntryP2.Selected += JoinGameMenuEntrySelected;
            joinGameMenuEntryP3.Selected += JoinGameMenuEntrySelected;
            joinGameMenuEntryP4.Selected += JoinGameMenuEntrySelected;

            readyGameMenuEntryP1.Selected += ReadyGameMenuEntrySelected;
            readyGameMenuEntryP2.Selected += ReadyGameMenuEntrySelected;
            readyGameMenuEntryP3.Selected += ReadyGameMenuEntrySelected;
            readyGameMenuEntryP4.Selected += ReadyGameMenuEntrySelected;


            #endregion

            #region GameMenuEntries AddedToList

            MenuEntries.Add(joinGameMenuEntryP1);
            MenuEntries2.Add(joinGameMenuEntryP2);
            MenuEntries3.Add(joinGameMenuEntryP3);
            MenuEntries4.Add(joinGameMenuEntryP4);

            MenuEntries.Add(playerMenuEntryP1);
            MenuEntries2.Add(playerMenuEntryP2);
            MenuEntries3.Add(playerMenuEntryP3);
            MenuEntries4.Add(playerMenuEntryP4);

            MenuEntries.Add(handicapMenuEntryP1);
            MenuEntries2.Add(handicapMenuEntryP2);
            MenuEntries3.Add(handicapMenuEntryP3);
            MenuEntries4.Add(handicapMenuEntryP4);

            MenuEntries.Add(readyGameMenuEntryP1);
            MenuEntries2.Add(readyGameMenuEntryP2);
            MenuEntries3.Add(readyGameMenuEntryP3);
            MenuEntries4.Add(readyGameMenuEntryP4);

            #endregion

            numberOfPlayersJoined = NumberOfPlayers;

            #region Reset Some Booleans
            if (numberOfPlayersJoined == 1)
            {
                p1NJ = false;     
                p1R = false;       
                p1JNR = true;      
            }
            if (numberOfPlayersJoined == 2)
            {
                p1NJ = false;
                p1R = false;
                p1JNR = true;
                p2NJ = false;
                p2R = false;
                p2JNR = true;
            }
            if (numberOfPlayersJoined == 3)
            {
                p1NJ = false;
                p1R = false;
                p1JNR = true;
                p2NJ = false;
                p2R = false;
                p2JNR = true;
                p3NJ = false;
                p3R = false;
                p3JNR = true;
            }
            if (numberOfPlayersJoined == 4)
            {
                p1NJ = false;
                p1R = false;
                p1JNR = true;
                p2NJ = false;
                p2R = false;
                p2JNR = true;
                p3NJ = false;
                p3R = false;
                p3JNR = true;
                p4NJ = false;
                p4R = false;
                p4JNR = true;
            }
            #endregion

            SetMenuEntryText();
        }

        public override void LoadContent()
        {
            Rectangle viewport = ScreenManager.Game.Window.ClientBounds;

            //loads the new spritebatch
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            //loads the video array
            videos = new Video[videoCount];

            safeBounds = new Rectangle(
                (int)(viewport.Width * SafeAreaPortion),
                (int)(viewport.Height * SafeAreaPortion),
                (int)(viewport.Width * (1 - 2 * SafeAreaPortion)),
                (int)(viewport.Height * (1 - 2 * SafeAreaPortion)));

            //loads all arrays
            sensitivityDisplays = new OtherObject[4];
            leftBunkers = new OtherObject[4];
            rightBunkers = new OtherObject[4];
            playerTriggerSensitivitys = new float[4];
            sourceRectangleSen = new Rectangle[4];

            //loads a screenmanager font
            font = ScreenManager.Font;

            //loads the starting up sensitivity source rectangle (the first image on the sprite sheet)
            for (int i = 0; i < 4; i++)
            {
                sourceRectangleSen[i] = new Rectangle(0, 0, 250, 100);
            }
            //loads the starting up left bunker source rectangle from the sprite sheet
            //loads the starting up right bunker source rectangle from the sprite sheet
            sourceRectangleLeft = new Rectangle(658, 3, 90, 75);
            sourceRectangleRight = new Rectangle(488, 3, 90, 75);

            #region LoadXboxControllerTextures
            center = content.Load<Texture2D>("Xboxbutton\\xbox controller-center");
            textureLength = center.Width;
            //the .left component of the picture for the xbox guide button
            left = (ScreenManager.GraphicsDevice.Viewport.Width / 2) - (textureLength / 2);
            //the .top component of the picture for the xbox guide button
            top = (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (textureLength / 2) - 10;

            p1NotJoined = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP1-notjoined");
            p1Ready = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP1-ready to play");
            p1JoinedNotReady = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP1-joinedbutnotready");

            p2NotJoined = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP2-notjoined");
            p2Ready = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP2-ready to play");
            p2JoinedNotReady = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP2-joinedbutnotready");

            p3NotJoined = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP3-notjoined");
            p3Ready = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP3-ready to play");
            p3JoinedNotReady = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP3-joinedbutnotready");

            p4NotJoined = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP4-notjoined");
            p4Ready = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP4-ready to play");
            p4JoinedNotReady = content.Load<Texture2D>("Xboxbutton\\xboxcontrollerP4-joinedbutnotready");
            #endregion

            #region Load Misc.
            //videos loaded into game
            videos[0] = content.Load<Video>("Video\\54321");
            player = new VideoPlayer();

            //audio engine in game
            audioEngine = new AudioEngine("Content\\Audio\\JAMAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");

            //loads the character textures
            Sayid = content.Load<Texture2D>("Sprites\\Players\\Sayid\\Images\\Sayid");
            Sir_Edward = content.Load<Texture2D>("Sprites\\Players\\Sir_Edward\\Images\\Sir_Edward");
            Wilhelm = content.Load<Texture2D>("Sprites\\Players\\Wilhelm\\Images\\Wilhelm");
            Juan = content.Load<Texture2D>("Sprites\\Players\\Juan\\Images\\Juan");

            //loads the sprite sheets for the sensitivity display, the left bunker, and the right bunker
            for (int i = 0; i < 4; i++)
            {
                sensitivityDisplays[i] = new OtherObject(content.Load<Texture2D>("Sprites\\Misc\\Sensitivity_Sprite_Sheet"));
                leftBunkers[i] = new OtherObject(content.Load<Texture2D>("Sprites\\Misc\\xboxControllerSpriteSheet"));
                rightBunkers[i] = new OtherObject(content.Load<Texture2D>("Sprites\\Misc\\xboxControllerSpriteSheet"));
            }

            #endregion

            #region StartUp KnickKnacks
            //sleep for a little so the game can load
            Thread.Sleep(1000);
            //resets so xbox doesnt try to catch up with lost time in thread
            ScreenManager.Game.ResetElapsedTime();
            //make sure to set up the new menu entries upon loading
            SetMenuEntryText();
            //locked and loaded, ready to start up game
            soundBank.PlayCue("loadreload2");
            #endregion

        }

        #region MenuMethods

        void SetMenuEntryText()
        {
            //are you joined?
            if (joinedP1 == Joined.Joined)
            {
                //if so...Are you ready?
                if (p1JNR == true)
                {
                    //if you're only joined but not ready...Clear all menu entries and replace with updated menu entry data
                    MenuEntries.Clear();
                    MenuEntries.Add(playerMenuEntryP1);
                    MenuEntries.Add(handicapMenuEntryP1);
                    MenuEntries.Add(readyGameMenuEntryP1);
                    playerMenuEntryP1.Text = "Player: " + playerP1;
                    handicapMenuEntryP1.Text = "Handicap: " + handicapP1;
                    readyGameMenuEntryP1.Text = "" + readyP1;
                }
                else if (p1R == true)
                {
                    //if you're actually ready then remove menu entries and replace with the "ready" menu entry
                    MenuEntries.Remove(playerMenuEntryP1);
                    MenuEntries.Remove(handicapMenuEntryP1);
                    readyGameMenuEntryP1.Text = "" + readyP1;

                }
                //allows for menuEntries to be added if clicked Ready with A instead of Backing out with B
                else
                {
                    //if none of the above, then clear the menu entries and replace with new data
                    MenuEntries.Clear();
                    MenuEntries.Add(playerMenuEntryP1);
                    MenuEntries.Add(handicapMenuEntryP1);
                    MenuEntries.Add(readyGameMenuEntryP1);
                    playerMenuEntryP1.Text = "Player: " + playerP1;
                    handicapMenuEntryP1.Text = "Handicap: " + handicapP1;
                    readyGameMenuEntryP1.Text = "" + readyP1;
                }
            }

            else
            {
                //if you're not joined then clear the menu entries and add the join game menu entry
                MenuEntries.Clear();
                MenuEntries.Add(joinGameMenuEntryP1);
                joinGameMenuEntryP1.Text = "Press A To Join";
            }
            //if you're sir edward then your name needs to be modified a bit
            if (playerP1 == Character.Sir_Edward)
            {
                playerMenuEntryP1.Text = "Player: Sir Edward";
            }

            #region OtherPlayers
            if (joinedP2 == Joined.Joined)
            {
                if (p2JNR == true)
                {
                    MenuEntries2.Clear();
                    MenuEntries2.Add(playerMenuEntryP2);
                    MenuEntries2.Add(handicapMenuEntryP2);
                    MenuEntries2.Add(readyGameMenuEntryP2);
                    playerMenuEntryP2.Text = "Player: " + playerP2;
                    handicapMenuEntryP2.Text = "Handicap: " + handicapP2;
                    readyGameMenuEntryP2.Text = "" + readyP2;
                }
                else if (p2R == true)
                {
                    MenuEntries2.Remove(playerMenuEntryP2);
                    MenuEntries2.Remove(handicapMenuEntryP2);
                    readyGameMenuEntryP2.Text = "" + readyP2;
                }
                else
                {
                    MenuEntries2.Clear();
                    MenuEntries2.Add(playerMenuEntryP2);
                    MenuEntries2.Add(handicapMenuEntryP2);
                    MenuEntries2.Add(readyGameMenuEntryP2);
                    playerMenuEntryP2.Text = "Player: " + playerP2;
                    handicapMenuEntryP2.Text = "Handicap: " + handicapP2;
                    readyGameMenuEntryP2.Text = "" + readyP2;

                }
            }
            else
            {
                MenuEntries2.Clear();
                MenuEntries2.Add(joinGameMenuEntryP2);
                joinGameMenuEntryP2.Text = "Press A To Join";
            }
            if (playerP2 == Character.Sir_Edward)
            {
                playerMenuEntryP2.Text = "Player: Sir Edward";
            }
            if (joinedP3 == Joined.Joined)
            {
                if (p3JNR == true)
                {
                    MenuEntries3.Clear();
                    MenuEntries3.Add(playerMenuEntryP3);
                    MenuEntries3.Add(handicapMenuEntryP3);
                    MenuEntries3.Add(readyGameMenuEntryP3);
                    playerMenuEntryP3.Text = "Player: " + playerP3;
                    handicapMenuEntryP3.Text = "Handicap: " + handicapP3;
                    readyGameMenuEntryP3.Text = "" + readyP3;
                    MenuEntries3.Remove(joinGameMenuEntryP3);
                }
                else if (p3R == true)
                {
                    MenuEntries3.Remove(playerMenuEntryP3);
                    MenuEntries3.Remove(handicapMenuEntryP3);
                    readyGameMenuEntryP3.Text = "" + readyP3;
                }
                else
                {
                    MenuEntries3.Clear();
                    MenuEntries3.Add(playerMenuEntryP3);
                    MenuEntries3.Add(handicapMenuEntryP3);
                    MenuEntries3.Add(readyGameMenuEntryP3);
                    playerMenuEntryP3.Text = "Player: " + playerP3;
                    handicapMenuEntryP3.Text = "Handicap: " + handicapP3;
                    readyGameMenuEntryP3.Text = "" + readyP3;
                }
            }
            else
            {
                MenuEntries3.Clear();
                MenuEntries3.Add(joinGameMenuEntryP3);
                joinGameMenuEntryP3.Text = "Press A To Join";
            }
            if (playerP3 == Character.Sir_Edward)
            {
                playerMenuEntryP3.Text = "Player: Sir Edward";
            }
            if (joinedP4 == Joined.Joined)
            {
                if (p4JNR == true)
                {
                    MenuEntries4.Clear();
                    MenuEntries4.Add(playerMenuEntryP4);
                    MenuEntries4.Add(handicapMenuEntryP4);
                    MenuEntries4.Add(readyGameMenuEntryP4);
                    playerMenuEntryP4.Text = "Player: " + playerP4;
                    handicapMenuEntryP4.Text = "Handicap: " + handicapP4;
                    readyGameMenuEntryP4.Text = "" + readyP4;
                    MenuEntries4.Remove(joinGameMenuEntryP4);
                }
                else if (p4R == true)
                {
                    MenuEntries4.Remove(playerMenuEntryP4);
                    MenuEntries4.Remove(handicapMenuEntryP4);
                    readyGameMenuEntryP4.Text = "" + readyP4;
                }
                else
                {
                    MenuEntries4.Clear();
                    MenuEntries4.Add(playerMenuEntryP4);
                    MenuEntries4.Add(handicapMenuEntryP4);
                    MenuEntries4.Add(readyGameMenuEntryP4);
                    playerMenuEntryP4.Text = "Player: " + playerP4;
                    handicapMenuEntryP4.Text = "Handicap: " + handicapP4;
                    readyGameMenuEntryP4.Text = "" + readyP4;
                }
            }
            else
            {
                MenuEntries4.Clear();
                MenuEntries4.Add(joinGameMenuEntryP4);
                joinGameMenuEntryP4.Text = "Press A To Join";
            }
            if (playerP4 == Character.Sir_Edward)
            {
                playerMenuEntryP4.Text = "Player: Sir Edward";
            }
            #endregion
        }

        void ReadyGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //Are you ready?
            if (e.PlayerIndex == PlayerIndex.One)
            {
                readyP1++;
                //You are now ready!
                p1JNR = false;
                p1R = true;
                numberOfPlayersReady++;
                if (readyP1 > Ready.Ready)
                {
                    readyP1--;
                }
                ControllingPlayer = e.PlayerIndex;

            }

            #region OtherPlayers
            if (e.PlayerIndex == PlayerIndex.Two)
            {
                readyP2++;
                p2JNR = false;
                p2R = true;
                numberOfPlayersReady++;
                if (readyP2 > Ready.Ready)
                {
                    readyP2--;
                }
                ControllingPlayer = e.PlayerIndex;

            }
            if (e.PlayerIndex == PlayerIndex.Three)
            {
                readyP3++;
                p3JNR = false;
                p3R = true;
                numberOfPlayersReady++;
                if (readyP3 > Ready.Ready)
                {
                    readyP3--;
                }
                ControllingPlayer = e.PlayerIndex;

            }
            if (e.PlayerIndex == PlayerIndex.Four)
            {
                readyP4++;
                p4JNR = false;
                p4R = true;
                numberOfPlayersReady++;
                if (readyP4 > Ready.Ready)
                {
                    readyP4--;
                }
                ControllingPlayer = e.PlayerIndex;

            }
            SetMenuEntryText();
            #endregion

            UpdateReady();
            

        }

        void JoinGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //Are you joining?
            if (e.PlayerIndex == PlayerIndex.One)
            {
                joinedP1++;
                p1JNR = true;
                p1NJ = false;
                numberOfPlayersJoined++;
                do
                {
                    playerP1++;
                    if (playerP1 > Character.Sir_Edward)
                    {
                        playerP1 = 0;
                    }
                } while (playerP1 == playerP4 || playerP1 == playerP2 || playerP1 == playerP3);
                if (joinedP1 > Joined.Joined)
                {
                    joinedP1 = 0;
                    p1NJ = true;
                }

            }

            #region OtherPlayers
            if (e.PlayerIndex == PlayerIndex.Two)
            {
                joinedP2++;
                p2JNR = true;
                p2NJ = false;
                numberOfPlayersJoined++;
                do
                {
                    playerP2++;
                    if (playerP2 > Character.Sir_Edward)
                    {
                        playerP2 = 0;
                    }
                } while (playerP2 == playerP1 || playerP2 == playerP4 || playerP2 == playerP3);
                if (joinedP2 > Joined.Joined)
                {
                    joinedP2 = 0;
                    p2NJ = true;
                    
                }

            }
            if (e.PlayerIndex == PlayerIndex.Three)
            {
                joinedP3++;
                p3JNR = true;
                p3NJ = false;
                numberOfPlayersJoined++;
                do
                {
                    playerP3++;
                    if (playerP3 > Character.Sir_Edward)
                    {
                        playerP3 = 0;
                    }
                } while (playerP3 == playerP1 || playerP3 == playerP2 || playerP3 == playerP4);
                if (joinedP3 > Joined.Joined)
                {
                    joinedP3 = 0;
                    p3NJ = true;
                    
                }

            }
            if (e.PlayerIndex == PlayerIndex.Four)
            {
                joinedP4++;
                p4JNR = true;
                p4NJ = false;
                numberOfPlayersJoined++;
                do
                {
                    playerP4++;
                    if (playerP4 > Character.Sir_Edward)
                    {
                        playerP4 = 0;
                    }
                } while (playerP4 == playerP1 || playerP4 == playerP2 || playerP4 == playerP3);
                if (joinedP4 > Joined.Joined)
                {
                    joinedP4 = 0;
                    p4NJ = true;
                    
                }


            }
            SetMenuEntryText();
            #endregion
        }

        void PlayerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //You changed your character...
            if (e.PlayerIndex == PlayerIndex.One)
            {
                //It is now the next AVAILABLE character.
                do
                {
                    playerP1++;
                    if (playerP1 > Character.Sir_Edward)
                    {
                        playerP1 = 0;
                    }
                } while (playerP1 == playerP2 || playerP1 == playerP3 || playerP1 == playerP4);
            }

            #region OtherPlayers
            if (e.PlayerIndex == PlayerIndex.Two)
            {
                do
                {
                    playerP2++;
                    if (playerP2 > Character.Sir_Edward)
                    {
                        playerP2 = 0;
                    }
                } while (playerP2 == playerP1 || playerP2 == playerP3 || playerP2 == playerP4);
            }
            if (e.PlayerIndex == PlayerIndex.Three)
            {
                do
                {
                    playerP3++;
                    if (playerP3 > Character.Sir_Edward)
                    {
                        playerP3 = 0;
                    }
                } while (playerP3 == playerP1 || playerP3 == playerP2 || playerP3 == playerP4);

            }
            if (e.PlayerIndex == PlayerIndex.Four)
            {
                do
                {
                    playerP4++;
                    if (playerP4 > Character.Sir_Edward)
                    {
                        playerP4 = 0;
                    }
                } while (playerP4 == playerP1 || playerP4 == playerP2 || playerP4 == playerP3);
            }
            SetMenuEntryText();
            #endregion
        }

        void HandicapMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            //Did you select the HandicapMenuEntry?
            if (e.PlayerIndex == PlayerIndex.One)
            {
                //Your handicap now has changed...
                handicapP1++;
                //if (Guide.IsTrialMode)
                //{
                //    handicapP1--;
                //}
                if (handicapP1 > Handicap.VeryHard)
                {
                    handicapP1 = 0;
                }
            }

            #region OtherPlayers
            if (e.PlayerIndex == PlayerIndex.Two)
            {
                handicapP2++;
                
                if (handicapP2 > Handicap.VeryHard)
                {
                    handicapP2 = 0;
                }
            }
            if (e.PlayerIndex == PlayerIndex.Three)
            {
                handicapP3++;
                
                if (handicapP3 > Handicap.VeryHard)
                {
                    handicapP3 = 0;
                }
            }
            if (e.PlayerIndex == PlayerIndex.Four)
            {
                handicapP4++;
                
                if (handicapP4 > Handicap.VeryHard)
                {
                    handicapP4 = 0;
                }
            }
            SetMenuEntryText();
            #endregion
        }

        /// <summary>
        /// This checks if all players are ready to play who are joined in.
        /// <bool name="play"> This is the boolean that if set to true allows for the count down video to start
        /// therefore allowing for the next screen to load after the video is finished </bool>
        /// </summary>
        void UpdateReady()
        {
            switch(numberOfPlayersJoined)
            {
                case 1:
                    if (numberOfPlayersJoined == numberOfPlayersReady)
                    {
                        //is everyone who joined, ready?
                        if (p1R == true && p2NJ == true && p3NJ == true && p4NJ == true)
                        {
                            //if so...then now you can play (and set the number of players playing too)
                            play = true;
                            numberOfPlayers = 1;
                        }
                        else
                        {
                            const string message = "Please Adjust Players To Be In Order (No Skipping)";

                            WarningBoxScreen confirmExitMessageBox2 = new WarningBoxScreen(message, true);

                            //confirmExitMessageBox2.Accepted += ConfirmWarningMessageBoxAccepted2;

                            ScreenManager.AddScreen(confirmExitMessageBox2, ControllingPlayer);
                        }
                    }
                    break;
                case 2:
                    {
                        if (numberOfPlayersJoined == numberOfPlayersReady)
                        {
                            //is everyone who joined, ready?
                            if (p1R == true && p2R == true && p3NJ == true && p4NJ == true)
                            {
                                //if so...then now you can play (and set the number of players playing too)
                                play = true;
                                numberOfPlayers = 2;
                            }
                            else
                            {
                                const string message = "Please Adjust Players To Be In Order (No Skipping)";

                                WarningBoxScreen confirmExitMessageBox2 = new WarningBoxScreen(message, true);

                                //confirmExitMessageBox2.Accepted += ConfirmWarningMessageBoxAccepted2;

                                ScreenManager.AddScreen(confirmExitMessageBox2, ControllingPlayer);
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        if (numberOfPlayersJoined == numberOfPlayersReady)
                        {
                            //is everyone who joined, ready?
                            if (p1R == true && p2R == true && p3R == true && p4NJ == true)
                            {
                                //if so...then now you can play (and set the number of players playing too)
                                play = true;
                                numberOfPlayers = 3;
                            }
                            else
                            {
                                const string message = "Please Adjust Players To Be In Order (No Skipping)";

                                WarningBoxScreen confirmExitMessageBox2 = new WarningBoxScreen(message, true);

                                //confirmExitMessageBox2.Accepted += ConfirmWarningMessageBoxAccepted2;

                                ScreenManager.AddScreen(confirmExitMessageBox2, ControllingPlayer);
                            }
                        }
                    }
                    break;
                case 4:
                    {
                        if (numberOfPlayersJoined == numberOfPlayersReady)
                        {
                            if (p1R == true && p2R == true && p3R == true && p4R == true)
                            {
                                play = true;
                                numberOfPlayers = 4;
                            }
                        }
                    }
                    break;
                    
        }
                
            

        }
        #endregion

        /// <summary>
        /// This draws the Xbox Guide Button in the center to indicate whos ready, joined but not ready, and whos not joined.
        /// This also calls the base.Draw(gameTime) to ensure that the menu screen's menuEntries are being draw as well and not just being overriden.
        /// </summary>
        /// <param name="gameTime"> The elapsed game time that has passed </param>
        public override void Draw(GameTime gameTime)
        {

            DrawXBoxGuideButton();

            DrawPlayerImages(gameTime);

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            //if the game is active right now
            if (active)
            {

                #region StartUp and CountDown Video (with loadingScreen)

                //Then check if we are ready to play, the player isn't gone, and the player has stopped
                if (play == true && !player.IsDisposed && player.State == MediaState.Stopped)
                {
                    //do not loop
                    player.IsLooped = false;

                    //this will only happen after the '5, 4, 3, 2, 1' video
                    if (counter == 1)
                    {
                        if (LevelLoading == 1)
                        {
                            MaxZombies = 20;
                            //load the next screen and unload this one
                            LoadingScreen.Load(ScreenManager, true, ControllingPlayer, false, new Level1(playerP1, playerP2, playerP3, playerP4,
                                                       handicapP1, handicapP2, handicapP3, handicapP4, numberOfPlayers, playerTriggerSensitivitys[0], playerTriggerSensitivitys[1], playerTriggerSensitivitys[2], playerTriggerSensitivitys[3]));
                        }
                        if (LevelLoading == 2)
                        {
                            //the new zombie count
                            MaxZombies = GetNewZombieCount();

                            LoadingScreen.Load(ScreenManager, true, ControllingPlayer, false, new Level2(playerP1, playerP2, playerP3, playerP4,
                                                       handicapP1, handicapP2, handicapP3, handicapP4, numberOfPlayers, playerTriggerSensitivitys[0], playerTriggerSensitivitys[1], playerTriggerSensitivitys[2], playerTriggerSensitivitys[3]));
                            

                        }
                        if (LevelLoading == 3)
                        {
                            MaxZombies = GetNewZombieCount();

                            LoadingScreen.Load(ScreenManager, true, ControllingPlayer, false, new Level2(playerP1, playerP2, playerP3, playerP4,
                                                       handicapP1, handicapP2, handicapP3, handicapP4, numberOfPlayers, playerTriggerSensitivitys[0], playerTriggerSensitivitys[1], playerTriggerSensitivitys[2], playerTriggerSensitivitys[3]));
                        }
                        //make sure to dispose of the player
                        player.Dispose();
                        
                    }
                    else if (counter == 0)
                    {
                        //'54321' video
                        player.Play(videos[0]);
                        counter++;
                    }
                }
            
                #endregion

                #region MakePlayersNullIfGone
                if (p1JNR == true && p2NJ == true && p3NJ == true && p4NJ == true)
                {
                    playerP2 = Character.Null;
                    playerP3 = Character.Null;
                    playerP4 = Character.Null;
                }
                if (p1JNR == true && p2JNR == true && p3NJ == true && p4NJ == true)
                {
                    playerP3 = Character.Null;
                    playerP4 = Character.Null;
                }
                if (p1JNR == true && p2JNR == true && p3JNR == true && p4NJ == true)
                {
                    playerP4 = Character.Null;
                }
                #endregion

                base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
            }
            else if (ScreenManager.GetScreens().Length == 2 && !active)
            {
                active = true;
            }

        }

        private int GetNewZombieCount()
        {

            int newZombieCount = (int)(MaxZombies * 0.10);
            double remainder = 0;
            remainder %= MaxZombies * 0.10;
            newZombieCount -= (int)remainder;
            
            return newZombieCount;
        }

        public override void HandleInput(InputState input)
        {
            //if the game is active right now
            if (active)
            {
                for (int i = 0; i < 4; i++)
                {

                    #region TriggerSensitivities
                    if (input.CurrentGamePadStates[i].Buttons.RightShoulder == ButtonState.Pressed && input.PreviousGamePadStates[i].Buttons.RightShoulder == ButtonState.Released)
                    {
                        playerTriggerSensitivitys[i] += 0.33f;
                        if (playerTriggerSensitivitys[i] > 1.0f)
                            playerTriggerSensitivitys[i] = 0.99f;
                    }
                    if (input.CurrentGamePadStates[i].Buttons.LeftShoulder == ButtonState.Pressed && input.PreviousGamePadStates[i].Buttons.LeftShoulder == ButtonState.Released)
                    {
                        playerTriggerSensitivitys[i] -= 0.33f;
                        if (playerTriggerSensitivitys[i] < 0.0f)
                            playerTriggerSensitivitys[i] = 0.0f;
                    }
                    #endregion

                }

                UpdateInfo(input);

                UpdateSensitivity();
            }

            base.HandleInput(input);

        }

        private void UpdateInfo(InputState input)
        {
            #region Character && Gun Info Pull Up
            for (int i = 0; i < numberOfPlayersJoined; i++)
            {
                if (input.CurrentGamePadStates[i].Buttons.Y == ButtonState.Pressed && input.PreviousGamePadStates[i].Buttons.Y == ButtonState.Released)
                {
                    ControllingPlayer = (PlayerIndex)i;
                    active = false;
                    ScreenManager.AddScreen(new CharacterInfoScreen(), ControllingPlayer);
                }
                if (input.CurrentGamePadStates[i].Buttons.X == ButtonState.Pressed && input.PreviousGamePadStates[i].Buttons.Y == ButtonState.Released)
                {
                    ControllingPlayer = (PlayerIndex)i;
                    active = false;
                    ScreenManager.AddScreen(new GunInfoScreen(), ControllingPlayer);
                }
            }
            #endregion
        }

        private void UpdateSensitivity()
        {
            for (int i = 0; i < numberOfPlayersJoined; i++)
            {
                sensitivityDisplays[0].position = new Vector2(100, 200);
                sensitivityDisplays[0].textPosition = new Vector2(375, 225);
                leftBunkers[0].position = new Vector2(100, 250);
                rightBunkers[0].position = new Vector2(260, 250);

                sensitivityDisplays[i].position = sensitivityDisplays[0].position + GetPosition(i);
                sensitivityDisplays[i].textPosition = sensitivityDisplays[0].textPosition + GetPosition(i);
                leftBunkers[i].position = leftBunkers[0].position + GetPosition(i);
                rightBunkers[i].position = rightBunkers[0].position + GetPosition(i);
            }

            #region ChangeSourceRectangle for Sensitivity
            for (int i = 0; i < numberOfPlayersJoined; i++)
            {
                if (playerTriggerSensitivitys[i] == 0.0f)
                {
                    sourceRectangleSen[i] = new Rectangle(0, 0, 250, 100);
                    sensitivityDisplays[i].text = "High Sensitivity";
                }
                if (playerTriggerSensitivitys[i] > 0.3f && playerTriggerSensitivitys[i] < 0.4f)
                {
                    sourceRectangleSen[i] = new Rectangle(250, 0, 250, 100);
                    sensitivityDisplays[i].text = "Medium Sensitivity";
                }
                if (playerTriggerSensitivitys[i] > 0.6f && playerTriggerSensitivitys[i] < 0.7f)
                {
                    sourceRectangleSen[i] = new Rectangle(500, 0, 250, 100);
                    sensitivityDisplays[i].text = "Low Sensitivity";
                }
                if (playerTriggerSensitivitys[i] == 0.99f)
                {
                    sourceRectangleSen[i] = new Rectangle(750, 0, 250, 100);
                    sensitivityDisplays[i].text = "Very Low" +
                                                  "\nSensitivity";
                }
            }
            #endregion
        }

        private Vector2 GetPosition(int i)
        {
            Vector2 newPosition = Vector2.Zero;

            if (i == 0)
                newPosition = new Vector2(0, 0);
            if (i == 1)
                newPosition = new Vector2(650, 0);
            if (i == 2)
                newPosition = new Vector2(0, 350);
            if (i == 3)
                newPosition = new Vector2(650, 350);

            return newPosition;
        }

        private void DrawXBoxGuideButton()
        {

            if (!player.IsDisposed)
            {
                if (player.State == MediaState.Stopped)
                {

                    SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

                    Vector2 positionCenter = new Vector2(left, top);

                    spriteBatch.Begin();
                    //basic xbox guide button without colors
                    spriteBatch.Draw(center, positionCenter, Color.White);

                    //Player is not Joined
                    if (p1NJ == true)
                    {
                        spriteBatch.Draw(p1NotJoined, positionCenter, Color.White);
                    }
                    //Player is Joined but not Ready
                    if (p1JNR == true && p1R == false)
                    {
                        spriteBatch.Draw(p1JoinedNotReady, positionCenter, Color.White);
                    }
                    //Player is Ready to play
                    if (p1R == true)
                    {
                        spriteBatch.Draw(p1Ready, positionCenter, Color.White);
                    }

                    #region OtherPlayers
                    if (p2NJ == true)
                    {
                        spriteBatch.Draw(p2NotJoined, positionCenter, Color.White);
                    }
                    if (p2JNR == true && p2R == false)
                    {
                        spriteBatch.Draw(p2JoinedNotReady, positionCenter, Color.White);
                    }
                    if (p2R == true)
                    {
                        spriteBatch.Draw(p2Ready, positionCenter, Color.White);
                    }


                    if (p3NJ == true)
                    {
                        spriteBatch.Draw(p3NotJoined, positionCenter, Color.White);
                    }
                    if (p3JNR == true && p3R == false)
                    {
                        spriteBatch.Draw(p3JoinedNotReady, positionCenter, Color.White);
                    }
                    if (p3R == true)
                    {
                        spriteBatch.Draw(p3Ready, positionCenter, Color.White);
                    }

                    if (p4NJ == true)
                    {
                        spriteBatch.Draw(p4NotJoined, positionCenter, Color.White);
                    }
                    if (p4JNR == true && p4R == false)
                    {
                        spriteBatch.Draw(p4JoinedNotReady, positionCenter, Color.White);
                    }
                    if (p4R == true)
                    {
                        spriteBatch.Draw(p4Ready, positionCenter, Color.White);
                    }
                    #endregion

                    spriteBatch.End();
                }
                if (player.State != MediaState.Stopped)
                {
                    videoTexture = player.GetTexture();
                }

                //small rectangle under the xbox guide button symbol
                Rectangle position = new Rectangle((int)left, (int)top, textureLength, textureLength);
                
                if (videoTexture != null)
                {
                    spriteBatch.Begin();

                    spriteBatch.Draw(videoTexture, position, Color.White);
                    
                    spriteBatch.End();
                }
            }
        }

        private void DrawPlayerImages(GameTime gameTime)
        {

            //MenuScreen Draw Method
            
            spriteBatch.Begin();
            if (!p1NJ)
            {
                switch (playerP1)
                {
                    case Character.Juan:
                        spriteBatch.Draw(Juan, texturePosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sayid:
                        spriteBatch.Draw(Sayid, texturePosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sir_Edward:
                        spriteBatch.Draw(Sir_Edward, texturePosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Wilhelm:
                        spriteBatch.Draw(Wilhelm, texturePosition, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                }
            }
            #region OtherPlayers
            if (!p2NJ)
            {
                switch (playerP2)
                {
                    case Character.Juan:
                        spriteBatch.Draw(Juan, texturePosition2, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sayid:
                        spriteBatch.Draw(Sayid, texturePosition2, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sir_Edward:
                        spriteBatch.Draw(Sir_Edward, texturePosition2, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Wilhelm:
                        spriteBatch.Draw(Wilhelm, texturePosition2, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                }
            }
            if (!p3NJ)
            {
                switch (playerP3)
                {
                    case Character.Juan:
                        spriteBatch.Draw(Juan, texturePosition3, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sayid:
                        spriteBatch.Draw(Sayid, texturePosition3, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sir_Edward:
                        spriteBatch.Draw(Sir_Edward, texturePosition3, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Wilhelm:
                        spriteBatch.Draw(Wilhelm, texturePosition3, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                }
            }
            if (!p4NJ)
            {
                switch (playerP4)
                {
                    case Character.Juan:
                        spriteBatch.Draw(Juan, texturePosition4, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sayid:
                        spriteBatch.Draw(Sayid, texturePosition4, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Sir_Edward:
                        spriteBatch.Draw(Sir_Edward, texturePosition4, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                    case Character.Wilhelm:
                        spriteBatch.Draw(Wilhelm, texturePosition4, null, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                        break;
                }
            }
            #endregion

            if (!play)
            {
                if (p1JNR)
                {
                    int i = 0;
                    spriteBatch.Draw(sensitivityDisplays[i].sprite, sensitivityDisplays[i].position, sourceRectangleSen[i], Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(leftBunkers[i].sprite, leftBunkers[i].position, sourceRectangleLeft, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(rightBunkers[i].sprite, rightBunkers[i].position, sourceRectangleRight, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, sensitivityDisplays[i].text, sensitivityDisplays[i].textPosition, Color.White);
                }
                if (p2JNR)
                {
                    int i = 1;
                    spriteBatch.Draw(sensitivityDisplays[i].sprite, sensitivityDisplays[i].position, sourceRectangleSen[i], Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(leftBunkers[i].sprite, leftBunkers[i].position, sourceRectangleLeft, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(rightBunkers[i].sprite, rightBunkers[i].position, sourceRectangleRight, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, sensitivityDisplays[i].text, sensitivityDisplays[i].textPosition, Color.White);
                }
                if (p3JNR)
                {
                    int i = 2;
                    spriteBatch.Draw(sensitivityDisplays[i].sprite, sensitivityDisplays[i].position, sourceRectangleSen[i], Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(leftBunkers[i].sprite, leftBunkers[i].position, sourceRectangleLeft, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(rightBunkers[i].sprite, rightBunkers[i].position, sourceRectangleRight, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, sensitivityDisplays[i].text, sensitivityDisplays[i].textPosition, Color.White);
                }
                if (p4JNR)
                {
                    int i = 3;
                    spriteBatch.Draw(sensitivityDisplays[i].sprite, sensitivityDisplays[i].position, sourceRectangleSen[i], Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(leftBunkers[i].sprite, leftBunkers[i].position, sourceRectangleLeft, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(rightBunkers[i].sprite, rightBunkers[i].position, sourceRectangleRight, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(font, sensitivityDisplays[i].text, sensitivityDisplays[i].textPosition, Color.White);
                }
            }

            spriteBatch.End();

            if (active)
            {
                base.Draw(gameTime);
            }
            
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {

            //Are all players not joined and wanting to exit??
            if (p1NJ == true && p2NJ == true && p3NJ == true && p4NJ == true)
            {
                const string message = "Are you sure you want to exit?";

                MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message, true, true);

                confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

                ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
            }
            //Back out of the game but doesn't end game unless everyone backs out
            if (playerIndex == PlayerIndex.One)
            {
                if (active)
                {
                    p1NJ = true;
                    p1JNR = false;
                    joinedP1 = 0;
                    numberOfPlayersJoined--;
                    if (p1R == true)
                    {
                        p1NJ = false;
                        p1JNR = true;
                        p1R = false;
                        readyP1--;
                        joinedP1++;
                        play = false;
                        numberOfPlayersJoined++;
                    }
                }
                else
                {
                    //reactivates the screen
                    active = true;
                }
            }

            #region OtherPlayers
            if (playerIndex == PlayerIndex.Two)
            {
                if (active)
                {
                    p2NJ = true;
                    p2JNR = false;
                    joinedP2 = 0;
                    numberOfPlayersJoined--;
                    if (p2R == true)
                    {
                        p2NJ = false;
                        p2JNR = true;
                        p2R = false;
                        readyP2--;
                        joinedP2++;
                        play = false;
                        numberOfPlayersJoined++;
                    }
                }
                else
                {
                    active = true;
                }
            }
            if (playerIndex == PlayerIndex.Three)
            {
                if (active)
                {
                    p3NJ = true;
                    p3JNR = false;
                    joinedP3 = 0;
                    numberOfPlayersJoined--;
                    if (p3R == true)
                    {
                        p3NJ = false;
                        p3JNR = true;
                        p3R = false;
                        readyP3--;
                        joinedP3++;
                        play = false;
                        numberOfPlayersJoined++;
                    }
                }
                else
                {
                    active = true;
                }
            }
            if (playerIndex == PlayerIndex.Four)
            {
                if (active)
                {
                    p4NJ = true;
                    p4JNR = false;
                    joinedP4 = 0;
                    numberOfPlayersJoined--;
                    if (p4R == true)
                    {
                        p4NJ = false;
                        p4JNR = true;
                        p4R = false;
                        readyP4--;
                        joinedP4++;
                        play = false;
                        numberOfPlayersJoined++;
                    }
                }
                else
                {
                    active = true;
                }
            }
            if (player.State == MediaState.Playing)
            {
                player.Stop();
                play = false;
                counter = 0;
            }
            #endregion

            SetMenuEntryText();

        }

        //Ends the game :(
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new BackgroundScreen(), e.PlayerIndex);
            ScreenManager.AddScreen(new StartUpScreen(), e.PlayerIndex);

            this.ExitScreen();
        }

    }
}