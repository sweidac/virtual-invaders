namespace AUTU
{
  public class FehlerSpaetesHinzufuegenVonGruppe : Fehler
  {
    public FehlerSpaetesHinzufuegenVonGruppe(string Untergruppe, string Gruppe) : base("Die Gruppe "+Untergruppe+" wurde erst hinzugefügt, nachdem schon alle Testcases von "+Gruppe+" ermittelt wurden.")
    {
    }
  }
}
