using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LbKStudiosGame
{
    public abstract class GameObject
    {
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public bool isColliding;
        public bool collision;

        public bool Collision(Rectangle primaryRectangle, Rectangle secondaryRectangle)
        {
            
            if (primaryRectangle.Intersects(secondaryRectangle) || primaryRectangle.Contains(secondaryRectangle))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool Collision(BoundingSphere primarySphere, BoundingSphere secondarySphere)
        {
            //example of what a circle code should look like
            //primarySphere = new BoundingSphere(new Vector3(center, 0), center.Length());

            ContainmentType contains = primarySphere.Contains(secondarySphere);

            if (primarySphere.Intersects(secondarySphere) || contains == ContainmentType.Intersects)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
