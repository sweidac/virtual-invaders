namespace AUTU
{
  public class CTestOptionen : ITestOptionen
  {
    private TestOptionen.LoggingStatus KonsolenLoggingStatus = TestOptionen.LoggingStatus.Vererbt;

    public CTestOptionen()
    {
      KonsolenLoggingStatus = TestOptionen.LoggingStatus.Vererbt;
      AbbruchBeiFehler = true;
    }

    public ITestOptionen merge(ITestOptionen Optionen)
    {
      CTestOptionen NeueOptionen = new CTestOptionen();
      CTestOptionen HauptOptionen = Optionen as CTestOptionen;

      // Merge Konsolen Logging
      if (HauptOptionen != null && KonsolenLoggingStatus == TestOptionen.LoggingStatus.Vererbt)
      {
        NeueOptionen.KonsolenLoggingStatus = HauptOptionen.KonsolenLoggingStatus;
      }
      else
      {
        NeueOptionen.KonsolenLoggingStatus = KonsolenLoggingStatus;
      }

      // Merge Abbruch Bei Fehler
      if (HauptOptionen != null)
      {
        NeueOptionen.AbbruchBeiFehler = HauptOptionen.AbbruchBeiFehler;
      }
      else
      {
        NeueOptionen.AbbruchBeiFehler = AbbruchBeiFehler;
      }

      return NeueOptionen;
    }

    public bool KonsolenLogging
    {
      get
      {
        return (KonsolenLoggingStatus != TestOptionen.LoggingStatus.Aus);
      }
    }

    public void setKonsolenLogging(TestOptionen.LoggingStatus Status)
    {
      KonsolenLoggingStatus = Status;
    }

    public bool AbbruchBeiFehler { get; set; }
  }
}
