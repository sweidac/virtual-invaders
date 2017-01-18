using System.Collections.Generic;

namespace AUTU
{
  public interface IProzessorKomponenten
  {
    /// <summary>
    /// Sucht eine Komponente im Prozessor und gibt diese zurück.
    /// </summary>
    /// <param name="Name">Ist der Name der Komponente.</param>
    /// <param name="AlternativObjekt">Ist ein Alternativobjekt welches zurückgegeben wird wenn das richtige nicht gefunden wurde.</param>
    /// <typeparam name="T">Ist die Klasse der Komponente.</typeparam>
    /// <returns>Gibt die gefundene Instanz zurück oder null, falls keine Komponente gefunden wurde.</returns>
    T suche<T>(string Name = null, T AlternativObjekt = null) where T : class;

    /// <summary>
    /// Sucht eine Komponente im Prozessor und gibt diese zurück.
    /// </summary>
    /// <param name="Name">Ist der Name der Komponente.</param>
    /// <returns>Gibt die gefundene Instanz zurück oder null, falls keine Komponente gefunden wurde.</returns>
    object suche(string Name);

    /// <summary>
    /// Gibt alle Komponenten zurück die dem Typen besitzen und den angegebenen Namen haben.
    /// </summary>
    /// <param name="Name">Ist der Name der Komponente.</param>
    /// <typeparam name="T">Ist die Klasse der Komponente.</typeparam>
    /// <returns>Gibt eine Liste aller gefunden Instanzen zurück.</returns>
    List<T> sucheAlle<T>(string Name = null) where T : class;

    /// <summary>
    /// Schaltet alle Komponenten des Prozessors ab.
    /// </summary>
    void alleKomponentenAbschalten();
  }
}
