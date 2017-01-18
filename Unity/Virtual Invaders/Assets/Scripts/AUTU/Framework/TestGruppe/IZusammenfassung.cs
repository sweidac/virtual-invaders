namespace AUTU
{
  public interface IZusammenfassung
  {
    /// <summary>
    /// Die gesamte Anzahl an Tests.
    /// </summary>
    uint Tests { get; }

    /// <summary>
    /// Die Anzahl der ausgeführten Tests.
    /// </summary>
    uint GelaufeneTests { get; }

    /// <summary>
    /// Die Anzahl der Tests die ignoriert wurden.
    /// </summary>
    uint IgnorierteTests { get; }

    /// <summary>
    /// Die Anzahl der korrekt ausgeführten Tests.
    /// </summary>
    uint KorrekteTests { get; }

    /// <summary>
    /// Die Anzahl der Fehlerhaften Tests.
    /// </summary>
    uint FehlerhafteTests { get; }

    /// <summary>
    /// Die Anzahl der durchgeführten Prüfungen.
    /// </summary>
    uint Pruefungen { get; }

    /// <summary>
    /// Die Anzahl der bemerkten Fehler.
    /// </summary>
    uint Fehler { get; }

    /// <summary>
    /// Die Anzahl der fatalen Fehler.
    /// </summary>
    uint FataleFehler { get; }

    /// <returns>Gibt die Zusammenfassung als String zurück.</returns>
    string ToString();
  }
}
