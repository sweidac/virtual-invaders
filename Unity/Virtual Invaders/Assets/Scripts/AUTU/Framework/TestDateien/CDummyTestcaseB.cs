using System;
using System.Collections;

namespace AUTU
{
  public class CDummyTestcaseB : CTestGruppe
  {
    public uint TestB1Aufrufe { get; set; }
    public uint TestB2Aufrufe { get; set; }
    public uint TestB3Aufrufe { get; set; }
    public uint PreTestAufrufe { get; set; }
    public uint PostTestAufrufe { get; set; }
    public uint AnzahlGelaufenerTests { get; set; }
    public ITestProzessorSchreiber UebergebenerTestProzessorInTestB2 = null;

    public CDummyTestcaseB()
    {
      TestB1Aufrufe = 0;
      PreTestAufrufe = 0;
      PostTestAufrufe = 0;
      AnzahlGelaufenerTests = 0;
      UebergebenerTestProzessorInTestB2 = null;
  }

    [Test]
    public IEnumerator TestB1()
    {
      //yield return true;
      TestB1Aufrufe++;
      throw new Exception("This is a test only...");
    }

    [Test]
    public IEnumerator TestB2()
    {
      TestB2Aufrufe++;
      UebergebenerTestProzessorInTestB2 = Prozessor;
      yield break;
    }

    [Test]
    public IEnumerator TestB3()
    {
      TestB3Aufrufe++;
      Pruefer.istTrue(false);
      yield break;
    }

    public override bool preStartTest()
    {
      PreTestAufrufe++;
      return true;
    }

    public override void postStartTest(bool IstTestGelaufen)
    {
      if (IstTestGelaufen)
      {
        AnzahlGelaufenerTests++;
      }
      PostTestAufrufe++;
    }

  }
}
