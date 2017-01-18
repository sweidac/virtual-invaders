using UnityEngine;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using System;
using System.Diagnostics;

namespace AUTU
{
  [Category("UnityOnly")]
  public class TestTestcase
  {
    class CStartWrapper
    {
      public delegate IEnumerator StartFunktionDelegate(IZusammenfassungsAenderer Aenderer, ILogger Logger, ITestOptionen Optionen, ITestProzessorSchreiber Prozessor);

      private StartFunktionDelegate StartFunktion = null;
      private IZusammenfassungsAenderer Aenderer = null;
      private ILogger Logger = null;
      private ITestOptionen Optionen = null;
      private ITestProzessorSchreiber Prozessor = null;

      public CStartWrapper(StartFunktionDelegate StartFunktion, IZusammenfassungsAenderer Aenderer, ILogger Logger, ITestOptionen Optionen, ITestProzessorSchreiber Prozessor)
      {
        this.StartFunktion = StartFunktion;
        this.Aenderer = Aenderer;
        this.Logger = Logger;
        this.Optionen = Optionen;
        this.Prozessor = Prozessor;
      }

      public IEnumerator starte()
      {
        yield return StartFunktion(Aenderer, Logger, Optionen, Prozessor);
      }
    }

    private ILogger Logger = null;
    private ITestOptionen Optionen = null;
    private ITestProzessorSchreiber Prozessor = null;

    [SetUp]
    public void Setup()
    {
      Logger = Substitute.For<ILogger>();
      Optionen = new CTestOptionen();
      Optionen.AbbruchBeiFehler = false;
      Prozessor = Substitute.For<ITestProzessorSchreiber>();
      Prozessor.warteBisFertig().Returns(new CYieldNullWarteAnweisung());
    }

    [TearDown]
    public void Cleanup()
    {
      Logger = null;
      Optionen = null;
      Prozessor = null;
    }

    private T createGruppe<T>(string Name = "TestGruppe") where T : CTestGruppe
    {
      GameObject Object = new GameObject();
      Object.gameObject.name = Name;
      return Object.AddComponent<T>();
    }

    [NUnit.Framework.Test]
    public void TestcaseNamen()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseA>("CDummyTestcaseA");

      Assert.AreEqual("TestA1", TestGruppe.Tests[0].Name);
      Assert.AreEqual("CDummyTestcaseA", TestGruppe.Tests[0].GruppenName);
      Assert.AreEqual("CDummyTestcaseA.TestA1", TestGruppe.Tests[0].VollerName);
    }

    [NUnit.Framework.Test]
    public void StarteTestcase()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseA TestGruppeA = TestGruppe as CDummyTestcaseA;

      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppe), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 1);
      TestGruppeA.TestA1Aufrufe = 0;

      Starter.starte();

      Assert.AreEqual(1, TestGruppeA.TestA1Aufrufe);
    }

    [NUnit.Framework.Test]
    public void PreUndPostStartFunktionen()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseA TestGruppeA = TestGruppe as CDummyTestcaseA;

      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppe), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 1);
      TestGruppeA.PreTestAufrufe = 0;
      TestGruppeA.PostTestAufrufe = 0;
      TestGruppeA.AnzahlGelaufenerTests = 0;

      Starter.starte();

      Assert.AreEqual(1, TestGruppeA.PreTestAufrufe);
      Assert.AreEqual(1, TestGruppeA.PostTestAufrufe);
      Assert.AreEqual(1, TestGruppeA.AnzahlGelaufenerTests);
    }

    [NUnit.Framework.Test]
    public void StarteTestcaseRecursive()
    {
      ITestGruppe TestGruppeA = createGruppe<CDummyTestcaseA>("CDummyTestcaseA");
      CDummyTestcaseA GruppeA = TestGruppeA as CDummyTestcaseA;

      ITestGruppe TestGruppeB = createGruppe<CDummyTestcaseB>("CDummyTestcaseB");
      TestGruppeB.gruppeHinzufuegen(TestGruppeA);

      ITestcase Test = TestGruppeB.Tests[3];
      Assert.AreEqual("CDummyTestcaseB.CDummyTestcaseA.TestA1", Test.VollerName);
      CStartWrapper StartWrapper = new CStartWrapper(Test.starte, new CZusammenfassung(TestGruppeB), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 1);

      GruppeA.TestA1Aufrufe = 0;
      GruppeA.PreTestAufrufe = 0;
      GruppeA.PostTestAufrufe = 0;
      GruppeA.AnzahlGelaufenerTests = 0;

      Starter.starte();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(1, GruppeA.PreTestAufrufe);
      Assert.AreEqual(1, GruppeA.PostTestAufrufe);
      Assert.AreEqual(1, GruppeA.AnzahlGelaufenerTests);
    }

    [NUnit.Framework.Test]
    public void StarteKeinenTestcaseWennPreStartEsVerhindert()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseA TestGruppeA = TestGruppe as CDummyTestcaseA;

      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppeA), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 1);
      TestGruppeA.VerhindereTestA1 = true;
      TestGruppeA.PreTestAufrufe = 0;
      TestGruppeA.PostTestAufrufe = 0;
      TestGruppeA.AnzahlGelaufenerTests = 0;

      Starter.starte();

      Assert.AreEqual(1, TestGruppeA.PreTestAufrufe);
      Assert.AreEqual(1, TestGruppeA.PostTestAufrufe);
      Assert.AreEqual(0, TestGruppeA.AnzahlGelaufenerTests);
      Assert.AreEqual(0, TestGruppeA.TestA1Aufrufe);
    }

    [NUnit.Framework.Test]
    public void ExceptionInTestcase()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseB>("CDummyTestcaseB");
      CDummyTestcaseB TestGruppeB = TestGruppe as CDummyTestcaseB;

      ITestcase Test = TestGruppeB.Tests[0];
      Assert.AreEqual("CDummyTestcaseB.TestB1", Test.VollerName);
      CStartWrapper StartWrapper = new CStartWrapper(Test.starte, new CZusammenfassung(TestGruppeB), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 1);

      TestGruppeB.TestB1Aufrufe = 0;
      TestGruppeB.PreTestAufrufe = 0;
      TestGruppeB.PostTestAufrufe = 0;
      TestGruppeB.AnzahlGelaufenerTests = 0;

      Starter.starte();
      Assert.AreEqual(1, Test.Status.FataleFehler);
      Assert.AreEqual(0, Test.Status.Fehler);
      Assert.AreEqual(0, Test.Status.Pruefungen);

      Logger.Received().ExceptionIstAufgetreten(Arg.Any<string>(), Arg.Is<Exception>(Ex => Ex.ToString().Contains("This is a test only...")), Arg.Any<StackTrace>());

      Assert.AreEqual(1, TestGruppeB.PreTestAufrufe);
      Assert.AreEqual(1, TestGruppeB.PostTestAufrufe);
      Assert.AreEqual(1, TestGruppeB.AnzahlGelaufenerTests);
      Assert.AreEqual(1, TestGruppeB.TestB1Aufrufe);
    }

    [NUnit.Framework.Test]
    public void TestStatusWirdVeraendert()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseA>();

      ITestcase Test = TestGruppe.Tests[0];
      CStartWrapper StartWrapper = new CStartWrapper(Test.starte, new CZusammenfassung(TestGruppe), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 1);

      Starter.starte();

      Assert.AreEqual(2, Test.Status.Pruefungen);
      Assert.AreEqual(1, Test.Status.Fehler);
      Assert.AreEqual(0, Test.Status.FataleFehler);
      Assert.AreEqual(1, Test.Status.Durchlaeufe);

      Starter.starte();

      Assert.AreEqual(2, Test.Status.Pruefungen);
      Assert.AreEqual(1, Test.Status.Fehler);
      Assert.AreEqual(0, Test.Status.FataleFehler);
      Assert.AreEqual(2, Test.Status.Durchlaeufe);
    }

    [NUnit.Framework.Test]
    public void LoggerIstImTestcaseVerfuegbar()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseC>();

      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppe), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 2);

      Starter.starte();

      Logger.Received().Log("Das ist ein Test!");
    }

    [NUnit.Framework.Test]
    public void OptionenWerdenDurchgereicht()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseC>();

      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppe), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 2);

      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);
      Starter.starte();
      Logger.Received().ConsoleEin = false;
      Logger.ClearReceivedCalls();

      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);
      Starter.starte();
      Logger.Received().ConsoleEin = true;
      Logger.ClearReceivedCalls();
    }

    [NUnit.Framework.Test]
    public void OptionenWerdenVererbt()
    {
      CDummyTestcaseC TestGruppe = createGruppe<CDummyTestcaseC>();

      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppe), Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 2);

      TestGruppe.Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);
      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);
      Starter.starte();
      Logger.Received().ConsoleEin = true;
      Logger.ClearReceivedCalls();

      TestGruppe.Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);
      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);
      Starter.starte();
      Logger.Received().ConsoleEin = false;
      Logger.ClearReceivedCalls();

      TestGruppe.Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Vererbt);
      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);
      Starter.starte();
      Logger.Received().ConsoleEin = true;
      Logger.ClearReceivedCalls();

      TestGruppe.Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Vererbt);
      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);
      Starter.starte();
      Logger.Received().ConsoleEin = false;
      Logger.ClearReceivedCalls();
    }


    private CTestProzessor createProzessor()
    {
      GameObject Objekt = new GameObject();
      return Objekt.AddComponent<CTestProzessor>();
    }

    [NUnit.Framework.Test]
    public void BeendeTestcaseErstWennProzessorFertigIst()
    {
      CDummyTestcaseE TestGruppe = createGruppe<CDummyTestcaseE>();

      IBefehl DummyBefehl1 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl2 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl3 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl4 = Substitute.For<IBefehl>();

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl2.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl3.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(x => false, x => true);
      DummyBefehl4.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      TestGruppe.addBefehl(DummyBefehl1);
      TestGruppe.addBefehl(DummyBefehl2);
      TestGruppe.addBefehl(DummyBefehl3);
      TestGruppe.addBefehl(DummyBefehl4);

      CTestProzessor EchterProzessor = createProzessor();
      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppe), Logger, Optionen, EchterProzessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 10);

      Starter.starte();

      Assert.AreEqual(0,    EchterProzessor.Befehle);
      Assert.AreEqual(true, EchterProzessor.IstFertig);
      DummyBefehl1.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl2.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl3.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl4.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
    }

    [NUnit.Framework.Test]
    public void StarteTestcaseErstWennProzessorFertigIst()
    {
      CDummyTestcaseA TestGruppe = createGruppe<CDummyTestcaseA>();
      CTestProzessor EchterProzessor = createProzessor();

      IBefehl DummyBefehl1 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl2 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl3 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl4 = Substitute.For<IBefehl>();

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl2.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl3.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(x => false, x => true);
      DummyBefehl4.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      EchterProzessor.add(DummyBefehl1);
      EchterProzessor.add(DummyBefehl2);
      EchterProzessor.add(DummyBefehl3);
      EchterProzessor.add(DummyBefehl4);

      Assert.AreEqual(4, EchterProzessor.Befehle);

      Optionen.AbbruchBeiFehler = true;
      CStartWrapper StartWrapper = new CStartWrapper(TestGruppe.Tests[0].starte, new CZusammenfassung(TestGruppe), Logger, Optionen, EchterProzessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 10);

      try
      {
        Starter.starte();
      }
      catch(Fehler)
      {

      }

      Assert.AreEqual(0, EchterProzessor.Befehle);
      Assert.AreEqual(true, EchterProzessor.IstFertig);
      DummyBefehl1.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl2.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl3.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl4.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());

    }

    [NUnit.Framework.Test]
    public void StarteKeineIgnoriertenTestcase()
    {
      ITestGruppe TestGruppe = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseA TestGruppeA = TestGruppe as CDummyTestcaseA;

      TestGruppeA.TestGruppeAktiv = false;
      ITestcase Test = TestGruppe.Tests[0];

      Assert.AreEqual(true, Test.Status.IgnoriereTest);

      CZusammenfassung Zusammenfassung = new CZusammenfassung(TestGruppe);

      CStartWrapper StartWrapper = new CStartWrapper(Test.starte, Zusammenfassung, Logger, Optionen, Prozessor);
      CCoroutineStarter Starter = new CCoroutineStarter(StartWrapper.starte, 1);
      TestGruppeA.TestA1Aufrufe = 0;

      Starter.starte();

      Assert.AreEqual(0, TestGruppeA.TestA1Aufrufe);
      Assert.AreEqual(1, Zusammenfassung.IgnorierteTests);
    }

  }
}