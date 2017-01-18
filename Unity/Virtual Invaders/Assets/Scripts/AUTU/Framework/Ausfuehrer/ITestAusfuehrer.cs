using System.Collections;

namespace AUTU
{
  public interface ITestAusfuehrer
  {
    /// <summary>
    /// Gibt eine Zusammenfassung über den Testverlauf.
    /// </summary>
    IZusammenfassung Zusammenfassung { get; }

    /// <summary>
    /// Startet alle bisher ungelaufenen Tests.
    /// </summary>
    /// <returns>Gibt ein IEnumerator Objekt zurück um die Tests in einer Coroutine ausführen zu können.</returns>
    IEnumerator starteUngelaufeneTests();

    /// <summary>
    /// Startet alle Tests, auch solche die schon einmal gelaufen sind.
    /// </summary>
    /// <returns>Gibt ein IEnumerator Objekt zurück um die Tests in einer Coroutine ausführen zu können.</returns>
    IEnumerator starteAlleTests();

    /// <summary>
    /// Startet alle Tests, die entweder noch gar nicht gelaufen sind oder die beim letzten Durchlauf fehlerhaft waren.
    /// </summary>
    /// <returns>Gibt ein IEnumerator Objekt zurück um die Tests in einer Coroutine ausführen zu können.</returns>
    IEnumerator starteAlleFehlerhaftenTests();
  }
}
