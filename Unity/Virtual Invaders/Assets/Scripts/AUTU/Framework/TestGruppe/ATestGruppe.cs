using UnityEngine;
using System.Collections.ObjectModel;

namespace AUTU
{
  public abstract class ATestGruppe : MonoBehaviour, ITestGruppe, ITestGruppenUmgebung
  {
    /// Von ITestGruppe
    public abstract string Name { get; }
    public abstract uint AnzahlGruppen { get; }
    public abstract ReadOnlyCollection<ITestGruppe> Gruppen { get; }
    public abstract void gruppeHinzufuegen(ITestGruppe Gruppe);
    public abstract uint AnzahlTests { get; }
    public abstract ReadOnlyCollection<ITestcase> Tests { get; }
    public abstract bool IstTestGruppeAktiv { get; }

    /// Von ITestGruppenUmgebung
    public abstract bool preTest(IPruefer Pruefer, ILogger Logger, ITestProzessorSchreiber Prozessor);
    public abstract void postTest(bool IstTestGelaufen);
  }
}
