using UnityEngine;

namespace AUTU.Befehle
{
  public class CDreheKopf : IBefehl
  {
    private Vector3 Kopfbewegung = Vector3.zero;
    private float Geschwindigkeit = 0;
    private bool IstFertig = false;

    public CDreheKopf(Vector3 Kopfbewegung, float Geschwindigkeit = 100)
    {
      this.Kopfbewegung = Kopfbewegung;
      this.Geschwindigkeit = Geschwindigkeit;
      IstFertig = false;
    }

    public bool bearbeiten(ILogger Logger, IProzessorKomponenten Komponenten)
    {
      IKopfRotationsInjektion KopfEingabe = Komponenten.suche<IKopfRotationsInjektion>();
      Debug.Assert(KopfEingabe != null, "Es konnte keine Prozessor Komponente vom Typ IKopfRotationsInjektion gefunden werden (Kamera).");

      float FrameGeschwindigkeit = Geschwindigkeit * Time.deltaTime;

      if (!IstFertig)
      {
        if (Kopfbewegung.magnitude > FrameGeschwindigkeit)
        {
          Vector3 TeilKopfbewegung = Kopfbewegung.normalized * FrameGeschwindigkeit;
          Kopfbewegung -= TeilKopfbewegung;
          KopfEingabe.KopfRotation = Quaternion.Euler(KopfEingabe.KopfRotation.eulerAngles + TeilKopfbewegung);
          IstFertig = false;
        }
        else
        {
          KopfEingabe.KopfRotation = Quaternion.Euler(KopfEingabe.KopfRotation.eulerAngles + Kopfbewegung);
          IstFertig = true;
        }

        return false;
      }

      return true;
    }

    public override string ToString()
    {
      return "Bewege Kopf um die Winkel: "+ Kopfbewegung;
    }
  }
}
