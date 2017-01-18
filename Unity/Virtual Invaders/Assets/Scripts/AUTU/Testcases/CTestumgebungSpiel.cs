using Assets.Scripts;
using AUTU;
using AUTU.Befehle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTestumgebungSpiel : CTestGruppe
{
  public Transform Kamera = null;
  public Transform StartKnopf = null;
  public float GegnerEntfernung = 30f;
  public float GegnerHoehe = 6f;

  protected IEnumerator starteSpiel(ISpielsteuerung SpielSteuerungsObjekt)
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");
    Debug.Assert(StartKnopf != null, "Es ist kein Start-Button Objekt mit dem Testcase verbunden.");

    SpielSteuerungsObjekt.GameOverWarteZeit = 0;

    // das Spiel starten, falls es noch nicht läuft
    if (SpielSteuerungsObjekt.Gameover)
    {
      Prozessor.add(new CRichteKopfAufObjektAus(Kamera, StartKnopf));
      Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));
      Prozessor.add(new CDrueckeKnopf());
      Prozessor.add(new CWarteRelativ(0.5f));
      yield return Prozessor.warteBisFertig();
    }

    Pruefer.istFalse(SpielSteuerungsObjekt.Gameover, "Das Spiel wurde nicht gestartet.");
  }

  protected IEnumerator stoppeSpiel(ISpielsteuerung SpielSteuerungsObjekt)
  {
    if (!SpielSteuerungsObjekt.Gameover)
    {
      SpielSteuerungsObjekt.SpielStopp();
    }

    yield return true;
  }

  protected List<GameObject> erzeugeGegner(IGegnerSteuerung GegnerSteuerungsObjekt)
  {
    List<GameObject> GegnerListe = new List<GameObject>();
    int GegnerAnzahl = GegnerSteuerungsObjekt.GegnerPreFabs.Count;
    int i = 0;

    Pruefer.istGroesser(0, GegnerAnzahl, "Es sind keine Gegner-Typen definiert worden.");

    foreach (GameObject GegnerTyp in GegnerSteuerungsObjekt.GegnerPreFabs)
    {
      float Winkel = (2 * Mathf.PI * i) / GegnerAnzahl;
      Vector3 GegnerPosition = new Vector3(Mathf.Sin(Winkel) * GegnerEntfernung, GegnerHoehe, Mathf.Cos(Winkel) * GegnerEntfernung);
      GameObject GegnerGruppe = GegnerSteuerungsObjekt.SpawneGegner(GegnerTyp, Kamera.position + GegnerPosition);

      Pruefer.istTrue(GegnerGruppe != null, "Gegner-Gruppe wurde nicht erstellt.");

      GegnerListe.Add(GegnerGruppe);
      i++;
    }

    Pruefer.istGroesser(0, GegnerListe.Count, "Es wurden keine Gegner erstellt.");

    return GegnerListe;
  }

  protected uint zaehleGegner(List<GameObject> GegnerGruppenListe)
  {
    uint Anzahl = 0;

    foreach (GameObject GegnerGruppe in GegnerGruppenListe)
    {
      foreach (Transform Gegner in GegnerGruppe.transform)
      {
        if (Gegner != null)
        {
          BoxCollider GegnerCollider = Gegner.GetComponent<BoxCollider>();
          if (GegnerCollider != null)
          {
            Anzahl++;
          }
        }
      }
    }

    return Anzahl;
  }

  protected List<T> sucheKlasse<T>(string Name = null) where T : class
  {
    List<T> Liste = new List<T>();

    Object[] ObjektListe = FindObjectsOfType(typeof(GameObject));
    foreach (Object Objekt in ObjektListe)
    {
      GameObject GameObjekt = Objekt as GameObject;

      if (Name == null || Name == GameObjekt.name)
      {
        if (typeof(T).Name == typeof(GameObject).Name)
        {
          Liste.Add(GameObjekt as T);
        }
        else
        {
          foreach (T Script in GameObjekt.GetComponents<T>())
          {
            Liste.Add(Script);
          }
        }
      }

    }

    return Liste;
  }

}
