using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Silhouetta
{
    public enum BackgroundType
    {
        Normal,
        Other
    }

    public class Background : GameObject
    {
        public BackgroundType type = BackgroundType.Normal;
        public int objectNumber = 1;
        public int layerNumber = 0;
        public Vector2 scrollOffset;

        public Background(Texture2D loadedTexture)
        {
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

    }
}
