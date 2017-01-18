using UnityEngine;

namespace AUTU.Befehle
{
  public class CRotatiosWert : IVectorWert
  {
    Transform Objekt = null;
    Quaternion LetzteRotation;

    public CRotatiosWert(Transform Objekt)
    {
      this.Objekt = Objekt;
      LetzteRotation = Objekt.rotation;
    }

    public bool istKonstant()
    {
      Quaternion AktuelleRotation = Objekt.rotation;
      bool IstKonstant = AktuelleRotation == LetzteRotation;
      LetzteRotation = AktuelleRotation;
      return IstKonstant;
    }

    public Vector3 Vektor
    {
      get
      {
        return Objekt.rotation.eulerAngles;
      }
    }

  }
}
