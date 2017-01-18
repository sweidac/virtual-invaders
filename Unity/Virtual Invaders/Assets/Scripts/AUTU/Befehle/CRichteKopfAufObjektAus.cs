using UnityEngine;

namespace AUTU.Befehle
{
  /// <summary>
  /// Richtet den Kopf auf ein Objekt aus.
  /// </summary>
  class CRichteKopfAufObjektAus : CDreheKopfBisWinkelErreicht
  {
    private Transform Objekt;

    public CRichteKopfAufObjektAus(Transform Kamera, Transform Objekt, float Geschwindigkeit = 100) :
      base(new CRotatiosWert(Kamera), new CBlickrichtung(Kamera, Objekt), Geschwindigkeit)
    {
      this.Objekt = Objekt;
    }

    public CRichteKopfAufObjektAus(Transform Kamera, Transform Objekt, Vector3 Offset, float Geschwindigkeit = 100) :
      base(new CRotatiosWert(Kamera), new CBlickrichtung(Kamera, Objekt, Offset), Geschwindigkeit)
    {
      this.Objekt = Objekt;
    }

    public override string ToString()
    {
      if (Objekt != null)
      {
        return "Richte Kopf auf " + Objekt.name + " aus.";
      }

      return "Richte Kopf auf unbekanntes Objekt aus.";
    }
  }
}
