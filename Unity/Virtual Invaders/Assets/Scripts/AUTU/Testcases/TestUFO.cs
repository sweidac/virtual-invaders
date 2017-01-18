using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

public class TestUFO : CTestumgebungSpiel
{

  [Test]
  public IEnumerator TesteObUFOErscheint()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = true;

    yield return starteSpiel(SpielSteuerungsObjekt);

    UFO = null;
    float WarteZeit = SpielSteuerungsObjekt.MaximaleWartezeitAufUFO;
    yield return warteAufUFO(WarteZeit);

    Pruefer.istTrue(UFO != null, "Es konnte nach "+WarteZeit+"s kein UFO gefunden werden.");

    GameObject UFOObjekt = UFO.Objekt;

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    Prozessor.add(new CWarteRelativ(0.5f));
    yield return Prozessor.warteBisFertig();

    Pruefer.istTrue(UFOObjekt == null, "Das UFO ist auch noch nach Spielende vorhanden.");
  }

  [Test]
  public IEnumerator SchiesseUFOAb()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = true;

    yield return starteSpiel(SpielSteuerungsObjekt);

    int Score = SpielSteuerungsObjekt.Score;

    UFO = null;
    float WarteZeit = SpielSteuerungsObjekt.MaximaleWartezeitAufUFO;
    yield return warteAufUFO(WarteZeit);

    Pruefer.istTrue(UFO != null, "Es konnte nach " + WarteZeit + "s kein UFO gefunden werden.");

    GameObject UFOObjekt = UFO.Objekt;
    CapsuleCollider UFOCollider = UFOObjekt.GetComponent<CapsuleCollider>();

    Pruefer.istTrue(UFOCollider != null, "Das UFO Objekt hat keinen BoxCollider!");

    int i = 0;
    while (UFOObjekt != null && i < 20)
    {
      i++;
      Vector3 Offset = UFOCollider.bounds.center;
      Prozessor.add(new CRichteKopfAufObjektAus(Kamera, UFOObjekt.transform, Offset));
      Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));
      Prozessor.add(new CWarteRelativ(0.2f));

      yield return Prozessor.warteBisFertig();
    }

    Pruefer.istTrue(UFOObjekt == null, "Das UFO konnte nicht abgeschossen werden.");
    Pruefer.istGroesser(Score, SpielSteuerungsObjekt.Score, "Das UFO hat keine zusätzlichen Punkte eingebracht.");
    Pruefer.istGroesser(Score + 1, SpielSteuerungsObjekt.Score, "Das UFO hat nur einen Punkt gebracht.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);
  }

  IUFO UFO = null;

  private IEnumerator warteAufUFO(float WarteZeit)
  {
    float WarteSchritt = 0.5f;

    float ZeitFaktor = Time.timeScale;
    Time.timeScale = 10;

    for(float Zeit = 0f; Zeit < WarteZeit; Zeit += WarteSchritt)
    {
      var UFOListe = sucheKlasse<IUFO>();

      if (UFOListe.Count > 0)
      {
        UFO = UFOListe[0];
        break;
      }

      Prozessor.add(new CWarteRelativ(WarteSchritt));
      yield return Prozessor.warteBisFertig();
    }

    Time.timeScale = ZeitFaktor;

    yield return true;
  }
}