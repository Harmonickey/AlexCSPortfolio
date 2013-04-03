using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JAMGameFinal
{
    public class Zombie : GameObject
    {

        public int numberOfPlayersLeft;
        public Zombie zombie;
        public Hero player;

        public bool[] playerChecked = new bool[4];

        const float ZombieChaseDistance = 4405.8f;
        const float ZombieCaughtDistance = 10.0f;
        const float ZombieHysteresis = 15.0f;

        static float maxZombieSpeed = 3.0f;
        static float maxZombieTurning = 0.10f;

        enum ZombieAiState
        {
            Chasing,
            Caught,
        }
        ZombieAiState zombieState = ZombieAiState.Chasing;

        public Zombie(Texture2D loadedTexture)
            :base()
        {
            alive = false;
            sprite = loadedTexture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            health = 50;
            fullHealth = 50;
            textureData = new Color[sprite.Width * sprite.Height];
            sprite.GetData(textureData);
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

        public void CheckForCollision(Hero Player, int NumberOfPlayersLeft, int NumberOfZombies, Vector2 scrollOffset)
        {
            numberOfZombies = NumberOfZombies;
            numberOfPlayersLeft = NumberOfPlayersLeft;

            #region Transformations and Rectangles
            Matrix zombieTransform =
                Matrix.CreateTranslation(new Vector3(-center, 0.0f)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(new Vector3(position + scrollOffset, 0.0f));

            Rectangle zombieRectangle = CalculateBoundingRectangle(
                new Rectangle(0, 0, sprite.Width, sprite.Height),
                zombieTransform);

            Matrix rectangleTransform =
                Matrix.CreateTranslation(new Vector3(-Player.center, 0.0f)) *
                Matrix.CreateRotationZ(Player.rotation) *
                Matrix.CreateTranslation(new Vector3(Player.position + scrollOffset, 0.0f));

            Rectangle gunRect = CalculateBoundingRectangle(
                new Rectangle((int)3.5, (int)47.4, 2, 2),
                rectangleTransform);

            Rectangle rightForearmRect = CalculateBoundingRectangle(
                new Rectangle((int)28.9, (int)34.1, 2, 2),
                rectangleTransform);

            Rectangle rightElbowRect = CalculateBoundingRectangle(
                new Rectangle(46, (int)21.5, 2, 2),
                rectangleTransform);

            Rectangle rightShoulderRect = CalculateBoundingRectangle(
                new Rectangle((int)82.2, (int)28.2, 2, 2),
                rectangleTransform);

            Rectangle headRect = CalculateBoundingRectangle(
                new Rectangle((int)94.7, (int)47.4, 2, 2),
                rectangleTransform);

            Rectangle leftShoulderRect = CalculateBoundingRectangle(
                new Rectangle(84, (int)69.4, 2, 2),
                rectangleTransform);

            Rectangle leftElbowRect = CalculateBoundingRectangle(
                new Rectangle((int)45.6, (int)74.3, 2, 2),
                rectangleTransform);

            Rectangle leftForearmRect = CalculateBoundingRectangle(
                new Rectangle((int)29.3, (int)66.6, 2, 2),
                rectangleTransform);
            #endregion

            if (zombieRectangle.Intersects(gunRect) ||
                zombieRectangle.Intersects(rightForearmRect) ||
                zombieRectangle.Intersects(rightElbowRect) ||
                zombieRectangle.Intersects(rightShoulderRect) ||
                zombieRectangle.Intersects(headRect) ||
                zombieRectangle.Intersects(leftShoulderRect) ||
                zombieRectangle.Intersects(leftElbowRect) ||
                zombieRectangle.Intersects(leftForearmRect))
            {
                alive = false;
                NumberOfZombies--;
                Player.health--;
                if (Player.health <= 0)
                {
                    Player.alive = false;
                    NumberOfPlayersLeft--;
                }
            }

            numberOfZombies = NumberOfZombies;
            numberOfPlayersLeft = NumberOfPlayersLeft;

            zombie = this;
            player = Player;
        }

        public void UpdateZombies(bool? player1Alive, bool? player2Alive, bool? player3Alive, bool? player4Alive, Vector2 screenCenter, Vector2 scrollOffset, Hero[] players, int NumberOfPlayersLeft)
        {

            //the threshold of which the zombie is either chasing or actually caught the player(s)
            //for example if the zombie is within 10 pixels of the player he is considered caught and the 10 pixels is the threshold
            //  and everything else is within the chase threshold
            float zombieChaseThreshold = ZombieChaseDistance;
            float zombieCaughtThreshold = ZombieCaughtDistance;

            if (zombieState == ZombieAiState.Chasing)
            {
                zombieChaseThreshold += ZombieHysteresis / 2;
                zombieCaughtThreshold -= ZombieHysteresis / 2;
            }

            else if (zombieState == ZombieAiState.Caught)
            {
                zombieCaughtThreshold += ZombieHysteresis / 2;
            }

            UncheckPlayers();

            //gives a position for the moving zombies...towards the closest player by giving a heading, and speed
            
            if (alive)
            {
                #region Players 1
                if (NumberOfPlayersLeft == 1)
                {
                    int j = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);

                    UncheckPlayers();

                    //gives the distance from zombie to player
                    float distanceFromPlayer1 = Vector2.Distance(position + scrollOffset,
                        players[j].position + scrollOffset);
                    //if the distance from player is greater than the caught threshold...
                    if (distanceFromPlayer1 > zombieCaughtThreshold)
                    {
                        //then the zombie must still be chasing the player...
                        zombieState = ZombieAiState.Chasing;
                    }
                    else
                    {
                        //otherwise the player must be caught!
                        zombieState = ZombieAiState.Caught;

                    }

                    // the zombie's speed, either 0 or the max speed (then its added to the position vector)
                    float currentZombieSpeed;

                    //while the zombie is chasing he needs to know which way to chase
                    if (zombieState == ZombieAiState.Chasing)
                    {
                        //so towards the closest player he turns to face the player through the TurnToFace method further below
                        zombieOrientation = TurnToFace(position + scrollOffset, players[j].position + scrollOffset, zombieOrientation,
                            maxZombieTurning);

                        currentZombieSpeed = maxZombieSpeed;
                    }
                    else  //if the zombie isn't chasing then he has caught the player and there is no need to move further
                    {
                        currentZombieSpeed = 0.0f;
                    }
                    //the zombie's new heading is given by the sin and cos given above (heading is created through cos and sin vectors,
                    //      because it is a certain angle one faces in reference to a previous direction
                    Vector2 heading = new Vector2(
                    (float)Math.Cos(zombieOrientation), (float)Math.Sin(zombieOrientation));

                    position += heading * currentZombieSpeed;

                }
                #endregion
                #region Players 2
                if (NumberOfPlayersLeft == 2)
                {
                    int j = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);
                    int k = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);

                    UncheckPlayers();

                    float distanceFromPlayer1 = Vector2.Distance(position + scrollOffset,
                        players[j].position + scrollOffset);
                    float distanceFromPlayer2 = Vector2.Distance(position + scrollOffset,
                        players[k].position + scrollOffset);

                    if (distanceFromPlayer1 > zombieCaughtThreshold || distanceFromPlayer2 > zombieCaughtThreshold)
                    {
                        zombieState = ZombieAiState.Chasing;
                    }
                    else
                    {
                        zombieState = ZombieAiState.Caught;
                    }

                    float currentZombieSpeed;

                    if (zombieState == ZombieAiState.Chasing)
                    {
                        if (players[j].alive)
                        {
                            if (distanceFromPlayer1 < distanceFromPlayer2)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[j].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        if (players[k].alive)
                        {
                            if (distanceFromPlayer2 < distanceFromPlayer1)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[k].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        currentZombieSpeed = maxZombieSpeed;
                    }
                    else
                    {
                        currentZombieSpeed = 0.0f;
                    }

                    Vector2 heading = new Vector2(
                        (float)Math.Cos(zombieOrientation), (float)Math.Sin(zombieOrientation));

                    position += heading * currentZombieSpeed;
                }
                #endregion
                #region Players 3
                if (NumberOfPlayersLeft == 3)
                {
                    int j = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);
                    int k = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);
                    int l = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);

                    UncheckPlayers();

                    float distanceFromPlayer1 = Vector2.Distance(position + scrollOffset,
                        players[j].position + scrollOffset);
                    float distanceFromPlayer2 = Vector2.Distance(position + scrollOffset,
                        players[k].position + scrollOffset);
                    float distanceFromPlayer3 = Vector2.Distance(position + scrollOffset,
                        players[l].position + scrollOffset);


                    if (distanceFromPlayer1 > zombieCaughtThreshold || distanceFromPlayer2 > zombieCaughtThreshold || distanceFromPlayer3 > zombieCaughtThreshold)
                    {
                        zombieState = ZombieAiState.Chasing;
                    }
                    else
                    {
                        zombieState = ZombieAiState.Caught;
                    }

                    float currentZombieSpeed;

                    if (zombieState == ZombieAiState.Chasing)
                    {
                        if (players[0].alive)
                        {
                            if (distanceFromPlayer1 < distanceFromPlayer2 && distanceFromPlayer1 < distanceFromPlayer3)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[j].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        if (players[1].alive)
                        {
                            if (distanceFromPlayer2 < distanceFromPlayer1 && distanceFromPlayer2 < distanceFromPlayer3)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[k].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        if (players[2].alive)
                        {
                            if (distanceFromPlayer3 < distanceFromPlayer1 && distanceFromPlayer3 < distanceFromPlayer2)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[l].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        currentZombieSpeed = maxZombieSpeed;
                    }
                    else
                    {
                        currentZombieSpeed = 0.0f;
                    }

                    Vector2 heading = new Vector2(
                    (float)Math.Cos(zombieOrientation), (float)Math.Sin(zombieOrientation));

                    position += heading * currentZombieSpeed;
                }
                #endregion
                #region Players 4
                if (NumberOfPlayersLeft == 4)
                {
                    int j = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);
                    int k = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);
                    int l = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);
                    int m = GetPlayerValues(player1Alive, player2Alive, player3Alive, player4Alive);

                    UncheckPlayers();

                    float distanceFromPlayer1 = Vector2.Distance(position + scrollOffset,
                        players[j].position + scrollOffset);
                    float distanceFromPlayer2 = Vector2.Distance(position + scrollOffset,
                        players[k].position + scrollOffset);
                    float distanceFromPlayer3 = Vector2.Distance(position + scrollOffset,
                        players[l].position + scrollOffset);
                    float distanceFromPlayer4 = Vector2.Distance(position + scrollOffset,
                        players[m].position + scrollOffset);

                    if (distanceFromPlayer1 > zombieCaughtThreshold || distanceFromPlayer2 > zombieCaughtThreshold || distanceFromPlayer3 > zombieCaughtThreshold || distanceFromPlayer4 > zombieCaughtThreshold)
                    {
                        zombieState = ZombieAiState.Chasing;
                    }
                    else
                    {
                        zombieState = ZombieAiState.Caught;
                    }

                    float currentZombieSpeed;

                    if (zombieState == ZombieAiState.Chasing)
                    {
                        if (players[0].alive)
                        {
                            if (distanceFromPlayer1 < distanceFromPlayer2 && distanceFromPlayer1 < distanceFromPlayer3 && distanceFromPlayer1 < distanceFromPlayer4)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[j].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        if (players[1].alive)
                        {
                            if (distanceFromPlayer2 < distanceFromPlayer1 && distanceFromPlayer2 < distanceFromPlayer3 && distanceFromPlayer2 < distanceFromPlayer4)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[k].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        if (players[2].alive)
                        {
                            if (distanceFromPlayer3 < distanceFromPlayer1 && distanceFromPlayer3 < distanceFromPlayer2 && distanceFromPlayer3 < distanceFromPlayer4)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[l].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }
                        if (players[3].alive)
                        {
                            if (distanceFromPlayer4 < distanceFromPlayer1 && distanceFromPlayer4 < distanceFromPlayer2 && distanceFromPlayer4 < distanceFromPlayer3)
                            {
                                zombieOrientation = TurnToFace(position + scrollOffset, players[m].position + scrollOffset, zombieOrientation,
                                    maxZombieTurning);
                            }
                        }


                        currentZombieSpeed = maxZombieSpeed;
                    }
                    else
                    {
                        currentZombieSpeed = 0.0f;
                    }

                    Vector2 heading = new Vector2(
                    (float)Math.Cos(zombieOrientation), (float)Math.Sin(zombieOrientation));

                    position += heading * currentZombieSpeed;
                }
                #endregion
            }
        }


        public void UncheckPlayers()
        {
            for (int x = 0; x < 4; x++)
            {
                playerChecked[x] = false;
            }
        }

        //this returns which player is alive with a number that corresponds with the number they are in the array
        public int GetPlayerValues(bool? player1Alive, bool? player2Alive, bool? player3Alive, bool? player4Alive)
        {
            int playerNumber = 0;

            if (player1Alive == true && !playerChecked[0])
            {
                playerNumber = 0;
                playerChecked[0] = true;
            }
            else if (player2Alive == true && !playerChecked[1])
            {
                playerNumber = 1;
                playerChecked[1] = true;
            }
            else if (player3Alive == true && !playerChecked[2])
            {
                playerNumber = 2;
                playerChecked[2] = true;
            }
            else if (player4Alive == true && !playerChecked[3])
            {
                playerNumber = 3;
                playerChecked[3] = true;
            }
            
            return playerNumber;
            

        }

        private static float TurnToFace(Vector2 position, Vector2 faceThis,
            float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            float desiredAngle = (float)Math.Atan2(y, x);

            float difference = WrapAngle(desiredAngle - currentAngle);

            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            return WrapAngle(currentAngle + difference);
        }

        //the actual angle determined for rotation of the zombie
        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }
    }
}