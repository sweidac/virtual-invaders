using AUTU;

interface IKnopfInjektion : ITestKomponente
{
  /// <summary>
  /// Drückt den Knopf oder lässt ihn los.
  /// </summary>
  /// <param name="Gedrueckt">Muss true sein um den Knopf zu drücken oder false um ihn loszulassen.</param>
  void drueckeKnopf(bool Gedrueckt);
}
