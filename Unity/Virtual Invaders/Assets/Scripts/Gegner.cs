using UnityEngine;
using Assets.Scripts.Controllers;
using UnityEngine.Audio;

namespace Assets.Scripts
{
    public class Gegner : MonoBehaviour, IGegner
    {
        #region UNITY PARAMETER

        /// <summary>
        /// Geschwindigkeit vom Gegner auf Spieler zu</summary>
        [Range(0, 100)]public float Speed;
        /// <summary>
        /// Schadenspunkte des Gegners</summary>
        [Range(0, float.MaxValue)]public float Hitpoints;
        /// <summary>
        /// Prefab vom Projektil</summary>
        public GameObject ProjectilePrefab;

        /// <summary>
        /// Schussverzögerung</summary>
        public int ShotDelay = 20;

        public AudioClip destructionClip;
        public AudioMixerGroup audioOutput;


        #endregion

        private GegnerSteuerung _gegnerSteuerung;
        private Rigidbody _rigidbody;

        void Start()
        {
            _gegnerSteuerung = GegnerSteuerung.Instance;
            _gegnerSteuerung.RegistriereGegner();

            _rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            //Bewegung zum Spieler nur wenn nicht in Gruppe
            if (GetComponentInParent<Gruppe>() == null)
            {
                Vector3 ziel = Vector3.MoveTowards(
                    gameObject.transform.position, Camera.main.transform.position, Speed);

                _rigidbody.velocity = (ziel - gameObject.transform.position).normalized * Speed;

            }
            //Zum Spieler schauen
            gameObject.transform.transform.LookAt(Camera.main.transform);

            // Test auf Kollision mit dem Spieler
            if (gameObject.transform.position.magnitude < 2.5f)
            {
                // Spieler wird getroffen
                Spielsteuerung.Instance.PlayerHit();
                DestroyEnemy();
            }
        }

        /// <summary>
        /// Fügt dem Gegner Schaden zu.
        /// </summary>
        public void DealDamage()
        {
            Hitpoints--;

            // Test auf Zerstörung
            if (Hitpoints <= 0)
            {
                Spielsteuerung.Instance.PlayerScored();
                DestroyEnemy();
            }

        }
        /// <summary>
        /// Zerstört Gegner
        /// </summary>
        public void DestroyEnemy()
        {
            _gegnerSteuerung.EntferneGegner();
            Destroy(gameObject);
            PlayDestruction();
        }
        /// <summary>
        /// Ist Gegner noch am Leben
        /// </summary>
        public bool IsAlive()
        {
            return Hitpoints > 0;
        }
        /// <summary>
        /// Gegner fängt an zu schießen.
        /// </summary>
        public void StartShooting()
        {
            InvokeRepeating("Shoot", Random.Range(0, ShotDelay), ShotDelay);
        }
        /// <summary>
        /// Gegner hört auf zu schießen.
        /// </summary>
        public void StopShooting()
        {
            CancelInvoke("Shoot");
        }

        private void Shoot()
        {
            IGegnerSteuerung GegnerSteuerungsObjekt = GegnerSteuerung.Instance;
            Debug.Assert(GegnerSteuerungsObjekt != null, "Es konnte kein GegnerSteuerung-Objekt gefunden werden.");

            if (GegnerSteuerungsObjekt.ProjektileAktiv)
            {
                if(transform.position.magnitude < 5) StopShooting();

                ErstelleProjektil();
            }
        }

        public GameObject ErstelleProjektil()
        {
          if (ProjectilePrefab != null)
          {

            RaycastHit hit;
            if (Physics.Raycast(new Ray(transform.position + transform.forward.normalized * 0.1f, transform.forward), out hit) &&
                hit.collider.gameObject.tag != "MainCamera")
            {
              //Debug.Log("Kein Sichtkontakt zum Spieler sondern zu Objekt: "+ hit.collider.gameObject.name);
              return null;
            }

            GameObject projectile = Instantiate(ProjectilePrefab, transform.position, transform.rotation) as GameObject;
            if (projectile != null)
            {
                //Ignoriere Kollision bei diesem und bei allen anderen Gegnern
                Physics.IgnoreCollision(projectile.transform.GetComponent<Collider>(), GetComponent<Collider>());
                GameObject[] gegner = GameObject.FindGameObjectsWithTag("Enemy");
                for (var i = gegner.Length - 1; i >= 0; i--)
                {
                    Physics.IgnoreCollision(projectile.transform.GetComponent<Collider>(), gegner[i].GetComponent<Collider>());
                }
            }

            return projectile;
          }

          return null;
        }

        private void PlayDestruction()
        {
            if(destructionClip == null||audioOutput == null) return;

            AudioSource tempQuelle = Utility.PlayClipAt(transform.position, destructionClip, audioOutput);

            tempQuelle.Play();
        }

    // Testinterface IGegner
    #region IGegner

    public float HitPoints
    {
      get { return Hitpoints; }
    }

    public int SchussVerzoegerung
    {
      get { return ShotDelay; }
    }

    #endregion

  }
}

