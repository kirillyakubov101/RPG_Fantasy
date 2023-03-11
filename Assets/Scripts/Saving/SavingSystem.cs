using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace FantasyTown.Saving
{
    public static class SavingSystem
    {
        public static string path = Application.persistentDataPath + "/SaveFile.sav";



        public static void SaveFile(object state)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            Debug.Log(path);

            formatter.Serialize(stream, state);
            stream.Close();
        }

        public static object LoadFile()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            var temp =  formatter.Deserialize(stream);
            stream.Close();

            return temp;
        }

        //private static string GetPathFromSaveFile(string saveFile)
        //{
        //    return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        //}
    }
}


