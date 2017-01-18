using NUnit.Framework;
using NSubstitute;
using UnityEngine;

namespace AUTU
{
  [Category("UnityOnly")]
  class TestCTestProzessor
  {

    private class CDummyBefehl : IBefehl
    {
      public bool bearbeiten(ILogger Logger, IProzessorKomponenten Komponenten) { return true; }
    }

    private T createProzessor<T>() where T : CTestProzessor
    {
      GameObject Objekt = new GameObject();
      return Objekt.AddComponent<T>();
    }

    [NUnit.Framework.Test]
    public void InitialisiereProzessor()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;

      Assert.AreEqual(true, Ausfuehrer.IstFertig);
      Assert.AreEqual(0,    Ausfuehrer.Befehle);
    }

    [NUnit.Framework.Test]
    public void SchreibeBefehle()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;

      CDummyBefehl Befehl1 = new CDummyBefehl();
      CDummyBefehl Befehl2 = Schreiber.add(Befehl1);

      Assert.AreSame(Befehl1, Befehl2);
      Assert.AreEqual(1, Ausfuehrer.Befehle);

      Schreiber.add(new CDummyBefehl());

      Assert.AreEqual(2, Ausfuehrer.Befehle);

      Schreiber.add(new CDummyBefehl());

      Assert.AreEqual(3, Ausfuehrer.Befehle);
    }

    [NUnit.Framework.Test]
    public void BefehleAbarbeiten()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;

      IBefehl DummyBefehl = Substitute.For<IBefehl>();
      Schreiber.add(DummyBefehl);

      Assert.AreEqual(1, Ausfuehrer.Befehle);

      DummyBefehl.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(false);

      Ausfuehrer.bearbeiteBefehl();

      DummyBefehl.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl.ClearReceivedCalls();
      Assert.AreEqual(1, Ausfuehrer.Befehle);

      DummyBefehl.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      Ausfuehrer.bearbeiteBefehl();

      DummyBefehl.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      Assert.AreEqual(0, Ausfuehrer.Befehle);
    }

    [NUnit.Framework.Test]
    public void BearbeiteBefehleSolangeWelcheVorhandenSind()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;

      IBefehl DummyBefehl1 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl2 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl3 = Substitute.For<IBefehl>();
      IBefehl DummyBefehl4 = Substitute.For<IBefehl>();
      Schreiber.add(DummyBefehl1);
      Schreiber.add(DummyBefehl2);
      Schreiber.add(DummyBefehl3);
      Schreiber.add(DummyBefehl4);

      Assert.AreEqual(4, Ausfuehrer.Befehle);

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl2.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl3.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(false);

      Ausfuehrer.bearbeiteBefehl();

      DummyBefehl1.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl2.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl3.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl4.DidNotReceiveWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl1.ClearReceivedCalls();
      DummyBefehl2.ClearReceivedCalls();
      DummyBefehl3.ClearReceivedCalls();
      DummyBefehl4.ClearReceivedCalls();
      Assert.AreEqual(2, Ausfuehrer.Befehle);

      DummyBefehl3.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl4.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      Ausfuehrer.bearbeiteBefehl();

      DummyBefehl1.DidNotReceiveWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl2.DidNotReceiveWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl3.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl4.ReceivedWithAnyArgs().bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>());
      DummyBefehl1.ClearReceivedCalls();
      DummyBefehl2.ClearReceivedCalls();
      DummyBefehl3.ClearReceivedCalls();
      DummyBefehl4.ClearReceivedCalls();
      Assert.AreEqual(0, Ausfuehrer.Befehle);
    }

    [NUnit.Framework.Test]
    public void BearbeiteKeineLeereSchlange()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      //ITestProzessorSchreiber Schreiber = Prozessor;

      Assert.AreEqual(0, Ausfuehrer.Befehle);

      Ausfuehrer.bearbeiteBefehl();

      Assert.AreEqual(0, Ausfuehrer.Befehle);
    }

    [NUnit.Framework.Test]
    public void WarteBisProzessorFertigIst()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;

      Schreiber.add(new CDummyBefehl());
      Schreiber.add(new CDummyBefehl());
      Schreiber.add(new CDummyBefehl());
      Schreiber.add(new CDummyBefehl());

      Assert.AreEqual(4, Ausfuehrer.Befehle);

      CustomYieldInstruction WarteAnweisung = Schreiber.warteBisFertig();

      Assert.AreEqual(true, WarteAnweisung.keepWaiting);

      Ausfuehrer.bearbeiteBefehl();

      Assert.AreEqual(0, Ausfuehrer.Befehle);

      Assert.AreEqual(false, WarteAnweisung.keepWaiting);
    }

    [NUnit.Framework.Test]
    public void Reset()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;

      IBefehl DummyBefehl1 = Schreiber.add(Substitute.For<IBefehl>());
      IBefehl DummyBefehl2 = Schreiber.add(Substitute.For<IBefehl>());
      IBefehl DummyBefehl3 = Schreiber.add(Substitute.For<IBefehl>());
      IBefehl DummyBefehl4 = Schreiber.add(Substitute.For<IBefehl>());

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(false);
      DummyBefehl2.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl3.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl4.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      Assert.AreEqual(4, Ausfuehrer.Befehle);
      Assert.AreEqual(false, Ausfuehrer.IstFertig);

      Ausfuehrer.bearbeiteBefehl();

      Assert.AreEqual(4, Ausfuehrer.Befehle);
      Assert.AreEqual(false, Ausfuehrer.IstFertig);

      Ausfuehrer.reset();

      Assert.AreEqual(0, Ausfuehrer.Befehle);
      Assert.AreEqual(true, Ausfuehrer.IstFertig);
    }

    [NUnit.Framework.Test]
    public void DebugAusgabe()
    {
      ILogger Logger = Substitute.For<ILogger>();
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;
      Ausfuehrer.setLogger(Logger);

      IBefehl DummyBefehl1 = Schreiber.add(Substitute.For<IBefehl>());
      IBefehl DummyBefehl2 = Schreiber.add(Substitute.For<IBefehl>());
      IBefehl DummyBefehl3 = Schreiber.add(Substitute.For<IBefehl>());
      IBefehl DummyBefehl4 = Schreiber.add(Substitute.For<IBefehl>());

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl1.ToString().Returns("DummyBefehl1");
      DummyBefehl2.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl2.ToString().Returns("DummyBefehl2");
      DummyBefehl3.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl3.ToString().Returns("DummyBefehl3");
      DummyBefehl4.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);
      DummyBefehl4.ToString().Returns("DummyBefehl4");

      Ausfuehrer.bearbeiteBefehl();

      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("DummyBefehl1")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("DummyBefehl2")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("DummyBefehl3")));
      Logger.Received().Log(Arg.Is<string>(Text => Text.Contains("DummyBefehl4")));
    }

    [NUnit.Framework.Test]
    public void AbschaltenVonDebugAusgabe()
    {
      ILogger Logger = Substitute.For<ILogger>();
      CDummyTestProzessor Prozessor = createProzessor<CDummyTestProzessor>();
      Prozessor.setLogger(Logger);

      Prozessor.DebugAusgaben = true;
      Prozessor.rufeAufOnValidate();

      Logger.Received().ConsoleEin = true;

      Prozessor.DebugAusgaben = false;
      Prozessor.rufeAufOnValidate();

      Logger.Received().ConsoleEin = false;
    }

    [NUnit.Framework.Test]
    public void LoggerWirdAnBefehlWeitergereicht()
    {
      ILogger Logger = Substitute.For<ILogger>();
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;
      Ausfuehrer.setLogger(Logger);

      IBefehl DummyBefehl1 = Schreiber.add(Substitute.For<IBefehl>());

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      Ausfuehrer.bearbeiteBefehl();

      DummyBefehl1.Received().bearbeiten(Logger, Arg.Any<IProzessorKomponenten>());
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenWerdenAnBefehlWeitergereicht()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      ITestProzessorAusfuehrer Ausfuehrer = Prozessor;
      ITestProzessorSchreiber Schreiber = Prozessor;

      IBefehl DummyBefehl1 = Schreiber.add(Substitute.For<IBefehl>());

      DummyBefehl1.bearbeiten(Arg.Any<ILogger>(), Arg.Any<IProzessorKomponenten>()).Returns(true);

      Ausfuehrer.bearbeiteBefehl();

      DummyBefehl1.Received().bearbeiten(Arg.Any<ILogger>(), Prozessor);
    }

    private class CTestKomponente : Object, ITestKomponente
    {
      public bool KomponenteAktiv { get; set; }
    }

    [NUnit.Framework.Test]
    public void ProzessorGibtNullZurueckWennKomponenteNichtExistiert()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      Assert.AreEqual(null, Komponenten.suche<CTestKomponente>());
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenKoennenUeberKlasseErfragtWerden()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      CTestKomponente TestKomponente = new CTestKomponente();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente));

      Assert.AreSame(TestKomponente, Komponenten.suche<CTestKomponente>());
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenKoennenUeberNamenErfragtWerden()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      CTestKomponente TestKomponente1 = new CTestKomponente();
      CTestKomponente TestKomponente2 = new CTestKomponente();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente2, "Komp2"));

      Assert.AreSame(TestKomponente2, Komponenten.suche<Object>("Komp2"));
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenKoennenNurUeberNamenErfragtWerden()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      CTestKomponente TestKomponente1 = new CTestKomponente();
      CTestKomponente TestKomponente2 = new CTestKomponente();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente2, "Komp2"));

      Assert.AreSame(TestKomponente2, Komponenten.suche("Komp2"));
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenGebenAlternativesObjektZurueck()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      CTestKomponente TestKomponente1 = new CTestKomponente();

      Assert.AreSame(TestKomponente1, Komponenten.suche(null, TestKomponente1));
      Assert.AreSame(TestKomponente1, Komponenten.suche("", TestKomponente1));
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenGebenAlternativesObjektZurueckNurUeberDenNamen()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      CTestKomponente TestKomponente1 = new CTestKomponente();
      CTestKomponente TestKomponente2 = new CTestKomponente();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente1, "Komp1"));

      Assert.AreSame(TestKomponente2, Komponenten.suche("Komp2", TestKomponente2));
    }

    [NUnit.Framework.Test]
    public void PasseNamenVonKomponentenAutomatischAn()
    {
      CDummyTestProzessor Prozessor = createProzessor<CDummyTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      GameObject Objekt = new GameObject();
      CDummyTestcaseE Testcase = Objekt.AddComponent<CDummyTestcaseE>();
      Testcase.name = "DummyTestcase";

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Testcase));

      Assert.AreSame(null, Komponenten.suche("DummyTestcase"));

      Prozessor.rufeAufOnValidate();

      Assert.AreSame(Testcase, Komponenten.suche("DummyTestcase"));
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenKoennenAnGameObjektAngebundenSein()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      GameObject Objekt1 = new GameObject();
      CDummyTestcaseA TestKomponente1 = Objekt1.AddComponent<CDummyTestcaseA>();
      GameObject Objekt2 = new GameObject();
      CDummyTestcaseB TestKomponente2 = Objekt1.AddComponent<CDummyTestcaseB>();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt1));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt2));

      Assert.AreSame(TestKomponente1, Komponenten.suche<CDummyTestcaseA>());
      Assert.AreSame(TestKomponente2, Komponenten.suche<CDummyTestcaseB>());
    }

    [NUnit.Framework.Test]
    public void ProzessorKomponentenKoennenAnGameObjektAngebundenSeinSucheUeberNamen()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();
      IProzessorKomponenten Komponenten = Prozessor;

      GameObject Objekt1 = new GameObject();
      CDummyTestcaseA TestKomponente1 = Objekt1.AddComponent<CDummyTestcaseA>();
      GameObject Objekt2 = new GameObject();
      CDummyTestcaseA TestKomponente2 = Objekt2.AddComponent<CDummyTestcaseA>();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt2, "Komp2"));

      Assert.AreSame(TestKomponente1, Komponenten.suche<CDummyTestcaseA>("Komp1"));
      Assert.AreSame(TestKomponente2, Komponenten.suche<CDummyTestcaseA>("Komp2"));
    }

    [NUnit.Framework.Test]
    public void SucheAlleUnterKomponentenDieDemTypenEntsprechen()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();

      GameObject Objekt1 = new GameObject();
      CDummyTestcaseA TestKomponente1 = Objekt1.AddComponent<CDummyTestcaseA>();
      GameObject Objekt2 = new GameObject();
      CDummyTestcaseA TestKomponente2 = Objekt2.AddComponent<CDummyTestcaseA>();
      GameObject Objekt3 = new GameObject();
      CDummyTestcaseB TestKomponente3 = Objekt3.AddComponent<CDummyTestcaseB>();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt2, "Komp2"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt3, "Komp3"));

      var Liste1 = Prozessor.sucheAlle<CDummyTestcaseA>();
      Assert.AreSame(TestKomponente1, Liste1.Find(E => (ReferenceEquals(E, TestKomponente1))));
      Assert.AreSame(TestKomponente2, Liste1.Find(E => (ReferenceEquals(E, TestKomponente2))));
      var Liste2 = Prozessor.sucheAlle<CDummyTestcaseB>();
      Assert.AreSame(TestKomponente3, Liste2.Find(E => (ReferenceEquals(E, TestKomponente3))));
    }

    [NUnit.Framework.Test]
    public void SucheAlleUnterKomponentenDieDemTypenEntsprechenUeberNamen()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();

      GameObject Objekt1 = new GameObject();
      CDummyTestcaseA TestKomponente1 = Objekt1.AddComponent<CDummyTestcaseA>();
      CDummyTestcaseA TestKomponente2 = Objekt1.AddComponent<CDummyTestcaseA>();
      GameObject Objekt3 = new GameObject();
      CDummyTestcaseA TestKomponente3 = Objekt3.AddComponent<CDummyTestcaseA>();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt3, "Komp3"));

      var Liste = Prozessor.sucheAlle<CDummyTestcaseA>("Komp1");
      Assert.AreSame(TestKomponente1, Liste.Find(E => (ReferenceEquals(E, TestKomponente1))));
      Assert.AreSame(TestKomponente2, Liste.Find(E => (ReferenceEquals(E, TestKomponente2))));
      Assert.AreEqual(null, Liste.Find(E => (ReferenceEquals(E, TestKomponente3))));
    }

    [NUnit.Framework.Test]
    public void SucheAlleKomponentenDieDemTypenEntsprechen()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();

      CTestKomponente TestKomponente1 = new CTestKomponente();
      CTestKomponente TestKomponente2 = new CTestKomponente();
      CTestKomponente TestKomponente3 = new CTestKomponente();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente2, "Komp2"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente3, "Komp3"));

      var Liste = Prozessor.sucheAlle<CTestKomponente>();
      Assert.AreSame(TestKomponente1, Liste.Find(E => (ReferenceEquals(E, TestKomponente1))));
      Assert.AreSame(TestKomponente2, Liste.Find(E => (ReferenceEquals(E, TestKomponente2))));
      Assert.AreSame(TestKomponente3, Liste.Find(E => (ReferenceEquals(E, TestKomponente3))));
    }

    [NUnit.Framework.Test]
    public void SucheAlleKomponentenDieDemTypenEntsprechenUeberDenNamen()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();

      CTestKomponente TestKomponente1 = new CTestKomponente();
      CTestKomponente TestKomponente2 = new CTestKomponente();
      CTestKomponente TestKomponente3 = new CTestKomponente();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente2, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente3, "Komp3"));

      var Liste = Prozessor.sucheAlle<CTestKomponente>("Komp1");

      Assert.AreSame(TestKomponente1, Liste.Find(E => (ReferenceEquals(E, TestKomponente1))));
      Assert.AreSame(TestKomponente2, Liste.Find(E => (ReferenceEquals(E, TestKomponente2))));
      Assert.AreEqual(null, Liste.Find(E => (ReferenceEquals(E, TestKomponente3))));
    }

    [NUnit.Framework.Test]
    public void AbschaltenAllerKomponenten()
    {
      CTestProzessor Prozessor = createProzessor<CTestProzessor>();

      CTestKomponente TestKomponente1 = new CTestKomponente();
      CTestKomponente TestKomponente2 = new CTestKomponente();
      CTestKomponente TestKomponente3 = new CTestKomponente();
      GameObject Objekt3 = new GameObject();
      CDummyTestcaseE TestKomponente4 = Objekt3.AddComponent<CDummyTestcaseE>();
      CDummyTestcaseE TestKomponente5 = Objekt3.AddComponent<CDummyTestcaseE>();

      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente1, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente2, "Komp1"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(TestKomponente3, "Komp3"));
      Prozessor.KomponentenListe.Add(new CTestProzessor.CKomponentenBeschreibung(Objekt3, "Komp4"));

      TestKomponente1.KomponenteAktiv = true;
      TestKomponente2.KomponenteAktiv = true;
      TestKomponente3.KomponenteAktiv = true;
      TestKomponente4.KomponenteAktiv = true;
      TestKomponente5.KomponenteAktiv = true;

      Prozessor.alleKomponentenAbschalten();

      Assert.AreEqual(false, TestKomponente1.KomponenteAktiv);
      Assert.AreEqual(false, TestKomponente2.KomponenteAktiv);
      Assert.AreEqual(false, TestKomponente3.KomponenteAktiv);
      Assert.AreEqual(false, TestKomponente4.KomponenteAktiv);
      Assert.AreEqual(false, TestKomponente5.KomponenteAktiv);
    }

  }
}
