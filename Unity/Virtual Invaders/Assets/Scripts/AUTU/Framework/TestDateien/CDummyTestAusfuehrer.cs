
namespace AUTU
{
  public class CDummyTestAusfuehrer : CTestAusfuehrer
  {
    public void rufeAufUpdate()
    {
      Update();
    }

    public void rufeAufStart()
    {
      Start();
    }

    public ITestProzessorSchreiber getProzessorSchreiber()
    {
      return TestProzessorSchreiber;
    }

    public ITestProzessorAusfuehrer getProzessorAusfuehrer()
    {
      return TestProzessorAusfuehrer;
    }

    public IProzessorKomponenten getProzessorKomponenten()
    {
      return TestProzessorKomponenten;
    }

    public void setProzessorAusfuehrer(ITestProzessorAusfuehrer ProzessorAusfuehrer)
    {
      TestProzessorAusfuehrer = ProzessorAusfuehrer;
    }

    public void setProzessorSchreiber(ITestProzessorSchreiber ProzessorSchreiber)
    {
      TestProzessorSchreiber = ProzessorSchreiber;
    }

    public void setProzessorKomponenten(IProzessorKomponenten ProzessorKomponenten)
    {
      TestProzessorKomponenten = ProzessorKomponenten;
    }

    override protected void starteTestCoroutine(TestCoroutine Coroutine)
    {
      CCoroutineStarter.CoroutineFunktion Funktion = new CCoroutineStarter.CoroutineFunktion(Coroutine);
      CCoroutineStarter Starter = new CCoroutineStarter(Funktion, 10);
      Starter.starte();
    }
  }
}
