using System;
using System.Collections;
using UnityEngine;

namespace AUTU
{
  /// <summary>
  /// Diese Klasse führt intern eine Coroutine aus und repräsentiert diese nach außen hin.
  /// Zusätzlich fängt sie Exceptions ab und speichert diese zwischen. Bei einer Exception wird die
  /// Ausführung der aktuellen Coroutine sofort beendet.
  /// </summary>
  class CExceptionCoroutine
  {
    /// <summary>
    /// Ein Delegate, welches die Coroutine zum Ausführen repräsentiert.
    /// </summary>
    public delegate IEnumerator CoroutineFunktion();

    private CoroutineFunktion InterneCoroutine = null;

    /// <summary>
    /// Die Exception die aufgetreten ist. Ist keine Exception aufgetreten, ist dieser Wert null.
    /// </summary>
    public Exception AufgetreteneException { get; private set; }

    /// <summary>
    /// Gibt true zurück, wenn eine Exception aufgetreten ist.
    /// </summary>
    public bool IstEineExeptionAufgetreten { get { return AufgetreteneException != null; } }

    /// <summary>
    /// Konstruktor.
    /// </summary>
    /// <param name="Coroutine">Ist die Coroutine die ausgeführt werden soll.</param>
    public CExceptionCoroutine(CoroutineFunktion Coroutine)
    {
      InterneCoroutine = Coroutine;
      AufgetreteneException = null;
    }

    /// <summary>
    /// Startet die interne Coroutine und repräsiert diese nach außen hin.
    /// </summary>
    /// <returns>Gibt ein IEnumerator Object zurück, um nach außen wie eine Coroutine zu wirken.</returns>
    public IEnumerator starte()
    {
      Debug.Assert(InterneCoroutine != null, "Es ist keine gültige Coroutine zum Ausführen definiert.");

      IEnumerator Iterator = null;

      try
      {
        Iterator = InterneCoroutine();
      }
      catch (Exception DieseExeption)
      {
        AufgetreteneException = DieseExeption;
      }

      if (!IstEineExeptionAufgetreten && (Iterator != null))
      {
        for (;;)
        {
          try
          {
            if (!Iterator.MoveNext())
            {
              break;
            }
          }
          catch (Exception DieseExeption)
          {
            AufgetreteneException = DieseExeption;
            break;
          }

          yield return Iterator.Current;
        }

      }
    }
  }
}
