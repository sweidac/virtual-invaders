using System.Collections;

namespace AUTU
{
  public class CDummyTestcaseA : CTestGruppe
  {
    public uint TestA1Aufrufe { get; set; }
    public uint PreTestAufrufe { get; set; }
    public uint PostTestAufrufe { get; set; }
    public uint AnzahlGelaufenerTests { get; set; }
    public bool VerhindereTestA1 { get; set; }

    public CDummyTestcaseA()
    {
      TestA1Aufrufe = 0;
      PreTestAufrufe = 0;
      PostTestAufrufe = 0;
      AnzahlGelaufenerTests = 0;
      VerhindereTestA1 = false;
    }

    public override bool preStartTest()
    {
      PreTestAufrufe++;
      return !VerhindereTestA1;
    }

    public override void postStartTest(bool IstTestGelaufen)
    {
      if (IstTestGelaufen)
      {
        AnzahlGelaufenerTests++;
      }
      PostTestAufrufe++;
    }

    [Test]
    public IEnumerator TestA1()
    {
      TestA1Aufrufe++;

      Pruefer.istTrue(true);
      Pruefer.istTrue(false);

      yield break;

    }
  }
}
