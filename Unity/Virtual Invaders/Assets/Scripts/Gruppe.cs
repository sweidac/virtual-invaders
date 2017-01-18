using System;
using System.Linq;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Gruppe : MonoBehaviour, IGegnerGruppe
    {
        #region UNITY PARAMETER

        /// <summary>
        /// Alle Gegner bzw. Gegnergruppen Prefabs, die spawnen sollen</summary>
        public GameObject[] GegnerTypen;

        /// <summary>
        /// Geschwindigkeit von Gruppe auf Spieler zu</summary>
        public float Geschwindigkeit;

        /// <summary>
        /// Punkt vorm Gegner an dem sich die Gruppe auflöst</summary>
        public float Aufloesungspunkt;

        /// <summary>
        /// Höhenvarianz zum spawnen</summary>
        [Range(1, 45)] public int MaxGruppenGroesse;

        /// <summary>
        /// Höhenvarianz zum spawnen</summary>
        [Range(0, 10)] public int GeschwindigkeitSteigerung;

        #endregion

        private int _derzeitigeReihenLaenge = 1;
        private int _derzeitigeReihenAnzahl = 1;
        private int _reihenbrecher = 5;

        private int _initialeGruppenGroesse;

        // Use this for initialization
        void Awake()
        {
            if (GegnerTypen.Length <= 0 || MaxGruppenGroesse < 1)
            {
                Destroy(gameObject);
            }

            _initialeGruppenGroesse = Mathf.CeilToInt(MaxGruppenGroesse * Spielsteuerung.Instance.Schwierigkeitsgrad);

            if (_initialeGruppenGroesse < 1)
            {
                _initialeGruppenGroesse = 1;
            }


            while (_initialeGruppenGroesse > _derzeitigeReihenAnzahl * _derzeitigeReihenLaenge)
            {
                if (_derzeitigeReihenLaenge < _reihenbrecher)
                {
                    _derzeitigeReihenLaenge++;
                }
                else
                {
                    _reihenbrecher = Mathf.CeilToInt(_reihenbrecher + _reihenbrecher * 0.2f);
                    _derzeitigeReihenAnzahl++;
                    _derzeitigeReihenLaenge = Mathf.CeilToInt(_derzeitigeReihenLaenge * 0.6f);
                }
            }

            // Spawn whole Group
            for (var i = +(_derzeitigeReihenAnzahl / 2.0f); i > -_derzeitigeReihenAnzahl / 2.0f; i--)
            {
                for (var j = -(_derzeitigeReihenLaenge / 2.0f); j < _derzeitigeReihenLaenge / 2.0f; j++)
                {
                    var spawnPosition = new Vector3(j * 3, i * 2, i * 3);

                    GameObject gegner =
                        (GameObject)
                        Instantiate(GegnerTypen[Random.Range(0, GegnerTypen.Length)], spawnPosition, Quaternion.identity,
                            transform);


                    gegner.GetComponent<Gegner>().StartShooting();
                }
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            CheckGruppe();
            BewegeGruppe();
        }

        private void BewegeGruppe()
        {
            int aktuelleGruppenGroeße = GetComponentsInChildren<Gegner>().Length;
            float neueGeschwindigkeit = Geschwindigkeit *
                                        (1 + GeschwindigkeitSteigerung *
                                         (_initialeGruppenGroesse - aktuelleGruppenGroeße) / _initialeGruppenGroesse);

            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position, Camera.main.transform.position, neueGeschwindigkeit / 100);

            gameObject.transform.LookAt(Camera.main.transform);

            //Lasse Gruppe vorm Spieler frei
            if (gameObject.transform.position.magnitude < Aufloesungspunkt)
            {
                gameObject.transform.DetachChildren();
            }
        }

        private void CheckGruppe()
        {
            //Kontrolle ob Gruppe besiegt
            if (!GetComponentsInChildren<Gegner>().Any())
            {
                Destroy(gameObject);
            }
        }

    #region IGegnerGruppe

    public GameObject[] TypenVonGegnern
    {
      get { return GegnerTypen; }
    }

    public GameObject Objekt
    {
      get { return gameObject; }
    }

    #endregion


  }
}