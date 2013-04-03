using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JAMGameFinal
{
    class SidescrollerScreen : GameScreen
    {
        #region worthless variables
        SpriteBatch spriteBatch;
        Rectangle viewportRect;
        #endregion
        /*
        #region video variables
        //background video
        Video backgroundvideo;
        VideoPlayer player;
        Texture2D videoTexture;
        #endregion
        */
        #region player and handicap variables
        //player definitions

        Players[] players;
        PlayerHalo[] playerHalos;
        Color[] player1TextureData;
        Color[] player2TextureData;
        Color[] player3TextureData;
        Color[] player4TextureData;

        Player player1;
        Player player2;
        Player player3;
        Player player4;
        float fullPlayer1Health;
        float fullPlayer2Health;
        float fullPlayer3Health;
        float fullPlayer4Health;

        Handicap handicap1;
        Handicap handicap2;
        Handicap handicap3;
        Handicap handicap4;
        int numberOfPlayers = 0;
        int numberOfPlayersLeft = 0;
        int playerRectangleScale = 4;


        const float SafeAreaPortion = 0.05f;
        Rectangle safeBounds;
        #endregion

        #region gamepadstate variables
        //the actual gamepadstate before anything is pressed...at first
        GamePadState previousGamePadState1 = GamePad.GetState(PlayerIndex.One);
        GamePadState previousGamePadState2 = GamePad.GetState(PlayerIndex.Two);
        GamePadState previousGamePadState3 = GamePad.GetState(PlayerIndex.Three);
        GamePadState previousGamePadState4 = GamePad.GetState(PlayerIndex.Four);
        #endregion

        #region zombie variables
        const int maxzombies = 40;
        const float maxzombieheight = 1.0f;
        const float minzombieheight = 0.0f;
        const float maxzombievelocity =1.1f;
        const float minzombievelocity = 1.0f;
        const float minzombieymovement = -0.001f;
        const float maxzombieymovement = 0.001f;
        Random random = new Random();
        Zombie[] zombies;
        #endregion
        
        public SidescrollerScreen(int numberofplayers)
        {
            
        }

        public override void LoadContent()
        {
            Viewport viewportRect = ScreenManager.GraphicsDevice.Viewport;
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            safeBounds = new Rectangle(
                (int)(viewportRect.Width * SafeAreaPortion),
                (int)(viewportRect.Height * SafeAreaPortion),
                (int)(viewportRect.Width * (1 - 2 * SafeAreaPortion)),
                (int)(viewportRect.Height * (1 - 2 * SafeAreaPortion)));

            #region load zombie stuff
            zombies = new Zombie[maxzombies];
            foreach (Zombie zombie in zombies)
            {

                for (int i = 0; i <= 10; i++)
                {
                    zombies[i] = new Zombie(content.Load<Texture2D>("Sprites\\Zombies\\Images\\zombie1"));
                }
                for (int i = 11; i <= 20; i++)
                {
                    zombies[i] = new Zombie(content.Load<Texture2D>("Sprites\\Zombies\\Images\\zombie2"));
                }
                for (int i = 21; i <= 30; i++)
                {
                    zombies[i] = new Zombie(content.Load<Texture2D>("Sprites\\Zombies\\Images\\zombie3"));
                }
                for (int i = 31; i <= 40; i++)
                {
                    zombies[i] = new Zombie(content.Load<Texture2D>("Sprites\\Zombies\\Images\\zombie4"));
                }
            }
            #endregion

            switch (numberOfPlayers)
            {
                case 1:
                    #region TextureData
                    if (player1 == Player.Tyrone)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player1 == Player.Sir_Edward)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player1 == Player.Wilhelm)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    if (player1 == Player.Bob)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Images\\Bob"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Halo\\Bob-Halo"));
                    }


                    player1TextureData =
                        new Color[players[0].sprite.Width * players[0].sprite.Height];
                    players[0].sprite.GetData(player1TextureData);
                    #endregion
                    #region Position
                    players[0].position.X = (safeBounds.Width - players[0].sprite.Width) / 2;
                    players[0].position.Y = safeBounds.Height - players[0].sprite.Height;
                    #endregion
                    #region Health
                    if (handicap1 == Handicap.Easy)
                    {
                        players[0].health = 20;
                    }
                    else if (handicap1 == Handicap.Medium)
                    {
                        players[0].health = 15;
                    }
                    else if (handicap1 == Handicap.Hard)
                    {
                        players[0].health = 10;
                    }
                    else if (handicap1 == Handicap.VeryHard)
                    {
                        players[0].health = 5;
                    }
                    fullPlayer1Health = players[0].health;
                    #endregion
                    break;
                case 2:
                    #region TextureData
                    if (player1 == Player.Tyrone)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player1 == Player.Sir_Edward)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player1 == Player.Wilhelm)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    if (player1 == Player.Bob)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Images\\Bob"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Halo\\Bob-Halo"));
                    }
                    player1TextureData =
                        new Color[players[0].sprite.Width * players[0].sprite.Height];
                    players[0].sprite.GetData(player1TextureData);

                    if (player2 == Player.Tyrone)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player2 == Player.Sir_Edward)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player2 == Player.Wilhelm)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    if (player2 == Player.Bob)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Images\\Bob"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Halo\\Bob-Halo"));
                    }
                    player2TextureData =
                        new Color[players[1].sprite.Width * players[1].sprite.Height];
                    players[1].sprite.GetData(player2TextureData);
                    #endregion
                    #region Positions
                    players[0].position.X = (safeBounds.Width - players[0].sprite.Width) / 2;
                    players[0].position.Y = safeBounds.Height - players[0].sprite.Height;
                    players[1].position.X = (safeBounds.Width - players[1].sprite.Width) / 2 + 150;
                    players[1].position.Y = safeBounds.Height - players[1].sprite.Height;
                    #endregion
                    #region Health
                    if (handicap1 == Handicap.Easy)
                    {
                        players[0].health = 20;
                    }
                    else if (handicap1 == Handicap.Medium)
                    {
                        players[0].health = 15;
                    }
                    else if (handicap1 == Handicap.Hard)
                    {
                        players[0].health = 10;
                    }
                    else if (handicap1 == Handicap.VeryHard)
                    {
                        players[0].health = 5;
                    }
                    if (handicap2 == Handicap.Easy)
                    {
                        players[1].health = 20;
                    }
                    else if (handicap2 == Handicap.Medium)
                    {
                        players[1].health = 15;
                    }
                    else if (handicap2 == Handicap.Hard)
                    {
                        players[1].health = 10;
                    }
                    else if (handicap2 == Handicap.VeryHard)
                    {
                        players[1].health = 5;
                    }
                    fullPlayer1Health = players[0].health;
                    fullPlayer2Health = players[1].health;
                    #endregion
                    break;
                case 3:
                    #region TextureData
                    if (player1 == Player.Tyrone)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player1 == Player.Sir_Edward)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player1 == Player.Wilhelm)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    if (player1 == Player.Bob)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Images\\Bob"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Halo\\Bob-Halo"));
                    }
                    player1TextureData =
                        new Color[players[0].sprite.Width * players[0].sprite.Height];
                    players[0].sprite.GetData(player1TextureData);

                    if (player2 == Player.Tyrone)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player2 == Player.Sir_Edward)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player2 == Player.Wilhelm)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    if (player2 == Player.Bob)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Images\\Bob"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Halo\\Bob-Halo"));
                    }
                    player2TextureData =
                        new Color[players[1].sprite.Width * players[1].sprite.Height];
                    players[1].sprite.GetData(player2TextureData);

                    if (player3 == Player.Tyrone)
                    {
                        players[2] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[2] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player3 == Player.Sir_Edward)
                    {
                        players[2] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[2] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player3 == Player.Wilhelm)
                    {
                        players[2] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[2] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    if (player3 == Player.Bob)
                    {
                        players[2] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Images\\Bob"));
                        playerHalos[2] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Bob\\Halo\\Bob-Halo"));
                    }
                    player3TextureData =
                        new Color[players[2].sprite.Width * players[2].sprite.Height];
                    players[2].sprite.GetData(player3TextureData);
                    #endregion
                    #region Positions
                    players[0].position.X = (safeBounds.Width - players[0].sprite.Width) / 2;
                    players[0].position.Y = safeBounds.Height - players[0].sprite.Height;
                    players[1].position.X = (safeBounds.Width - players[1].sprite.Width) / 2 + 150;
                    players[1].position.Y = safeBounds.Height - players[1].sprite.Height;
                    players[2].position.X = (safeBounds.Width - players[2].sprite.Width) / 2 + 300;
                    players[2].position.Y = safeBounds.Height - players[2].sprite.Height;
                    #endregion
                    #region Health
                    if (handicap1 == Handicap.Easy)
                    {
                        players[0].health = 20;
                    }
                    else if (handicap1 == Handicap.Medium)
                    {
                        players[0].health = 15;
                    }
                    else if (handicap1 == Handicap.Hard)
                    {
                        players[0].health = 10;
                    }
                    else if (handicap1 == Handicap.VeryHard)
                    {
                        players[0].health = 5;
                    }
                    if (handicap2 == Handicap.Easy)
                    {
                        players[1].health = 20;
                    }
                    else if (handicap2 == Handicap.Medium)
                    {
                        players[1].health = 15;
                    }
                    else if (handicap2 == Handicap.Hard)
                    {
                        players[1].health = 10;
                    }
                    else if (handicap2 == Handicap.VeryHard)
                    {
                        players[1].health = 5;
                    }
                    if (handicap3 == Handicap.Easy)
                    {
                        players[2].health = 20;
                    }
                    else if (handicap3 == Handicap.Medium)
                    {
                        players[2].health = 15;
                    }
                    else if (handicap3 == Handicap.Hard)
                    {
                        players[2].health = 10;
                    }
                    else if (handicap3 == Handicap.VeryHard)
                    {
                        players[2].health = 5;
                    }
                    fullPlayer1Health = players[0].health;
                    fullPlayer2Health = players[1].health;
                    fullPlayer3Health = players[2].health;
                    #endregion
                    break;
                case 4:
                    #region TextureData
                    if (player1 == Player.Tyrone)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player1 == Player.Sir_Edward)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player1 == Player.Wilhelm)
                    {
                        players[0] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[0] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    player1TextureData =
                        new Color[players[0].sprite.Width * players[0].sprite.Height];
                    players[0].sprite.GetData(player1TextureData);

                    if (player2 == Player.Tyrone)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player2 == Player.Sir_Edward)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player2 == Player.Wilhelm)
                    {
                        players[1] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[1] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    player2TextureData =
                        new Color[players[1].sprite.Width * players[1].sprite.Height];
                    players[1].sprite.GetData(player2TextureData);

                    if (player3 == Player.Tyrone)
                    {
                        players[2] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[2] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player3 == Player.Sir_Edward)
                    {
                        players[2] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[2] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player3 == Player.Wilhelm)
                    {
                        players[2] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[2] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    player3TextureData =
                        new Color[players[2].sprite.Width * players[2].sprite.Height];
                    players[2].sprite.GetData(player3TextureData);

                    if (player4 == Player.Tyrone)
                    {
                        players[3] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Images\\Tyrone"));
                        playerHalos[3] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Tyrone\\Halo\\Tyrone-Halo"));
                    }
                    if (player4 == Player.Sir_Edward)
                    {
                        players[3] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Images\\Sir_Edward"));
                        playerHalos[3] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Sir_Edward\\Halo\\Sir_Edward-Halo"));
                    }
                    if (player4 == Player.Wilhelm)
                    {
                        players[3] = new Players(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Images\\Wilhelm"));
                        playerHalos[3] = new PlayerHalo(ScreenManager.Game.Content.Load<Texture2D>("Sprites\\Wilhelm\\Halo\\Wilhelm-Halo"));
                    }
                    player4TextureData =
                        new Color[players[3].sprite.Width * players[3].sprite.Height];
                    players[3].sprite.GetData(player4TextureData);
                    #endregion
                    #region Positions
                    players[0].position.X = (safeBounds.Width - players[0].sprite.Width) / 2;
                    players[0].position.Y = safeBounds.Height - players[0].sprite.Height;
                    players[1].position.X = (safeBounds.Width - players[1].sprite.Width) / 2 + 150;
                    players[1].position.Y = safeBounds.Height - players[1].sprite.Height;
                    players[2].position.X = (safeBounds.Width - players[2].sprite.Width) / 2 + 300;
                    players[2].position.Y = safeBounds.Height - players[2].sprite.Height;
                    players[3].position.X = (safeBounds.Width - players[3].sprite.Width) / 2 + 450;
                    players[3].position.Y = safeBounds.Height - players[3].sprite.Height;
                    #endregion
                    #region Health
                    if (handicap1 == Handicap.Easy)
                    {
                        players[0].health = 20;
                    }
                    else if (handicap1 == Handicap.Medium)
                    {
                        players[0].health = 15;
                    }
                    else if (handicap1 == Handicap.Hard)
                    {
                        players[0].health = 10;
                    }
                    else if (handicap1 == Handicap.VeryHard)
                    {
                        players[0].health = 5;
                    }
                    if (handicap2 == Handicap.Easy)
                    {
                        players[1].health = 20;
                    }
                    else if (handicap2 == Handicap.Medium)
                    {
                        players[1].health = 15;
                    }
                    else if (handicap2 == Handicap.Hard)
                    {
                        players[1].health = 10;
                    }
                    else if (handicap2 == Handicap.VeryHard)
                    {
                        players[1].health = 5;
                    }
                    if (handicap3 == Handicap.Easy)
                    {
                        players[2].health = 20;
                    }
                    else if (handicap3 == Handicap.Medium)
                    {
                        players[2].health = 15;
                    }
                    else if (handicap3 == Handicap.Hard)
                    {
                        players[2].health = 10;
                    }
                    else if (handicap3 == Handicap.VeryHard)
                    {
                        players[2].health = 5;
                    }
                    if (handicap4 == Handicap.Easy)
                    {
                        players[3].health = 20;
                    }
                    else if (handicap4 == Handicap.Medium)
                    {
                        players[3].health = 15;
                    }
                    else if (handicap4 == Handicap.Hard)
                    {
                        players[3].health = 10;
                    }
                    else if (handicap4 == Handicap.VeryHard)
                    {
                        players[3].health = 5;
                    }
                    fullPlayer1Health = players[0].health;
                    fullPlayer2Health = players[1].health;
                    fullPlayer3Health = players[2].health;
                    fullPlayer4Health = players[3].health;
                    #endregion
                    break;
            }
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            /*
            player.IsLooped = true;
            player.Play(backgroundvideo);
            */
            #region gamepadstate getting
            GamePadState gamePadState1 = GamePad.GetState(PlayerIndex.One);
            GamePadState gamePadState2 = GamePad.GetState(PlayerIndex.Two);
            GamePadState gamePadState3 = GamePad.GetState(PlayerIndex.Three);
            GamePadState gamePadState4 = GamePad.GetState(PlayerIndex.Four);
            #endregion

            #region rectangles for intersections
            Rectangle player1Rectangle = new Rectangle(0, 0, 0, 0);
            Rectangle player2Rectangle = new Rectangle(0, 0, 0, 0);
            Rectangle player3Rectangle = new Rectangle(0, 0, 0, 0);
            Rectangle player4Rectangle = new Rectangle(0, 0, 0, 0);

            //reference rectangle for the player
            //also used to tell IntersectPixels() when to check for intersections
            if (numberOfPlayers == 1)
            {
                player1Rectangle =
                    new Rectangle((int)players[0].position.X, (int)players[0].position.Y,
                        players[0].sprite.Width / playerRectangleScale, players[0].sprite.Height / playerRectangleScale);
            }
            else if (numberOfPlayers == 2)
            {
                player1Rectangle =
                    new Rectangle((int)players[0].position.X, (int)players[0].position.Y,
                        players[0].sprite.Width / 4, players[0].sprite.Height / 4);

                player2Rectangle =
                    new Rectangle((int)players[1].position.X, (int)players[1].position.Y,
                        players[1].sprite.Width / 4, players[1].sprite.Height / 4);
            }
            else if (numberOfPlayers == 3)
            {
                player1Rectangle =
                    new Rectangle((int)players[0].position.X, (int)players[0].position.Y,
                        players[0].sprite.Width / 4, players[0].sprite.Height / 4);

                player2Rectangle =
                    new Rectangle((int)players[1].position.X, (int)players[1].position.Y,
                        players[1].sprite.Width / 4, players[1].sprite.Height / 4);

                player3Rectangle =
                    new Rectangle((int)players[2].position.X, (int)players[2].position.Y,
                        players[2].sprite.Width / 4, players[2].sprite.Height / 4);
            }
            else if (numberOfPlayers == 4)
            {
                player1Rectangle =
                    new Rectangle((int)players[0].position.X, (int)players[0].position.Y,
                        players[0].sprite.Width / 4, players[0].sprite.Height / 4);

                player2Rectangle =
                    new Rectangle((int)players[1].position.X, (int)players[1].position.Y,
                        players[1].sprite.Width / 4, players[1].sprite.Height / 4);

                player3Rectangle =
                    new Rectangle((int)players[2].position.X, (int)players[2].position.Y,
                        players[2].sprite.Width / 4, players[2].sprite.Height / 4);

                player4Rectangle =
                    new Rectangle((int)players[3].position.X, (int)players[3].position.Y,
                        players[3].sprite.Width / 4, players[3].sprite.Height / 4);
            }
            #endregion

            //zombie dies with bullet-fix it
//            if(bullet.intersect(zombies[i]))
//            {
//                zombies[i].alive = false;
            //            }


            #region previosgamepadstates...
            previousGamePadState1 = gamePadState1;
            previousGamePadState2 = gamePadState2;
            previousGamePadState3 = gamePadState3;
            previousGamePadState4 = gamePadState4;
            #endregion

            #region update submethods
            UpdateZOMBIEMOVE();
            UpdateControllers();
            #endregion

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public void UpdateZOMBIEMOVE()
        {
            foreach (Zombie zombie in zombies)
            {
                if (zombie.alive)
                {
                    zombie.position = new Vector2(
                        viewportRect.Right, MathHelper.Lerp(
                        (float)viewportRect.Height * minzombieheight,
                        (float)viewportRect.Height * maxzombieheight,
                        (float)random.NextDouble()));
                    zombie.position += (zombie.velocity = new Vector2(
                        MathHelper.Lerp(
                        -minzombievelocity,
                        -maxzombievelocity,
                        (float)random.NextDouble()), 
                        MathHelper.Lerp(
                        minzombieymovement,
                        maxzombieymovement,
                        (float)random.NextDouble())));
                    if (!viewportRect.Contains(new Point(
                        (int)zombie.position.X,
                        (int)zombie.position.Y)))
                    {
                        zombie.alive = false;
                    }
                }
                else
                {
                    zombie.alive = true;
                    zombie.position = new Vector2(
                        viewportRect.Right, MathHelper.Lerp(
                        (float)viewportRect.Height * minzombieheight,
                        (float)viewportRect.Height * maxzombieheight,
                        (float)random.NextDouble()));
                    zombie.velocity = new Vector2(
                        MathHelper.Lerp(
                        -minzombievelocity,
                        -maxzombievelocity,
                        (float)random.NextDouble()), 
                        MathHelper.Lerp(
                        minzombieymovement,
                        maxzombieymovement,
                        (float)random.NextDouble()));
                }
            }
        }

        public void UpdateControllers()
        {

        }

        void OnCancel()
        {

            ScreenManager.RemoveScreen(this);
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            if (player.State != MediaState.Stopped)
                videoTexture = player.GetTexture();

            Rectangle screen = new Rectangle(ScreenManager.GraphicsDevice.Viewport.X,
                ScreenManager.GraphicsDevice.Viewport.Y,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Height);

            if (videoTexture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(videoTexture, screen, Color.White);
                spriteBatch.End();
            }
            */
            for (int i = 0; i < maxzombies; i++)
            {
                if (zombies[i].alive)
                {
                    {
                        spriteBatch.Draw(zombies[i].sprite,
                            zombies[i].position,
                            null,
                            Color.White,
                            zombies[i].rotation,
                            zombies[i].center,
                            0.25f,
                            SpriteEffects.None,
                            0);

                    }
                    
                }
            }

        }
    }
}