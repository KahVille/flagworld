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


    public static IEnumerator LoadRoundDataFromWeb(System.Action<string> onSuccess) 
    {
        UnityWebRequest www = new UnityWebRequest("https://gist.githubusercontent.com/KahVille/5a23729971d6905b91f8cf23217b33b8/raw/d096dff982a32aeae5f62c1b198d30ef2cabc9e9/flagworldDataTest.json");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            string result = www.downloadHandler.text;
            onSuccess(result);

            // make sure the data is in correct form?
            //parse data based on the type of data recieved.
            //from web -> json -> QuestionData[] -> binary?
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
