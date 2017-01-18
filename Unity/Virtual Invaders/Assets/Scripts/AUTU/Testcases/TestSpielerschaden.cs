using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

public class TestSpielerschaden : CTestumgebungSpiel
{
  private IEnumerator warteBisSchadensAnzeigeUnsichtbarIst(SpriteRenderer RoteSchadenAnzeige)
  {
    // warte bis Schadensanzeige wieder verschwunden ist
    for (int i = 0; i < 50; i++)
    {
      Prozessor.add(new CWarteRelativ(0.1f));
      yield return Prozessor.warteBisFertig();

      if (RoteSchadenAnzeige.color.a < 0.001f)
      {
        break;
      }
    }

    //Debug.Log("Alpha: "+ RoteSchadenAnzeige.color.a);

    Pruefer.istGleich(0.0f, RoteSchadenAnzeige.color.a, 0.05f, "Die (rote) Schadensanzeige ist direkt nach dem Spielstart schon sichtbar.");
  }

  public SpriteRenderer RoteSchadenAnzeige = null;

  [Test]
  public IEnumerator SpielerNimmtSchadenDurchGegner()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    Debug.Assert(RoteSchadenAnzeige != null, "Es konnte keine (rote) Schadensanzeige beim Kamera-Objekt gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    yield return starteSpiel(SpielSteuerungsObjekt);

    GegnerEntfernung = 10;
    GegnerHoehe = 0;
    int Hitpoints = SpielSteuerungsObjekt.Hitpoints;

    yield return warteBisSchadensAnzeigeUnsichtbarIst(RoteSchadenAnzeige);

    // erzeuge Gegner der nahe ist
    List<GameObject> GegnerList = erzeugeGegner(GegnerSteuerungsObjekt);

    Pruefer.istGroesser(0, GegnerList.Count, "Es konnten keine Gegner zum Testen erzeugt werden.");

    // warte max. 5 Sekunden (50x 0.1s)
    for (int i = 0; i < 50; i++)
    {
      Prozessor.add(new CWarteRelativ(0.1f));
      yield return Prozessor.warteBisFertig();

      // breche ab, wenn Spieler Schaden genommen hat
      if (SpielSteuerungsObjekt.Hitpoints < Hitpoints)
      {
        break;
      }

      // oder wenn es keine Gegner mehr gibt
      foreach(GameObject Objekt in GegnerList)
      {
        if (Objekt == null)
        {
          break;
        }
      }
    }

    Prozessor.add(new CWarteRelativ(0.2f));
    yield return Prozessor.warteBisFertig();

    Pruefer.istKleiner(Hitpoints, SpielSteuerungsObjekt.Hitpoints);
    float AktuellesAlpha = RoteSchadenAnzeige.color.a;
    Pruefer.istGroesser(0.05f, AktuellesAlpha, "Die (rote) Schadensanzeige ist nicht eingeblendet.");

    Prozessor.add(new CWarteRelativ(0.1f));
    yield return Prozessor.warteBisFertig();

    Pruefer.istKleiner(AktuellesAlpha, RoteSchadenAnzeige.color.a, "Die (rote) Schadensanzeige wird nicht langsam ausgeblendet.");

    if (GegnerList.Count > 0)
    {
      foreach (GameObject Objekt in GegnerList)
      {
        Pruefer.istTrue(Objekt == null, "Die Gegnergruppen existieren noch, sollten aber zerstört sein.");
      }
    }

    yield return warteBisSchadensAnzeigeUnsichtbarIst(RoteSchadenAnzeige);

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

  [Test]
  public IEnumerator SpielerNimmtSchadenDurchProjektil()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    Debug.Assert(RoteSchadenAnzeige != null, "Es konnte keine (rote) Schadensanzeige beim Kamera-Objekt gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    yield return starteSpiel(SpielSteuerungsObjekt);

    GegnerEntfernung = 30;
    GegnerHoehe = 10;
    int Hitpoints = SpielSteuerungsObjekt.Hitpoints;

    yield return warteBisSchadensAnzeigeUnsichtbarIst(RoteSchadenAnzeige);

    // erzeuge Gegner der nahe ist
    List<GameObject> GegnerListe = erzeugeGegner(GegnerSteuerungsObjekt);

    Pruefer.istGroesser(0, GegnerListe.Count, "Es konnten keine Gegner zum Testen erzeugt werden.");

    // den Gegner ein paar Frames Zeit lassen zum Ausrichten
    Prozessor.add(new CWarteRelativ(0.2f));
    yield return Prozessor.warteBisFertig();

    List<GameObject> ProjektilListe = new List<GameObject>();
    foreach(GameObject GegnerGruppe in GegnerListe)
    {
      Pruefer.istGroesser(0, GegnerGruppe.transform.childCount, "Die Gegner Gruppe enthält keine Gegner!");

      foreach (Transform GegnerObjekt in GegnerGruppe.transform)
      {
        if (GegnerObjekt != null)
        {
          IGegner Gegner = GegnerObjekt.GetComponent<IGegner>();
          if (Gegner != null)
          {
            GegnerObjekt.rotation.SetLookRotation(Kamera.position);

            GameObject Projektil = Gegner.ErstelleProjektil();

            Pruefer.istTrue(Projektil != null, "Es konnte kein Projektil erstellt werden.");

            Vector3 RichtungVonKamera = (Projektil.transform.position - Kamera.position).normalized;
            Projektil.transform.position = Kamera.position + RichtungVonKamera * 5;

            ProjektilListe.Add(Projektil);
          }
        }
      }
    }

    Pruefer.istGroesser(0, ProjektilListe.Count, "Es konnten keine Projektile zum Testen erzeugt werden.");

    // den Gegner ein paar Frames Zeit zum Abfeuern der Rakete
    Prozessor.add(new CWarteRelativ(1f));
    yield return Prozessor.warteBisFertig();

    // zerstöre alle Gegner, damit nur die Projektile übrig bleiben
    foreach(GameObject GegnerGruppe in GegnerListe)
    {
      Destroy(GegnerGruppe);
    }

    // warte max. 5 Sekunden (50x 0.1s)
    for (int i = 0; i < 50; i++)
    {
      Prozessor.add(new CWarteRelativ(0.1f));
      yield return Prozessor.warteBisFertig();

      // Breche ab, wenn es keine Projektile mehr gibt
      bool KeineProjektile = true;
      foreach (GameObject Objekt in ProjektilListe)
      {
        if (Objekt != null)
        {
          KeineProjektile = false;
        }
      }

      if (KeineProjektile)
      {
        break;
      }
    }

    Pruefer.istKleiner(Hitpoints, SpielSteuerungsObjekt.Hitpoints);
    float AktuellesAlpha = RoteSchadenAnzeige.color.a;
    Pruefer.istGroesser(0.05f, AktuellesAlpha, "Die (rote) Schadensanzeige ist nicht eingeblendet.");

    Prozessor.add(new CWarteRelativ(0.1f));
    yield return Prozessor.warteBisFertig();

    Pruefer.istKleiner(AktuellesAlpha, RoteSchadenAnzeige.color.a, "Die (rote) Schadensanzeige wird nicht langsam ausgeblendet.");

    if (ProjektilListe.Count > 0)
    {
      foreach (GameObject Objekt in ProjektilListe)
      {
        Pruefer.istTrue(Objekt == null, "Die Projektile existieren noch.");
      }
    }

    yield return warteBisSchadensAnzeigeUnsichtbarIst(RoteSchadenAnzeige);

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

}