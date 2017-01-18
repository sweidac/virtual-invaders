using System;

namespace AUTU
{
  public interface IPruefer
  {
    /// <summary>
    /// Prüft, ob die Bedingung true ist.
    /// </summary>
    /// <param name="Kondition">Die Kondition zum Prüfen.</param>
    void istTrue(bool Kondition, string Text = null);

    /// <summary>
    /// Prüft, ob die Bedingung false ist.
    /// </summary>
    /// <param name="Kondition">Die Kondition zum Prüfen.</param>
    void istFalse(bool Kondition, string Text = null);

    /// <summary>
    /// Prüft ob beide Objekte von der gleichen Referenz sind.
    /// </summary>
    /// <param name="Objekt1">Ist das erste Objekt zum Überprüfen.</param>
    /// <param name="Objekt2">Ist das zweite Objekt zum Überprüfen.</param>
    void istGleicheReferenz(object Objekt1, object Objekt2, string Text = null);

    /// <summary>
    /// Prüft ob die rechte Seite größer als die linke Seite ist.
    /// </summary>
    /// <param name="Vergleichswert">Der Wert mit dem verglichen werden soll.</param>
    /// <param name="Pruefwert">Der Wert der geprüft werden soll.</param>
    /// <typeparam name="T"></typeparam>
    void istGroesser<T>(T Vergleichswert, T Pruefwert, string Text = null) where T : IComparable;

    /// <summary>
    /// Prüft ob die rechte Seite kleiner als die linke Seite ist.
    /// </summary>
    /// <param name="Vergleichswert">Der Wert mit dem verglichen werden soll.</param>
    /// <param name="Pruefwert">Der Wert der geprüft werden soll.</param>
    /// <typeparam name="T"></typeparam>
    void istKleiner<T>(T Vergleichswert, T Pruefwert, string Text = null) where T : IComparable;

    /// <summary>
    /// Prüft ob zwei Werte gleich sind (bzw. sich in einem Toleranzintervall befinden).
    /// </summary>
    /// <param name="Vergleichswert">Der Wert mit dem verglichen wird.</param>
    /// <param name="Pruefwert">Der Wert der überprüft werden soll.</param>
    /// <param name="Toleranz">Eine Toleranz.</param>
    void istGleich(double Vergleichswert, double Pruefwert, double Toleranz = 0.0f, string Text = null);
  }
}
