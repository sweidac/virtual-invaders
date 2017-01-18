using System;

namespace AUTU
{
  public class Fehler : Exception
  {
    public string Fehlermeldung { get; private set; }

    public Fehler(string Fehlermeldung)
    {
      this.Fehlermeldung = Fehlermeldung;
    }
  }

  public class FehlerAbbruchWegenFehler : Fehler
  {
    public FehlerAbbruchWegenFehler() : base("Testcase wurde wegen einem Fehler vorzeitig abgebrochen.") { }
  }
}
