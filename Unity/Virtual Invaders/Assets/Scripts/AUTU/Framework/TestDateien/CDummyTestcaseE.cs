using System.Collections;
using System.Collections.Generic;

namespace AUTU
{
  public class CDummyTestcaseE : CTestGruppe, ITestKomponente
  {
    List<IBefehl> BefehlsListe = null;

    public void addBefehl(IBefehl Befehl)
    {
      if (BefehlsListe == null)
      {
        BefehlsListe = new List<IBefehl>();
      }

      BefehlsListe.Add(Befehl);
    }

    [Test]
    public IEnumerator TestE1()
    {
      if (BefehlsListe != null)
      {
        foreach (IBefehl Befehl in BefehlsListe)
        {
          if (Befehl != null)
          {
            Prozessor.add(Befehl);
          }
        }
      }

      yield return true;
    }

    public bool KomponenteAktiv { get; set; }
  }
}
