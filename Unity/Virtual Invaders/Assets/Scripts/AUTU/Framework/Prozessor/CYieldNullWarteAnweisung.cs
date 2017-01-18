using UnityEngine;

namespace AUTU
{
  public class CYieldNullWarteAnweisung : CustomYieldInstruction
  {
    public override bool keepWaiting
    {
      get
      {
        return false;
      }
    }
  }
}
