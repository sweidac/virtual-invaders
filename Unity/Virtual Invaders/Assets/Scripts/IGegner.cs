using UnityEngine;

namespace Assets.Scripts
{
  interface IGegner
  {
    /// <summary>
    /// Gibt die Anzahl der Lebenspunkte des Gegners zurück.
    /// </summary>
    float HitPoints { get; }

    /// <summary>
    /// Erstellt ein Projektil und gibt das GameObjekt dazu zurück.
    /// </summary>
    GameObject ErstelleProjektil();

    /// <summary>
    /// Die maximale Schussverzögerung zwischen zwei Raketen.
    /// </summary>
    int SchussVerzoegerung { get; }
  }
}
