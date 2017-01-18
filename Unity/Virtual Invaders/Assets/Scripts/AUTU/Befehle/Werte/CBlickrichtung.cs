using UnityEngine;

namespace AUTU.Befehle
{
  class CBlickrichtung : IVectorWert
  {
    Transform Kamera = null;
    Transform Ziel = null;
    Vector3 Offset = Vector3.zero;

    public CBlickrichtung(Transform KameraTransform, Transform ZielTransform)
    {
      Kamera = KameraTransform;
      Ziel = ZielTransform;
    }

    public CBlickrichtung(Transform KameraTransform, Transform ZielTransform, Vector3 Offset)
    {
      Kamera = KameraTransform;
      Ziel = ZielTransform;
      this.Offset = Offset;
    }

    public Vector3 Vektor
    {
      get
      {
        if (Ziel != null && Kamera != null)
        {
          Quaternion ZielBlickrichtung = Quaternion.LookRotation(Ziel.position + Offset - Kamera.position);

          return ZielBlickrichtung.eulerAngles;
        }
        else
        {
          return Kamera.rotation.eulerAngles;
        }
      }
    }

    public bool istKonstant()
    {
      return false;
    }
  }
}
