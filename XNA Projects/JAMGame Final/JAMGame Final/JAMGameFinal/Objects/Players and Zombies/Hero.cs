using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    public class Hero : GameObject
    {
        
        public Hero(Texture2D loadedTexture)
        {
            alive = true;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            textureData = new Color[sprite.Width * sprite.Height];
            sprite.GetData(textureData);

        }

        public void LoadUpBulletManager()
        {
            if (playerCharacter == Character.Juan)
            {
                juanBulletManager = new JuanBulletManager();
            }
            if (playerCharacter == Character.Sayid)
            {
                sayidBulletManager = new SayidBulletManager();
            }
            if (playerCharacter == Character.Sir_Edward)
            {
                sirEdwardBulletManager = new SirEdwardBulletManager();
            }
            if (playerCharacter == Character.Wilhelm)
            {
                wilhelmBulletManager = new WilhelmBulletManager();
            }
        }

    }
}