using AUTU;

namespace Assets.Scripts
{
  public interface ISpielsteuerung : ITestKomponente
  {
    /// <summary>
    /// Gibt die aktuelle Punktzahl des Spielers zurück.
    /// </summary>
    int Score { get; }

    /// <summary>
    /// Gibt die aktuelle Zahl der Hitpoints zurück;
    /// </summary>
    int Hitpoints { get; set; }

    /// <summary>
    /// Gibt zurück ob das Spiel läuft (false) oder nicht (true).
    /// </summary>
    bool Gameover { get; }

    /// <summary>
    /// Gibt oder setzt den Schwierigkeitsgrad im Spiel.
    /// </summary>
    float Schwierigkeitsgrad { get; set; }

    /// <summary>
    /// Die Anzahl der initialen Lebenspunkte beim Spielstart.
    /// </summary>
    int InitialeLebenspunkte { get; }

    /// <summary>
    /// Die Zeit in Sekunden bis der maximale Schwierigkeitsgrad erreicht ist.
    /// </summary>
    float ZeitBisMaximalenSchwierigkeitsgrad { get; }

    /// <summary>
    /// Die maximale Zeit, die auf ein UFO gewartet werden muss.
    /// </summary>
    float MaximaleWartezeitAufUFO { get; }

    /// <summary>
    /// Gibt die Wartezeit für den GameOver Bildschirm zurück.
    /// </summary>
    float GameOverWarteZeit { set; get; }

    /// <summary>
    /// Beendet das Spiel.
    /// </summary>
    void SpielStopp();

    /// <summary>
    /// Zieht den Spieler einen Trefferpunkt ab.
    /// </summary>
    void PlayerHit();

    /// <summary>
    /// Vergibt einen Punkt an den Spieler.
    /// </summary>
    void PlayerScored();
  }
}
