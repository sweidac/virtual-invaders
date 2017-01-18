using UnityEngine;

namespace AUTU.Befehle
{
  public interface IVectorWert : IWert
  {
    Vector3 Vektor { get; }
  }
}
