using System;
using System.Diagnostics;

namespace AUTU
{
  /// <summary>
  /// Ein Wrapper für die Logging-Funktionalität der Unity-Debug-Klasse.
  /// Er hilft dabei Testcases ruhig zu stellen oder die Ausgaben der Prüf-Klasse in Unit-Tests zu überprüfen.
  /// </summary>
  public interface ILogger
  {
    /// <summary>
    /// Gibt einen Text als normale Mitteilung aus.
    /// </summary>
    /// <param name="Text">Der Text der ausgegeben werden soll.</param>
    /// <param name="StackDiagnose">Ist ein Trace vom Stack.</param>
    void Log(string Text, StackTrace StackDiagnose = null);

    /// <summary>
    /// Gibt einen Text als Warnung aus.
    /// </summary>
    /// <param name="Text">Der Text der ausgegeben werden soll.</param>
    /// <param name="StackDiagnose">Ist ein Trace vom Stack.</param>
    void LogWarning(string Text, StackTrace StackDiagnose = null);

    /// <summary>
    /// Gibt einen Text als Fehler aus.
    /// </summary>
    /// <param name="Text">Der Text der ausgegeben werden soll.</param>
    /// <param name="StackDiagnose">Ist ein Trace vom Stack.</param>
    void LogError(string Text, StackTrace StackDiagnose = null);

    /// <summary>
    /// Gibt einen Fehlertext aus wegen einem falschen Wert.
    /// </summary>
    /// <param name="Text">Ist der Fehlertext.</param>
    /// <param name="ErwarteterWert">Ist der erwartete Wert.</param>
    /// <param name="TatseachlicherWert">Ist der tatsächliche Wert.</param>
    /// <param name="StackDiagnose">Ist ein Trace vom Stack.</param>
    void FalscherWert(string Text, object ErwarteterWert, object TatseachlicherWert, StackTrace StackDiagnose = null);

    /// <summary>
    /// Gibt eine Fehlermeldung wegen einer Exception aus.
    /// </summary>
    /// <param name="Text">Ist der Fehlertext.</param>
    /// <param name="DieseExeption">Die aufgetretene Exception.</param>
    /// <param name="StackDiagnose">Ist ein Trace vom Stack.</param>
    void ExceptionIstAufgetreten(string Text, Exception DieseExeption, StackTrace StackDiagnose = null);

    /// <summary>
    /// Gibt einen Fehlertext aus einem kleineren Wert als der Vergleichswert.
    /// </summary>
    /// <param name="Text">Ist der Fehlertext.</param>
    /// <param name="Vergleichswert">Ist der Wergleichswertwert.</param>
    /// <param name="TatseachlicherWert">Ist der tatsächliche Wert.</param>
    /// <param name="StackDiagnose">Ist ein Trace vom Stack.</param>
    void FalscherVergleich(string Text, object Vergleichswert, object TatseachlicherWert, StackTrace StackDiagnose = null);

    /// <summary>
    /// Gibt einen Fehlertext aus, weil ein Wert außerhalb des Toleranzintervalls liegt.
    /// </summary>
    /// <param name="Text">Ist der Fehlertext.</param>
    /// <param name="UntereGrenze">Ist die untere Grenze des Intervalls.</param>
    /// <param name="ObereGrenze">Ist die obere Grenze des Intervalls.</param>
    /// <param name="TatseachlicherWert">Ist der tatsächliche Wert.</param>
    /// <param name="StackDiagnose">Ist ein Trace vom Stack.</param>
    void WertAusserhalbToleranz(string Text, object UntereGrenze, object ObereGrenze, object TatseachlicherWert, StackTrace StackDiagnose = null);

    /// <summary>
    /// Schaltet die Konsolenausgabe ein oder aus.
    /// </summary>
    bool ConsoleEin { get; set; }
  }
}
