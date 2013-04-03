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
    public class PlayTestScreen : GamePlayScreen
    {
        string path;

        int numberOfTiles;
        int numberOfBackgrounds;

        private bool keepFlipped = false;

        public PlayTestScreen(string path)
        {
            
            
            this.path = path;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();  //read the current line

                    string[] parts = line.Split(',');

                    if (parts[5] == "Tile")
                    {
                        numberOfTiles++;
                    }
                    if (parts[5] == "Background")
                    {
                        numberOfBackgrounds++;
                    }
                }

                tiles = new Tile[numberOfTiles];
                backgrounds = new Background[numberOfBackgrounds];
                sr.Close();
            }

            using (StreamReader sr = new StreamReader(path))
            {

                int i = 0;    //tiles
                int j = 0;    //backgrounds

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
                        
                        tiles[i] = new Tile(content.Load<Texture2D>("Sprites\\" + parts[2] + "\\" + parts[2] + objectNumber.ToString()));
                        tiles[i].position = new Vector2(X, Y);
                        tiles[i].type = type;
                        tiles[i].objectNumber = objectNumber;
                        tiles[i].layerNumber = layerNumber;

                        tiles[i].textureData =
                        new Color[tiles[i].sprite.Width * tiles[i].sprite.Height];
                        tiles[i].sprite.GetData(tiles[i].textureData);

                        i++;
                    }
                    else if (parts[5] == "Background")
                    {

                        if (parts[2] == "Normal")
                            bType = BackgroundType.Normal;
                        if (parts[2] == "Other")
                            bType = BackgroundType.Other;

                        backgrounds[j] = new Background(content.Load<Texture2D>("Backgrounds\\BackgroundTypes\\" + parts[2] + "\\" + parts[2] + "Background" + objectNumber.ToString()));
                        backgrounds[j].position = new Vector2(X, Y);
                        backgrounds[j].type = bType;
                        backgrounds[j].objectNumber = objectNumber;
                        backgrounds[j].layerNumber = layerNumber;

                        j++;
                    }
                }
                sr.Close();
            }

            LevelCreateSession = false;

            Rectangle titleSafeArea = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;

            player = new Player(content.Load<Texture2D>("Sprites\\player2.0"));

            playerCollisionReference = new Player(content.Load<Texture2D>("Sprites\\playerCollisionReference"));
            playerCollisionReference.textureData =
                        new Color[playerCollisionReference.sprite.Width * playerCollisionReference.sprite.Height];
            playerCollisionReference.sprite.GetData(playerCollisionReference.textureData);

            //start his position on top of the first tile
            
            player.position = new Vector2(tiles[0].position.X - 50, tiles[0].position.Y - tiles[0].sprite.Height - 100);

        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            DrawBackground();

            DrawTiles();

            DrawPlayer();

            SpriteBatch.DrawString(ScreenManager.Font, scrollOffset.ToString(), new Vector2(500, 500), Color.White);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawBackground()
        {
            
            foreach (Background background in backgrounds)
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

                Vector2 position = background.position + background.scrollOffset;

                SpriteBatch.Draw(background.sprite,
                    position,
                    null, 
                    Color.White,
                    0.0f,
                    new Vector2(0.0f),
                    1.0f,
                    SpriteEffects.None, 0);
            }

        }

        

        void DrawPlayer()
        {

            Vector2 position = player.position + scrollOffset;
            SpriteEffects spriteEffect = GetSpriteEffect();

            SpriteBatch.Draw(player.sprite,
                position,
                null,
                Color.White,
                player.rotation,
                player.center,
                1.0f,
                spriteEffect, 1);

            /*
            playerTransform =
                Matrix.CreateTranslation(new Vector3(-player.center, 0.0f)) *
                Matrix.CreateRotationZ(player.rotation) *
                Matrix.CreateTranslation(new Vector3(player.position + scrollOffset, 0.0f));

            playerRectangle = GameObject.CalculateBoundingRectangle(
                new Rectangle(0, 0, player.sprite.Width, player.sprite.Height),
                playerTransform);

            SpriteBatch.Draw(barOutline, playerRectangle, Color.White);
            */

            
        }

        void DrawTiles()
        {
            foreach (Tile tile in tiles)
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

                Vector2 position = tile.position + tile.scrollOffset;
                
                if (tile.type == TileType.Scenery && tile.objectNumber >= 15)
                {

                    SpriteBatch.Draw(tile.sprite,
                        position,
                        null,
                        Color.White,
                        tile.rotation,
                        tile.center,
                        0.1f,
                        SpriteEffects.None, 0);


                }
                else
                {
                    SpriteBatch.Draw(tile.sprite,
                        position,
                        null,
                        Color.White,
                        tile.rotation,
                        tile.center,
                        1.0f,
                        SpriteEffects.None, 0);

                    //tileRectangle = new Rectangle((int)((tile.position.X - tile.center.X) + scrollOffset.X), (int)((tile.position.Y - tile.center.Y) + scrollOffset.Y), tile.sprite.Width, tile.sprite.Height);

                    //SpriteBatch.Draw(barOutline, tileRectangle, Color.White);
                }
            }
        }

        //flip the sprite if he is turning another direction or keep the spriteeffect even if there is no movement, so check direction vector
        private SpriteEffects GetSpriteEffect()
        {
            SpriteEffects spriteEffect = SpriteEffects.None;

            if (player.velocity.X == 0 && keepFlipped)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
                return spriteEffect;
            }
            else
            {
                if (player.velocity.X < 0)
                {
                    keepFlipped = true;
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    return spriteEffect;
                }
                else
                {
                    keepFlipped = false;
                    spriteEffect = SpriteEffects.None;
                    return spriteEffect;
                }
            }
        }

       

    }
}
