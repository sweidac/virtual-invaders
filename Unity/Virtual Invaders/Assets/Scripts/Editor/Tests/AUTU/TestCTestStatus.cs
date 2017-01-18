using NUnit.Framework;
using AUTU;

namespace AUTU
{
  public class TestTestStatus
  {
    [NUnit.Framework.Test]
    public void IntialeWerteSind0()
    {
      CTestStatus Status = new CTestStatus();

      Assert.AreEqual(0, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
      Assert.AreEqual(0, Status.Durchlaeufe);
      Assert.AreEqual(false, Status.IgnoriereTest);
      Assert.AreEqual(false, Status.IstGelaufen);
    }

    [NUnit.Framework.Test]
    public void AddierePruefungen()
    {
      CTestStatus Status = new CTestStatus();

      Status.addierePruefung();

      Assert.AreEqual(1, Status.Pruefungen);

      Status.addierePruefung(10);

      Assert.AreEqual(11, Status.Pruefungen);
    }

    [NUnit.Framework.Test]
    public void AddiereFehler()
    {
      CTestStatus Status = new CTestStatus();

      Status.addiereFehler();

      Assert.AreEqual(1, Status.Fehler);

      Status.addiereFehler(9);

      Assert.AreEqual(10, Status.Fehler);
    }

    [NUnit.Framework.Test]
    public void AddiereFataleFehler()
    {
      CTestStatus Status = new CTestStatus();

      Status.addiereFataleFehler();

      Assert.AreEqual(1, Status.FataleFehler);

      Status.addiereFataleFehler(2);

      Assert.AreEqual(3, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void AddiereAnzhalDurchlaufe()
    {
      CTestStatus Status = new CTestStatus();

      Assert.AreEqual(false, Status.IstGelaufen);

      Status.addiereDurchlaeufe();

      Assert.AreEqual(1, Status.Durchlaeufe);
      Assert.AreEqual(true, Status.IstGelaufen);

      Status.addiereDurchlaeufe(4);

      Assert.AreEqual(5, Status.Durchlaeufe);
      Assert.AreEqual(true, Status.IstGelaufen);
    }

    [NUnit.Framework.Test]
    public void Reset()
    {
      CTestStatus Status = new CTestStatus();

      Status.addiereDurchlaeufe(2);
      Status.addierePruefung(6);
      Status.addiereFehler(4);
      Status.addiereFataleFehler(1);

      Status.reset();

      Assert.AreEqual(0, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
      Assert.AreEqual(0, Status.Durchlaeufe);
      Assert.AreEqual(false, Status.IstGelaufen);
    }

    [NUnit.Framework.Test]
    public void ErgebnisLoeschen()
    {
      CTestStatus Status = new CTestStatus();

      Status.addiereDurchlaeufe(2);
      Status.addierePruefung(6);
      Status.addiereFehler(4);
      Status.addiereFataleFehler(1);

      Status.loescheErgebnis();

      Assert.AreEqual(0, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
      Assert.AreEqual(2, Status.Durchlaeufe);
      Assert.AreEqual(true, Status.IstGelaufen);
    }

    [NUnit.Framework.Test]
    public void SetzeIgnoreFlag()
    {
      CTestStatus Status = new CTestStatus();

      Status.ignoriereTest(true);

      Assert.AreEqual(true, Status.IgnoriereTest);

      Status.ignoriereTest(false);

      Assert.AreEqual(false, Status.IgnoriereTest);
    }
  }
}
