using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string fileName;
    public static void SaveLines(Line[] lines, string fileNameInput)
    {
        fileName = fileNameInput;
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log("saving to " + (Application.persistentDataPath));
        string path = MakeLineRiderSaveFileName(fileName);
        FileStream stream = new FileStream(path, FileMode.Create);
        try
        {
            LineData data = new LineData(lines);
            formatter.Serialize(stream, data);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            stream.Close();
        }
        
    }

    public static LineData LoadLines(string fileNameInput)
    {
        string path = MakeLineRiderSaveFileName(fileNameInput);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            LineData data = formatter.Deserialize(stream) as LineData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static string MakeLineRiderSaveFileName(string input)
    {
        return Application.persistentDataPath + "/" + input + ".lineRiderLevel";
    }
}
