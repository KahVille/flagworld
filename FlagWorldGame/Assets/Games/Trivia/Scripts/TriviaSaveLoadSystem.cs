using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
/*
Load Round Data from a data file to the trivia manager
Save new round data to a data file from the web. 
 */

public static class TriviaSaveLoadSystem
{


    public static IEnumerator LoadContactPointsFromWeb() 
    {
        UnityWebRequest www = new UnityWebRequest("https://gist.githubusercontent.com/KahVille/5a23729971d6905b91f8cf23217b33b8/raw/5d79f297d129dea6860ea9bc9f691fcf82297a43/flagworldDataTest.json");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            yield return null;
        
        }
        else {
            // Show results as text
            string result = www.downloadHandler.text;

            ContactPointCollection contactionPoints = new ContactPointCollection();
            JsonUtility.FromJsonOverwrite(result,contactionPoints);

            SaveContactPoints(contactionPoints);
            // make sure the data is in correct form?
            //parse data based on the type of data recieved.
            //from web -> json -> QuestionData[] -> binary?
        }
    }

    private static void SaveContactPoints(ContactPointCollection contactPoints) {
        Debug.Log("saving collection points");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/collectionPointTest.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        ContactPointCollection data = new ContactPointCollection();
        data = contactPoints;

        formatter.Serialize(stream,data);
        stream.Close();

        Debug.Log("collection points saved");
    }

    public static ContactPointCollection LoadContactPoints () {
        string path = Application.persistentDataPath + "/collectionPointTest.dat";
        if (File.Exists(path)) 
        {
            Debug.Log("File load started");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ContactPointCollection data = formatter.Deserialize(stream) as ContactPointCollection;
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

    //save round data from an array to a file 
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
