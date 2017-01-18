using System.Collections.Generic;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace AUTU
{
  public class CTestGruppe : ATestGruppe
  {
    /// Optionen die von Unity aus gesetzt werden können
    public TestOptionen.LoggingStatus Konsolenausgabe = TestOptionen.LoggingStatus.Vererbt;
    public bool TestGruppeAktiv = true;

    protected IPruefer Pruefer { get; private set; }
    protected ILogger  Logger { get; private set; }
    protected ITestProzessorSchreiber Prozessor { get; private set; }

    public override bool IstTestGruppeAktiv
    {
      get
      {
        return TestGruppeAktiv;
      }
    }

    public ITestOptionen Optionen { get; private set; }

    public List<ATestGruppe> TestGruppen = new List<ATestGruppe>();

    public CTestGruppe()
    {
      Optionen = new CTestOptionen();
    }

    public override string Name
    {
      get { return gameObject.name; }
    }

    public override uint AnzahlGruppen
    {
      get
      {
        uint AnzahlGruppen = 0;

        foreach (ITestGruppe Gruppe in Gruppen)
        {
          AnzahlGruppen += 1 + Gruppe.AnzahlGruppen;
        }

        return AnzahlGruppen;
      }
    }

    public override uint AnzahlTests
    {
      get
      {
        return (uint)Tests.Count;
      }
    }

    private MethodInfo[] getMethodenliste(Type Klasse)
    {
      BindingFlags TestMethodFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

      return Klasse.GetMethods(TestMethodFlags);
    }

    private bool istMethodeEinTestcase(MethodInfo MethodenInformation)
    {
      bool HatKeineParameter          = MethodenInformation.GetParameters().Length == 0;
      bool HatRichtigenRueckgabetypen = MethodenInformation.ReturnType == typeof(IEnumerator);

      return HatKeineParameter && HatRichtigenRueckgabetypen && besitztTestcaseAttribut(MethodenInformation);
    }

    private bool besitztTestcaseAttribut(MethodInfo MethodenInformation)
    {
      foreach (Attribute Attr in MethodenInformation.GetCustomAttributes(true))
      {
        if (Attr is Test)
        {
          return true;
        }
      }

      return false;
    }

    private bool besitztIgnoriereAttribut(MethodInfo MethodenInformation)
    {
      foreach (Attribute Attr in MethodenInformation.GetCustomAttributes(true))
      {
        if (Attr is Ignoriere)
        {
          return true;
        }
      }

      return false;
    }

    public override ReadOnlyCollection<ITestGruppe> Gruppen
    {
      get
      {
        if (GruppenListe == null)
        {
          GruppenListe = new List<ITestGruppe>();
        }

        return GruppenListe.AsReadOnly();
      }
    }

    public override void gruppeHinzufuegen(ITestGruppe Gruppe)
    {
      if (TestListe != null)
      {
        throw new FehlerSpaetesHinzufuegenVonGruppe(Gruppe.Name, Name);
      }

      if (Gruppen != null)
      {
        GruppenListe.Add(Gruppe);
      }
    }

    private List<ITestGruppe> GruppenListe = null;

    public override ReadOnlyCollection<ITestcase> Tests
    {
      get
      {
        if (TestListe == null)
        {
          TestListe = new List<ITestcase>();

          findeTestcases(TestListe);
        }

        return TestListe.AsReadOnly();
      }
    }

    private List<ITestcase> TestListe = null;

    private void findeTestcases(List<ITestcase> Liste)
    {
      foreach (MethodInfo Methode in getMethodenliste(GetType()))
      {
        if (istMethodeEinTestcase(Methode))
        {
          CTestStatus Status = new CTestStatus();
          Status.ignoriereTest((!IstTestGruppeAktiv) || besitztIgnoriereAttribut(Methode));
          Liste.Add(new CTestcase(Methode.Name, Name, Methode, this, Status, Status, Optionen));
        }
      }

      foreach (ITestGruppe Gruppe in Gruppen)
      {
        foreach (ITestcase Test in Gruppe.Tests)
        {
          Liste.Add(new CTestcase(Test, Name, Optionen));
        }
      }
    }

    public override bool preTest(IPruefer Pruefer, ILogger Logger, ITestProzessorSchreiber Prozessor)
    {
      this.Pruefer = Pruefer;
      this.Logger = Logger;
      this.Prozessor = Prozessor;
      return preStartTest();
    }

    public override void postTest(bool IstTestGelaufen)
    {
      postStartTest(IstTestGelaufen);
      Pruefer = null;
      Logger = null;
    }

    public virtual bool preStartTest()
    {
      return true;
    }

    public virtual void postStartTest(bool IstTestGelaufen)
    {

    }

    protected void Start()
    {
      fuegeGruppenAusListeHinzu();
      fuegeUntergruppenHinzu();
    }

    private void fuegeGruppenAusListeHinzu()
    {
      if (GruppenListe == null)
      {
        GruppenListe = new List<ITestGruppe>();
      }

      foreach (ITestGruppe Gruppe in TestGruppen)
      {
        if (!GruppenListe.Contains(Gruppe))
        {
          gruppeHinzufuegen(Gruppe);
        }
      }
    }

    private void fuegeUntergruppenHinzu()
    {
      if (GruppenListe == null)
      {
        GruppenListe = new List<ITestGruppe>();
      }

      foreach (ITestGruppe Gruppe in GetComponentsInChildren<ATestGruppe>())
      {
        if (!GruppenListe.Contains(Gruppe))
        {
          if (!ReferenceEquals(Gruppe, this))
          {
            gruppeHinzufuegen(Gruppe);
          }
        }
      }

    }

    private TestOptionen.LoggingStatus LetzteKonsolenausgabe = TestOptionen.LoggingStatus.Vererbt;

    protected void OnValidate()
    {
      if (Konsolenausgabe != LetzteKonsolenausgabe)
      {
        LetzteKonsolenausgabe = Konsolenausgabe;
        Optionen.setKonsolenLogging(Konsolenausgabe);
      }
    }

  }
}
