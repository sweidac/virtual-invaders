using UnityEngine;

namespace AUTU.Befehle
{
  class CKonstanterVektor : IVectorWert
  {
    public CKonstanterVektor(Vector3 Vektor)
    {
      this.Vektor = Vektor;
    }

    public CKonstanterVektor(float X = 0f, float Y = 0f, float Z = 0f)
    {
      Vektor = new Vector3(X, Y, Z);
    }

    public Vector3 Vektor { get; private set; }

    public bool istKonstant() { return true; }
  }
}
