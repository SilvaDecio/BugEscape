using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using System.IO;
using System.IO.IsolatedStorage;

using System.Xml.Serialization;
using System.Xml;

using Bug_Escape.Management;

namespace Bug_Escape.DataBase
{
    public class Preferences
    {
        public float SongVolume, EffectVolume;
        
        public bool HasVibrationControl;

        public Preferences()
        {}

        public static void Save(Preferences Settings)
        {
            using (IsolatedStorageFile MyIsolatedStorageFile =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream Stream =
                    MyIsolatedStorageFile.OpenFile(StateManager.PathForPreferences,
                    FileMode.Open))
                {
                    XmlSerializer Serializer = new XmlSerializer(typeof(Preferences));
                    Serializer.Serialize(Stream, Settings);
                }
            }
        }

        public static Preferences Load()
        {
            using (IsolatedStorageFile MyIsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (MyIsolatedStorageFile.FileExists(StateManager.PathForPreferences))
                {
                    using (IsolatedStorageFileStream Stream =
                        MyIsolatedStorageFile.OpenFile(StateManager.PathForPreferences , FileMode.Open))
                    {
                        XmlSerializer Serializer = new XmlSerializer(typeof(Preferences));

                        return (Preferences)Serializer.Deserialize(Stream);
                    }
                }
            }

            return new Preferences();
        }

        public static void Creating()
        {
            using (IsolatedStorageFile MyIsolatedStorageFile =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!MyIsolatedStorageFile.FileExists(StateManager.PathForPreferences))
                {
                    using (IsolatedStorageFileStream Stream =
                        MyIsolatedStorageFile.OpenFile(StateManager.PathForPreferences,
                        FileMode.CreateNew))
                    {
                        XmlSerializer Serializer = new XmlSerializer(typeof(Preferences));

                        Serializer.Serialize(Stream, new Preferences());
                    }
                }
            }
        }
    }
}