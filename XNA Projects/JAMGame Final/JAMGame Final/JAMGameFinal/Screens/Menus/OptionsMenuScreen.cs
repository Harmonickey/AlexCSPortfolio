using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace JAMGameFinal
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : GameScreen
    {
        
        //a few misc. game objects being used in the menu screen
        OtherObject sensitivityDisplay;          //The actual sensitivity Heads Up Display
        OtherObject leftBunker;                  //The actual left bunker object
        OtherObject rightBunker;                 //The actual right bunker object

        //you'll be able to modify this in the UpdateSensitivity method
        Rectangle sourceRectangleSen;              //Sensitivity Bar Rectangle from sprite sheet

        //you'll be able to modify this in the Load Content Method
        Rectangle sourceRectangleLeft;               //Left Bunker Rectangle from sprite sheet
        Rectangle sourceRectangleRight;              //Right Bunker Rectangle from sprite sheet

        float playerTriggerSensitivity;

        Texture2D backgroundTexture;

        ContentManager content;
       
        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen(float triggerSen)
        {
            playerTriggerSensitivity = triggerSen;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //loads the sprite sheets for the sensitivity display, the left bunker, and the right bunker
            
            sensitivityDisplay = new OtherObject(content.Load<Texture2D>("Sprites\\Misc\\Sensitivity_Sprite_Sheet"));
            leftBunker = new OtherObject(content.Load<Texture2D>("Sprites\\Misc\\xboxControllerSpriteSheet"));
            rightBunker = new OtherObject(content.Load<Texture2D>("Sprites\\Misc\\xboxControllerSpriteSheet"));

            sourceRectangleSen = new Rectangle(0, 0, 250, 100);

            backgroundTexture = content.Load<Texture2D>("Background\\background");
            
            //loads the starting up left bunker source rectangle from the sprite sheet
            //loads the starting up right bunker source rectangle from the sprite sheet
            sourceRectangleLeft = new Rectangle(658, 3, 90, 75);
            sourceRectangleRight = new Rectangle(488, 3, 90, 75);

        }

        public override void HandleInput(InputState input)
        {
            int playerIndex = (int)ControllingPlayer.Value;

            if (input.CurrentGamePadStates[playerIndex].Buttons.RightShoulder == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.RightShoulder == ButtonState.Released)
            {
                playerTriggerSensitivity += 0.33f;
                if (playerTriggerSensitivity > 1.0f)
                    playerTriggerSensitivity = 0.99f;
            }
            if (input.CurrentGamePadStates[playerIndex].Buttons.LeftShoulder == ButtonState.Pressed && input.PreviousGamePadStates[playerIndex].Buttons.LeftShoulder == ButtonState.Released)
            {
                playerTriggerSensitivity -= 0.33f;
                if (playerTriggerSensitivity < 0.0f)
                    playerTriggerSensitivity = 0.0f;
            }
            if (input.CurrentGamePadStates[playerIndex].Buttons.B == ButtonState.Pressed)
            {
                OnCancel();
            }
            TriggerSensitivity = playerTriggerSensitivity;
            UpdateSensitivity();

            base.HandleInput(input);
        }

        void OnCancel()
        {
            ScreenManager.RemoveScreen(this);
        }

        private void UpdateSensitivity()
        {
            #region DisplayPosition, TextPosition, BunkerPositions
        
            sensitivityDisplay.position = new Vector2(100, 225);
            sensitivityDisplay.textPosition = new Vector2(375, 250);
            leftBunker.position = new Vector2(100, 275);
            rightBunker.position = new Vector2(260, 275);

            #endregion

            #region ChangeSourceRectangle for Sensitivity
            
            if (playerTriggerSensitivity == 0.0f)
            {
                sourceRectangleSen = new Rectangle(0, 0, 250, 100);
                sensitivityDisplay.text = "High Sensitivity";
            }
            if (playerTriggerSensitivity > 0.3f && playerTriggerSensitivity < 0.4f)
            {
                sourceRectangleSen = new Rectangle(250, 0, 250, 100);
                sensitivityDisplay.text = "Medium Sensitivity";
            }
            if (playerTriggerSensitivity > 0.6f && playerTriggerSensitivity < 0.7f)
            {
                sourceRectangleSen = new Rectangle(500, 0, 250, 100);
                sensitivityDisplay.text = "Low Sensitivity";
            }
            if (playerTriggerSensitivity == 0.99f)
            {
                sourceRectangleSen = new Rectangle(750, 0, 250, 100);
                sensitivityDisplay.text = "Very Low" +
                                              "\nSensitivity";
            }
        
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteFont font = ScreenManager.Font;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(backgroundTexture, fullscreen,
                            new Color(fade, fade, fade));
            ScreenManager.SpriteBatch.Draw(sensitivityDisplay.sprite, sensitivityDisplay.position, sourceRectangleSen, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.Draw(leftBunker.sprite, leftBunker.position, sourceRectangleLeft, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.Draw(rightBunker.sprite, rightBunker.position, sourceRectangleRight, Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.DrawString(font, sensitivityDisplay.text, sensitivityDisplay.textPosition, Color.White);

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
