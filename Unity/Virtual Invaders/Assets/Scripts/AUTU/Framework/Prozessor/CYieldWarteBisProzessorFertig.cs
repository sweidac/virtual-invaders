using UnityEngine;

namespace AUTU
{
  class CYieldWarteBisProzessorFertig : CustomYieldInstruction
  {
    public ITestProzessorAusfuehrer ProzessorAusfuehrer { get; private set; }

    public CYieldWarteBisProzessorFertig(ITestProzessorAusfuehrer ProzessorAusfuehrer)
    {
      this.ProzessorAusfuehrer = ProzessorAusfuehrer;
    }

    public override bool keepWaiting
    {
      get
      {
        return !ProzessorAusfuehrer.IstFertig;
      }
    }

  }
}
