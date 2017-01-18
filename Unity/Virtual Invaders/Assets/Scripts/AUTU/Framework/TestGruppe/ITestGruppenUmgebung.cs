namespace AUTU
{
  interface ITestGruppenUmgebung
  {
    /// <summary>
    /// Wird direkt vor dem Start eines Tests ausgeführt.
    /// </summary>
    /// <returns>Gibt true zurück, wenn der Test gestartet werden darf.</returns>
    /// <param name="Pruefer">Ist das Objekt welches zum Pruefen der Testcases verwendet wird.</param>
    /// <param name="Logger">Ist das Objekt zum Loggen von Fehlern und Ausgaben.</param>
    /// <param name="Prozessor">Ist das Objekt für den Test-Prozessor.</param>
    bool preTest(IPruefer Pruefer, ILogger Logger, ITestProzessorSchreiber Prozessor);

    /// <summary>
    /// Wird direkt nach dem Start eines Tests ausgeführt.
    /// </summary>
    /// <param name="IstTestGelaufen">Ist true, wenn der Test ausgeführt wurde.</param>
    void postTest(bool IstTestGelaufen);
  }
}
