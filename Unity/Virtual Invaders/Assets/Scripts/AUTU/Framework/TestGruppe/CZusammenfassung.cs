using UnityEngine;

namespace AUTU
{
  public class CZusammenfassung : IZusammenfassung, IZusammenfassungsAenderer
  {
    public uint Tests
    {
      get
      {
        Debug.Assert(TestGruppe != null, "Es ist kein valides Test-Gruppen Objekt definiert.");
        return TestGruppe.AnzahlTests;
      }
    }

    public uint GelaufeneTests
    {
      get;
      private set;
    }

    public uint IgnorierteTests
    {
      get;
      private set;
    }

    public uint KorrekteTests
    {
      get;
      private set;
    }

    public uint FehlerhafteTests
    {
      get;
      private set;
    }

    public uint Pruefungen
    {
      get;
      private set;
    }

    public uint Fehler
    {
      get;
      private set;
    }

    public uint FataleFehler
    {
      get;
      private set;
    }

    private ITestGruppe TestGruppe = null;

    public CZusammenfassung(ITestGruppe Gruppe)
    {
      TestGruppe = Gruppe;
      reset();

      Debug.Assert(TestGruppe != null, "Es ist kein valides Test-Gruppen Objekt definiert.");
    }

    public void addiereStatus(ITestStatus Status)
    {
      if (!Status.IgnoriereTest)
      {
        GelaufeneTests += 1;
        if ((Status.Fehler == 0) && (Status.FataleFehler == 0))
        {
          KorrekteTests += 1;
        }
        else
        {
          FehlerhafteTests += 1;
        }

        Pruefungen += Status.Pruefungen;
        Fehler += Status.Fehler;
        FataleFehler += Status.FataleFehler;
      }
      else
      {
        IgnorierteTests++;
      }
    }

    public void reset()
    {
      GelaufeneTests = 0;
      KorrekteTests = 0;
      FehlerhafteTests = 0;
      Pruefungen = 0;
      Fehler = 0;
      FataleFehler = 0;
      IgnorierteTests = 0;
    }

    public override string ToString()
    {
      string String = "";
      String += "-> Vorhandene Tests: " + Tests + " (";
      String += "ausgeführt: " + GelaufeneTests + ", ";
      String += "fehlerfrei: " + KorrekteTests + ", ";
      String += "fehlerhaft: " + FehlerhafteTests + ", ";
      String += "ignoriert: " + IgnorierteTests + ")\n";
      String += "-> Prüfungen: " + Pruefungen + " (";
      String += "Fehler: " + Fehler + ", ";
      String += "fatale Fehler: " + FataleFehler + ")\n";
      return String;
    }

  }
}