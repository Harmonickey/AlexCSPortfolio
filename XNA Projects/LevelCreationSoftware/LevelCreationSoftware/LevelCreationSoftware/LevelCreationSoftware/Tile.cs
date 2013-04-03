using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelCreationSoftware
{

    public class Tile : GameObject
    {
        public TileType type = TileType.Block;
        public int objectNumber = 1;
        public int layerNumber = 0;
        public Vector2 scrollOffset;

        public Tile(Texture2D loadedTexture)
        {
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

    }
}
