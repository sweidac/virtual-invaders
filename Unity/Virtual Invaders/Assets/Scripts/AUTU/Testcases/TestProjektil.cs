using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

public class TestProjektil : CTestumgebungSpiel
{
  private void setzeGegnerPositionZurueck(List<GameObject> GegnerGruppen, List<Vector3> Positionen)
  {
    int i = 0;
    foreach (GameObject GegnerGruppe in GegnerGruppen)
    {
      GegnerGruppe.transform.position = Positionen[i];
      i++;
    }
  }

  [Test]
  public IEnumerator GegnerErstellenProjektil()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = true;

    yield return starteSpiel(SpielSteuerungsObjekt);

    GegnerEntfernung = 30;
    GegnerHoehe = 10;

    List<GameObject> GegnerListe = erzeugeGegner(GegnerSteuerungsObjekt);
    List<Vector3> PositionsListe = new List<Vector3>();

    float MaximaleWarteZeit = 0;
    Pruefer.istGroesser(0, GegnerListe.Count, "Es konnten keine Gegner zum Testen erzeugt werden.");

    foreach (GameObject GegnerGruppe in GegnerListe)
    {
      PositionsListe.Add(GegnerGruppe.transform.position);
      Pruefer.istGroesser(0, GegnerGruppe.transform.childCount, "Die Gegner Gruppe enthält keine Gegner!");

      foreach (Transform GegnerObjekt in GegnerGruppe.transform)
      {
        if (GegnerObjekt != null)
        {
          IGegner Gegner = GegnerObjekt.GetComponent<IGegner>();
          if (Gegner != null)
          {
            if (Gegner.SchussVerzoegerung > MaximaleWarteZeit)
            {
              MaximaleWarteZeit = Gegner.SchussVerzoegerung;
            }
          }
        }
      }
    }

    Logger.Log("Warte auf eine Rakete maximal: "+MaximaleWarteZeit+"s");

    float ZeitFaktor = Time.timeScale;
    Time.timeScale = 10 * ZeitFaktor;

    // warte max. die zuvor berechnete Zeit
    for (int i = 0; i < (int)(MaximaleWarteZeit / 0.5f); i++)
    {
      Prozessor.add(new CWarteRelativ(0.5f));
      yield return Prozessor.warteBisFertig();

      if (sucheKlasse<IGegnerProjektil>().Count > 0)
      {
        break;
      }

      setzeGegnerPositionZurueck(GegnerListe, PositionsListe);
    }

    Time.timeScale = ZeitFaktor;

    Pruefer.istGroesser(0, sucheKlasse<IGegnerProjektil>().Count, "Es sind nach "+MaximaleWarteZeit+" Sekunden keine Raketen erzeugt worden!");

    yield return stoppeSpiel(SpielSteuerungsObjekt);
  }

  [Test]
  public IEnumerator ProjektileKoennenVomLaserZerstoertWerden()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    yield return starteSpiel(SpielSteuerungsObjekt);

    GegnerEntfernung = 30;
    GegnerHoehe = 10;

    List<GameObject> GegnerListe = erzeugeGegner(GegnerSteuerungsObjekt);
    List<Vector3> PositionsListe = new List<Vector3>();

    Pruefer.istGroesser(0, GegnerListe.Count, "Es konnten keine Gegner zum Testen erzeugt werden.");

    List<GameObject> PreFabListe = new List<GameObject>();
    List<GameObject> ProjektilListe = new List<GameObject>();
    foreach (GameObject GegnerGruppe in GegnerListe)
    {
      PositionsListe.Add(GegnerGruppe.transform.position);
      Pruefer.istGroesser(0, GegnerGruppe.transform.childCount, "Die Gegner Gruppe enthält keine Gegner!");

      foreach (Transform GegnerObjekt in GegnerGruppe.transform)
      {
        if (GegnerObjekt != null)
        {
          IGegner Gegner = GegnerObjekt.GetComponent<IGegner>();
          if (Gegner != null)
          {
            GameObject ProjektilObjekt = Gegner.ErstelleProjektil();

            Pruefer.istTrue(ProjektilObjekt != null, "Es konnte kein Projektil erstellt werden!");

            IGegnerProjektil Projektil = ProjektilObjekt.GetComponent<IGegnerProjektil>();

            Pruefer.istTrue(Projektil != null, "Das Projektil "+ ProjektilObjekt.name+" bedient nicht das Interface IGegnerProjektil.");

            GameObject ExplositionsPreFab = Projektil.ExplositionsPreFab;

            Pruefer.istTrue(ExplositionsPreFab != null, "Das Projektil " + ProjektilObjekt.name + " bedient besitzt kein Explosionen-Prefab!");

            PreFabListe.Add(ExplositionsPreFab);
            ProjektilListe.Add(ProjektilObjekt);
          }
        }
      }
    }

    int NummerProjektile = ProjektilListe.Count;
    Pruefer.istGroesser(0, NummerProjektile, "Es wurden keine Projektile erstellt.");

    List<GameObject> ExplositionsListe = new List<GameObject>();

    foreach (GameObject Projektil in ProjektilListe)
    {
      Collider ProjektilCollider = Projektil.GetComponentInChildren<Collider>();
      if (ProjektilCollider != null)
      {
        int i = 0;
        while(Projektil != null && i < 20)
        {
          i++;
          Vector3 Offset = ProjektilCollider.bounds.center;
          Prozessor.add(new CRichteKopfAufObjektAus(Kamera, Projektil.transform, Offset));
          Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));
          Prozessor.add(new CWarteRelativ(0.2f));


          yield return Prozessor.warteBisFertig();
        }

        foreach (GameObject PreFab in PreFabListe)
        {
          if (PreFab != null)
          {
            Debug.Log("Suche nach Explosionen mit dem Namen: " + PreFab.name + "(Clone)");
            var Explositionen = sucheKlasse<GameObject>(PreFab.name + "(Clone)");
            Debug.Log("Gefunden: "+Explositionen.Count);
            foreach (GameObject Explosition in Explositionen)
            {
              if (!ExplositionsListe.Contains(Explosition))
              {
                ExplositionsListe.Add(Explosition);

                AudioSource Audio = Explosition.GetComponentInChildren<AudioSource>();
              }
            }
          }
        }

      }
    }

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(NummerProjektile, ExplositionsListe.Count, 0, "Es eine andere Anzahl an Explosionen als es Projektile gab!");

    Prozessor.add(new CWarteRelativ(4.0f));
    yield return Prozessor.warteBisFertig();

    foreach(GameObject Explosition in ExplositionsListe)
    {
      Pruefer.istTrue(Explosition == null, "Explosion wurde nicht zerstört nach der Benutzung.");
    }

    Pruefer.istGleich(0, sucheKlasse<IGegnerProjektil>().Count, 0, "Es sind immer noch Projektile vorhanden!");

    yield return stoppeSpiel(SpielSteuerungsObjekt);
  }

}