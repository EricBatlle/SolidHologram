using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameProgressConstants
{
    public static int numberOfChapters = 5;
    public static int sizeOfChapters = 3;
}

//The following structure is not the "logical approach" due to Unity's incapacity to serialize multidimensional arrays
[Serializable]
public class PlayerData 
{
    [SerializeField] public bool[] chapters;
    [SerializeField] public bool[] levels;
}

public class PlayerInfoController : MonoBehaviour
{

    static public PlayerInfoController s_Singleton;
    public PlayerData data;
    
    private void Start()
    {
        if (s_Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            s_Singleton = this;
            data.chapters = new bool[GameProgressConstants.numberOfChapters];
            data.levels = new bool[GameProgressConstants.numberOfChapters * GameProgressConstants.sizeOfChapters];
            Load();
        }
        else if (s_Singleton != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        //Add data values
        PlayerData data = new PlayerData();
        data = this.data;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //assign data values
            this.data = data;
        }
        else
        {
            data.chapters[0] = true;
            data.levels[0] = true;
            Save();
        }
    }

    public void UpdateProgress()
    {
        //Get from scene name the chapter and the level
        string name = SceneManager.GetActiveScene().name;
        print("scene name" + name);
        string level = name.Substring(name.LastIndexOf('.') + 1);
        char chapter = name[name.LastIndexOf('.') - 1];
        //Cast to int to operate with them
        int level_int = int.Parse(level);
        int chapter_int = (int)Char.GetNumericValue(chapter);
        print("chapter " + chapter_int + " level: " + level);
        //Update level
        data.levels[((chapter_int - 1) * 3) + level_int] = true;

        //If it's the last level of the chapter, open new chapter
        if (level_int >= 3)
        {
            data.chapters[chapter_int] = true;
        }
        Save();
    }

    //Get last chapter/level completed
    #region GetLastCompleted
    public int GetLastChapterCompleted()
    {
        return GetLastCompleted(data.chapters);
    }

    public int GetLastLevelCompleted()
    {
        return GetLastCompleted(data.levels);
    }

    public int GetLastCompleted(bool[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == false)
            {
                return i;
            }
        }
        return 0;
    }
    #endregion

}

