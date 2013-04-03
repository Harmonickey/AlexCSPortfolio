using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;

namespace LevelCreationSoftware
{
    class VoidTest : GamePlayScreen
    {
       
        Texture2D background;
        Rectangle renderTarget;
        Texture2D renderTargetReference;
        Rectangle renderTargetReferenceRectangle;
        ParticleEmitter emitter;
        
        bool isVoided;

        public VoidTest()
        {
            LevelCreateSession = true;
        }

        
        public override void LoadContent()
        {
            base.LoadContent();
            
            renderTargetReference = content.Load<Texture2D>("Sprites\\barOutline");
            renderTargetReferenceRectangle = new Rectangle(0, 0, 100, 100);

            background = content.Load<Texture2D>("Backgrounds\\GameBackgrounds\\background");

            emitter = new ParticleEmitter(EmitterSystem, 60, new Vector2(400, 240));
            

        }

        public override void HandleInput(InputState input)
        {
            
            if (input.CurrentMouseStates[(int)PlayerIndex.One].LeftButton == ButtonState.Pressed && input.PreviousMouseStates[(int)PlayerIndex.One].LeftButton == ButtonState.Released)
            {
               
                if (!isVoided)
                {
                    isVoided = true;
                    renderTarget = new Rectangle((int)input.CurrentMouseStates[(int)PlayerIndex.One].X, (int)input.CurrentMouseStates[(int)PlayerIndex.One].Y, 100, 100);
                }
                else
                {
                    isVoided = false;
                }
                
            }

            renderTargetReferenceRectangle.X = input.CurrentMouseStates[(int)PlayerIndex.One].X;
            renderTargetReferenceRectangle.Y = input.CurrentMouseStates[(int)PlayerIndex.One].Y;
            
            base.HandleInput(input);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreens)
        {
            if (isVoided)
            {
                UpdateEmitter(gameTime);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreens);
        }

        private void UpdateEmitter(GameTime gameTime)
        {
            // start with our current position
            Vector2 newPosition = emitter.Position;
            
            // Windows and Windows Phone use our Mouse class to update
            // the position of the emitter.
            MouseState mouseState = Mouse.GetState();
            newPosition = new Vector2(mouseState.X, mouseState.Y);

            // updating the emitter not only assigns a new location, but handles creating
            // the particles for our system based on the particlesPerSecond parameter of
            // the ParticleEmitter constructor.
            emitter.Update(gameTime, newPosition);
        }

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch.Begin();

            SpriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            
            SpriteBatch.Draw(renderTargetReference, renderTargetReferenceRectangle, Color.White);

            SpriteBatch.End(); 
            
            base.Draw(gameTime);
            
        }



    }
}
