using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Assets.Scripts;

public class Menue : MonoBehaviour {

    Canvas credits;
    Text highscore;
    GameObject startButton;
    Vector3 button;

    private static Menue instance = null;

    public static Menue Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        Destroy(gameObject);
        instance = null;
    }
    /// <summary>
    /// Started das Menu beim Programstart
    /// </summary>
    void Start ()
    {
        highscore = GameObject.Find("HighscoreText").GetComponent<Text>();
        startButton = GameObject.Find("StartButtonHighlighted");
        button = startButton.transform.forward;
        SetHighscore();
    }

    void Update()
    {
        startButtonAktivieren();
    }
    /// <summary>
    /// Started das Menu nachdem das Spiel verloren wurde und alle weiteren male
    /// </summary>
    public void StartMenue()
    {
        SetHighscore();
        gameObject.SetActive(true);
        startButton.SetActive(true);
        button = startButton.transform.forward;
        startButton.SetActive(false);
        Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        rotation.x = 0;
        rotation.z = 0;
        gameObject.transform.rotation = rotation;

    }
    /// <summary>
    /// Beendet die Anwendung auf dem Handy
    /// </summary>
    public void SpielBeenden()
    {
        Application.Quit();
    }
    /// <summary>
    /// Startet das Spiel.
    /// </summary>
    public void SpielStart()
    {
        Debug.Log("start");
        //SceneManager.LoadScene(1);
        gameObject.SetActive(false);
        Spielsteuerung.Instance.SpielStart();

    }
    /// <summary>
    /// Prüft ob es einen neuen Highscore gibt und schreibt ihn ins Menue und indirect ins File
    /// </summary>
    public void SetHighscore()
    {
        int scoreValue = Spielsteuerung.Instance.Score;

        string value = Highscore.ladeHighscore();
        int points = Int32.Parse(value);

        if(scoreValue > points)
        {
            value = scoreValue.ToString();
            Highscore.speicherHighscore(value);
        }
        highscore.text = "Highscore: " + value;
    }
    /// <summary>
    /// Aktiviert und Deaktiviert den hervorgehobenen startButton
    /// </summary>
    public void startButtonAktivieren()
    {

        Vector3 camera = Camera.main.transform.forward;
        if ((camera.x <= button.x + 0.2) && (camera.x >= button.x - 0.2) && (camera.y <= button.y + 0.1) && (camera.y >= button.y - 0.1))
            startButton.SetActive(true);
        else
            startButton.SetActive(false);
    }
}
