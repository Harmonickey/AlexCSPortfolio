using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    public class OtherObject : GameObject
    {

        public OtherObject(Texture2D loadedTexture)
            : base()
        {
            this.alive = true;
            this.sprite = loadedTexture;
            this.center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            this.text = String.Empty;
        }
        
    }
}