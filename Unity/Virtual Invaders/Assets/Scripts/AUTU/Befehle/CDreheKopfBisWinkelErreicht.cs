using UnityEngine;

namespace AUTU.Befehle
{
  class CDreheKopfBisWinkelErreicht : IBefehl
  {
    IVectorWert AktuellerWert = null;
    IVectorWert ZielWert = null;
    float Geschwindigkeit = 1000;
    uint Frames = 10;

    public CDreheKopfBisWinkelErreicht(IVectorWert AktuellerWert, IVectorWert ZielWert, float Geschwindigkeit = 100, uint Frames = 10)
    {
      this.AktuellerWert = AktuellerWert;
      this.ZielWert = ZielWert;
      this.Geschwindigkeit = Geschwindigkeit;
      this.Frames = Frames;
    }

    public bool bearbeiten(ILogger Logger, IProzessorKomponenten Komponenten)
    {
      IKopfRotationsInjektion KopfEingabe = Komponenten.suche<IKopfRotationsInjektion>();
      Debug.Assert(KopfEingabe != null, "Es konnte keine Prozessor Komponente vom Typ IKopfRotationsInjektion gefunden werden (Kamera).");

      Vector3 DeltaVektor = Vector3.zero;

      DeltaVektor.x = Mathf.DeltaAngle(AktuellerWert.Vektor.x, ZielWert.Vektor.x);
      DeltaVektor.y = Mathf.DeltaAngle(AktuellerWert.Vektor.y, ZielWert.Vektor.y);
      DeltaVektor.z = Mathf.DeltaAngle(AktuellerWert.Vektor.z, ZielWert.Vektor.z);

      float FrameGeschwindigkeit = Geschwindigkeit * Time.deltaTime;

      if (DeltaVektor.magnitude > FrameGeschwindigkeit)
      {
        Vector3 Bewegung = DeltaVektor.normalized * FrameGeschwindigkeit;
        KopfEingabe.KopfRotation = Quaternion.Euler(KopfEingabe.KopfRotation.eulerAngles + Bewegung);
      }
      else if (DeltaVektor.magnitude > 0.1f)
      {
        KopfEingabe.KopfRotation = Quaternion.Euler(KopfEingabe.KopfRotation.eulerAngles + DeltaVektor);
      }
      else if (Frames > 0)
      {
        Frames--;
      }

      return Frames == 0;
    }

    public override string ToString()
    {
      return "Bewege Kopf bis Winkel erreicht wurde.";
    }
  }
}
