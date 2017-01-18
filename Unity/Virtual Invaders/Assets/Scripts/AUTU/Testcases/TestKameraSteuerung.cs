using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;

public class TestKameraSteuerung : CTestGruppe
{
  public Transform Kamera = null;

  [Test]
  public IEnumerator BewegeKopfRunter()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht( new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(45, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istKleiner(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.x, KameraRotation.x));
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    Prozessor.add(new CDreheKopf(new Vector3(-45, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 1.0f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    yield return true;
  }

  [Test]
  public IEnumerator BewegeKopfHoch()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(-45, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGroesser(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.x, KameraRotation.x));
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    Prozessor.add(new CDreheKopf(new Vector3(45, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 1.0f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    yield return true;
  }

  [Test]
  public IEnumerator BewegeKopfNachLinks()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(0, -90, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istGroesser(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.y, KameraRotation.y));
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    Prozessor.add(new CDreheKopf(new Vector3(0, 90, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 1.0f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    yield return true;
  }

  [Test]
  public IEnumerator BewegeKopfNachRechts()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(0, 90, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istKleiner(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.y, KameraRotation.y));
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    Prozessor.add(new CDreheKopf(new Vector3(0, -90, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 1.0f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 0.1f);

    yield return true;
  }

  [Test]
  public IEnumerator RotiereKopfNachLinks()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(0, 0, -60)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istGroesser(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.z, KameraRotation.z));

    Prozessor.add(new CDreheKopf(new Vector3(0, 0, 60)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 1.0f);

    yield return true;
  }

  [Test]
  public IEnumerator RotiereKopfNachRechts()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(0, 0, 60)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istKleiner(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.z, KameraRotation.z));

    Prozessor.add(new CDreheKopf(new Vector3(0, 0, -60)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 0.1f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 0.1f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 1.0f);

    yield return true;
  }

  [Test]
  public IEnumerator KopfbewegungSchraekRechtsOben()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(-30, 70, -40)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGroesser(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.x, KameraRotation.x));
    Pruefer.istKleiner(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.y, KameraRotation.y));
    Pruefer.istGroesser(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.z, KameraRotation.z));

    Prozessor.add(new CDreheKopf(new Vector3(30, -70, 40)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 1.0f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 1.0f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 1.0f);

    yield return true;
  }

  [Test]
  public IEnumerator KopfbewegungSchraekLinksOben()
  {
    Debug.Assert(Kamera != null, "Es ist kein Kamera Objekt mit dem Testcase verbunden.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Vector3 KameraRotation = Kamera.rotation.eulerAngles;

    Prozessor.add(new CDreheKopf(new Vector3(-30, -70, 40)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGroesser(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.x, KameraRotation.x));
    Pruefer.istGroesser(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.y, KameraRotation.y));
    Pruefer.istKleiner(0, Mathf.DeltaAngle(Kamera.rotation.eulerAngles.z, KameraRotation.z));

    Prozessor.add(new CDreheKopf(new Vector3(30, 70, -40)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istGleich(KameraRotation.x, Kamera.rotation.eulerAngles.x, 1.0f);
    Pruefer.istGleich(KameraRotation.y, Kamera.rotation.eulerAngles.y, 1.0f);
    Pruefer.istGleich(KameraRotation.z, Kamera.rotation.eulerAngles.z, 1.0f);

    yield return true;
  }
}