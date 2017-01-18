using System.Collections;
using System.Linq;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public sealed class Spielsteuerung : MonoBehaviour, ISpielsteuerung
    {
        private static volatile Spielsteuerung instance = null;
        private static volatile Sichtsteuerung sichtCtrl = Sichtsteuerung.Instance;
        private static volatile Schusssteuerung schussCtrl = Schusssteuerung.Instance;
        public AudioSteuerung soundSteuerung = null;
        private static Menue _menue;

        #region UNITY PARAMETER

        public Text scoreText;
        public Text hitpointsText;
        public Canvas addScore;

        public SpriteRenderer SchadensIndikator;

        public Transform laserStart;
        public int laserLength;

        public GameObject funkenPartikel;

        public GameObject Hud;
        public GameObject GvrRecticle;
        public GameObject Laser;

        public int initialeLebenspunkte = 100;
        [Range(1, 10000)] public float laufzeitBisMaxSchwierigkeit;

        public int ufoMinSekunden = 5;
        public int ufoMaxSekunden = 10;

        public int ufoRadius = 50;

        public int ufoHoehe = 50;

        public GameObject ufoPrefab;

        public GameObject endScreen;
        [Range(2, 10)] public int endScreenDauer;

        #endregion

        private bool gameover = true;
        private int hitpoints;
        private int score;
        private float schwierigkeitsgrad;

        private float schadenAnimationAlpha = 0.0f;


        /// <summary>
        /// Schwierigkeitsgrad zwischen 0 und 1
        /// 0 Niedrig
        /// 0.5 Mittel
        /// 1 Maximal
        /// </summary>
        public float Schwierigkeitsgrad
        {
            get { return schwierigkeitsgrad; }
            set
            {
                schwierigkeitsgrad = Mathf.Clamp(value, 0.0f, 1.0f);
            }
        }

        public int Score
        {
            get { return score; }
        }


        /// <summary>
        /// Gibt die Singleton Instanz zurück.
        /// </summary>
        public static Spielsteuerung Instance
        {
            get { return instance; }
        }

        public bool Gameover
        {
            get { return gameover; }
        }


        void Awake()
        {
            instance = this;
            Logger.OpenFile();
        }

        void OnDestroy()
        {
            Destroy(gameObject);
            instance = null;

            Logger.CloseFile();
        }

        void Start()
        {
            _menue = Menue.Instance;
        }

        public void OnDrawGizmos()
        {
            schussCtrl.OnDrawGizmos(laserStart, laserLength);
        }

        void Update()
        {
            UpdateSchwierigkeitsgrad();
        }

        void FixedUpdate()
        {
            if (!gameover)
            {
                schussCtrl.HitDetection(sichtCtrl.ViewDirection());
            }
        }

        /// <summary>
        /// Spieler wurde getroffen.
        /// </summary>
        public void PlayerHit()
        {
            if (hitpoints > 1)
            {
                SetHitpoints(hitpoints - 1);
                this.soundSteuerung.starteSpielerTrefferSound();
                if (schadenAnimationAlpha < 0.01f)
                {
                    // Starte Animation
                    schadenAnimationAlpha = 0.5f;
                    StartCoroutine(SchadenAnimation());
                }
                else
                {
                    // Setze Animation zurück
                    schadenAnimationAlpha = 0.5f;
                }
            }
            else
            {
                SpielStopp();
            }
        }

        IEnumerator SchadenAnimation()
        {
            while (schadenAnimationAlpha > 0.01f)
            {
                schadenAnimationAlpha -= 0.02f;
                SchadensIndikator.color = new Color(1.0f, 1.0f, 1.0f, schadenAnimationAlpha);
                yield return new WaitForSeconds(0.02f);
            }
            schadenAnimationAlpha = 0.0f;
            SchadensIndikator.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        /// <summary>
        /// Spieler hat einen Punkt erhalten
        /// </summary>
        public void PlayerScored()
        {
            SetScore(score + 1);
        }

        /// <summary>
        /// Spieler hat n Punkte erhalten
        /// </summary>
        public void PlayerScored(int n)
        {
            SetScore(score + n);
        }

        /// <summary>
        /// Stellt die den Schwierigkeitsgrad in Abhängigkeit von der Zeit ein
        /// </summary>
        public void UpdateSchwierigkeitsgrad()
        {
            Schwierigkeitsgrad = Schwierigkeitsgrad + (Time.deltaTime / laufzeitBisMaxSchwierigkeit);
        }

        private void SetHitpoints(int value)
        {
            hitpoints = value;
            hitpointsText.text = hitpoints.ToString();
        }

        private void SetScore(int value)
        {
            score = value;
            scoreText.text = score.ToString();
        }

        /// <summary>
        /// Started das InGame Spiel
        /// </summary>
        public void SpielStart()
        {
            SetScore(0);
            SetHitpoints(initialeLebenspunkte);
            Schwierigkeitsgrad = 0;
            gameover = false;
            Hud.SetActive(true);
            GegnerSteuerung.Instance.StartSpawn();
			AudioSteuerung.Instance.starteSpiel ();

            GvrRecticle.SetActive(false);
            Laser.SetActive(true);

            schussCtrl.Start(laserStart, laserLength*2, funkenPartikel, addScore);

            StartCoroutine(SpawneUFOLoop());
        }

        /// <summary>
        /// Stoppt das InGame Spiel,
        /// soweit nötig
        /// </summary>
        public void SpielStopp()
        {
            gameover = true;

            Hud.SetActive(false);
            schussCtrl.Stop();
            Laser.SetActive(false);
            AudioSteuerung.Instance.beendeSpiel();

            StartCoroutine(EndScreenTransition());
            StopCoroutine(SpawneUFOLoop());
        }

        public IEnumerator EndScreenTransition()
        {
            endScreen.SetActive(true);
            yield return new WaitForSeconds(endScreenDauer);
            endScreen.SetActive(false);

            _menue.StartMenue();
            GvrRecticle.SetActive(true);
        }

        public IEnumerator SpawneUFOLoop()
        {

            while (!gameover)
            {
                int diff = this.ufoMaxSekunden - this.ufoMinSekunden;
                int offset = Random.Range(0, diff);
                int spawnTime = this.ufoMinSekunden + offset;


                float lastSpawn = Time.time;
                yield return new WaitUntil(() => Gameover || Time.time >= lastSpawn + spawnTime);


                if (!gameover)
                    SpawneUFO();
            }
        }

        public void SpawneUFO()
        {

            if(ufoPrefab == null)
                return;

            int rot = Random.Range(0, 360);
            float x = Mathf.Cos(rot) * this.ufoRadius;
            float z = Mathf.Sin(rot) * this.ufoRadius;

            Debug.Log("Spawn ufo at " + x + " "  + z);

            Vector3 pos = new Vector3(x, this.ufoHoehe, z);

            GameObject ufo = Instantiate(ufoPrefab, pos, new Quaternion()) as GameObject;
            if (ufo == null)
                return;


            ufo.transform.LookAt(new Vector3(0, this.ufoHoehe, 0));

        }

    // Zusätzliche Methoden für das Test-Interface
    #region ISpielsteuerung

    public int Hitpoints
    {
      get
      {
        return hitpoints;
      }

      set
      {
        SetHitpoints(value);
      }
    }

    public int InitialeLebenspunkte
    {
      get { return initialeLebenspunkte; }
    }

    public float ZeitBisMaximalenSchwierigkeitsgrad
    {
      get { return laufzeitBisMaxSchwierigkeit; }
    }

    public float MaximaleWartezeitAufUFO
    {
      get { return ufoMaxSekunden; }
    }

    float AlteGameOverWarteZeit = 0f;

    public float GameOverWarteZeit
    {
      set
      {
        KomponenteAktiv = true;
        endScreenDauer = (int)value;
      }

      get
      {
        return endScreenDauer;
      }

    }

    private bool IstKomponenteAktiv = false;

    public bool KomponenteAktiv
    {
      get
      {
        return IstKomponenteAktiv;
      }

      set
      {
        if (value)
        {
          AlteGameOverWarteZeit = endScreenDauer;
        }
        else if (IstKomponenteAktiv)
        {
          endScreenDauer = (int)AlteGameOverWarteZeit;
        }

        IstKomponenteAktiv = value;
      }
    }

    #endregion
  }
}