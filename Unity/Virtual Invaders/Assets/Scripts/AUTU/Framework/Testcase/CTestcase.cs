using System.Collections;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace AUTU
{
  class CTestcase : ITestcase
  {
    public string Name
    {
      get;
      private set;
    }

    public string VollerName
    {
      get { return GruppenName + "." + Name; }
    }

    public string GruppenName
    {
      get;
      private set;
    }

    private ITestStatus TestStatus = null;
    private ITestStatusAenderer StatusAenderer = null;

    public ITestStatus Status
    {
      get { return TestStatus; }
      private set { TestStatus = value; }
    }

    private delegate IEnumerator StartMethodenDelegate(IZusammenfassungsAenderer ZusammenfassungsAenderer, ILogger Logger, ITestOptionen Optionen, ITestProzessorSchreiber Prozessor);
    private MethodInfo            MethodenInformation = null;
    private ITestGruppenUmgebung  TestUmgebung = null;
    private StartMethodenDelegate StartMethode = null;
    private ITestOptionen         Optionen = null;

    private IEnumerator coroutineWrapper()
    {
      UnityEngine.Debug.Assert(MethodenInformation != null);
      UnityEngine.Debug.Assert(TestUmgebung != null);

      yield return (IEnumerator)MethodenInformation.Invoke(TestUmgebung, null);
    }

    private IEnumerator starteTestMethode(IZusammenfassungsAenderer ZusammenfassungsAenderer, ILogger Logger, ITestOptionen Optionen, ITestProzessorSchreiber Prozessor)
    {
      UnityEngine.Debug.Assert(TestUmgebung != null);
      UnityEngine.Debug.Assert(StatusAenderer != null);

      if (!Status.IgnoriereTest)
      {
        // setze Optionen vom Logger
        Logger.ConsoleEin = Optionen.KonsolenLogging;

        bool SollTestLaufen = TestUmgebung.preTest(new CPruefer(StatusAenderer, Logger, Optionen.AbbruchBeiFehler), Logger, Prozessor);

        if (SollTestLaufen)
        {
          StatusAenderer.loescheErgebnis();
          StatusAenderer.addiereDurchlaeufe();

          yield return Prozessor.warteBisFertig();

          CExceptionCoroutine Coroutine = new CExceptionCoroutine(coroutineWrapper);
          yield return Coroutine.starte();

          yield return Prozessor.warteBisFertig();

          if (Coroutine.IstEineExeptionAufgetreten)
          {
            StatusAenderer.addiereFataleFehler(1);

            if (Coroutine.AufgetreteneException is FehlerAbbruchWegenFehler)
            {
              Logger.LogError("Breche Testlauf wegen einem Fehler vorzeitig ab.");
              throw Coroutine.AufgetreteneException;
            }
            else
            {
              Logger.ExceptionIstAufgetreten("Testcase " + VollerName + " musste wegen einer Exception abgebrochen werden", Coroutine.AufgetreteneException, new StackTrace(2, true));
              if (Optionen.AbbruchBeiFehler)
              {
                throw new FehlerAbbruchWegenFehler();
              }
            }
          }

          ZusammenfassungsAenderer.addiereStatus(Status);
        }

        TestUmgebung.postTest(SollTestLaufen);
      }
      else
      {
        ZusammenfassungsAenderer.addiereStatus(Status);
        Logger.LogWarning("Ignoriere Testcase: "+VollerName);
      }

    }


    public CTestcase(string Name, string GruppenName, MethodInfo MethodenInformation, ITestGruppenUmgebung TestUmgebung, ITestStatus Status, ITestStatusAenderer StatusAenderer, ITestOptionen Optionen)
    {
      this.Name = Name;
      this.GruppenName = GruppenName;
      this.MethodenInformation = MethodenInformation;
      this.TestUmgebung = TestUmgebung;
      this.Status = Status;
      this.StatusAenderer = StatusAenderer;
      StartMethode = starteTestMethode;
      this.Optionen = Optionen;
    }

    public CTestcase(ITestcase AndererTest, string GruppenName, ITestOptionen Optionen)
    {
      Name = AndererTest.Name;
      this.GruppenName = GruppenName+"."+AndererTest.GruppenName;
      StartMethode = AndererTest.starte;
      Status = AndererTest.Status;
      StatusAenderer = null;
      this.Optionen = Optionen;
    }

    public IEnumerator starte(IZusammenfassungsAenderer ZusammenfassungsAenderer, ILogger Logger, ITestOptionen Optionen, ITestProzessorSchreiber Prozessor)
    {
      UnityEngine.Debug.Assert(StartMethode != null);

      yield return StartMethode(ZusammenfassungsAenderer, Logger, this.Optionen.merge(Optionen), Prozessor);
    }
  }
}
