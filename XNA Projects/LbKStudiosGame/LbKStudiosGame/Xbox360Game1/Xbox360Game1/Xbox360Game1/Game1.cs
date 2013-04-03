using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Xbox360Game1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: Load your game content here            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        IAsyncResult result;
        Object stateobj;
        bool GameSaveRequested = false;
        GamePadState currentState;

        protected override void Update(GameTime gameTime)
        {
            GamePadState previousState = currentState;
            currentState = GamePad.GetState(PlayerIndex.One);
            // Allows the default game to exit on Xbox 360 and Windows
            if (currentState.Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if ((currentState.Buttons.A == ButtonState.Pressed) &&
                (previousState.Buttons.A == ButtonState.Released))
            {
                // Set the request flag
                if ((!Guide.IsVisible) && (GameSaveRequested == false))
                {
                    GameSaveRequested = true;
                    result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);
                }
            }

            if ((currentState.Buttons.B == ButtonState.Pressed) &&
                (previousState.Buttons.B == ButtonState.Released))
            {
                if (!Guide.IsVisible)
                {
                    // Reset the device
                    device = null;
                    stateobj = (Object)"GetDevice for Player One";
                    StorageDevice.BeginShowSelector(
                            PlayerIndex.One, this.GetDevice, stateobj);
                }
            }
            // If a save is pending, save as soon as the
            // storage device is chosen
            if ((GameSaveRequested) && (result.IsCompleted))
            {
                StorageDevice device = StorageDevice.EndShowSelector(result);
                if (device != null && device.IsConnected)
                {
                    DoSaveGame(device);
                    DoLoadGame(device);
                    DoCreate(device);
                    DoOpen(device);
                    DoCopy(device);
                    DoEnumerate(device);
                    DoRename(device);
                    DoDelete(device);
                    DoOpenFile();
                }
                // Reset the request flag
                GameSaveRequested = false;
            }
            base.Update(gameTime);
        }

        StorageDevice device;
        void GetDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);
            if (device != null && device.IsConnected)
            {
                DoSaveGame(device);
                DoLoadGame(device);
                DoCreate(device);
                DoOpen(device);
                DoCopy(device);
                DoEnumerate(device);
                DoRename(device);
                DoDelete(device);
                DoOpenFile();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
        
        public struct SaveGameData
        {
            public string PlayerName;
            public Vector2 AvatarPosition;
            public int Level;
            public int Score;
        }
        /// <summary>
        /// This method serializes a data object into
        /// the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        private static void DoSaveGame(StorageDevice device)
        {
            // Create the data to save.
            SaveGameData data = new SaveGameData();
            data.PlayerName = "Hiro";
            data.AvatarPosition = new Vector2(360, 360);
            data.Level = 11;
            data.Score = 4200;

            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "savegame.sav";

            // Check to see whether the save exists.
            if (container.FileExists(filename))
                // Delete it so that we can create one fresh.
                container.DeleteFile(filename);

            // Create the file.
            Stream stream = container.CreateFile(filename);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(stream, data);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }
        /// <summary>
        /// This method loads a serialized data object
        /// from the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        private static void DoLoadGame(StorageDevice device)
        {
            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "savegame.sav";

            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                return;
            }

            // Open the file.
            Stream stream = container.OpenFile(filename, FileMode.Open);

            // Read the data from the file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = (SaveGameData)serializer.Deserialize(stream);

            // Close the file.
            stream.Close();

            // Dispose the container.
            container.Dispose();

            // Report the data to the console.
            Debug.WriteLine("Name:     " + data.PlayerName);
            Debug.WriteLine("Level:    " + data.Level.ToString());
            Debug.WriteLine("Score:    " + data.Score.ToString());
            Debug.WriteLine("Position: " + data.AvatarPosition.ToString());
        }
        /// <summary>
        /// This method creates a file called demobinary.sav and places
        /// it in the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        private static void DoCreate(StorageDevice device)
        {
            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";

            // Create a new file.
            if (!container.FileExists(filename))
            {
                Stream file = container.CreateFile(filename);
                file.Close();
            }
            // Dispose the container, to commit the data.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to open a file. It presumes
        /// that demobinary.sav has been created.
        /// </summary>
        /// <param name="device"></param>
        private static void DoOpen(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";

            Stream file = container.OpenFile(filename, FileMode.Open);
            file.Close();

            // Dispose the container.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to copy files.  It presumes
        /// that demobinary.sav has been created.
        /// </summary>
        /// <param name="device"></param>
        private static void DoCopy(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";
            string copyfilename = "copybinary.sav";

            if (container.FileExists(filename) && !container.FileExists(copyfilename))
            {
                Stream file = container.OpenFile(filename, FileMode.Open);
                Stream copyfile = container.CreateFile(copyfilename);
                //file.CopyTo(copyfile);

                file.Close();
                copyfile.Close();
            }

            // Dispose the container, to commit the change.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to rename files.  It presumes
        /// that demobinary.sav has been created.
        /// </summary>
        /// <param name="device"></param>
        private static void DoRename(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string oldfilename = "demobinary.sav";
            string newfilename = "renamebinary.sav";

            if (container.FileExists(oldfilename) && !container.FileExists(newfilename))
            {
                Stream oldfile = container.OpenFile(oldfilename, FileMode.Open);
                Stream newfile = container.CreateFile(newfilename);
                //oldfile.CopyTo(newfile);

                oldfile.Close();
                newfile.Close();
                container.DeleteFile(oldfilename);
            }

            // Dispose the container, to commit the change.
            container.Dispose();
        }
        /// <summary>
        /// This method illustrates how to enumerate files in a 
        /// StorageContainer.
        /// </summary>
        /// <param name="device"></param>
        private static void DoEnumerate(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string[] FileList = container.GetFileNames();
            foreach (string filename in FileList)
            {
                Console.WriteLine(filename);
            }

            // Dispose the container.
            container.Dispose();
        }
        /// <summary>
        /// This method deletes a file previously created by this demo.
        /// </summary>
        /// <param name="device"></param>
        private static void DoDelete(StorageDevice device)
        {
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Add the container path to our file name.
            string filename = "demobinary.sav";

            if (container.FileExists(filename))
            {
                container.DeleteFile(filename);
            }

            // Dispose the container, to commit the change.
            container.Dispose();
        }
        /// <summary>
        /// This method opens a file using System.IO classes and the
        /// TitleLocation property.  It presumes that a file named
        /// ship.dds has been deployed alongside the game.
        /// </summary>
        private static void DoOpenFile()
        {
            try
            {
                System.IO.Stream stream = TitleContainer.OpenStream("ship.dds");
                System.IO.StreamReader sreader = new System.IO.StreamReader(stream);
                // use StreamReader.ReadLine or other methods to read the file data

                Console.WriteLine("File Size: " + stream.Length);
                stream.Close();
            }
            catch (System.IO.FileNotFoundException)
            {
                // this will be thrown by OpenStream if gamedata.txt
                // doesn't exist in the title storage location
            }
        }
    }

}