namespace AUTU.Befehle
{
  public interface IWert
  {
    /// <summary>
    /// Gibt zurück, ob der Wert seit dem letzten Test oder der Erstellung konstant geblieben ist.
    /// </summary>
    /// <returns>Gibt true zurück, wenn der Wert konstant ist.</returns>
    bool istKonstant();
  }
}
