namespace AUTU.Befehle
{
  public class CWarteBisWertKonstantIst : IBefehl
  {
    IWert Wert = null;
    uint MaxFrames = 0;
    uint Frames = 0;

    public CWarteBisWertKonstantIst(IWert Wert, uint Frames = 10)
    {
      this.Wert = Wert;
      MaxFrames = Frames;
      Frames = MaxFrames;
    }

    public bool bearbeiten(AUTU.ILogger Logger, IProzessorKomponenten Komponenten)
    {
      if (Wert.istKonstant())
      {
        if (Frames == 0)
        {
          return true;
        }
        Frames--;
      }

      return false;
    }

    public override string ToString()
    {
      return "Warte bis der Wert Konstant ist: "+Wert.GetType().Name;
    }
  }
}
