using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace AUTU
{
  public class CPruefer : IPruefer
  {
    private ITestStatusAenderer StatusAenderer = null;
    private ILogger Logger = null;
    private bool AbbruchBeiFehler = true;

    public CPruefer(ITestStatusAenderer StatusAenderer, ILogger Logger, bool AbbruchBeiFehler)
    {
      this.StatusAenderer = StatusAenderer;
      this.Logger = Logger;
      this.AbbruchBeiFehler = AbbruchBeiFehler;

      UnityEngine.Debug.Assert(this.StatusAenderer != null, "Kein gültiges StatusAenderer Objekt vorhanden!");
      UnityEngine.Debug.Assert(this.Logger != null, "Kein gültiges StatusAenderer Objekt vorhanden!");
    }

    private void Abbruch()
    {
      if (AbbruchBeiFehler)
      {
        throw new FehlerAbbruchWegenFehler();
      }
    }

    public void istTrue(bool Kondition, string Text = null)
    {
      StatusAenderer.addierePruefung();

      if (!Kondition)
      {
        if (Text != null)
        {
          Logger.LogError(Text);
        }
        Logger.FalscherWert("Eine Bedingung hat einen falschen Wert.", true, Kondition, new StackTrace(1, true));
        StatusAenderer.addiereFehler();
        Abbruch();
      }
    }

    public void istFalse(bool Kondition, string Text = null)
    {
      StatusAenderer.addierePruefung();

      if (Kondition)
      {
        if (Text != null)
        {
          Logger.LogError(Text);
        }
        Logger.FalscherWert("Eine Bedingung hat einen falschen Wert.", false, Kondition, new StackTrace(1, true));
        StatusAenderer.addiereFehler();
        Abbruch();
      }
    }

    public void istGleicheReferenz(object Objekt1, object Objekt2, string Text = null)
    {
      StatusAenderer.addierePruefung();

      if (!ReferenceEquals(Objekt1, Objekt2))
      {
        if (Text != null)
        {
          Logger.LogError(Text);
        }
        Logger.FalscherWert("Eine gleiche Referenz wurde erwartet, aber sie waren unterschiedlich.", Objekt1, Objekt2, new StackTrace(1, true));
        StatusAenderer.addiereFehler();
        Abbruch();
      }

    }

    public void istGroesser<T>(T Vergleichswert, T Pruefwert, string Text = null) where T : IComparable
    {
      StatusAenderer.addierePruefung();

      bool IstGroesser = Vergleichswert.CompareTo(Pruefwert) < 0;
      if (!IstGroesser)
      {
        if (Text != null)
        {
          Logger.LogError(Text);
        }
        StatusAenderer.addiereFehler();
        Logger.FalscherVergleich("Der tatsächliche Wert ist nicht größer als der Vergleichswert.", Vergleichswert, Pruefwert, new StackTrace(1, true));
        Abbruch();
      }
    }

    public void istGleich(double Vergleichswert, double Pruefwert, double Toleranz = 0.0f, string Text = null)
    {
      StatusAenderer.addierePruefung();

      if (Toleranz == 0.0)
      {
        if (Vergleichswert != Pruefwert)
        {
          if (Text != null)
          {
            Logger.LogError(Text);
          }
          StatusAenderer.addiereFehler();
          Logger.FalscherWert("Der tatsächliche Wert entspricht nicht dem Erwarteten Wert.", Vergleichswert, Pruefwert, new StackTrace(1, true));
          Abbruch();
        }
      }
      else
      {
        double UntereGrenze = Vergleichswert - Toleranz;
        double ObereGrenze = Vergleichswert + Toleranz;
        if (!(Pruefwert >= UntereGrenze && Pruefwert <= ObereGrenze))
        {
          if (Text != null)
          {
            Logger.LogError(Text);
          }
          StatusAenderer.addiereFehler();
          Logger.WertAusserhalbToleranz("Der tatsächliche Wert liegt nicht innerhalb der Toleranz.", UntereGrenze, ObereGrenze, Pruefwert, new StackTrace(1, true));
          Abbruch();
        }
      }
    }

    public void istKleiner<T>(T Vergleichswert, T Pruefwert, string Text = null) where T : IComparable
    {
      StatusAenderer.addierePruefung();

      bool IstGroesser = Vergleichswert.CompareTo(Pruefwert) > 0;
      if (!IstGroesser)
      {
        if (Text != null)
        {
          Logger.LogError(Text);
        }
        StatusAenderer.addiereFehler();
        Logger.FalscherVergleich("Der tatsächliche Wert ist nicht kleiner als der Vergleichswert.", Vergleichswert, Pruefwert, new StackTrace(1, true));
        Abbruch();
      }
    }



  }
}
