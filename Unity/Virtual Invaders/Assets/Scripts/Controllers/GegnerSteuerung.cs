using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controllers
{
    /// <summary>
    /// Die Gegnersteuerung kümmert sich um die Koordnination aller Gegner bzw. Gegnergruppen.
    /// </summary>
    public class GegnerSteuerung : MonoBehaviour, IGegnerSteuerung
    {
        #region UNITY PARAMETER

        /// <summary>
        /// Die aktuelle Kamera</summary>
        public Camera Camera;

        /// <summary>
        /// Alle Gegner bzw. Gegnergruppen Prefabs, die spawnen sollen</summary>
        public GameObject[] GegnerTypen;

        /// <summary>
        /// Bild von Mutterschiff</summary>
        public GameObject Mutterschiff;

        /// <summary>
        /// Marker für Spawn außerhalb Blickfeld links</summary>
        public GameObject MarkierungLinks;

        /// <summary>
        /// Marker für Spawn außerhalb Blickfeld rechts</summary>
        public GameObject MarkierungRechts;

        /// <summary>
        /// Verzögerung bis Spiel start</summary>
        [Range(0.0f, 100.0f)] public float StartWartezeit;

        /// <summary>
        /// Verzögerung zwischen dem autom. spawnen</summary>
        [Range(1.0f, 100.0f)] public float StartMaxSpawnWartezeit;

        /// <summary>
        /// Verzögerung zwischen dem autom. spawnen</summary>
        [Range(1.0f, 100.0f)] public float MinSpawnWartezeit;

        /// <summary>
        /// Chance zum spawnen unabhängig von der Blickrichtung</summary>
        [Range(0.0f, 1.0f)] public float ZufaelligeSpawnPositionsChance;

        /// <summary>
        /// Entfernung zum spawnen</summary>
        [Range(0.0f, 100.0f)] public float SpawnEntfernung;

        /// <summary>
        /// Höhenvarianz zum spawnen</summary>
        [Range(0.0f, 100.0f)] public float SpawnHoehenVarianz;

        #endregion


        private object LOCK = new object();

        private static GegnerSteuerung _instance = null;
        private Spielsteuerung _spielsteuerung;
        private int _derzeitigeGegnerAnzahl = 0;
        private bool _sofortSpawnen = false;

        private const float MutterschiffDistance = 10f;
        private const float MutterschiffAnimDauer = 5;

        private const float GegnerSichtWinkel = 45;
        private List<Vector3> SpawnPositionenNichtSichtbar = new List<Vector3>();

        /// <summary>
        /// Gibt die Singleton Instanz zurück.
        /// </summary>
        public static GegnerSteuerung Instance
        {
            get { return _instance; }
        }

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            _spielsteuerung = Spielsteuerung.Instance;
        }

        void Update()
        {
        }

        /// <summary>
        /// Gegner meldet sich an der Steuerung an.
        ///  </summary>
        public void RegistriereGegner()
        {
            lock (LOCK)
            {
                _derzeitigeGegnerAnzahl++;
            }
        }

        /// <summary>
        /// Starte spawnen von Gegnern.
        ///  </summary>
        public void StartSpawn()
        {
            StartCoroutine(Spawn());
            StartCoroutine(AngriffStoppAutomatik());
            StartCoroutine(SpawnHinweis());
        }

        /// <summary>
        /// Entfernt zerstörten Gegner.
        ///  </summary>
        public void EntferneGegner()
        {
            lock (LOCK)
            {
                _derzeitigeGegnerAnzahl = _derzeitigeGegnerAnzahl > 0
                    ? _derzeitigeGegnerAnzahl - 1
                    : _derzeitigeGegnerAnzahl;
            }

            if (_derzeitigeGegnerAnzahl == 0)
            {
                _sofortSpawnen = true;
            }
        }

        private IEnumerator AngriffStoppAutomatik()
        {
            yield return new WaitUntil(() => _spielsteuerung.Gameover);

            StopAllCoroutines();

            EntferneAlleGegner();

            SpawnhinweisZurücksetzen();
        }

        private void SpawnhinweisZurücksetzen()
        {
            lock (SpawnPositionenNichtSichtbar)
            {
                SpawnPositionenNichtSichtbar.Clear();
            }
            //Deaktivere Marker bei SpielEnde
            MarkierungLinks.SetActive(false);
            MarkierungRechts.SetActive(false);
        }

        private void EntferneAlleGegner()
        {
            //Alle Mutterschiffe entfernen bei GameOver
            GameObject[] mothership = GameObject.FindGameObjectsWithTag("Mothership");
            for (var i = mothership.Length - 1; i >= 0; i--)
            {
                Destroy(mothership[i]);
            }

            //Alle Gegner entfernen bei GameOver
            GameObject[] gegner = GameObject.FindGameObjectsWithTag("Enemy");
            for (var i = gegner.Length - 1; i >= 0; i--)
            {
                Destroy(gegner[i]);
            }

            //Alle Projektile entfernen bei GameOver
            GameObject[] projektile = GameObject.FindGameObjectsWithTag("EnemyProjectile");
            for (var i = projektile.Length - 1; i >= 0; i--)
            {
                Destroy(projektile[i]);
            }

            lock (LOCK)
            {
                _derzeitigeGegnerAnzahl = 0;
            }
        }

        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(StartWartezeit);

            while (!_spielsteuerung.Gameover)
            {
                // Spawne Gegner nur, falls diese Klasse nicht als Test-Komponente verwendet wird
                if (!KomponenteAktiv)
                {
                    StartCoroutine(SpawnGegner());
                }

                // Warten
                float lastSpawn = Time.time;
                float nextSpawn = Mathf.Lerp(StartMaxSpawnWartezeit, MinSpawnWartezeit,
                    _spielsteuerung.Schwierigkeitsgrad);

                Debug.Log("Spawn Time:" + nextSpawn + "  Mit Grad:" + _spielsteuerung.Schwierigkeitsgrad);
                yield return
                    new WaitUntil(
                        () => _spielsteuerung.Gameover || _sofortSpawnen || Time.time >= lastSpawn + nextSpawn);
                _sofortSpawnen = false;
            }
        }

        private IEnumerator SpawnGegner()
        {
            if (GegnerTypen.Length <= 0) yield break;

            // Berechne Spawn-Position
            Vector3 cameraDirection = Camera.transform.forward;

            var spawnPosition = SpawnPosition(cameraDirection);

            //Aktiviere Hinweis wenn außerhalb vom Sichtbereich
            if (Vector3.Angle(Camera.main.transform.forward, spawnPosition) > GegnerSichtWinkel)
                lock (SpawnPositionenNichtSichtbar)
                    SpawnPositionenNichtSichtbar.Add(spawnPosition);

            GameObject mutterschiff = Instantiate(Mutterschiff);
            mutterschiff.transform.Translate(spawnPosition * MutterschiffDistance);
            mutterschiff.transform.LookAt(Camera.transform.position);

            float startTime = Time.time;

            //Mutterschiff Anflug
            while (startTime + MutterschiffAnimDauer > Time.time)
            {
                mutterschiff.transform.position = Vector3.Lerp(
                    spawnPosition * MutterschiffDistance, spawnPosition, (Time.time - startTime) / MutterschiffAnimDauer);
                yield return new WaitForEndOfFrame();
            }

            mutterschiff.GetComponentInChildren<Mutterschiff>().spieleSpawnSound();

            ParticleSystem system = mutterschiff.GetComponent<ParticleSystem>();
            if (system != null) system.Play();

            yield return new WaitForSeconds(2);

            SpawneGegner(GegnerTypen[Random.Range(0, GegnerTypen.Length)], spawnPosition);

            if (system != null) system.Stop();

            yield return new WaitForSeconds(3);

            startTime = Time.time;

            //Mutterschiff Abflug
            while (startTime + MutterschiffAnimDauer > Time.time)
            {
                mutterschiff.transform.position = Vector3.Lerp(
                    spawnPosition, spawnPosition * MutterschiffDistance, (Time.time - startTime) / MutterschiffAnimDauer);
                yield return new WaitForEndOfFrame();
            }

            Destroy(mutterschiff);
        }

        /// <summary>
        /// Spawnt einen Gegner an einer spezifischen Position
        /// <value>PreFab zum erstellen</value>
        /// <value>Positon an dem der Gegner erscheint</value>
        ///  </summary>
        public GameObject SpawneGegner(GameObject PreFab, Vector3 Position)
        {
            Debug.Assert(PreFab != null, "Es ist kein Gegner-PreFab angegeben worden.");

            GameObject gegner = Instantiate(PreFab);

            gegner.transform.Translate(Position);
            gegner.transform.LookAt(Camera.transform.position);

            return gegner;
        }

        private IEnumerator SpawnHinweis()
        {
            bool linksAktiv;
            bool rechtsAktiv;

            do
            {
                linksAktiv = false;
                rechtsAktiv = false;

                lock (SpawnPositionenNichtSichtbar)
                {
                    //Lösche alle sichtbaren Positionen
                    SpawnPositionenNichtSichtbar.RemoveAll(pos =>
                            Vector3.Angle(Camera.main.transform.forward, pos) < GegnerSichtWinkel);

                    //Aktiviere passende Marker
                    foreach (Vector3 pos in SpawnPositionenNichtSichtbar)
                    {
                        if (Vector3.Dot(Camera.main.transform.right, pos) > 0)
                        {
                            if (!rechtsAktiv) rechtsAktiv = true;
                        }
                        else
                        {
                            if (!linksAktiv) linksAktiv = true;
                        }
                    }
                }


                MarkierungLinks.SetActive(linksAktiv && !MarkierungLinks.activeSelf);
                MarkierungRechts.SetActive(rechtsAktiv && !MarkierungRechts.activeSelf);

                yield return new WaitForSeconds(0.5f); //Blink Rhythmus 0.5s
            } while (!Spielsteuerung.Instance.Gameover);
        }

        private Vector3 SpawnPosition(Vector3 cameraDirection)
        {
            Vector3 spawnPosition;

            if (Random.Range(0.0f, 1.0f) >= ZufaelligeSpawnPositionsChance)
            {
                spawnPosition = new Vector3(
                                    Random.Range(cameraDirection.x - 0.5f, cameraDirection.x + 0.5f),
                                    0,
                                    Random.Range(cameraDirection.z - 0.5f, cameraDirection.z + 0.5f)).normalized *
                                SpawnEntfernung;
            }
            else
            {
                spawnPosition = new Vector3(
                                    Random.Range(-1.0f, 1.0f),
                                    0,
                                    Random.Range(-1.0f, 1.0f)).normalized * SpawnEntfernung;
            }
            spawnPosition.y = Random.Range(0, SpawnHoehenVarianz);

            // Position höher setzen, wenn Gebäude im Weg
            while (Physics.SphereCast(
                new Ray(spawnPosition * MutterschiffDistance, -spawnPosition),
                15,
                Vector3.Distance(spawnPosition, spawnPosition * MutterschiffDistance)))
            {
                spawnPosition.y += 10;
            }

            return spawnPosition;
        }

        #region IGegnerSteuerung

        private bool IstKomponenteAktiv = false;
        private bool SindProjektileAktiv = true;

        public bool KomponenteAktiv
        {
            get { return IstKomponenteAktiv; }

            set { IstKomponenteAktiv = value; }
        }

        public ReadOnlyCollection<GameObject> GegnerPreFabs
        {
            get { return GegnerTypen.ToList().AsReadOnly(); }
        }

        public bool ProjektileAktiv
        {
            get
            {
                // gibt true zurück, wenn die Test-Komponente nicht aktiv ist (also keine Tests laufen) oder wenn Projektile aktiviert wurden (im Test)
                return !IstKomponenteAktiv || SindProjektileAktiv;
            }

            set { SindProjektileAktiv = value; }
        }

        public float MaximaleMutterschiffSpawnzeit
        {
            get { return StartMaxSpawnWartezeit; }
        }

        #endregion
    }
}