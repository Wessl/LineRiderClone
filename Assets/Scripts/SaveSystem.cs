using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);
    
    
    private static string fileName;
    public static void SaveLines(Line[] lines, string fileNameInput)
    {
        fileName = fileNameInput;
        BinaryFormatter formatter = new BinaryFormatter();
        Debug.Log("saving to " + (Application.persistentDataPath));
        string path = MakeLineRiderSaveFileName(fileName);
        FileStream stream;
        try
        {
            stream = new FileStream(path, FileMode.Create);
            LineData data = new LineData(lines);
            formatter.Serialize(stream, data);
            stream.Close();

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }
        }
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to save: " + e);
        }

    }

    public static LineData LoadLines(string fileNameInput)
    {
        LineData data = null;
        string path = MakeLineRiderSaveFileName(fileNameInput);
        try
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
            
                data = formatter.Deserialize(stream) as LineData;
                stream.Close();
            }
        }
        
        catch (Exception e)
        {
            PlatformSafeMessage("Failed to load: " + path + ". " + e);
            return null;
        }
        return data;
    }

    public static string MakeLineRiderSaveFileName(string input)
    {
        return Application.persistentDataPath + "/" + input + ".lineRiderLevel";
    }
    
    private static void PlatformSafeMessage(string message)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            WindowAlert(message);
        }
        else
        {
            Debug.Log(message);
        }
    }
}
