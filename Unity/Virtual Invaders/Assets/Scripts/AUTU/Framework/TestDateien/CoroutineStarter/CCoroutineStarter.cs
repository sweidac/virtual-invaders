using System.Collections;
using UnityEngine;

namespace AUTU
{
  /// <summary>
  /// Eine Hilfsklasse zum Ausführen von Coroutinen in einem Unit-Test.
  /// </summary>
  public class CCoroutineStarter
  {
    class FehlerKeineCoroutineDefiniert : Fehler
    {
      public FehlerKeineCoroutineDefiniert() : base("Es wurde keine Coroutine zum Starten definiert!") { }
    }

    class FehlerMaximaleIterationenErreicht : Fehler
    {
      public FehlerMaximaleIterationenErreicht(string Name, uint MaximaleIterationen) :
        base("Die Coroutine "+Name+" hat die maximale Anzahl an Iterationen ("+ MaximaleIterationen + ") überschritten!") { }
    }

    public delegate IEnumerator CoroutineFunktion();

    private CoroutineFunktion Coroutine = null;
    private uint MaximaleIterationen = 0;
    private uint Iterationen = 0;

    /// <summary>
    /// Bindet eine Coroutine an diese Klasse.
    /// </summary>
    /// <param name="Coroutine">Ist die Coroutine die ausgeführt werden soll.</param>
    /// <param name="MaximaleIterationen">Ist die maximale Anzahl an Iterationen für diese Coroutine (0 = unendlich viele).</param>
    public CCoroutineStarter(CoroutineFunktion Coroutine, uint MaximaleIterationen = 0)
    {
      this.Coroutine = Coroutine;
      this.MaximaleIterationen = MaximaleIterationen;
    }

    /// <summary>
    /// Startet die Coroutine.
    /// </summary>
    public void starte()
    {
      if (Coroutine == null)
      {
        throw new FehlerKeineCoroutineDefiniert();
      }

      Iterationen = MaximaleIterationen;

      IEnumerator Iterator = Coroutine();
      iteratorSchleife(Iterator);
    }

    private void bearbeiteWarteAnweisung(CustomYieldInstruction WarteAnweisung)
    {
      if (WarteAnweisung is CYieldWarteBisProzessorFertig)
      {
        CYieldWarteBisProzessorFertig ProzessorWarteAnweisung = WarteAnweisung as CYieldWarteBisProzessorFertig;

        while (ProzessorWarteAnweisung.keepWaiting)
        {
          ProzessorWarteAnweisung.ProzessorAusfuehrer.bearbeiteBefehl();
          substrahiereIterationen();
        }
      }
    }

    private void substrahiereIterationen()
    {
      if (MaximaleIterationen > 0)
      {
        if (Iterationen <= 1)
        {
          throw new FehlerMaximaleIterationenErreicht(Coroutine.GetType().Name, MaximaleIterationen);
        }
        else
        {
          Iterationen--;
        }
      }
    }

    private void iteratorSchleife(IEnumerator Iterator)
    {
      while(Iterator.MoveNext())
      {
        if (Iterator.Current is CustomYieldInstruction)
        {
          bearbeiteWarteAnweisung(Iterator.Current as CustomYieldInstruction);
        }
        else if (Iterator.Current is IEnumerator)
        {
          iteratorSchleife(Iterator.Current as IEnumerator);
        }
        else
        {
          substrahiereIterationen();
        }
      }
    }

  }
}
