namespace AUTU
{
  public interface IZusammenfassungsAenderer
  {
    /// <summary>
    /// Fügt den Test-Status der Zusammenfassung zu.
    /// </summary>
    /// <param name="Status">Ist der Test-Status der hinzugefügt werden soll.</param>
    void addiereStatus(ITestStatus Status);

    /// <summary>
    /// Setzt die Zähler der Zusammenfassung zurück.
    /// </summary>
    void reset();
  }
}
