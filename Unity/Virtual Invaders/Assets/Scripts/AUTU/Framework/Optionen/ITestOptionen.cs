namespace AUTU
{
  public class TestOptionen
  {
    public enum LoggingStatus
    {
      Ein,
      Aus,
      Vererbt
    }
  }

  public interface ITestOptionen
  {
    /// <summary>
    /// Mergt diese Optionen mit übergeordneten Optionen zusammen und gibt das gemergte Optionen Objekt zurück.
    /// </summary>
    /// <param name="HauptOptionen">Sind die übergeordneten Optionen.</param>
    /// <returns>Ein neues Optionen-Objekt welches den Merge enthält.</returns>
    ITestOptionen merge(ITestOptionen HauptOptionen);

    /// <summary>
    /// Gibt true zurück, wenn Logging auf der Konsole eingeschaltet ist.
    /// </summary>
    bool KonsolenLogging { get; }

    /// <summary>
    /// Setzt den Status des Konsolen Loggings.
    /// </summary>
    /// <param name="Status">Der Status der gesetzt werden soll.</param>
    void setKonsolenLogging(TestOptionen.LoggingStatus Status);

    /// <summary>
    /// Ist true, wenn bei einem Fehler sofort abgebrochen werden soll.
    /// </summary>
    bool AbbruchBeiFehler { get; set; }
  }
}
