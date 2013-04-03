using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace LevelCreationSoftware
{
    public abstract class LbKStorageLevelCreation
    {
        #region Variables, Objects, and Fields
        
        static Vector2[] position;
        public static Vector2[] Position
        {
            get { return position; }
            set { position = value; }
        }

        static TileType[] type;
        public static TileType[] Type
        {
            get { return type; }
            set { type = value; }
        }

        static int[] objectNumber;
        public static int[] ObjectNumber
        {
            get { return objectNumber; }
            set { objectNumber = value; }
        }

        static int count;
        public static int Count
        {
            get { return count; }
            set { count = value; }
        }

        static List<string> fileNames;
        public static List<string> FileNames
        {
            get { return fileNames; }
            set { fileNames = value; }
        }
        

        #endregion

        #region SaveGameData

        public struct SaveGameData
        {
            public Vector2[] TilePosition;
            public TileType[] TileType;
            public int[] TileObjectNumber;
            public int TileCount;

            public List<string> Names;
        }
        /// <summary>
        /// This method save to the filename indicated by the user.  Also it saves to the universal file LbKTileData so as to get filenames to load from
        /// separately later
        /// </summary>
        /// <param name="device"></param>
        /// <param name="gamer"></param>
        /// <param name="playTest"></param>
        public static void SaveGame(StorageDevice device, SignedInGamer gamer, bool playTest)
        {
            
            SaveGameData data = new SaveGameData();
            data.TilePosition = new Vector2[position.Length];
            data.TilePosition = position;

            data.TileType = new TileType[type.Length];
            data.TileType = type;

            data.TileObjectNumber = new int[objectNumber.Length];
            data.TileObjectNumber = objectNumber;

            data.TileCount = count;

            data.Names = new List<string>();
            data.Names = fileNames;
            
            IAsyncResult result =
                device.BeginOpenContainer(gamer.Gamertag, null, null);

            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            //string filename = "LbKTileData.sav";
            string filename = string.Empty;

            filename = fileNames[fileNames.Count - 1] + ".sav";

            if (playTest)
            {
                filename = "TemporaryTileSave.sav";
            }
            
            if (!container.FileExists(filename))
            {
                Stream file = container.CreateFile(filename);

                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

                serializer.Serialize(file, data);

                file.Close();
            }
            else
            {

                container.DeleteFile(filename);

                Stream file = container.CreateFile(filename);

                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

                serializer.Serialize(file, data);

                file.Close();
            }

            /*
            // Dispose the container, to commit the data.
            container.Dispose();

            //do secondary save


            result =
                device.BeginOpenContainer(gamer.Gamertag, null, null);

            result.AsyncWaitHandle.WaitOne();

            container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();
            */

            filename = "LbKTileData.sav";

            if (!container.FileExists(filename))
            {
                Stream file = container.CreateFile(filename);

                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

                serializer.Serialize(file, data);

                file.Close();
            }
            else
            {

                container.DeleteFile(filename);

                Stream file = container.CreateFile(filename);

                XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));

                serializer.Serialize(file, data);

                file.Close();
            }

            // Dispose the container, to commit the data.
            container.Dispose();
            

        }

        /// <summary>
        /// This method gets the filenames from the universal storage file LbKTileData.sav
        /// </summary>
        /// <param name="device"></param>
        /// <param name="gamer"></param>
        /// <param name="fileNamesOnly"></param>
        public static void LoadGame(StorageDevice device, SignedInGamer gamer, bool fileNamesOnly)
        {
            // Open a storage container.
            // name of container is LbK Storage Device
            IAsyncResult result =
                device.BeginOpenContainer(gamer.Gamertag, null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "LbKTileData.sav";

            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                return;
            }

            // Open the file.
            Stream file = container.OpenFile(filename, FileMode.Open);

            // Read the data from the file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = (SaveGameData)serializer.Deserialize(file);

            // Close the file.
            file.Close();

            // Dispose the container.
            container.Dispose();

            // Report the data to the console.
            if (fileNamesOnly)
            {
                fileNames = data.Names;
            }
            else
            {
                position = data.TilePosition;
                type = data.TileType;
                objectNumber = data.TileObjectNumber;
                count = data.TileCount;
                fileNames = data.Names;
            }

            GamePlayScreen.storageDevice = device;
            // load up game with respective device


        }

        /// <summary>
        /// This loads the specific game from the filename indicated by the user.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="gamer"></param>
        /// <param name="levelName"></param>
        public static void LoadGame(StorageDevice device, SignedInGamer gamer, string levelName)
        {
            // Open a storage container.
            // name of container is LbK Storage Device
            IAsyncResult result =
                device.BeginOpenContainer(gamer.Gamertag, null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();
            //".sav" IS VERY IMPORTANT GOD DAMMIT!!!
            string filename = levelName + ".sav";

            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                return;
            }

            // Open the file.
            Stream file = container.OpenFile(filename, FileMode.Open);

            // Read the data from the file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = (SaveGameData)serializer.Deserialize(file);

            // Close the file.
            file.Close();

            // Dispose the container.
            container.Dispose();

            // Report the data to the console.
            position = data.TilePosition;
            type = data.TileType;
            objectNumber = data.TileObjectNumber;
            count = data.TileCount;
            fileNames = data.Names;

            GamePlayScreen.storageDevice = device;
            // load up game with respective device


        }

        #endregion

    }
}
