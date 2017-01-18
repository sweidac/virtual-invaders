namespace AUTU
{
  public interface ITestStatusAenderer
  {
    /// <summary>
    /// Setzt den Test-Status zurück.
    /// </summary>
    void reset();

    /// <summary>
    /// Addirt einen Wert zum Zähler für Prüfungen.
    /// </summary>
    /// <param name="Wert">Der zu addierende Wert.</param>
    void addierePruefung(uint Wert = 1);

    /// <summary>
    /// Addirt einen Wert zum Zähler für Fehler.
    /// </summary>
    /// <param name="Wert">Der zu addierende Wert.</param>
    void addiereFehler(uint Wert = 1);

    /// <summary>
    /// Addirt einen Wert zum Zähler für fatale Fehler.
    /// </summary>
    /// <param name="Wert">Der zu addierende Wert.</param>
    void addiereFataleFehler(uint Wert = 1);

    /// <summary>
    /// Addirt einen Wert zum Zähler für die Durchläufe.
    /// </summary>
    /// <param name="Wert">Der zu addierende Wert.</param>
    void addiereDurchlaeufe(uint Wert = 1);

    /// <summary>
    /// Löscht alle Test-Ergebnisse, aber nicht die Anzahl der Durchläufe vom Test.
    /// </summary>
    void loescheErgebnis();

    /// <summary>
    /// Ignoriert den Test, wenn der Wert auf true gesetzt wird.
    /// </summary>
    /// <param name="Ignoriere">Gibt an, ob der Test ignoriert werden soll oder nicht.</param>
    void ignoriereTest(bool Ignoriere);
  }
}
