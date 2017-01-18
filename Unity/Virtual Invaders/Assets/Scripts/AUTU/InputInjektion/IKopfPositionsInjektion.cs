using AUTU;
using UnityEngine;

interface IKopfRotationsInjektion : ITestKomponente
{
  /// <summary>
  /// Setzt die Rotation vom Kopf.
  /// </summary>
  Quaternion KopfRotation { get; set; }
}
