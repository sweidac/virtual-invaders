using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

public class TestLaser : CTestumgebungSpiel
{
  public GameObject Laser = null;

  [Test]
  public IEnumerator LaserWirdAusgeschaltetBeiBlickNachUnten()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    Debug.Assert(Laser != null, "Es muss dem Testcase ein Laser-Objekt hinzugefügt werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = true;

    yield return starteSpiel(SpielSteuerungsObjekt);

    Pruefer.istTrue(Laser.activeSelf == true, "Laser ist nicht eingeschaltet beim Spielstart.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(85, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istTrue(Laser.activeSelf == false, "Laser ist nicht ausgeschaltet wenn nach unten geguckt wird.");

    Prozessor.add(new CDreheKopfBisWinkelErreicht(new CRotatiosWert(Kamera), new CKonstanterVektor(0, 0, 0)));
    Prozessor.add(new CWarteBisWertKonstantIst(new CRotatiosWert(Kamera)));

    yield return Prozessor.warteBisFertig();

    Pruefer.istTrue(Laser.activeSelf == true, "Laser wird nach einem Blick nach unten nicht mehr eingeschaltet.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    Pruefer.istTrue(Laser.activeSelf == false, "Laser ist nicht ausgeschaltet nach Spielende.");
  }
}