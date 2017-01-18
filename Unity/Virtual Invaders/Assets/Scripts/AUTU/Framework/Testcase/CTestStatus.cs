namespace AUTU
{
  public class CTestStatus : ITestStatus, ITestStatusAenderer
  {
    public bool IgnoriereTest
    {
      get;
      private set;
    }

    public uint Pruefungen
    {
      get;
      private set;
    }

    public uint Fehler
    {
      get;
      private set;
    }

    public uint FataleFehler
    {
      get;
      private set;
    }

    public uint Durchlaeufe
    {
      get;
      private set;
    }

    public bool IstGelaufen
    {
      get
      {
        return Durchlaeufe > 0;
      }
    }

    public CTestStatus()
    {
      reset();
    }

    public void loescheErgebnis()
    {
      Pruefungen = 0;
      Fehler = 0;
      FataleFehler = 0;
    }

    public void reset()
    {
      loescheErgebnis();
      Durchlaeufe = 0;
    }

    public void addierePruefung(uint Wert = 1)
    {
      Pruefungen += Wert;
    }

    public void addiereFehler(uint Wert = 1)
    {
      Fehler += Wert;
    }

    public void addiereFataleFehler(uint Wert = 1)
    {
      FataleFehler += Wert;
    }

    public void addiereDurchlaeufe(uint Wert = 1)
    {
      Durchlaeufe += Wert;
    }

    public void ignoriereTest(bool Ignoriere)
    {
      IgnoriereTest = Ignoriere;
    }
  }
}
