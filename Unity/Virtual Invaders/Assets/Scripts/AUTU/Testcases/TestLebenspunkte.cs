using UnityEngine;
using AUTU;
using System.Collections;
using Assets.Scripts;
using AUTU.Befehle;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

public class TestLebenspunkte : CTestumgebungSpiel
{
  [Test]
  public IEnumerator TesteAnzahlLebenspunkte()
  {
    ISpielsteuerung SpielSteuerungsObjekt = Spielsteuerung.Instance;
    Debug.Assert(SpielSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
    Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte keine Spielsteuerung-Komponente gefunden werden.");

    GegnerSteuerungsObjekt.KomponenteAktiv = true;
    GegnerSteuerungsObjekt.ProjektileAktiv = false;

    yield return starteSpiel(SpielSteuerungsObjekt);

    Pruefer.istGleich(SpielSteuerungsObjekt.InitialeLebenspunkte, SpielSteuerungsObjekt.Hitpoints, 0, "Die Anzahl der Lebenspunkte stimmt nicht mit den Einstellungen überein.");

    SpielSteuerungsObjekt.PlayerHit();

    Pruefer.istGleich(SpielSteuerungsObjekt.InitialeLebenspunkte - 1, SpielSteuerungsObjekt.Hitpoints, 0, "Die Anzahl der Lebenspunkte wird nicht korrekt reduziert.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return starteSpiel(SpielSteuerungsObjekt);

    Pruefer.istGleich(SpielSteuerungsObjekt.InitialeLebenspunkte, SpielSteuerungsObjekt.Hitpoints, 0, "Die Anzahl der Lebenspunkte stimmt nicht mit den Einstellungen überein.");

    SpielSteuerungsObjekt.PlayerHit();

    Pruefer.istGleich(SpielSteuerungsObjekt.InitialeLebenspunkte - 1, SpielSteuerungsObjekt.Hitpoints, 0, "Die Anzahl der Lebenspunkte wird nicht korrekt reduziert.");

    yield return stoppeSpiel(SpielSteuerungsObjekt);

    yield return true;
  }

}