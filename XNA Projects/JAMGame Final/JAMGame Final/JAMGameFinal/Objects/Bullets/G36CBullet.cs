using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    public class G36CBullet : GameObject
    {
        
        public G36CBullet(Texture2D loadedTexture)
        {
            alive = false;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            textureData =
                        new Color[sprite.Width * sprite.Height];
            sprite.GetData(textureData);

        }

        public void CheckForCollision(Hero Player, Zombie Zombie, int NumberOfZombies, int NumberOfZombiesKilled, Vector2 scrollOffset)
        {
            collision = false;
            #region Bullets to Zombies

            Matrix bulletTransform =
                Matrix.CreateTranslation(new Vector3(-center, 0.0f)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(new Vector3(position + scrollOffset, 0.0f));

            Rectangle bulletRectangle = CalculateBoundingRectangle(
                new Rectangle(0, 0, sprite.Width, sprite.Height),
                bulletTransform);

            Matrix zombieTransform =
                Matrix.CreateTranslation(new Vector3(-Zombie.center, 0.0f)) *
                Matrix.CreateRotationZ(Zombie.rotation) *
                Matrix.CreateTranslation(new Vector3(Zombie.position + scrollOffset, 0.0f));

            Rectangle zombieRectangle = CalculateBoundingRectangle(
                new Rectangle(0, 0, Zombie.sprite.Width, Zombie.sprite.Height),
                zombieTransform);

            if (bulletRectangle.Intersects(zombieRectangle))
            {
                if (IntersectPixels(bulletTransform, sprite.Width,
                            sprite.Height, textureData,
                            zombieTransform, Zombie.sprite.Width,
                            Zombie.sprite.Height, Zombie.textureData))
                {

                    Zombie.health -= 100;
                    collision = true;
                    if (Zombie.health <= 0)
                    {
                        NumberOfZombies--;
                        NumberOfZombiesKilled++;
                        Zombie.alive = false;
                        Zombie.health = Zombie.fullHealth;
                    }

                    alive = false;
                    if (Player.playerCharacter == Character.Wilhelm)
                    {
                        Player.score += 10;
                    }
                }
            }
            #endregion

            numberOfZombies = NumberOfZombies;
            numberOfZombiesKilled = NumberOfZombiesKilled;

        }
        
        private Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                                           Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }


        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels between two
        /// sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        private static bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }
    }
}