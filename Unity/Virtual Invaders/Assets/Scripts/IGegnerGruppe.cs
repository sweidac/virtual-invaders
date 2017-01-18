using UnityEngine;

namespace Assets.Scripts
{
  interface IGegnerGruppe
  {
    /// <summary>
    /// Gibt ein Array von Gegnertypen zurück, welche durch die Gruppe gespawn werden können.
    /// </summary>
    GameObject[] TypenVonGegnern { get; }

    /// <summary>
    /// Gibt das GameObject der Gegner-Gruppe zurück.
    /// </summary>
    GameObject Objekt { get; }
  }
}
