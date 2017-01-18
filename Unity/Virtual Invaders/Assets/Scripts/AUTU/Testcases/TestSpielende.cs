using AUTU;
using AUTU.Befehle;
using System.Collections;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Controllers;
using System.IO;
using System;

class TestSpielende : CTestumgebungSpiel
{
  [Test]
  public IEnumerator SpielEndetWennHitpoints0()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;

    yield return starteSpiel(SpielSteuerungsObjekt);

    yield return toeteSpieler(SpielSteuerungsObjekt);

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

  public IEnumerator toeteSpieler(ISpielsteuerung SpielSteuerungsObjekt)
  {
    Pruefer.istGroesser(0, SpielSteuerungsObjekt.Hitpoints, "Der Spieler hat keine Hitpoints direkt nach dem Spielstart.");

    for (int i = SpielSteuerungsObjekt.Hitpoints; SpielSteuerungsObjekt.Hitpoints > 0 && i > 0;)
    {
      SpielSteuerungsObjekt.PlayerHit();
      i--;
    }

    Prozessor.add(new CWarteRelativ(0.5f));
    yield return Prozessor.warteBisFertig();

    // Toleranz ist 1, denn es ist ok, wenn der Gegner noch 1 Hitpoint hat, weil der letzte Hitpoint nicht mehr abgezogen wurde
    Pruefer.istGleich(0, SpielSteuerungsObjekt.Hitpoints, 1, "Der Spieler hat noch Hitpoints, obwohl er beschossen wurde.");
    Pruefer.istTrue(SpielSteuerungsObjekt.Gameover, "Das Spiel ist nicht beendet worden, obwohl der Spieler keine Hitpoints mehr hat.");
  }

  private string ScoreDateiPfad
  {
    get { return Application.persistentDataPath + Path.DirectorySeparatorChar + "highscore.txt"; }
  }

  private void loescheScoreDatei()
  {
    if (File.Exists(ScoreDateiPfad))
    {
      File.Delete(ScoreDateiPfad);
    }
  }

  private int leseScoreAusDatei()
  {
    Pruefer.istTrue(File.Exists(ScoreDateiPfad), "Es wurde keine Highscore-Datei angelegt.");
    var DateiStream = new StreamReader(ScoreDateiPfad);
    string ScoreText = DateiStream.ReadLine();

    int Score = 0;
    if (!int.TryParse(ScoreText, out Score))
    {
      Pruefer.istTrue(false, "Score konnte nicht aus Datei gelesen werden: " + ScoreText);
    }

    DateiStream.Close();

    return Score;
  }

  private void schreibeScoreInDatei(int Score)
  {
    loescheScoreDatei();

    var DateiStream = new StreamWriter(ScoreDateiPfad);
    DateiStream.WriteLine(Score);

    Pruefer.istTrue(File.Exists(ScoreDateiPfad), "Es wurde keine Highscore-Datei angelegt.");

    DateiStream.Close();
  }

  [Test]
  public IEnumerator SpeichereHighscoreWennSpielEndet()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    SpielSteuerungsObjekt.GameOverWarteZeit = 0.0f;

    GegnerSteuerungsObjekt.KomponenteAktiv = true;

    loescheScoreDatei();

    yield return starteSpiel(SpielSteuerungsObjekt);

    int VergleichsScore = 11;
    for(int i = 0; i < VergleichsScore; i++)
    {
      SpielSteuerungsObjekt.PlayerScored();
    }

    yield return toeteSpieler(SpielSteuerungsObjekt);

    Pruefer.istGleich(VergleichsScore, leseScoreAusDatei(), 0, "Score konnte nicht richtig zurückgelesen werden.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

  [Test]
  public IEnumerator UeberschreibeHighscoreWennHoehererWertErreicht()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;

    schreibeScoreInDatei(5);

    yield return starteSpiel(SpielSteuerungsObjekt);

    int VergleichsScore = 6;
    for (int i = 0; i < VergleichsScore; i++)
    {
      SpielSteuerungsObjekt.PlayerScored();
    }

    yield return toeteSpieler(SpielSteuerungsObjekt);

    Pruefer.istGleich(VergleichsScore, leseScoreAusDatei(), 0, "Score konnte nicht richtig zurückgelesen werden.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

  [Test]
  public IEnumerator UeberschreibeHighscoreNichtWennWertNiedriger()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;

    int VergleichsScore = 7;
    schreibeScoreInDatei(VergleichsScore);

    yield return starteSpiel(SpielSteuerungsObjekt);

    int NeuerScore = 6;
    for (int i = 0; i < NeuerScore; i++)
    {
      SpielSteuerungsObjekt.PlayerScored();
    }

    yield return toeteSpieler(SpielSteuerungsObjekt);

    Pruefer.istGleich(VergleichsScore, leseScoreAusDatei(), 0, "Score konnte nicht richtig zurückgelesen werden.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }
}

