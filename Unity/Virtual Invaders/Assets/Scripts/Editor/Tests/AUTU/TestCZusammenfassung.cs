using NUnit.Framework;
using UnityEngine;

namespace AUTU
{
  [Category("UnityOnly")]
  class TestCZusammenfassung
  {
    private ITestGruppe createGruppe<T>() where T : CTestGruppe
    {
      GameObject Object = new GameObject();
      return Object.AddComponent<T>();
    }

    [NUnit.Framework.Test]
    public void ErstelleObjekt()
    {
      IZusammenfassung Zusammenfassung = new CZusammenfassung(createGruppe<CTestGruppe>());

      Assert.AreEqual(0, Zusammenfassung.Tests);
      Assert.AreEqual(0, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
    }

    [NUnit.Framework.Test]
    public void AnzahlVonTestsAusGruppe()
    {
      ITestGruppe GruppeA = createGruppe<CDummyTestcaseA>();
      ITestGruppe GruppeB = createGruppe<CDummyTestcaseB>();
      GruppeA.gruppeHinzufuegen(GruppeB);

      IZusammenfassung Zusammenfassung = new CZusammenfassung(GruppeA);

      Assert.AreEqual(4, Zusammenfassung.Tests);
      Assert.AreEqual(0, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(0, Zusammenfassung.Pruefungen);
      Assert.AreEqual(0, Zusammenfassung.Fehler);
      Assert.AreEqual(0, Zusammenfassung.FataleFehler);
    }

    [NUnit.Framework.Test]
    public void AddiereTestStatus()
    {
      ITestGruppe GruppeB = createGruppe<CDummyTestcaseB>();
      CZusammenfassung Zusammenfassung = new CZusammenfassung(GruppeB);

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(0, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(0, Zusammenfassung.Pruefungen);
      Assert.AreEqual(0, Zusammenfassung.Fehler);
      Assert.AreEqual(0, Zusammenfassung.FataleFehler);

      /// Test lief erfolgereich
      CTestStatus StatusTest1 = new CTestStatus();
      StatusTest1.addierePruefung(2);

      Zusammenfassung.addiereStatus(StatusTest1);

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(1, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(2, Zusammenfassung.Pruefungen);
      Assert.AreEqual(0, Zusammenfassung.Fehler);
      Assert.AreEqual(0, Zusammenfassung.FataleFehler);

      /// Test war fehlerhaft
      CTestStatus StatusTest2 = new CTestStatus();
      StatusTest2.addierePruefung(3);
      StatusTest2.addiereFehler(1);

      Zusammenfassung.addiereStatus(StatusTest2);

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(2, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(1, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(5, Zusammenfassung.Pruefungen);
      Assert.AreEqual(1, Zusammenfassung.Fehler);
      Assert.AreEqual(0, Zusammenfassung.FataleFehler);

      /// Test hatte fatalen Fehler
      CTestStatus StatusTest3 = new CTestStatus();
      StatusTest3.addierePruefung(1);
      StatusTest3.addiereFataleFehler(1);

      Zusammenfassung.addiereStatus(StatusTest3);

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(3, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(2, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(6, Zusammenfassung.Pruefungen);
      Assert.AreEqual(1, Zusammenfassung.Fehler);
      Assert.AreEqual(1, Zusammenfassung.FataleFehler);

      /// Test wird ignoriert
      CTestStatus StatusTest4 = new CTestStatus();
      StatusTest4.ignoriereTest(true);

      Zusammenfassung.addiereStatus(StatusTest4);

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(3, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(1, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(2, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(1, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(6, Zusammenfassung.Pruefungen);
      Assert.AreEqual(1, Zusammenfassung.Fehler);
      Assert.AreEqual(1, Zusammenfassung.FataleFehler);

    }

    [NUnit.Framework.Test]
    public void ResetteZusammenfassung()
    {
      ITestGruppe GruppeB = createGruppe<CDummyTestcaseB>();
      CZusammenfassung Zusammenfassung = new CZusammenfassung(GruppeB);

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(0, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(0, Zusammenfassung.Pruefungen);
      Assert.AreEqual(0, Zusammenfassung.Fehler);
      Assert.AreEqual(0, Zusammenfassung.FataleFehler);

      CTestStatus StatusTest1 = new CTestStatus();
      StatusTest1.addierePruefung(5);
      StatusTest1.addiereFehler(5);
      StatusTest1.addiereFataleFehler(1);

      Zusammenfassung.addiereStatus(StatusTest1);

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(1, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(1, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(5, Zusammenfassung.Pruefungen);
      Assert.AreEqual(5, Zusammenfassung.Fehler);
      Assert.AreEqual(1, Zusammenfassung.FataleFehler);

      Zusammenfassung.reset();

      Assert.AreEqual(3, Zusammenfassung.Tests);
      Assert.AreEqual(0, Zusammenfassung.GelaufeneTests);
      Assert.AreEqual(0, Zusammenfassung.KorrekteTests);
      Assert.AreEqual(0, Zusammenfassung.FehlerhafteTests);
      Assert.AreEqual(0, Zusammenfassung.IgnorierteTests);
      Assert.AreEqual(0, Zusammenfassung.Pruefungen);
      Assert.AreEqual(0, Zusammenfassung.Fehler);
      Assert.AreEqual(0, Zusammenfassung.FataleFehler);
    }


  }
}
