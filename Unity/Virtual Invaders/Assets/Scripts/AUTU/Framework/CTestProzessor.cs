using UnityEngine;
using System.Collections.Generic;

namespace AUTU
{
  public class CTestProzessor : MonoBehaviour, ITestProzessorSchreiber, ITestProzessorAusfuehrer, IProzessorKomponenten
  {
    // Optionen die von Unity aus gesetzt werden
    public bool DebugAusgaben = false;

    private Queue<IBefehl> BefehlsListe = new Queue<IBefehl>();
    private IBefehl AktuellerBefehl = null;
    protected ILogger Logger { get; private set; }

    public bool IstFertig
    {
      get
      {
        return Befehle == 0;
      }
    }

    public uint Befehle
    {
      get
      {
        if (AktuellerBefehl != null)
        {
          return (uint)BefehlsListe.Count + 1;
        }
        else
        {
          return (uint)BefehlsListe.Count;
        }
      }
    }

    public CTestProzessor()
    {
      reset();
      Logger = new CLogger();
      Logger.ConsoleEin = DebugAusgaben;
    }

    public T add<T>(T Befehl) where T : IBefehl
    {
      BefehlsListe.Enqueue(Befehl);
      return Befehl;
    }

    private IBefehl holeNaechstenBefehl()
    {
      if (Befehle > 0)
      {
        if (AktuellerBefehl != null)
        {
          return AktuellerBefehl;
        }
        else
        {
          IBefehl NeuerBefehl = BefehlsListe.Dequeue();
          Logger.Log("Nächster Befehl: "+NeuerBefehl.ToString());
          return NeuerBefehl;
        }
      }
      else
      {
        return null;
      }
    }

    public void bearbeiteBefehl()
    {
      do
      {
        AktuellerBefehl = holeNaechstenBefehl();
        if (AktuellerBefehl != null)
        {
          if (AktuellerBefehl.bearbeiten(Logger, this))
          {
            AktuellerBefehl = null;
          }
        }
      } while (Befehle > 0 && AktuellerBefehl == null);
    }

    public CustomYieldInstruction warteBisFertig()
    {
      return new CYieldWarteBisProzessorFertig(this);
    }

    public void reset()
    {
      BefehlsListe.Clear();
      AktuellerBefehl = null;
    }

    public void setLogger(ILogger Logger)
    {
      this.Logger = Logger;
    }

    protected void OnValidate()
    {
      Logger.ConsoleEin = DebugAusgaben;

      foreach(CKomponentenBeschreibung Beschreibung in KomponentenListe)
      {
        if (Beschreibung.Name == "" && Beschreibung.Komponente != null)
        {
          Beschreibung.Name = Beschreibung.Komponente.name;
        }
      }
    }

    [System.Serializable]
    public class CKomponentenBeschreibung
    {
      public string Name;
      public Object Komponente;

      public CKomponentenBeschreibung(Object Komponente, string Name = "")
      {
        this.Komponente = Komponente;
        this.Name = Name;
      }
    }

    public List<CKomponentenBeschreibung> KomponentenListe = new List<CKomponentenBeschreibung>();

    private bool istNameGleich(string GesuchterName, string Name)
    {
      if (GesuchterName != null && GesuchterName != "" && Name == GesuchterName)
      {
        return true;
      }
      else if (GesuchterName == null || GesuchterName == "")
      {
        return true;
      }

      return false;
    }

    public T suche<T>(string Name = null, T AlternativObjekt = null) where T : class
    {
      foreach(CKomponentenBeschreibung Beschreibung in KomponentenListe)
      {
        if (Beschreibung.Komponente is T)
        {
          if (istNameGleich(Name, Beschreibung.Name))
          {
            return Beschreibung.Komponente as T;
          }
        }
        else if (Beschreibung.Komponente is GameObject)
        {
          GameObject Objekt = Beschreibung.Komponente as GameObject;
          foreach (T UnterKomponente in Objekt.GetComponents<T>())
          {
            if (istNameGleich(Name, Beschreibung.Name))
            {
              return UnterKomponente;
            }
          }
        }
      }

      return AlternativObjekt;
    }

    public object suche(string Name)
    {
      return suche<object>(Name);
    }

    public List<T> sucheAlle<T>(string Name = null) where T : class
    {
      List<T> Liste = new List<T>();

      foreach (CKomponentenBeschreibung Beschreibung in KomponentenListe)
      {
        if (Beschreibung.Komponente is T)
        {
          if (istNameGleich(Name, Beschreibung.Name))
          {
            Liste.Add(Beschreibung.Komponente as T);
          }
        }
        else if (Beschreibung.Komponente is GameObject)
        {
          GameObject Objekt = Beschreibung.Komponente as GameObject;
          foreach (T UnterKomponente in Objekt.GetComponents<T>())
          {
            if (istNameGleich(Name, Beschreibung.Name))
            {
              Liste.Add(UnterKomponente);
            }
          }
        }
      }

      return Liste;
    }

    public void alleKomponentenAbschalten()
    {
      var Liste = sucheAlle<ITestKomponente>();
      foreach (ITestKomponente Komponente in Liste)
      {
        Komponente.KomponenteAktiv = false;
      }
    }
  }
}
