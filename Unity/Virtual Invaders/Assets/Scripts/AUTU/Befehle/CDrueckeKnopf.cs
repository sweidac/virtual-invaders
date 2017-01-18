using UnityEngine;
using UnityEngine.EventSystems;

namespace AUTU.Befehle
{
  class CDrueckeKnopf : IBefehl
  {
    private uint Anzahl = 1;
    private float Wartezeit = 0.25f;
    private float AktuelleWarteZeit = 0.0f;
    private bool Gedrueckt = false;

    public bool bearbeiten(ILogger Logger, IProzessorKomponenten Komponenten)
    {
      IKnopfInjektion Injektion = Komponenten.suche<IKnopfInjektion>();
      Debug.Assert(Injektion != null, "Es konnte keine Injektion-Komponente für den Knopf beim Prozessor gefunden werden!");

      if (AktuelleWarteZeit > 0.0f)
      {
        AktuelleWarteZeit -= Time.deltaTime;
      }
      else if (Anzahl == 0)
      {
        return true;
      }
      else if (Gedrueckt)
      {
        Injektion.drueckeKnopf(false);
        Gedrueckt = false;
        AktuelleWarteZeit = Wartezeit;
        Anzahl--;
      }
      else
      {
        Injektion.drueckeKnopf(true);
        Gedrueckt = true;
        AktuelleWarteZeit = Wartezeit;
      }

      return false;
    }

    public override string ToString()
    {
      return "Drücke den Button der GoogleVR Brille.";
    }
  }
}
