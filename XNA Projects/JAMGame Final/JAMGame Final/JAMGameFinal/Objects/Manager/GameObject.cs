using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    abstract public class GameObject
    {
        
        public JuanBulletManager juanBulletManager;
        public SirEdwardBulletManager sirEdwardBulletManager;
        public SayidBulletManager sayidBulletManager;
        public WilhelmBulletManager wilhelmBulletManager;
        
        //object properties
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public int health;
        public int fullHealth;

        //special object properties
        public Vector2 healthBarPosition;
        public Rectangle sourceRectangle;
        public string text;
        public Vector2 textPosition;

        //special player properties
        public Character playerCharacter;
        public Handicap playerHandicap;
        public float triggerSensitivity;
        public int score;
        public Color[] textureData;

        public int numberOfZombies;
        public int numberOfZombiesKilled;

        public float zombieOrientation;

        public bool collision;

    }
}