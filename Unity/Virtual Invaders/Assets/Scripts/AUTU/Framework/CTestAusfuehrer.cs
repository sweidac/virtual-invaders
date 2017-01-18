using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace AUTU
{
  public class TestAusfuehrer
  {
    public enum AusfuehrModus
    {
      Stopp,
      Einmalig,
      FehlerhafteTestsNochmal,
      Wiederholung
    }
  }

  public class CTestAusfuehrer : CTestGruppe, ITestAusfuehrer
  {
    /// Optionen die von Unity aus zu sehen sind
    public TestAusfuehrer.AusfuehrModus Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
    public bool AbbruchBeiFehler = true;

    private IZusammenfassungsAenderer ZusammenfassungsAenderer = null;
    private ILogger TestLogger = null;

    protected ITestProzessorSchreiber TestProzessorSchreiber { get; set; }
    protected ITestProzessorAusfuehrer TestProzessorAusfuehrer { get; set; }
    protected IProzessorKomponenten TestProzessorKomponenten { get; set; }

    public IZusammenfassung Zusammenfassung
    {
      get;
      private set;
    }

    public CTestAusfuehrer()
    {
      CZusammenfassung Zusammenfassung = new CZusammenfassung(this);
      this.Zusammenfassung = Zusammenfassung;
      ZusammenfassungsAenderer = Zusammenfassung;

      TestLogger = new CLogger();
      CNullTestProzessor NullProzessor = new CNullTestProzessor();
      TestProzessorSchreiber = NullProzessor;
      TestProzessorAusfuehrer = NullProzessor;
      TestProzessorKomponenten = NullProzessor;
    }

    public IEnumerator starteUngelaufeneTests()
    {
      ZusammenfassungsAenderer.reset();
      Optionen.AbbruchBeiFehler = AbbruchBeiFehler;

      foreach (ITestcase Test in Tests)
      {
        if (!Test.Status.IstGelaufen)
        {
          TestLogger.Log("* Starte Test: " + Test.VollerName);
          yield return Test.starte(ZusammenfassungsAenderer, TestLogger, Optionen, TestProzessorSchreiber);

          if (Optionen.AbbruchBeiFehler)
          {
            if (Zusammenfassung.Fehler > 0 || Zusammenfassung.FataleFehler > 0)
            {
              Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
              break;
            }
          }
        }
      }

      TestProzessorKomponenten.alleKomponentenAbschalten();
    }

    public IEnumerator starteAlleTests()
    {
      ZusammenfassungsAenderer.reset();
      Optionen.AbbruchBeiFehler = AbbruchBeiFehler;
      TestProzessorKomponenten.alleKomponentenAbschalten();

      foreach (ITestcase Test in Tests)
      {
        TestLogger.Log("* Starte Test: " + Test.VollerName);
        yield return Test.starte(ZusammenfassungsAenderer, TestLogger, Optionen, TestProzessorSchreiber);

        if (Optionen.AbbruchBeiFehler)
        {
          if (Zusammenfassung.Fehler > 0 || Zusammenfassung.FataleFehler > 0)
          {
            Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
            break;
          }
        }
      }

      TestProzessorKomponenten.alleKomponentenAbschalten();
    }

    public IEnumerator starteAlleFehlerhaftenTests()
    {
      ZusammenfassungsAenderer.reset();
      Optionen.AbbruchBeiFehler = AbbruchBeiFehler;
      TestProzessorKomponenten.alleKomponentenAbschalten();

      foreach (ITestcase Test in Tests)
      {
        if ((!Test.Status.IstGelaufen) || (Test.Status.Fehler > 0) || (Test.Status.FataleFehler > 0))
        {
          TestLogger.Log("* Starte Test: " + Test.VollerName);
          yield return Test.starte(ZusammenfassungsAenderer, TestLogger, Optionen, TestProzessorSchreiber);

          if (Optionen.AbbruchBeiFehler)
          {
            if (Zusammenfassung.Fehler > 0 || Zusammenfassung.FataleFehler > 0)
            {
              Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
              break;
            }
          }
        }
      }

      TestProzessorKomponenten.alleKomponentenAbschalten();
    }

    public void setLogger(ILogger Logger)
    {
      Debug.Assert(Logger != null, "Es wurde kein gültiges Logger Objekt angegeben.");

      TestLogger = Logger;
    }

    private TestAusfuehrer.AusfuehrModus LetzterAusfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
    private bool IstTestlaufAktiv = false;

    protected new void Start()
    {
      base.Start();

      ITestProzessorSchreiber GefundenerTestProzessorSchreiber = GetComponentInChildren<ITestProzessorSchreiber>();
      ITestProzessorAusfuehrer GefundenerTestProzessorAusfuehrer = GetComponentInChildren<ITestProzessorAusfuehrer>();
      IProzessorKomponenten GefundenerTestProzessorKomponenten = GetComponentInChildren<IProzessorKomponenten>();

      if (GefundenerTestProzessorSchreiber == null && GefundenerTestProzessorAusfuehrer == null && GefundenerTestProzessorKomponenten == null)
      {
        GameObject Objekt = new GameObject();
        Objekt.transform.parent = this.transform;
        Objekt.name = "Prozessor";
        CTestProzessor NeuerProzessor = Objekt.AddComponent<CTestProzessor>();

        Debug.Assert(NeuerProzessor != null, "Kann kein korrektes ITestProzessor Objekt erstellen.");

        TestProzessorSchreiber = NeuerProzessor;
        TestProzessorAusfuehrer = NeuerProzessor;
        TestProzessorKomponenten = NeuerProzessor;
      }
      else if(GefundenerTestProzessorSchreiber != null && GefundenerTestProzessorAusfuehrer != null && GefundenerTestProzessorKomponenten != null)
      {
        TestProzessorSchreiber = GefundenerTestProzessorSchreiber;
        TestProzessorAusfuehrer = GefundenerTestProzessorAusfuehrer;
        TestProzessorKomponenten = GefundenerTestProzessorKomponenten;
      }
      else
      {
        Debug.LogError("Entweder es ist kein ITestProzessorSchreibe Objekt, kein ITestProzessorAusfuehrer Objekt und kein IProzessorKomponenten Objekt vorhanden, oder es sind alle drei vorhanden!");
      }
    }

    protected void Update()
    {
      TestProzessorAusfuehrer.bearbeiteBefehl();

      if (Input.GetKey(KeyCode.LeftControl))
      {
        if (!IstTestlaufAktiv)
        {
          if (Input.GetKeyDown(KeyCode.T))
          {
            Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Einmalig;
          }
          else if (Input.GetKeyDown(KeyCode.C))
          {
            Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Wiederholung;
          }
        }
        else
        {
          if (Input.GetKeyDown(KeyCode.T))
          {
            Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
          }
          else if (Input.GetKeyDown(KeyCode.C))
          {
            Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Wiederholung;
          }
        }
      }

      if (!IstTestlaufAktiv)
      {
        if (LetzterAusfuehrmodus != Ausfuehrmodus || Ausfuehrmodus == TestAusfuehrer.AusfuehrModus.Wiederholung)
        {
          LetzterAusfuehrmodus = Ausfuehrmodus;

          if (Ausfuehrmodus != TestAusfuehrer.AusfuehrModus.Stopp)
          {
            TestProzessorAusfuehrer.reset();
            starteTestCoroutine(generischeTestCoroutine);
          }
        }
      }
    }

    private uint Iteration = 0;

    private IEnumerator generischeTestCoroutine()
    {
      IstTestlaufAktiv = true;
      Iteration++;

      TestLogger.ConsoleEin = Optionen.KonsolenLogging;
      TestLogger.Log("*** Starte Test-Iteration "+Iteration);

      switch (Ausfuehrmodus)
      {
        case TestAusfuehrer.AusfuehrModus.Einmalig:
          yield return starteAlleTests();
          if (Ausfuehrmodus != TestAusfuehrer.AusfuehrModus.Wiederholung)
          {
            Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
          }
          break;

        case TestAusfuehrer.AusfuehrModus.FehlerhafteTestsNochmal:
          yield return starteAlleFehlerhaftenTests();
          if (Ausfuehrmodus != TestAusfuehrer.AusfuehrModus.Wiederholung)
          {
            Ausfuehrmodus = TestAusfuehrer.AusfuehrModus.Stopp;
          }
          break;

        case TestAusfuehrer.AusfuehrModus.Wiederholung:
          yield return starteAlleTests();
          break;

        default:
          Debug.Assert(false, "Ungültiger Test-Modus: " + Ausfuehrmodus);
          break;
      }

      TestLogger.Log("*** Beende Test-Iteration "+Iteration);
      TestLogger.Log(Zusammenfassung.ToString());
      if (Zusammenfassung.IgnorierteTests > 0)
      {
        TestLogger.LogWarning("*** Warnung: In Test-Iteration " + Iteration + " wurden Tests ignoriert.");
      }

      if (Zusammenfassung.Fehler > 0 || Zusammenfassung.FataleFehler > 0)
      {
        TestLogger.LogError("*** Achtung: In Test-Iteration "+Iteration+" ist ein Fehler aufgetreten.");
      }

      TestProzessorKomponenten.alleKomponentenAbschalten();
      LetzterAusfuehrmodus = Ausfuehrmodus;
      IstTestlaufAktiv = false;
    }

    public delegate IEnumerator TestCoroutine();

    virtual protected void starteTestCoroutine(TestCoroutine Coroutine)
    {
      StartCoroutine(Coroutine());
    }

  }
}
