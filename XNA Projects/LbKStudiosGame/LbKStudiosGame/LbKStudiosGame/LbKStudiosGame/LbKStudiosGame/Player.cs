using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LbKStudiosGame
{
    public class Player : GameObject
    {
        //give the player a score
        public int score;

        public Player(Texture2D loadedTexture)
        {
            sprite = loadedTexture;
            alive = true;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            score = 0;

        }

    }
}
