using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AUTU
{
  public interface ITestStatus
  {
    /// <summary>
    /// Gibt an ob der Test ignoriert werden soll.
    /// </summary>
    bool IgnoriereTest { get; }

    /// <summary>
    /// Gibt an wie viele Prüfungen im Test druchgeführt wurden.
    /// </summary>
    uint Pruefungen { get; }

    /// <summary>
    /// Gibt an wie viele Fehler im Test aufgetreten sind.
    /// </summary>
    uint Fehler { get; }

    /// <summary>
    /// Gibt an wie viele fatale Fehler (zum Beispiel Exceptions) im Testverlauf aufgetreten sind.
    /// </summary>
    uint FataleFehler { get; }

    /// <summary>
    /// Gibt die Anzahl der Testdurchläufe zurück.
    /// </summary>
    uint Durchlaeufe { get; }

    /// <summary>
    /// Gibt true zurück, wenn der Test gelaufen ist.
    /// </summary>
    bool IstGelaufen { get; }
  }
}
