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

namespace Silhouetta
{
    public abstract class LbKStorage
    {
        #region Variables, Objects, and Fields
        static int score;
        static int level;
        static int checkPoint;
        static bool nothingLoaded = false;
        
        public static int Level
        {
            get { return level; }
            set { level = value; }
        }

        public static int Score
        {
            get { return score; }
            set { score = value; }
        }

        public static int CheckPoint
        {
            get { return checkPoint; }
            set { checkPoint = value; }
        }

        public static bool NothingLoaded
        {
            get { return nothingLoaded; }
        }

        #endregion

        #region SaveGameData

        private struct SaveGameData
        {
            public int SavedPlayerScore;
            public int CurrentLevel;
            public int SavedCheckPoint;
        }

        public static void SaveGame(StorageDevice device, SignedInGamer gamer)
        {

            SaveGameData data = new SaveGameData();
            data.CurrentLevel = level;
            data.SavedPlayerScore = score;
            data.SavedCheckPoint = checkPoint;

            IAsyncResult result =
                device.BeginOpenContainer(gamer.Gamertag, null, null);

            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();
            //.sav is important
            string filename = "LbKSavedInfo.sav";

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
        /// This method loads a serialized data object
        /// from the StorageContainer for this game.
        /// </summary>
        /// <param name="device"></param>
        public static void LoadGame(StorageDevice device, SignedInGamer gamer)
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

            string filename = "LbKSavedInfo.sav";

            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                nothingLoaded = true;
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
            level = data.CurrentLevel;
            score = data.SavedPlayerScore;
            checkPoint = data.SavedCheckPoint;
            
            GamePlayScreen.storageDevice = device;
            // load up game with respective device
            

        }

        #endregion

    }
}
