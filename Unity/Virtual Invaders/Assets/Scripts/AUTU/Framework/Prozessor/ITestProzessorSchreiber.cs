using UnityEngine;

namespace AUTU
{
  public interface ITestProzessorSchreiber
  {
    /// <summary>
    /// Fügt einen Befehl zum Test-Prozessor hinzu und gibt die Referenz auf diesen Befehl zurück.
    /// </summary>
    /// <typeparam name="T">Die Klasse des Befehls der hinzugefügt wurde.</typeparam>
    /// <param name="Befehl">Eine Instanz auf den Befehl der hinzugefügt werden soll.</param>
    /// <returns>Gibt die gleiche Instanz des Befehls zurück.</returns>
    T add<T>(T Befehl) where T : IBefehl;

    /// <summary>
    /// Gibt ein Yield-Befehl zurück der dazu führt, dass der Testcase wartet bis der Prozessor fertig ist mit der Abarbeitung.
    /// </summary>
    /// <returns>Ein Yield-Befehl zum Warten.</returns>
    CustomYieldInstruction warteBisFertig();
  }
}
