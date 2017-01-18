using Assets.Scripts;
using AUTU;
using AUTU.Befehle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controllers;

class TestSchwierigkeitsgrad : CTestumgebungSpiel
{

  [Test]
  public IEnumerator TestSchwierigkeitsgradZunahme()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    {
      yield return starteSpiel(SpielSteuerungsObjekt);

      Pruefer.istGleich(0.0, SpielSteuerungsObjekt.Schwierigkeitsgrad, 0.01, "Der Schwierigkeitsgrad ist nicht 0 beim Spielstart.");
      float LetzterSchwierigkeitsgrad = SpielSteuerungsObjekt.Schwierigkeitsgrad;

      Prozessor.add(new CWarteRelativ(0.5f));
      yield return Prozessor.warteBisFertig();

      Pruefer.istGroesser(LetzterSchwierigkeitsgrad, SpielSteuerungsObjekt.Schwierigkeitsgrad, "Der Schwierigkeitsgrad wird über die Zeit hinweg nicht erhöht.");

      float ZeitFaktor = Time.timeScale;
      Time.timeScale = 10 * ZeitFaktor;

      Prozessor.add(new CWarteRelativ(SpielSteuerungsObjekt.ZeitBisMaximalenSchwierigkeitsgrad));
      yield return Prozessor.warteBisFertig();

      Time.timeScale = ZeitFaktor;

      Pruefer.istGleich(1.0, SpielSteuerungsObjekt.Schwierigkeitsgrad, 0.01, "Der Schwierigkeitsgrad ist nicht 1 nach X Sekunden.");

      yield return stoppeSpiel(SpielSteuerungsObjekt);
    }

    {
      yield return starteSpiel(SpielSteuerungsObjekt);

      Pruefer.istGleich(0.0, SpielSteuerungsObjekt.Schwierigkeitsgrad, 0.01, "Der Schwierigkeitsgrad ist nicht 0 beim Spielstart.");
      float LetzterSchwierigkeitsgrad = SpielSteuerungsObjekt.Schwierigkeitsgrad;

      Prozessor.add(new CWarteRelativ(0.5f));
      yield return Prozessor.warteBisFertig();

      Pruefer.istGroesser(LetzterSchwierigkeitsgrad, SpielSteuerungsObjekt.Schwierigkeitsgrad, "Der Schwierigkeitsgrad wird über die Zeit hinweg nicht erhöht.");

      float ZeitFaktor = Time.timeScale;
      Time.timeScale = 10 * ZeitFaktor;

      Prozessor.add(new CWarteRelativ(SpielSteuerungsObjekt.ZeitBisMaximalenSchwierigkeitsgrad));
      yield return Prozessor.warteBisFertig();

      Time.timeScale = ZeitFaktor;

      Pruefer.istGleich(1.0, SpielSteuerungsObjekt.Schwierigkeitsgrad, 0.01, "Der Schwierigkeitsgrad ist nicht 1 nach X Sekunden.");

      yield return stoppeSpiel(SpielSteuerungsObjekt);
    }

    yield return true;
  }

  [Test]
  public IEnumerator TestAnzahlMutterschiffe()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = false;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    Pruefer.istGleich(0, getMutterschiffe().Count, 0, "Es sind vor Spielbeginn schon Mutterschiffe vorhanden.");

    float LetzteMutterschiffRate = 0.0f;
    float WarteZeit = GegnerSteuerungsObjekt.MaximaleMutterschiffSpawnzeit*3;
    {
      yield return starteSpiel(SpielSteuerungsObjekt);

      yield return zeahleMutterschiffe(WarteZeit, SpielSteuerungsObjekt, 0.0f);
      Pruefer.istGroesser(0.0f, MutterschiffRate, "Innerhalb von 10 Sekunden ist kein Mutterschiff aufgetaucht.");
      LetzteMutterschiffRate = MutterschiffRate;
      Logger.Log("Gemessene Mutterschiff-Rate = "+ LetzteMutterschiffRate);

      yield return stoppeSpiel(SpielSteuerungsObjekt);
    }

    Prozessor.add(new CWarteRelativ(0.5f));
    yield return Prozessor.warteBisFertig();
    Pruefer.istGleich(0, getMutterschiffe().Count, 0, "Nach dem Spielende sind noch Mutterschiffe vorhanden.");

    {
      yield return starteSpiel(SpielSteuerungsObjekt);

      yield return zeahleMutterschiffe(WarteZeit, SpielSteuerungsObjekt, 0.5f);
      Pruefer.istGroesser(LetzteMutterschiffRate, MutterschiffRate, "Innerhalb von 10 Sekunden zu wenig Mutterschiffe aufgetaucht.");
      LetzteMutterschiffRate = MutterschiffRate;
      Logger.Log("Gemessene Mutterschiff-Rate = " + LetzteMutterschiffRate);

      yield return stoppeSpiel(SpielSteuerungsObjekt);
    }

    Prozessor.add(new CWarteRelativ(0.5f));
    yield return Prozessor.warteBisFertig();
    Pruefer.istGleich(0, getMutterschiffe().Count, 0, "Nach dem Spielende sind noch Mutterschiffe vorhanden.");

    {
      yield return starteSpiel(SpielSteuerungsObjekt);

      yield return zeahleMutterschiffe(WarteZeit, SpielSteuerungsObjekt, 1.0f);
      Pruefer.istGroesser(LetzteMutterschiffRate, MutterschiffRate, "Innerhalb von 10 Sekunden zu wenig Mutterschiffe aufgetaucht.");
      LetzteMutterschiffRate = MutterschiffRate;
      Logger.Log("Gemessene Mutterschiff-Rate = " + LetzteMutterschiffRate);

      yield return stoppeSpiel(SpielSteuerungsObjekt);
    }

    Prozessor.add(new CWarteRelativ(0.5f));
    yield return Prozessor.warteBisFertig();
    Pruefer.istGleich(0, getMutterschiffe().Count, 0, "Nach dem Spielende sind noch Mutterschiffe vorhanden.");

    yield return true;
  }

  private float MutterschiffRate = 0.0f;

  private IEnumerator zeahleMutterschiffe(float Zeit, ISpielsteuerung SpielSteuerungsObjekt, float Schwierigkeitsgrad)
  {
    float WarteZeit = 0.5f;

    var LetzteListe = getMutterschiffe();
    int Anzahl = LetzteListe.Count;

    float ZeitFaktor = Time.timeScale;
    Time.timeScale = 10 * Time.timeScale;

    for (int i = 0; i < (int)(Zeit/WarteZeit); i++)
    {
      SpielSteuerungsObjekt.Hitpoints = 100;
      SpielSteuerungsObjekt.Schwierigkeitsgrad = Schwierigkeitsgrad;

      Prozessor.add(new CWarteRelativ(WarteZeit));
      yield return Prozessor.warteBisFertig();

      var Liste = getMutterschiffe();
      foreach (IMutterschiff Schiff in Liste)
      {
        if (!LetzteListe.Contains(Schiff))
        {
          Anzahl++;
        }
      }

      LetzteListe = Liste;
    }

    Time.timeScale = ZeitFaktor;

    MutterschiffRate = Anzahl / Zeit;
  }

  private List<IMutterschiff> getMutterschiffe()
  {
    return sucheKlasse<IMutterschiff>();
  }

  [Test]
  public IEnumerator TestAnzahlGegner()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    Pruefer.istGleich(0, getMutterschiffe().Count, 0, "Es sind vor Spielbeginn schon Mutterschiffe vorhanden.");

    uint LetzteAnzahlGegner = 0;
    uint AnzahlGegner = 0;
    yield return starteSpiel(SpielSteuerungsObjekt);

    SpielSteuerungsObjekt.Schwierigkeitsgrad = 0.0f;

    AnzahlGegner = zaehleGegner(erzeugeGegner(GegnerSteuerungsObjekt));
    Pruefer.istGroesser(LetzteAnzahlGegner, AnzahlGegner, "Gegneranzahl nimmt mit Schwierigkeitsgrad nicht zu.");
    LetzteAnzahlGegner = AnzahlGegner;

    SpielSteuerungsObjekt.Schwierigkeitsgrad = 0.5f;

    AnzahlGegner = zaehleGegner(erzeugeGegner(GegnerSteuerungsObjekt));
    Pruefer.istGroesser(LetzteAnzahlGegner, AnzahlGegner, "Gegneranzahl nimmt mit Schwierigkeitsgrad nicht zu.");
    LetzteAnzahlGegner = AnzahlGegner;

    SpielSteuerungsObjekt.Schwierigkeitsgrad = 1.0f;

    AnzahlGegner = zaehleGegner(erzeugeGegner(GegnerSteuerungsObjekt));
    Pruefer.istGroesser(LetzteAnzahlGegner, AnzahlGegner, "Gegneranzahl nimmt mit Schwierigkeitsgrad nicht zu.");
    LetzteAnzahlGegner = AnzahlGegner;

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }
}

