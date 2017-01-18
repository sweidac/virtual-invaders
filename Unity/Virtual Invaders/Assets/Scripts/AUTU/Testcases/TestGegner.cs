using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

class TestGegner : CTestumgebungSpiel
{

  [Test]
  public IEnumerator VerschiedeneGegnertypen()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    yield return starteSpiel(SpielSteuerungsObjekt);

    GegnerEntfernung = 30;
    GegnerHoehe = 0;

    List<GameObject> GegnerList = erzeugeGegner(GegnerSteuerungsObjekt);

    foreach (GameObject GruppenObjekt in GegnerList)
    {
      IGegnerGruppe Gruppe = GruppenObjekt.GetComponent<IGegnerGruppe>();

      Pruefer.istTrue(Gruppe != null, "Die Gruppe kann nicht mit dem Interface IGegnerGruppe angesprochen werden.");

      Pruefer.istGroesser(1, Gruppe.TypenVonGegnern.Length, "Es gibt nicht mehrere Arten von Gegnern.");

      List<GameObject> GegnerListe = new List<GameObject>();
      foreach (GameObject GegnerObjekt in Gruppe.TypenVonGegnern)
      {
        Pruefer.istTrue(GegnerObjekt != null, "Es wurde keine valide Pre-Fab Instanz für den Gegner angegeben.");
        Pruefer.istFalse(GegnerListe.Contains(GegnerObjekt), "Gegner "+GegnerObjekt.name+" ist doppelt in der Pre-Fab Liste enthalten.");
        GegnerListe.Add(GegnerObjekt);
      }
    }

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

  [Test]
  public IEnumerator MutterschiffeSendenGruppenVonGegnernAus()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = false;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    yield return starteSpiel(SpielSteuerungsObjekt);

    SpielSteuerungsObjekt.Schwierigkeitsgrad = 0.5f;

    GegnerGruppe = null;
    yield return warteAufGegnerGruppe();

    Pruefer.istTrue(GegnerGruppe != null, "Das Gegnergruppen-Objekt kann nicht als Gegnergruppe angesprochen werden.");

    int GegnerAnzahl = 0;

    foreach (Transform GegnerObjekt in GegnerGruppe.Objekt.transform)
    {
      IGegner Gegner = GegnerObjekt.GetComponent<IGegner>();

      Pruefer.istTrue(Gegner != null, "Das Gegner Objekt "+GegnerObjekt.name+" konnte nicht ais IGegner angesprochen werden.");
      GegnerAnzahl++;
    }

    Pruefer.istGroesser(1, GegnerAnzahl, "Es wurde keine Gruppe von Gegnern ausgesendet.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

  IGegnerGruppe GegnerGruppe = null;

  private IEnumerator warteAufGegnerGruppe()
  {
    float WarteZeit = 30f;
    float WarteAbstand = 0.5f;

    for (float Zeit = 0f; Zeit < WarteZeit; Zeit += WarteAbstand)
    {
      Prozessor.add(new CWarteRelativ(0.5f));
      yield return Prozessor.warteBisFertig();

      var Liste = sucheKlasse<IGegnerGruppe>();

      if (Liste.Count > 0)
      {
        GegnerGruppe = Liste[0];
        break;
      }
    }

  }

  [Test]
  public IEnumerator GruppenGeschwindigkeit()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = false;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    yield return starteSpiel(SpielSteuerungsObjekt);

    SpielSteuerungsObjekt.Schwierigkeitsgrad = 0.5f;

    GegnerGruppe = null;
    yield return warteAufGegnerGruppe();

    Pruefer.istTrue(GegnerGruppe != null, "Das Gegnergruppen-Objekt kann nicht als Gegnergruppe angesprochen werden.");

    Geschwindigkeit = 0.0f;
    FlugZiel = Vector3.zero;
    yield return messeGeschwindigkeitUndFlugziel(GegnerGruppe);

    float LetzteGeschwindigkeit = Geschwindigkeit;
    Pruefer.istKleiner(2.0f, (FlugZiel - Kamera.position).magnitude, "Die Gegner steuern das falsche Ziel an: " + FlugZiel);

    yield return schiesseGegnerAbAusGruppe(GegnerGruppe);
    yield return schiesseGegnerAbAusGruppe(GegnerGruppe);

    Geschwindigkeit = 0.0f;
    FlugZiel = Vector3.zero;
    yield return messeGeschwindigkeitUndFlugziel(GegnerGruppe);

    Pruefer.istGroesser(LetzteGeschwindigkeit, Geschwindigkeit, "Die Geschwindigkeit ist nicht größer geworden.");
    Pruefer.istKleiner(2.0f, (FlugZiel - Kamera.position).magnitude, "Die Gegner steuern das falsche Ziel an: " + FlugZiel);

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

  private IEnumerator schiesseGegnerAbAusGruppe(IGegnerGruppe Gruppe)
  {
    Transform GegnerObjekt = Gruppe.Objekt.transform.GetChild(0);
    Pruefer.istTrue(GegnerObjekt != null, "An der Gruppe konnte kein Gegner-Objekt gefunden werden.");

    BoxCollider GegnerCollider = GegnerObjekt.GetComponent<BoxCollider>();
    Pruefer.istTrue(GegnerObjekt != null, "Der Gegner hat keinen Collider!");

    Prozessor.add(new CRichteKopfAufObjektAus(Kamera, GegnerObjekt, GegnerCollider.center));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));
    Prozessor.add(new CWarteRelativ(0.5f));

    yield return Prozessor.warteBisFertig();
  }

  float Geschwindigkeit = 0.0f;
  Vector3 FlugZiel = Vector3.zero;

  private IEnumerator messeGeschwindigkeitUndFlugziel(IGegnerGruppe Gruppe)
  {
    float WarteZeit = 0.5f;
    Vector3 LetzteGruppenPosition = Gruppe.Objekt.transform.position;

    Prozessor.add(new CWarteRelativ(WarteZeit));
    yield return Prozessor.warteBisFertig();

    Vector3 FlugRichtung = Gruppe.Objekt.transform.position - LetzteGruppenPosition;
    Geschwindigkeit = FlugRichtung.magnitude / WarteZeit;

    Logger.Log("Gemessene Geschwindigkeit: " + Geschwindigkeit + " 1/s");

    float GegnerAbstand = (Gruppe.Objekt.transform.position - Kamera.position).magnitude;
    //Logger.Log("Gegner-Abstand von Kamera: " + GegnerAbstand);

    FlugZiel = Gruppe.Objekt.transform.position + FlugRichtung.normalized * GegnerAbstand;
  }
}
