using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Highscore : MonoBehaviour {

    private const string FILENAME = "highscore.txt";
    private static StreamWriter outputStream = null;
    private static StreamReader inputStream = null;

    /// <summary>
    /// Lädt Highscore aus dem File
    /// </summary>
    public static string ladeHighscore()
    {
        string absolutePath = Application.persistentDataPath + Path.DirectorySeparatorChar + FILENAME;
        if (!File.Exists(absolutePath))
            return "0";
        using (inputStream = new StreamReader(absolutePath))
        {
            //Debug.Log("Lade Highscore von " + absolutePath);
            string highscore = inputStream.ReadLine();
            //Debug.Log(highscore);
            //Console.WriteLine(highscore);
            return highscore;
        }
    }
    /// <summary>
    /// schreib Highscore ins File
    /// </summary>
    public static void speicherHighscore(string highscore)
    {
        string absolutePath = Application.persistentDataPath + Path.DirectorySeparatorChar + FILENAME;
        if (!File.Exists(absolutePath))
            Debug.Log("kein file");
        using (outputStream = new StreamWriter(absolutePath))
        {
            outputStream.WriteLine(highscore);
            outputStream.Flush();
        }             
    }
}
