using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;

public class TestStartmenue : CTestGruppe
{
  public Transform StartButton = null;
  public Transform Kamera = null;
  public GameObject Hauptmenue = null;
  public GameObject GameOverBild = null;

  [Test]
  public IEnumerator SpielStartUndStopp()
  {
    Debug.Assert(StartButton != null, "Es ist kein Start-Button Objekt mit dem Testcase verbunden.");
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");
    Debug.Assert(Hauptmenue != null, "Es ist kein Hauptmenue Objekt mit dem Testcase verbunden.");
    Debug.Assert(GameOverBild != null, "Es ist kein GameOver-Bild Objekt mit dem Testcase verbunden.");
    ISpielsteuerung Steuerung = Spielsteuerung.Instance;

    // das Spiel stoppen falls es läuft
    if (!Steuerung.Gameover)
    {
      Steuerung.SpielStopp();
    }

    Pruefer.istTrue(Steuerung.Gameover, "Das Spiel läuft bereits.");
    Pruefer.istTrue(Hauptmenue.activeSelf, "Das Hauptmenü ist nicht sichtbar.");
    Pruefer.istFalse(GameOverBild.activeSelf, "Das GameOver-Bild ist schon vor Spielstart sichtbar.");

    Prozessor.add(new CRichteKopfAufObjektAus(Kamera, StartButton));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));
    Prozessor.add(new CDrueckeKnopf());
    Prozessor.add(new CWarteRelativ(0.2f));

    yield return Prozessor.warteBisFertig();

    Pruefer.istFalse(Steuerung.Gameover, "Das Spiel wurde nicht gestartet.");
    Pruefer.istFalse(Hauptmenue.activeSelf, "Das Hauptmenü wurde nicht ausgeblendet.");
    Pruefer.istFalse(GameOverBild.activeSelf, "Das GameOver-Bild ist direkt nach Spielstart sichtbar.");

    // das Spiel wieder stoppen
    Steuerung.SpielStopp();

    Pruefer.istTrue(GameOverBild.activeSelf, "Das GameOver-Bild nach dem Spielende nicht sichtbar.");
    Pruefer.istTrue(Steuerung.Gameover, "Das Spiel ist nicht beendet worden.");
    Pruefer.istFalse(Hauptmenue.activeSelf, "Das Hauptmenü ist schon sichtbar, obwohl noch der GameOver-Bildschirm gezeigt wird.");

    Prozessor.add(new CWarteRelativ(Steuerung.GameOverWarteZeit+0.5f));
    yield return Prozessor.warteBisFertig();

    Pruefer.istFalse(GameOverBild.activeSelf, "Das GameOver-Bild ist nach der Wartezeit immernoch sichtbar.");
    Pruefer.istTrue(Steuerung.Gameover, "Das Spiel ist nicht beendet worden.");
    Pruefer.istTrue(Hauptmenue.activeSelf, "Das Hauptmenü ist nicht sichtbar.");

    yield return true;
  }
}

