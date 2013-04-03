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

namespace Silhouetta
{
    class Level : GamePlayScreen
    {
        List<Rectangle> tileRectangles = new List<Rectangle>();
        List<Rectangle> backgroundRectangles = new List<Rectangle>();

        List<Tile> Tiles = new List<Tile>();
        Tile tempLoadedTile;

        List<Background> Backgrounds = new List<Background>();
        Background tempLoadedBackground;

        public override void LoadContent()
        {
            string path = "Content\\Levels\\Level" + LbKStorage.Level.ToString() + ".txt";
            
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

            tiles = new Tile[Tiles.Count];
            for (int i = 0; i < Tiles.Count; i++)
            {
                tiles[i] = Tiles[i];
            }
            backgrounds = new Background[Backgrounds.Count];
            for (int i = 0; i < Tiles.Count; i++)
            {
                backgrounds[i] = Backgrounds[i];
            }

            player = new Player(content.Load<Texture2D>("Sprites\\player2.0"));

            if (LbKStorage.Level == 1)
            {
                if (LbKStorage.CheckPoint == 1)
                {
                    player.position = new Vector2(100.0f);
                }
            }

            base.LoadContent();
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
                    (background.position + background.scrollOffset),
                    null,
                    Color.White,
                    background.rotation,
                    new Vector2(0.0f),
                    1.0f,
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
                        (tile.position + tile.scrollOffset),
                        null,
                        Color.White,
                        tile.rotation,
                        new Vector2(0.0f),
                        1.0f,
                        SpriteEffects.None, 0);

                }
                else
                {
                    SpriteBatch.Draw(tile.sprite,
                        (tile.position + tile.scrollOffset),
                        null,
                        Color.White,
                        tile.rotation,
                        new Vector2(0.0f),
                        1.0f,
                        SpriteEffects.None, 0);
                }


            }

            SpriteBatch.Draw(player.sprite,
                player.position,
                null,
                Color.White,
                player.rotation,
                new Vector2(0.0f),
                1.0f,
                SpriteEffects.None, 0);

            SpriteBatch.End();

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }
    }
}
