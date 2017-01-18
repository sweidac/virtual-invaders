using System.Collections.Generic;
using UnityEngine;

namespace AUTU
{
  public class CNullTestProzessor : ITestProzessorSchreiber, ITestProzessorAusfuehrer, IProzessorKomponenten
  {
    public bool IstFertig { get { return false; } }
    public uint Befehle { get { return 0; } }

    public T add<T>(T Befehl) where T : IBefehl { return Befehl; }
    public void bearbeiteBefehl() { }
    public CustomYieldInstruction warteBisFertig()
    {
      return new CYieldNullWarteAnweisung();
    }
    public void reset() { }
    public void setLogger(ILogger Logger) { }
    public T suche<T>(string Name = null, T AlternativObjekt = null) where T : class { return null; }
    public object suche(string Name) { return null; }
    public List<T> sucheAlle<T>(string Name = null) where T : class { return new List<T>(); }
    public void alleKomponentenAbschalten() { }
  }
}
