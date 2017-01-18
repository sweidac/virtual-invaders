using UnityEngine;


/// <summary>
/// Test-Interface für Projektile.
/// </summary>
interface IGegnerProjektil
{
  /// <summary>
  /// Gibt das Prefab für die Explosition zurück.
  /// </summary>
  GameObject ExplositionsPreFab { get; }
}
