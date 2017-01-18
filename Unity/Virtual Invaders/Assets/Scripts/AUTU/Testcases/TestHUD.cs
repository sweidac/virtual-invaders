using Assets.Scripts;
using Assets.Scripts.Controllers;
using AUTU;
using AUTU.Befehle;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

class TestHUD : CTestumgebungSpiel
{
  public GameObject HUD = null;

  [Test]
  public IEnumerator HUDAnzeigen()
  {
    Debug.Assert(HUD != null, "Es ist kein HUD-Objekt mit dem Testcase verlinkt.");

    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;

    Pruefer.istFalse(HUD.activeSelf, "HUD ist nicht deaktiviert im Menü!");

    yield return starteSpiel(SpielSteuerungsObjekt);

    Pruefer.istTrue(HUD.activeSelf, "HUD ist nicht aktiviert im Spiel!");

    Text HitPointText = null;
    Text ScoreText = null;
    Text[] TextFelder = HUD.GetComponentsInChildren<Text>();
    foreach(Text TextFeld in TextFelder)
    {
      if (TextFeld.text == SpielSteuerungsObjekt.Hitpoints.ToString())
      {
        SpielSteuerungsObjekt.PlayerHit();

        Prozessor.add(new CWarteRelativ(0.2f));
        yield return Prozessor.warteBisFertig();

        if (TextFeld.text == SpielSteuerungsObjekt.Hitpoints.ToString())
        {
          HitPointText = TextFeld;
        }
      }

      if (TextFeld.text == SpielSteuerungsObjekt.Score.ToString())
      {
        SpielSteuerungsObjekt.PlayerScored();

        Prozessor.add(new CWarteRelativ(0.2f));
        yield return Prozessor.warteBisFertig();

        if (TextFeld.text == SpielSteuerungsObjekt.Score.ToString())
        {
          ScoreText = TextFeld;
        }
      }
    }

    Pruefer.istTrue(HitPointText != null, "Es konnte kein Hit-Point Textfeld gefunden werden im HUD.");
    Pruefer.istTrue(ScoreText != null,    "Es konnte kein Score Textfeld gefunden werden im HUD.");

    SpielSteuerungsObjekt.PlayerHit();
    SpielSteuerungsObjekt.PlayerScored();
    SpielSteuerungsObjekt.PlayerScored();

    Prozessor.add(new CWarteRelativ(0.2f));
    yield return Prozessor.warteBisFertig();

    Pruefer.istTrue(HitPointText.text == SpielSteuerungsObjekt.Hitpoints.ToString(), "Der Hit-Point Text wird nicht korrekt angepasst, wenn die Hitpoints reduziert werden.");
    Pruefer.istTrue(ScoreText.text == SpielSteuerungsObjekt.Score.ToString(), "Der Score Text wird nicht korrekt angepasst, wenn die Punkte erhöht werden werden.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    Pruefer.istFalse(HUD.activeSelf, "HUD ist nicht deaktiviert nach dem Spielende");

    yield return true;
  }
}
