using UnityEngine;
using NUnit.Framework;
using AUTU;

namespace AUTU
{
  [Category("UnityOnly")]
  public class TestTestGruppe
  {
    private T createGruppe<T>(string Name = "TestGruppe") where T: CTestGruppe
    {
      GameObject Object = new GameObject();
      Object.name = Name;
      return Object.AddComponent<T>();
    }

    [NUnit.Framework.Test]
    public void ErstelleObjekt()
    {
#pragma warning disable 0219
      ITestGruppe TestGruppe = createGruppe<CTestGruppe>();
#pragma warning restore 0219
    }

    [NUnit.Framework.Test]
    public void GruppenNameLesen()
    {
      ITestGruppe TestGruppe = createGruppe<CTestGruppe>();

      Assert.AreEqual("TestGruppe", TestGruppe.Name);
    }

    [NUnit.Framework.Test]
    public void InitialeAnzahlGruppenIst0()
    {
      ITestGruppe TestGruppe = createGruppe<CTestGruppe>();

      Assert.AreEqual(0, TestGruppe.AnzahlGruppen);
    }

    [NUnit.Framework.Test]
    public void GruppenHinzufuegenUeberArray()
    {
      ITestGruppe TestGruppe = createGruppe<CTestGruppe>();

      TestGruppe.gruppeHinzufuegen(createGruppe<CTestGruppe>());

      Assert.AreEqual(1, TestGruppe.AnzahlGruppen);

      TestGruppe.gruppeHinzufuegen(createGruppe<CTestGruppe>());

      Assert.AreEqual(2, TestGruppe.AnzahlGruppen);
    }

    [NUnit.Framework.Test]
    public void AnzahlAnGruppenRekursiv()
    {
      ITestGruppe TestGruppe = createGruppe<CTestGruppe>();
      ITestGruppe GruppeA = createGruppe<CTestGruppe>();
      ITestGruppe GruppeB = createGruppe<CTestGruppe>();

      TestGruppe.gruppeHinzufuegen(GruppeA);
      TestGruppe.gruppeHinzufuegen(GruppeB);

      Assert.AreEqual(2, TestGruppe.AnzahlGruppen);

      GruppeA.gruppeHinzufuegen(createGruppe<CTestGruppe>());
      GruppeA.gruppeHinzufuegen(createGruppe<CTestGruppe>());
      GruppeA.gruppeHinzufuegen(createGruppe<CTestGruppe>());
      GruppeB.gruppeHinzufuegen(createGruppe<CTestGruppe>());

      Assert.AreEqual(6, TestGruppe.AnzahlGruppen);
    }

    [NUnit.Framework.Test]
    public void AnzahlTestsIst0InLeererGruppe()
    {
      ITestGruppe TestGruppe = createGruppe<CTestGruppe>();

      Assert.AreEqual(0, TestGruppe.AnzahlTests);
    }

    [NUnit.Framework.Test]
    public void AnzahlTestsInVollerGruppe()
    {
      GameObject ObjectA = new GameObject();
      ITestGruppe GruppeA = ObjectA.AddComponent<CDummyTestcaseA>();

      Assert.AreEqual(1, GruppeA.AnzahlTests);

      GameObject ObjectB = new GameObject();
      ITestGruppe GruppeB = ObjectB.AddComponent<CDummyTestcaseB>();

      Assert.AreEqual(3, GruppeB.AnzahlTests);
    }

    [NUnit.Framework.Test]
    public void AnzahlTestsRekrusiv()
    {
      GameObject ObjectA = new GameObject();
      ITestGruppe GruppeA = ObjectA.AddComponent<CDummyTestcaseA>();

      GameObject ObjectB = new GameObject();
      ITestGruppe GruppeB = ObjectB.AddComponent<CDummyTestcaseB>();

      ITestGruppe TestGruppe = createGruppe<CTestGruppe>();
      TestGruppe.gruppeHinzufuegen(GruppeA);
      TestGruppe.gruppeHinzufuegen(GruppeB);

      Assert.AreEqual(4, TestGruppe.AnzahlTests);
    }

    [NUnit.Framework.Test]
    public void LeseTestListe()
    {
      ITestGruppe GruppeB = createGruppe<CDummyTestcaseB>("CDummyTestcaseB");

      Assert.AreEqual(3, GruppeB.Tests.Count);

      Assert.AreEqual("CDummyTestcaseB.TestB1", GruppeB.Tests[0].VollerName);
      Assert.AreEqual("CDummyTestcaseB.TestB2", GruppeB.Tests[1].VollerName);
      Assert.AreEqual("CDummyTestcaseB.TestB3", GruppeB.Tests[2].VollerName);
    }

    [NUnit.Framework.Test]
    public void LeseTestListeRekursiv()
    {
      ITestGruppe GruppeA = createGruppe<CDummyTestcaseA>("CDummyTestcaseA");
      ITestGruppe GruppeB = createGruppe<CDummyTestcaseB>("CDummyTestcaseB");

      GruppeA.gruppeHinzufuegen(GruppeB);

      Assert.AreEqual(4, GruppeA.Tests.Count);

      Assert.AreEqual("CDummyTestcaseA.TestA1", GruppeA.Tests[0].VollerName);
      Assert.AreEqual("CDummyTestcaseA.CDummyTestcaseB.TestB1", GruppeA.Tests[1].VollerName);
      Assert.AreEqual("CDummyTestcaseA.CDummyTestcaseB.TestB2", GruppeA.Tests[2].VollerName);
      Assert.AreEqual("CDummyTestcaseA.CDummyTestcaseB.TestB3", GruppeA.Tests[3].VollerName);
    }

    [NUnit.Framework.Test]
    public void VerhindereSpaetesHinzufuegenVonGruppen()
    {
      ITestGruppe GruppeA = createGruppe<CDummyTestcaseA>();

      Assert.AreEqual(1, GruppeA.Tests.Count);

      ITestGruppe GruppeB = createGruppe<CDummyTestcaseB>();

      bool WarEinFehler = false;
      try
      {
        GruppeA.gruppeHinzufuegen(GruppeB);
      }
      catch (FehlerSpaetesHinzufuegenVonGruppe)
      {
        WarEinFehler = true;
      }

      Assert.AreEqual(true, WarEinFehler);
    }

    [NUnit.Framework.Test]
    public void FuegeTestGruppenAusListeBeimStartHinzu()
    {
      CDummyTestcaseD GruppeD = createGruppe<CDummyTestcaseD>();

      ATestGruppe GruppeA = createGruppe<CDummyTestcaseA>();
      ATestGruppe GruppeB = createGruppe<CDummyTestcaseB>();

      GruppeD.TestGruppen.Add(GruppeA);
      GruppeD.TestGruppen.Add(GruppeB);

      Assert.AreEqual(0, GruppeD.AnzahlGruppen);

      GruppeD.rufeAufStart();

      Assert.AreEqual(2, GruppeD.AnzahlGruppen);
      Assert.AreSame(GruppeA, GruppeD.Gruppen[0]);
      Assert.AreSame(GruppeB, GruppeD.Gruppen[1]);
    }

    [NUnit.Framework.Test]
    public void FuegeTestUntergruppenBeimStartHinzu()
    {
      CDummyTestcaseD GruppeD = createGruppe<CDummyTestcaseD>();

      ATestGruppe GruppeA = createGruppe<CDummyTestcaseA>();
      GruppeA.transform.parent = GruppeD.transform;

      ATestGruppe GruppeB = createGruppe<CDummyTestcaseB>();
      GruppeB.transform.parent = GruppeD.transform;

      Assert.AreEqual(0, GruppeD.AnzahlGruppen);

      GruppeD.rufeAufStart();

      Assert.AreEqual(2, GruppeD.AnzahlGruppen);
      Assert.AreSame(GruppeA, GruppeD.Gruppen[0]);
      Assert.AreSame(GruppeB, GruppeD.Gruppen[1]);
    }

    [NUnit.Framework.Test]
    public void SchreibeKonsolenausgabeOptionenVomInspektor()
    {
      CDummyTestcaseD GruppeD = createGruppe<CDummyTestcaseD>();

      Assert.AreEqual(true, GruppeD.Optionen.KonsolenLogging);

      GruppeD.Konsolenausgabe = TestOptionen.LoggingStatus.Aus;
      GruppeD.rufeAufOnValidate();

      Assert.AreEqual(false, GruppeD.Optionen.KonsolenLogging);

      GruppeD.Konsolenausgabe = TestOptionen.LoggingStatus.Ein;
      GruppeD.rufeAufOnValidate();

      Assert.AreEqual(true, GruppeD.Optionen.KonsolenLogging);
    }


  }
}
