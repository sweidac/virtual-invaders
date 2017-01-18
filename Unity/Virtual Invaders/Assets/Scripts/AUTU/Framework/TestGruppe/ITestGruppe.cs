using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AUTU
{
  public interface ITestGruppe
  {
    /// <summary>
    /// Gibt den Gruppennamen zurück.
    /// </summary>
    string Name { get; }

    /// <returns>
    /// Gibt die Anzahl der registrierten Untergruppen zurück.
    /// </return>
    uint AnzahlGruppen { get; }

    /// <summary>
    /// Speichert die bei dieser Gruppe angemeldeten Untergruppen.
    /// </summary>
    ReadOnlyCollection<ITestGruppe> Gruppen { get; }

    /// <summary>
    /// Fügt eine Untergruppe zur Gruppe hinzu.
    /// </summary>
    /// <param name="Gruppe">Die Untergruppe die Hinzugefügt werden soll.</param>
    void gruppeHinzufuegen(ITestGruppe Gruppe);

    /// <returns>
    /// Gibt die Anzahl der Testcases dieser und aller Untergruppen zurück.
    /// </returns>
    uint AnzahlTests { get; }

    /// <summay>
    /// Eine Liste aller Tests, die zu dieser Gruppe und den Untergruppen gehören.
    /// </summay>
    ReadOnlyCollection<ITestcase> Tests { get; }

    /// <summary>
    /// Gibt an, ob die Testgruppe aktiv ist oder nicht.
    /// </summary>
    bool IstTestGruppeAktiv { get; }
  }
}
