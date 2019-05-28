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

    public static void DeleteData() {
        string filePath = Application.persistentDataPath + "/collectionPointTest.dat";
        File.Delete(filePath);
    }

    public static void SaveContactPoints(ContactPointCollection contactPoints) {
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

}
