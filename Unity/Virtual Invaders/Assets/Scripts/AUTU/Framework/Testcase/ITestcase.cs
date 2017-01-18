using System.Collections;

namespace AUTU
{
  public interface ITestcase
  {
    /// <summary>
    /// Der Name des Testcases.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Der vollständige Name des Testcases zusammen mit den Gruppenamen.
    /// </summary>
    string VollerName { get; }

    /// <summary>
    /// Der Name der Gruppen in welchen sich der Testcase befindet.
    /// </summary>
    string GruppenName { get; }

    /// <summary>
    /// Startet den Testcase.
    /// </summary>
    /// <param name="ZusammenfassungsAenderer">Ein Objekt zum ändern der Test-Zusammenfassung.</param>
    /// <param name="Logger">Ein Objekt zum Loggen von Meldungen aus dem Testcase.</param>
    /// <param name="Optionen">Sind die Optionen mit denen der Test gestartet werden soll.</param>
    /// <param name="Prozessor">Ist der Prozessor mit dem der Test ausgeführt werden soll.</param>
    /// <returns>Gibt ein IEnumerator Objekt zurück, welches bestimmt, wann die nächste Iteration gestartet werden soll.</returns>
    IEnumerator starte(IZusammenfassungsAenderer ZusammenfassungsAenderer, ILogger Logger, ITestOptionen Optionen, ITestProzessorSchreiber Prozessor);

    /// <summary>
    /// Der Status des Testcases.
    /// </summary>
    ITestStatus Status { get; }
  }
}
