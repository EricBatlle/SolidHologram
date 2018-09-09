using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
class PlayerData
{
    public Chapter[] chapters;
}

[Serializable]
class Chapter
{
    public int[] level;
}

public class PlayerInfoController : MonoBehaviour {

    static public PlayerInfoController s_Singleton;
    [SerializeField] private PlayerData data = new PlayerData();

    private void Awake()
    {
        if(s_Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            s_Singleton = this;
            PlayerInfoController.s_Singleton.data.chapters[0].level[0] = 0;
            foreach (Chapter chapter in PlayerInfoController.s_Singleton.data.chapters)
            {
                foreach (int level in PlayerInfoController.s_Singleton.chapter.level)
                {
                    print("Chapter: " + chapter + "/ Level: " + level);
                }
            }
        }
        else if (s_Singleton != this)
        {
            Destroy(gameObject);
        }        
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        //PlayerData data = new PlayerData();
        //Add data values
        //...
        bf.Serialize(file, data);
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //assign data values
            //...
        }
    }
}

