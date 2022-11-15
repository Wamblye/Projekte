using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
   public static void Save(GameData data)
   {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(GetPath(), FileMode.Create);
        formatter.Serialize(fs, data);
        fs.Close();
   }
   public static GameData Load()
   {
       if (!File.Exists(GetPath()))
       {
           GameData emptyData = new GameData();
           SetDefaultLevelMenueDaten(emptyData);
           Save(emptyData);
           return emptyData;
       }

       BinaryFormatter formatter = new BinaryFormatter();
       FileStream fs = new FileStream(GetPath(), FileMode.Open);
       GameData data = formatter.Deserialize(fs) as GameData;
       fs.Close();

       return data;
   }

    public static void Delete()
    {
        File.Delete(GetPath());
    }

   private static string GetPath()
   {
       return Application.persistentDataPath + "/data.qnd";
   }

    private static string GetPathCustomeItems()
   {
       return Application.persistentDataPath + "/items.qnd";
   }

   public static void SetDefaultLevelMenueDaten(GameData emptyData)
   {
       for (int i = 1; i < 800; i++)
       {
           emptyData.levelMenueDaten.Add(new LevelMenueDaten(i, 0, 0, 0));
       }
   }

}
