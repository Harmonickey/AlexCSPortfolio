using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LbKStudiosGame
{
    class LevelOne : GamePlayScreen
    {
        #region Variables and Objects
        //able to be used to clamp falling acceleration if we want to have a terminal velocity when increasing fall speed
        private const float MaxFallSpeed = 3.0f;

        //the number of tiles to be used in level one
        private int numberOfTiles = 10;

        #endregion

        #region Initialization
        /// <summary>
        /// This is Level One to be loaded
        /// </summary>
        /// <param name="gamer">The passed in gamer profile</param>
        public LevelOne(SignedInGamer gamer)
        {
            GamerOne = gamer;
            LevelOn = 1;
        }

        #endregion

        #region Methods

        public override void LoadContent()
        {
            //load the base stuff first so we can load other stuff specific to this level
            base.LoadContent();

            tiles = new Tile[numberOfTiles];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(content.Load<Texture2D>("Sprites\\Tile"));
            }

            //I set the first tile position so that the others could just be iterated after it to make a floor
            tiles[0].position = new Vector2(500, 500);
            for (int i = 1; i < tiles.Length; i++)
            {
                //iterates right of the tiles[0] tile to create a floor of sorts
                tiles[i].position = new Vector2(tiles[i - 1].position.X + tiles[i].sprite.Width, 500);
                
            }

            player = new Player(content.Load<Texture2D>("Sprites\\Juan"));

            //start his position on top of the first tile
            player.position = new Vector2(tiles[0].position.X, tiles[0].position.Y - tiles[0].sprite.Height - 100);
        }
        /// <summary>
        /// Draw our level one stuff
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            screenCenter = new Vector2(ScreenWidth, ScreenHeight) / 2;
            scrollOffset = screenCenter - cameraPosition;

            SpriteBatch.Begin();

            DrawBackground();

            //may want to apply scrollOffset to position vectors if there is scrolling involved

            DrawPlayer();

            DrawTiles();
            
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawBackground()
        {
            SpriteBatch.Draw(gameBackgroundTexture, new Rectangle(0,0, viewport.Width, viewport.Height), Color.White);
        }

        void DrawPlayer()
        {
            SpriteBatch.Draw(player.sprite,
                player.position,
                null,
                Color.White,
                0.0f,
                player.center,
                1.0f,
                SpriteEffects.None, 0);
        }

        void DrawTiles()
        {
            foreach (Tile tile in tiles)
            {
                SpriteBatch.Draw(tile.sprite,
                    tile.position,
                    null,
                    Color.White,
                    0.0f,
                    tile.center,
                    1.0f,
                    SpriteEffects.None, 0);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            
            //add any important logic here that is specific to level one

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);

        }

        #endregion
    }
}
