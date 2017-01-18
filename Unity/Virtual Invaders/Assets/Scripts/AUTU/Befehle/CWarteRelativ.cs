using UnityEngine;

namespace AUTU.Befehle
{
  class CWarteRelativ : IBefehl
  {
    float Wartezeit = 0.0f;

    public CWarteRelativ(float Wartezeit)
    {
      this.Wartezeit = Wartezeit;
    }

    public bool bearbeiten(ILogger Logger, IProzessorKomponenten Komponenten)
    {
      if (Wartezeit > 0.0f)
      {
        Wartezeit -= Time.deltaTime;
      }

      return Wartezeit <= 0.0f;
    }

    public override string ToString()
    {
      return "Warte relative Zeit: " + Wartezeit;
    }
  }
}
