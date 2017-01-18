using System;
using System.Collections;

namespace AUTU
{
  public class CDummyTestcaseC : CTestGruppe
  {
    [Test]
    public IEnumerator TestC1()
    {
      Logger.Log("Das ist ein Test!");
      yield return true;
    }
  }
}
