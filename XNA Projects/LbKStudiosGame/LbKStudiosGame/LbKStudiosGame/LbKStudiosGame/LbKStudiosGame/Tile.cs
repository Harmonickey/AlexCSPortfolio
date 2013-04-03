using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LbKStudiosGame
{

    public class Tile : GameObject
    {
        
        public Tile(Texture2D loadedTexture)
        {
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);

        }

    }
}
