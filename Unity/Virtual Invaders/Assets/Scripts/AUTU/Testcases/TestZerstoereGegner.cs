using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

public class TestZerstoereGegner : CTestumgebungSpiel
{
  [Test]
  public IEnumerator ErzeugeUndZerstoereGegner()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");
    Debug.Assert(StartKnopf != null, "Es ist kein Start-Button Objekt mit dem Testcase verbunden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;

    yield return starteSpiel(SpielSteuerungsObjekt);

    List<GameObject> GegnerListe = erzeugeGegner(GegnerSteuerungsObjekt);
    uint GegnerAnzahl = zaehleGegner(GegnerListe);

    int Punkte = SpielSteuerungsObjekt.Score;

    foreach (GameObject GegnerGruppe in GegnerListe)
    {
      Pruefer.istGroesser(0, GegnerGruppe.transform.childCount, "Die Gegner Gruppe enthält keine Gegner!");

      foreach(Transform GegnerObjekt in GegnerGruppe.transform)
      {
        if (GegnerObjekt != null)
        {
          BoxCollider GegnerCollider = GegnerObjekt.GetComponent<BoxCollider>();
          if (GegnerCollider != null)
          {
            IGegner Gegner = GegnerObjekt.GetComponent<IGegner>();

            float GegnerHitpoints = 0;
            Pruefer.istTrue(Gegner != null, "Der Gegner "+GegnerObjekt.name+" verwendet kein Script mit dem Testinterface IGegner!");

            if (Gegner != null)
            {
              GegnerHitpoints = Gegner.HitPoints;
            }

            Vector3 Offset = GegnerCollider.center;
            Prozessor.add(new CRichteKopfAufObjektAus(Kamera, GegnerObjekt, Offset));
            Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));
            Prozessor.add(new CWarteRelativ(0.05f));

            yield return Prozessor.warteBisFertig();

            if (GegnerObjekt != null)
            {
              Pruefer.istKleiner(GegnerHitpoints, Gegner.HitPoints, "Die Hitpoints des Gegners " + GegnerObjekt.name + " haben sich nicht verringert!");
            }

            Prozessor.add(new CWarteRelativ(0.05f));

            yield return Prozessor.warteBisFertig();

            if (GegnerObjekt != null)
            {
              Pruefer.istKleiner(GegnerHitpoints, Gegner.HitPoints, "Die Hitpoints des Gegners " + GegnerObjekt.name + " haben sich nicht verringert!");
            }

            Prozessor.add(new CWarteRelativ(0.4f));
          }
        }
      }
    }

    yield return Prozessor.warteBisFertig();

    foreach(GameObject GegnerGruppe in GegnerListe)
    {
      Pruefer.istTrue(GegnerGruppe == null);
    }

    Pruefer.istGleich(Punkte + GegnerAnzahl, SpielSteuerungsObjekt.Score, 0, "Punktzahl wurde nicht erhöht, ob "+GegnerAnzahl+" Gegner zerstört wurden.");

    // das Spiel wieder stoppen
    SpielSteuerungsObjekt.SpielStopp();

    yield return true;
  }
}

