using UnityEngine;
using System.Collections;

public sealed class Sichtsteuerung {
     
    private static Sichtsteuerung _instance;
    public static Sichtsteuerung Instance
    {
        get
        {
            if (_instance == null) _instance = new Sichtsteuerung();
            return _instance;
        }
    }
    private Sichtsteuerung() { }
    /// <summary>
    /// Gibt einen Strahl zurück welcher v
    /// </summary>
    /// <returns>Einen Strahl welcher von der Kamera durch einen Viewport Punkt geht.</returns>
    public Ray ViewDirection()
    {
        return Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.01f));
    }

    public void Update()
    {
        
    }
}
