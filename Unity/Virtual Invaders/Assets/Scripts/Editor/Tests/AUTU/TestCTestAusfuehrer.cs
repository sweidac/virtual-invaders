using NUnit.Framework;
using UnityEngine;
using NSubstitute;

namespace AUTU
{
  [Category("UnityOnly")]
  class TestCTestAusfuehrer
  {
    private ILogger Logger = null;

    [SetUp]
    public void Setup()
    {
      Logger = Substitute.For<ILogger>();
    }

    [TearDown]
    public void Cleanup()
    {
      Logger = null;
    }

    private T createGruppe<T>() where T : CTestGruppe
    {
      GameObject Object = new GameObject();
      return Object.AddComponent<T>();
    }

    private CTestAusfuehrer createAusfuehrer()
    {
      GameObject Object = new GameObject();
      CTestAusfuehrer Ausfuehrer = Object.AddComponent<CTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;
      return Ausfuehrer;
    }

    [NUnit.Framework.Test]
    public void ErstelleObjekt()
    {
      ITestAusfuehrer TestAusfuehrer = createAusfuehrer();

      // Ein Ausführer erbt alles von der Test-Gruppe
      Assert.IsNotNull(TestAusfuehrer as ITestGruppe);
    }

    [NUnit.Framework.Test]
    public void InitialeZusammenfassung()
    {
      ITestAusfuehrer TestAusfuehrer = createAusfuehrer();

      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.Tests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
    }

    [NUnit.Framework.Test]
    public void StarteUngelaufeneTests()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeB.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteUngelaufeneTests, 10);
      Starter.starte();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB3Aufrufe);
    }

    [NUnit.Framework.Test]
    public void StarteUngelaufeneTestsNichtDoppelt()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      TestAusfuehrer.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteUngelaufeneTests, 10);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      Starter.starte();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB3Aufrufe);

      Starter.starte();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB3Aufrufe);
    }

    [NUnit.Framework.Test]
    public void StarteAlleTests()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeB.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteAlleTests, 10);
      Starter.starte();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB3Aufrufe);

      Starter.starte();

      Assert.AreEqual(2, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(2, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(2, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(2, GruppeB.TestB3Aufrufe);
    }

    [NUnit.Framework.Test]
    public void StarteAlleFehlerhaftenTests()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeB.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteAlleFehlerhaftenTests, 10);
      Starter.starte();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB3Aufrufe);

      Starter.starte();

      Assert.AreEqual(2, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(2, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(2, GruppeB.TestB3Aufrufe);
    }

    [NUnit.Framework.Test]
    public void VeraenderteZusammenfassungBeiAllenTests()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeB.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteAlleTests, 10);
      Starter.starte();

      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.Tests);
      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.KorrekteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.Pruefungen);
      Assert.AreEqual(2, TestAusfuehrer.Zusammenfassung.Fehler);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.FataleFehler);

      Starter.starte();

      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.Tests);
      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.KorrekteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.Pruefungen);
      Assert.AreEqual(2, TestAusfuehrer.Zusammenfassung.Fehler);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void VeraenderteZusammenfassungBeiUngelaufenenTests()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeB.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteUngelaufeneTests, 10);
      Starter.starte();

      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.Tests);
      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.KorrekteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.Pruefungen);
      Assert.AreEqual(2, TestAusfuehrer.Zusammenfassung.Fehler);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.FataleFehler);

      Starter.starte();

      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.Tests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.Pruefungen);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.Fehler);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void VeraenderteZusammenfassungBeiFehlerhaftenTests()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeB.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteAlleFehlerhaftenTests, 10);
      Starter.starte();

      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.Tests);
      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.KorrekteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.Pruefungen);
      Assert.AreEqual(2, TestAusfuehrer.Zusammenfassung.Fehler);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.FataleFehler);

      Starter.starte();

      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.Tests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.KorrekteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(3, TestAusfuehrer.Zusammenfassung.Pruefungen);
      Assert.AreEqual(2, TestAusfuehrer.Zusammenfassung.Fehler);
      Assert.AreEqual(1, TestAusfuehrer.Zusammenfassung.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void LoggerWirdDurchgereicht()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseC GruppeC = createGruppe<CDummyTestcaseC>();

      TestAusfuehrer.gruppeHinzufuegen(GruppeC);

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteAlleFehlerhaftenTests, 10);
      Starter.starte();

      Logger.Received().Log("Das ist ein Test!");
    }

    [NUnit.Framework.Test]
    public void OptionenWerdenDurchgereicht()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseC GruppeC = createGruppe<CDummyTestcaseC>();

      TestAusfuehrer.gruppeHinzufuegen(GruppeC);

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteAlleTests, 10);

      TestAusfuehrer.Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);
      Starter.starte();

      Logger.Received().ConsoleEin = false;
      Logger.ClearReceivedCalls();

      TestAusfuehrer.Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);
      Starter.starte();

      Logger.Received().ConsoleEin = true;
      Logger.ClearReceivedCalls();
    }

    [NUnit.Framework.Test]
    public void StarteKeineTestsWennModusAufStoppImInspektor()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      Ausfuehrer.gruppeHinzufuegen(GruppeA);

      Assert.AreEqual(TestAusfuehrer.AusfuehrModus.Stopp, Ausfuehrer.Ausfuehrmodus);

      GruppeA.TestA1Aufrufe = 0;

      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(0, GruppeA.TestA1Aufrufe);
      Logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<string>());
    }

    [NUnit.Framework.Test]
    public void StarteTestsEinmaligWennModusAufEinmaligImInspektor()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      Ausfuehrer.gruppeHinzufuegen(GruppeA);

      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Einmalig;

      GruppeA.TestA1Aufrufe = 0;

      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(TestAusfuehrer.AusfuehrModus.Stopp, Ausfuehrer.Ausfuehrmodus);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("fehlerhaft: 1")));

      Logger.ClearReceivedCalls();

      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(TestAusfuehrer.AusfuehrModus.Stopp, Ausfuehrer.Ausfuehrmodus);
      Logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<string>());
      Logger.ClearReceivedCalls();

      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Einmalig;
      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(2, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(TestAusfuehrer.AusfuehrModus.Stopp, Ausfuehrer.Ausfuehrmodus);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 2")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 2")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("fehlerhaft: 1")));
      Logger.ClearReceivedCalls();
    }

    [NUnit.Framework.Test]
    public void StarteFehlerhafteTestsWennModusAufFehlerhaftImInspektor()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseC GruppeC = createGruppe<CDummyTestcaseC>();
      Ausfuehrer.gruppeHinzufuegen(GruppeA);
      Ausfuehrer.gruppeHinzufuegen(GruppeC);

      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Einmalig;

      GruppeA.TestA1Aufrufe = 0;

      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(TestAusfuehrer.AusfuehrModus.Stopp, Ausfuehrer.Ausfuehrmodus);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 1")));
      Logger.Received().Log("Das ist ein Test!");
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 2")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("fehlerhaft: 1")));
      Logger.ClearReceivedCalls();

      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(TestAusfuehrer.AusfuehrModus.Stopp, Ausfuehrer.Ausfuehrmodus);
      Logger.DidNotReceiveWithAnyArgs().Log(Arg.Any<string>());
      Logger.ClearReceivedCalls();

      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.FehlerhafteTestsNochmal;
      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(2, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(TestAusfuehrer.AusfuehrModus.Stopp, Ausfuehrer.Ausfuehrmodus);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 2")));
      Logger.DidNotReceive().Log("Das ist ein Test!");
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 2")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("fehlerhaft: 1")));
      Logger.ClearReceivedCalls();
    }

    [NUnit.Framework.Test]
    public void WiederholeAlleTestsImWiederholeModus()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseC GruppeC = createGruppe<CDummyTestcaseC>();
      Ausfuehrer.gruppeHinzufuegen(GruppeA);
      Ausfuehrer.gruppeHinzufuegen(GruppeC);

      Ausfuehrer.AbbruchBeiFehler = false;
      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Wiederholung;

      GruppeA.TestA1Aufrufe = 0;

      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(1, GruppeA.TestA1Aufrufe);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 1")));
      Logger.Received().Log("Das ist ein Test!");
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 2")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("fehlerhaft: 1")));
      Logger.ClearReceivedCalls();

      Ausfuehrer.rufeAufUpdate();
      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(3, GruppeA.TestA1Aufrufe);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 3")));
      Logger.Received().Log("Das ist ein Test!");
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 3")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 2")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("fehlerhaft: 1")));
      Logger.ClearReceivedCalls();
    }

    [NUnit.Framework.Test]
    public void KeineFehlermeldungWennKeinTestEinenFehlerHat()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseC GruppeC = createGruppe<CDummyTestcaseC>();
      Ausfuehrer.gruppeHinzufuegen(GruppeC);

      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Wiederholung;

      Ausfuehrer.rufeAufUpdate();

      Logger.DidNotReceive().LogError(Arg.Is<string>(Text => Text.Contains("In Test-Iteration 1 ist ein Fehler aufgetreten.")));
      Logger.ClearReceivedCalls();

      Ausfuehrer.rufeAufUpdate();

      Logger.DidNotReceive().LogError(Arg.Is<string>(Text => Text.Contains("In Test-Iteration 2 ist ein Fehler aufgetreten.")));
      Logger.ClearReceivedCalls();
    }

    [NUnit.Framework.Test]
    public void FehlermeldungWennEinTestEinenFehlerHat()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      Ausfuehrer.gruppeHinzufuegen(GruppeA);

      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Wiederholung;

      Ausfuehrer.rufeAufUpdate();

      Logger.Received().LogError(Arg.Is<string>(Text => Text.Contains("In Test-Iteration 1 ist ein Fehler aufgetreten.")));
      Logger.ClearReceivedCalls();

      Ausfuehrer.rufeAufUpdate();

      Logger.Received().LogError(Arg.Is<string>(Text => Text.Contains("In Test-Iteration 2 ist ein Fehler aufgetreten.")));
      Logger.ClearReceivedCalls();
    }

    [NUnit.Framework.Test]
    public void SofortigerAbbruchBeiEinemFehler()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);

      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseC GruppeC = createGruppe<CDummyTestcaseC>();
      Ausfuehrer.gruppeHinzufuegen(GruppeA);
      Ausfuehrer.gruppeHinzufuegen(GruppeC);

      Ausfuehrer.AbbruchBeiFehler = true;
      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Wiederholung;

      GruppeA.TestA1Aufrufe = 0;

      bool WarFehler = false;

      try
      {
        Ausfuehrer.rufeAufUpdate();
      }
      catch(FehlerAbbruchWegenFehler)
      {
        WarFehler = true;
      }

      Assert.AreEqual(true, WarFehler);
    }

    [NUnit.Framework.Test]
    public void ErstelleProzessorBeiStartWennNochNichtVorhanden()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);

      Assert.IsNull(Ausfuehrer.GetComponentInChildren<ITestProzessorSchreiber>());
      Assert.IsNull(Ausfuehrer.GetComponentInChildren<ITestProzessorAusfuehrer>());
      Assert.IsNull(Ausfuehrer.GetComponentInChildren<IProzessorKomponenten>());

      Ausfuehrer.rufeAufStart();

      ITestProzessorSchreiber ProzessorSchreiber = Ausfuehrer.GetComponentInChildren<ITestProzessorSchreiber>();
      ITestProzessorAusfuehrer ProzessorAusfuehrer = Ausfuehrer.GetComponentInChildren<ITestProzessorAusfuehrer>();
      IProzessorKomponenten ProzessorKomponenten = Ausfuehrer.GetComponentInChildren<IProzessorKomponenten>();

      Assert.IsNotNull(ProzessorSchreiber);
      Assert.IsNotNull(ProzessorAusfuehrer);
      Assert.IsNotNull(ProzessorKomponenten);
      Assert.AreSame(ProzessorSchreiber, Ausfuehrer.getProzessorSchreiber());
      Assert.AreSame(ProzessorAusfuehrer, Ausfuehrer.getProzessorAusfuehrer());
      Assert.AreSame(ProzessorKomponenten, Ausfuehrer.getProzessorKomponenten());
    }

    [NUnit.Framework.Test]
    public void ErstelleKeinProzessorBeiStartWennSchonEinesVorhanden()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);

      GameObject ProzessorObjekt = new GameObject();
      ProzessorObjekt.transform.parent = Objekt.transform;
      ProzessorObjekt.name = "Prozessor";
      CTestProzessor NeuerProzessor = ProzessorObjekt.AddComponent<CTestProzessor>();

      Assert.IsNotNull(Ausfuehrer.getProzessorSchreiber() as CNullTestProzessor);
      Assert.IsNotNull(Ausfuehrer.getProzessorAusfuehrer() as CNullTestProzessor);
      Assert.IsNotNull(Ausfuehrer.getProzessorKomponenten() as CNullTestProzessor);
      Assert.IsNotNull(Ausfuehrer.GetComponentInChildren<ITestProzessorSchreiber>());
      Assert.IsNotNull(Ausfuehrer.GetComponentInChildren<ITestProzessorAusfuehrer>());
      Assert.IsNotNull(Ausfuehrer.GetComponentInChildren<IProzessorKomponenten>());
      foreach (ITestProzessorSchreiber TestProzessor in Ausfuehrer.GetComponentsInChildren<ITestProzessorSchreiber>())
      {
        Assert.AreSame(NeuerProzessor, TestProzessor);
      }
      foreach (ITestProzessorAusfuehrer TestProzessor in Ausfuehrer.GetComponentsInChildren<ITestProzessorAusfuehrer>())
      {
        Assert.AreSame(NeuerProzessor, TestProzessor);
      }
      foreach (IProzessorKomponenten TestProzessor in Ausfuehrer.GetComponentsInChildren<IProzessorKomponenten>())
      {
        Assert.AreSame(NeuerProzessor, TestProzessor);
      }

      Ausfuehrer.rufeAufStart();

      Assert.AreSame(NeuerProzessor, Ausfuehrer.getProzessorSchreiber());
      Assert.AreSame(NeuerProzessor, Ausfuehrer.getProzessorAusfuehrer());
      Assert.AreSame(NeuerProzessor, Ausfuehrer.getProzessorKomponenten());
      Assert.IsNotNull(Ausfuehrer.GetComponentInChildren<ITestProzessorSchreiber>());
      Assert.IsNotNull(Ausfuehrer.GetComponentInChildren<ITestProzessorAusfuehrer>());
      Assert.IsNotNull(Ausfuehrer.GetComponentInChildren<IProzessorKomponenten>());
      foreach (ITestProzessorSchreiber TestProzessor in Ausfuehrer.GetComponentsInChildren<ITestProzessorSchreiber>())
      {
        Assert.AreSame(NeuerProzessor, TestProzessor);
      }
      foreach (ITestProzessorAusfuehrer TestProzessor in Ausfuehrer.GetComponentsInChildren<ITestProzessorAusfuehrer>())
      {
        Assert.AreSame(NeuerProzessor, TestProzessor);
      }
      foreach (IProzessorKomponenten TestProzessor in Ausfuehrer.GetComponentsInChildren<IProzessorKomponenten>())
      {
        Assert.AreSame(NeuerProzessor, TestProzessor);
      }
    }

    [NUnit.Framework.Test]
    public void GebeProzessorZuTestcaseDurchZuUngelaufenenTests()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();
      Ausfuehrer.gruppeHinzufuegen(GruppeB);

      Ausfuehrer.rufeAufStart();

      GruppeB.TestB2Aufrufe = 0;
      GruppeB.UebergebenerTestProzessorInTestB2 = null;

      CCoroutineStarter Starter1 = new CCoroutineStarter(Ausfuehrer.starteUngelaufeneTests, 10);
      Starter1.starte();

      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreSame(Ausfuehrer.getProzessorSchreiber(), GruppeB.UebergebenerTestProzessorInTestB2);
    }

    [NUnit.Framework.Test]
    public void GebeProzessorZuTestcaseDurchZuFehlerhaftenTests()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();
      Ausfuehrer.gruppeHinzufuegen(GruppeB);

      Ausfuehrer.rufeAufStart();

      GruppeB.TestB2Aufrufe = 0;
      GruppeB.UebergebenerTestProzessorInTestB2 = null;

      CCoroutineStarter Starter1 = new CCoroutineStarter(Ausfuehrer.starteAlleFehlerhaftenTests, 10);
      Starter1.starte();

      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreSame(Ausfuehrer.getProzessorSchreiber(), GruppeB.UebergebenerTestProzessorInTestB2);
    }

    [NUnit.Framework.Test]
    public void GebeProzessorZuTestcaseDurchZuAllenTests()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();
      Ausfuehrer.gruppeHinzufuegen(GruppeB);

      Ausfuehrer.rufeAufStart();

      GruppeB.TestB2Aufrufe = 0;
      GruppeB.UebergebenerTestProzessorInTestB2 = null;

      CCoroutineStarter Starter1 = new CCoroutineStarter(Ausfuehrer.starteAlleTests, 10);
      Starter1.starte();

      Assert.AreEqual(1, GruppeB.TestB2Aufrufe);
      Assert.AreSame(Ausfuehrer.getProzessorSchreiber(), GruppeB.UebergebenerTestProzessorInTestB2);
    }

    [NUnit.Framework.Test]
    public void BearbeiteProzessorbefehleImUpdate()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);

      ITestProzessorAusfuehrer ProzessorAusfuehrer = Substitute.For<ITestProzessorAusfuehrer>();
      Ausfuehrer.setProzessorAusfuehrer(ProzessorAusfuehrer);

      Ausfuehrer.rufeAufUpdate();

      ProzessorAusfuehrer.Received().bearbeiteBefehl();
      ProzessorAusfuehrer.ClearReceivedCalls();

      Ausfuehrer.rufeAufUpdate();

      ProzessorAusfuehrer.Received().bearbeiteBefehl();
      ProzessorAusfuehrer.ClearReceivedCalls();
    }

    private T createProzessor<T>() where T : CTestProzessor
    {
      GameObject Objekt = new GameObject();
      return Objekt.AddComponent<T>();
    }

    [NUnit.Framework.Test]
    public void LoescheProzessorVorTeststart()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);

      CTestProzessor EchterProzessor = createProzessor<CTestProzessor>();
      Ausfuehrer.setProzessorAusfuehrer(EchterProzessor);
      Ausfuehrer.setProzessorSchreiber(EchterProzessor);

      CDummyTestcaseE GruppeE = createGruppe<CDummyTestcaseE>();
      Ausfuehrer.gruppeHinzufuegen(GruppeE);

      IBefehl DummyBefehl1 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl2 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl3 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl4 = Substitute.For<IBefehl>();

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(false);
      DummyBefehl2.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl3.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl4.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      EchterProzessor.add(DummyBefehl1);
      EchterProzessor.add(DummyBefehl2);
      GruppeE.addBefehl(DummyBefehl3);
      GruppeE.addBefehl(DummyBefehl4);

      Assert.AreEqual(2, EchterProzessor.Befehle);

      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Einmalig;
      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(0, EchterProzessor.Befehle);
      DummyBefehl2.DidNotReceiveWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl3.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl4.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
    }

    public class CTestKomponente : Object, ITestKomponente
    {
      public bool KomponenteAktiv { get; set; }
    }

    private class CSchalteKomponentenEinBefehl : IBefehl
    {
      public bool bearbeiten(ILogger Logger, IProzessorKomponenten Komponenten)
      {
        var Liste = Komponenten.sucheAlle<ITestKomponente>();
        foreach(ITestKomponente Komponente in Liste)
        {
          Komponente.KomponenteAktiv = true;
        }
        return true;
      }

      public override string ToString() { return "Schalte alle Komponenten ein."; }
    }

    [NUnit.Framework.Test]
    public void SchalteAlleProzessorKomponentenNachTestlaufAb()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);

      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      Ausfuehrer.setProzessorAusfuehrer(Prozessor);
      Ausfuehrer.setProzessorSchreiber(Prozessor);
      Ausfuehrer.setProzessorKomponenten(Prozessor);

      CTestKomponente TestKomponente1 = new CTestKomponente();
      CTestKomponente TestKomponente2 = new CTestKomponente();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente1));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente2));

      TestKomponente1.KomponenteAktiv = true;
      TestKomponente2.KomponenteAktiv = true;

      CDummyTestcaseE GruppeE = createGruppe<CDummyTestcaseE>();
      Ausfuehrer.gruppeHinzufuegen(GruppeE);
      GruppeE.addBefehl(new CSchalteKomponentenEinBefehl());

      CCoroutineStarter Starter1 = new CCoroutineStarter(Ausfuehrer.starteUngelaufeneTests, 10);
      Starter1.starte();

      Assert.AreEqual(false, TestKomponente1.KomponenteAktiv);
      Assert.AreEqual(false, TestKomponente2.KomponenteAktiv);

      TestKomponente1.KomponenteAktiv = true;
      TestKomponente2.KomponenteAktiv = true;

      CCoroutineStarter Starter2 = new CCoroutineStarter(Ausfuehrer.starteAlleFehlerhaftenTests, 10);
      Starter2.starte();

      Assert.AreEqual(false, TestKomponente1.KomponenteAktiv);
      Assert.AreEqual(false, TestKomponente2.KomponenteAktiv);

      TestKomponente1.KomponenteAktiv = true;
      TestKomponente2.KomponenteAktiv = true;

      CCoroutineStarter Starter3 = new CCoroutineStarter(Ausfuehrer.starteAlleTests, 10);
      Starter3.starte();

      Assert.AreEqual(false, TestKomponente1.KomponenteAktiv);
      Assert.AreEqual(false, TestKomponente2.KomponenteAktiv);
    }

    [NUnit.Framework.Test]
    public void IgnoriereTests()
    {
      CTestAusfuehrer TestAusfuehrer = createAusfuehrer();
      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseB GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeA.TestGruppeAktiv = false;
      GruppeB.TestGruppeAktiv = false;

      GruppeB.gruppeHinzufuegen(GruppeA);
      TestAusfuehrer.gruppeHinzufuegen(GruppeB);

      Assert.AreEqual(4, TestAusfuehrer.AnzahlTests);
      Assert.AreEqual(0, TestAusfuehrer.Zusammenfassung.IgnorierteTests);

      GruppeA.TestA1Aufrufe = 0;
      GruppeB.TestB1Aufrufe = 0;
      GruppeB.TestB2Aufrufe = 0;
      GruppeB.TestB3Aufrufe = 0;

      CCoroutineStarter Starter = new CCoroutineStarter(TestAusfuehrer.starteUngelaufeneTests, 10);
      Starter.starte();

      Assert.AreEqual(0, GruppeA.TestA1Aufrufe);
      Assert.AreEqual(0, GruppeB.TestB1Aufrufe);
      Assert.AreEqual(0, GruppeB.TestB2Aufrufe);
      Assert.AreEqual(0, GruppeB.TestB3Aufrufe);
      Assert.AreEqual(4, TestAusfuehrer.Zusammenfassung.IgnorierteTests);
    }

    [NUnit.Framework.Test]
    public void AusgabeBeiIgnoriertenTests()
    {
      GameObject Objekt = new GameObject();
      CDummyTestAusfuehrer Ausfuehrer = Objekt.AddComponent<CDummyTestAusfuehrer>();
      Ausfuehrer.setLogger(Logger);
      Ausfuehrer.AbbruchBeiFehler = false;

      CDummyTestcaseA GruppeA = createGruppe<CDummyTestcaseA>();
      CDummyTestcaseC GruppeC = createGruppe<CDummyTestcaseC>();

      GruppeA.TestGruppeAktiv = false;
      GruppeC.TestGruppeAktiv = false;

      Ausfuehrer.gruppeHinzufuegen(GruppeA);
      Ausfuehrer.gruppeHinzufuegen(GruppeC);

      Ausfuehrer.AbbruchBeiFehler = false;
      Ausfuehrer.Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Wiederholung;

      GruppeA.TestA1Aufrufe = 0;

      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(0, GruppeA.TestA1Aufrufe);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 0")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ignoriert: 2")));
      Logger.Received().LogWarning(Arg.Is<string>(Text => Text.Contains("wurden Tests ignoriert.")));
      Logger.ClearReceivedCalls();

      Ausfuehrer.rufeAufUpdate();
      Ausfuehrer.rufeAufUpdate();

      Assert.AreEqual(0, GruppeA.TestA1Aufrufe);
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Starte Test-Iteration 3")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("Beende Test-Iteration 3")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ausgeführt: 0")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("ignoriert: 2")));
      Logger.Received().LogWarning(Arg.Is<string>(Text => Text.Contains("wurden Tests ignoriert.")));
      Logger.ClearReceivedCalls();
    }

  }
}

