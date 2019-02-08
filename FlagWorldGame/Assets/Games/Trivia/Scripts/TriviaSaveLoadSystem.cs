using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
Load Round Data from a data file to the trivia manager
Save new round data to a data file from the web. 
 */

public static class TriviaSaveLoadSystem
{


    //save round data from an array 
    //shoud be called from the web handler in the final version.
    public static void SaveRoundData (QuestionData[] roundData) 
    {
        Debug.Log("saving started");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/roundDataTest.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        QuestionData[] data = new QuestionData[roundData.Length];
        data = roundData;

        formatter.Serialize(stream,data);
        stream.Close();

        Debug.Log("saving ended");


    }

    public static QuestionData[] LoadRoundData () 
    {
        string path = Application.persistentDataPath + "/roundDataTest.dat";
        if (File.Exists(path)) 
        {
            Debug.Log("File load started");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            QuestionData[] data = formatter.Deserialize(stream) as QuestionData[];
            stream.Close();
            Debug.Log("File loaded");
            return data;

        } 
        else 
        {
            Debug.LogError("File not Found in " + path);
            return null;
        }

    }

}
