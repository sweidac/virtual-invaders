using AUTU;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Assets.Scripts
{
  public interface IGegnerSteuerung : ITestKomponente
  {
    /// <summary>
    /// Gibt eine Liste aller möglichen Gegner Modelle zurück.
    /// </summary>
    ReadOnlyCollection<GameObject> GegnerPreFabs { get; }

    /// <summary>
    /// Erstellt einen neuen Gegner.
    /// </summary>
    /// <param name="PreFab">Ist die PreFab für den Gegner.</param>
    /// <param name="Position">Ist die Position für den Gegner.</param>
    /// <returns>Gibt das Gegner-Objekt zurück.</returns>
    GameObject SpawneGegner(GameObject PreFab, Vector3 Position);

    /// <summary>
    /// Steuert, ob Gegner Projektile verschießen oder nicht. Ist standardmäßig eingeschaltet.
    /// </summary>
    bool ProjektileAktiv { get; set; }

    /// <summary>
    /// Die maximale Spawn-Zeit für Mutterschiffe.
    /// </summary>
    float MaximaleMutterschiffSpawnzeit { get; }
  }
}
