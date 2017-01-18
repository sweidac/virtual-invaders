namespace AUTU
{
  public interface IBefehl
  {
    /// <summary>
    /// Bearbeitet den Befehl.
    /// </summary>
    /// <param name="Logger">Ist das Logger-Objekt welches verwendet werden soll.</param>
    /// <param name="Komponenten">Ein Objekt über das Prozessorkomponenten abgefragt werden können.</param>
    /// <returns>Gibt true zurück, wenn der Befehl fertig bearbeitet wurde. Ansonsten false.</returns>
    bool bearbeiten(ILogger Logger, IProzessorKomponenten Komponenten);

    /// <summary>
    /// Gibt den Befehl als String aus.
    /// </summary>
    /// <returns>Der String welcher diesem Befehl entspricht.</returns>
    string ToString();
  }
}
