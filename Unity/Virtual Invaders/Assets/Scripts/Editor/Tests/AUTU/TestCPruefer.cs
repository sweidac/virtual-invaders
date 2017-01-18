using NUnit.Framework;
using NSubstitute;
using System.Diagnostics;

namespace AUTU
{
  class TestCPruefer
  {
    private ILogger Logger = null;

    [SetUp]
    public void Init()
    {
      Logger = Substitute.For<ILogger>();
    }

    [TearDown]
    public void Cleanup()
    {
      Logger = null;
    }

    [NUnit.Framework.Test]
    public void IntialeWerteSind0()
    {
      CTestStatus Status = new CTestStatus();
#pragma warning disable 0219
      CPruefer Pruefer = new CPruefer(Status, Logger, false);
#pragma warning restore 0219

      Assert.AreEqual(0, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstTrueTest()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, false);

      Pruefer.istTrue(true);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert("Dummy", 1, 2);
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istTrue(false);

      Logger.Received().FalscherWert(Arg.Any<string>(), true, false, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstTrueAbbruchBeiFehler()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, true);

      Pruefer.istTrue(true);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert("Dummy", 1, 2);
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      bool WarFehler = false;
      try
      {
        Pruefer.istTrue(false);
      }
      catch(FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstFalseTest()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, false);

      Pruefer.istFalse(false);

      Logger.DidNotReceiveWithAnyArgs().LogError("Test");
      Logger.DidNotReceiveWithAnyArgs().FalscherWert("Dummy", 1, 2);
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istFalse(true, "Test");

      Logger.Received().LogError("Test");
      Logger.Received().FalscherWert(Arg.Any<string>(), false, true, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstFalseAbbruchBeiFehler()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, true);

      Pruefer.istFalse(false);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert("Dummy", 1, 2);
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      bool WarFehler = false;
      try
      {
        Pruefer.istFalse(true);
      }
      catch (FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    private class TestClass {}

    [NUnit.Framework.Test]
    public void IstGleicheReferenzTest()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, false);

      TestClass TestObjekt1 = new TestClass();

      Pruefer.istGleicheReferenz(TestObjekt1, TestObjekt1);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert("Dummy", 1, 2);
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleicheReferenz(null, null);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert("Dummy", 1, 2);
      Logger.ClearReceivedCalls();

      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleicheReferenz(null, TestObjekt1);

      Logger.Received().FalscherWert(Arg.Any<string>(), Arg.Any<object>(), TestObjekt1, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(3, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleicheReferenz(TestObjekt1, null);

      Logger.Received().FalscherWert(Arg.Any<string>(), TestObjekt1, Arg.Any<object>(), Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(4, Status.Pruefungen);
      Assert.AreEqual(2, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      TestClass TestObjekt2 = new TestClass();
      Pruefer.istGleicheReferenz(TestObjekt1, TestObjekt2);

      Logger.Received().FalscherWert(Arg.Any<string>(), TestObjekt1, TestObjekt2, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(5, Status.Pruefungen);
      Assert.AreEqual(3, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleicheReferenz(TestObjekt2, TestObjekt1);

      Logger.Received().FalscherWert(Arg.Any<string>(), TestObjekt2, TestObjekt1, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(6, Status.Pruefungen);
      Assert.AreEqual(4, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstGleicheReferenzAbbruchBeiFehler()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, true);

      TestClass TestObjekt1 = new TestClass();

      Pruefer.istGleicheReferenz(TestObjekt1, TestObjekt1);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert("Dummy", 1, 2);
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      bool WarFehler = false;
      try
      {
        Pruefer.istGleicheReferenz(null, TestObjekt1);
      }
      catch (FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstGroesserTest()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, false);

      Pruefer.istGroesser(2, 3);

      Logger.DidNotReceiveWithAnyArgs().FalscherVergleich(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGroesser(4.67, 4.68);

      Logger.DidNotReceiveWithAnyArgs().FalscherVergleich(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGroesser(5, 5);

      Logger.Received().FalscherVergleich(Arg.Any<string>(), 5, 5, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(3, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGroesser(987.561, 987.561);

      Logger.Received().FalscherVergleich(Arg.Any<string>(), 987.561, 987.561, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(4, Status.Pruefungen);
      Assert.AreEqual(2, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGroesser(1000.54, 987.561);

      Logger.Received().FalscherVergleich(Arg.Any<string>(), 1000.54, 987.561, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(5, Status.Pruefungen);
      Assert.AreEqual(3, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstGroesserAbbruchBeiFehler()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, true);

      Pruefer.istGroesser(4, 10);

      Logger.DidNotReceiveWithAnyArgs().FalscherVergleich(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      bool WarFehler = false;
      try
      {
        Pruefer.istGroesser(2, 1);
      }
      catch (FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstKleinerTest()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, false);

      Pruefer.istKleiner(3, 2);

      Logger.DidNotReceiveWithAnyArgs().FalscherVergleich(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istKleiner(4.69, 4.68);

      Logger.DidNotReceiveWithAnyArgs().FalscherVergleich(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istKleiner(5, 5);

      Logger.Received().FalscherVergleich(Arg.Any<string>(), 5, 5, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(3, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istKleiner(987.561, 987.561);

      Logger.Received().FalscherVergleich(Arg.Any<string>(), 987.561, 987.561, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(4, Status.Pruefungen);
      Assert.AreEqual(2, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istKleiner(987.561, 1000.54);

      Logger.Received().FalscherVergleich(Arg.Any<string>(), 987.561, 1000.54, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(5, Status.Pruefungen);
      Assert.AreEqual(3, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstKleinerAbbruchBeiFehler()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, true);

      Pruefer.istKleiner(10, 4);

      Logger.DidNotReceiveWithAnyArgs().FalscherVergleich(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      bool WarFehler = false;
      try
      {
        Pruefer.istKleiner(1, 2);
      }
      catch (FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstGleichTest()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, false);

      Pruefer.istGleich(1, 1);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(10.56f, 10.56f);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(10, 10.56f);

      Logger.Received().FalscherWert(Arg.Any<string>(), (double)10, (double)(10.56f), Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(3, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(15, 5);

      Logger.Received().FalscherWert(Arg.Any<string>(), (double)15, (double)5, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(4, Status.Pruefungen);
      Assert.AreEqual(2, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(100.54f, 987.561f);

      Logger.Received().FalscherWert(Arg.Any<string>(), (double)100.54f, (double)987.561f, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(5, Status.Pruefungen);
      Assert.AreEqual(3, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstGleichMitToleranzTest()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, false);

      Pruefer.istGleich(1, 2, 1);

      Logger.DidNotReceiveWithAnyArgs().WertAusserhalbToleranz(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(10.56f, 10f, 0.6f);

      Logger.DidNotReceiveWithAnyArgs().WertAusserhalbToleranz(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(10f, 10.56f, 0.6f);

      Logger.DidNotReceiveWithAnyArgs().WertAusserhalbToleranz(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(3, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(10f, 10.56f, 0.4f);

      Logger.Received().WertAusserhalbToleranz(Arg.Any<string>(), (double)10f - (double)0.4f, (double)10f + (double)0.4f, (double)10.56f, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(4, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(15, 5, 9);

      Logger.Received().WertAusserhalbToleranz(Arg.Any<string>(), (double)6, (double)24, (double)5, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(5, Status.Pruefungen);
      Assert.AreEqual(2, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      Pruefer.istGleich(100.54f, 987.561f, 10.6f);

      Logger.Received().WertAusserhalbToleranz(Arg.Any<string>(), (double)100.54f - (double)10.6f, (double)100.54f + (double)10.6f, (double)987.561f, Arg.Any<StackTrace>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(6, Status.Pruefungen);
      Assert.AreEqual(3, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void IstGleichAbbruchBeiFehler()
    {
      CTestStatus Status = new CTestStatus();
      CPruefer Pruefer = new CPruefer(Status, Logger, true);

      Pruefer.istGleich(10, 11, 1.0f);

      Logger.DidNotReceiveWithAnyArgs().FalscherWert(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<object>());
      Logger.ClearReceivedCalls();

      Assert.AreEqual(1, Status.Pruefungen);
      Assert.AreEqual(0, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      bool WarFehler = false;
      try
      {
        Pruefer.istGleich(10, 11, 0.5f);
      }
      catch (FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
      Assert.AreEqual(2, Status.Pruefungen);
      Assert.AreEqual(1, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);

      WarFehler = false;
      try
      {
        Pruefer.istGleich(10, 11);
      }
      catch (FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
      Assert.AreEqual(3, Status.Pruefungen);
      Assert.AreEqual(2, Status.Fehler);
      Assert.AreEqual(0, Status.FataleFehler);
    }
  }
}
