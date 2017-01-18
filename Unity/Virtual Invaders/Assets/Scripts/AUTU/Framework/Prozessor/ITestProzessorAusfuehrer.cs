namespace AUTU
{
  public interface ITestProzessorAusfuehrer
  {
    /// <summary>
    /// Ist true, wenn der Test-Prozessor keine Befehle mehr zum Abarbeiten hat.
    /// </summary>
    bool IstFertig { get; }

    /// <summary>
    /// Gibt die Anzahl der Befehle im Prozessor zurück.
    /// </summary>
    uint Befehle { get; }

    /// <summary>
    /// Bearbeitet den nächsten Befehl den es in der Schlange gibt.
    /// </summary>
    void bearbeiteBefehl();

    /// <summary>
    /// Lösche die komplette Befehlsschlange und stellt den ursprünglichen Zustand wieder her.
    /// </summary>
    void reset();

    /// <summary>
    /// Setzt das Logger Objekt, welches vom Prozessor verwendet wird.
    /// </summary>
    /// <param name="Logger">Ist das Logger Objekt das verwendet werden soll.</param>
    void setLogger(ILogger Logger);
  }
}
