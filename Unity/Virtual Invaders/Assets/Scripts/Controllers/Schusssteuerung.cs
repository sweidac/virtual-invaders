using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.Networking.Match;

public sealed class Schusssteuerung
{
    // Singleton Implementierung.
    // TODO: Muss vllt. ThreadSafe gemacht werden.
    private static Schusssteuerung _instance;
    public static Schusssteuerung Instance
    {
        get
        {
            if (_instance == null) _instance = new Schusssteuerung();
            return _instance;
        }
    }

    private Laser _laser;
    private GameObject hitEffect;
    private GameObject curHitEffect;
    private Canvas addScore;
    private bool hitEffectActive = false;
    private LineRenderer lr;
    private Transform startPoint;
    private float length;
    private Schusssteuerung()
    { }

    private float textureOffset = 0f;
    /// <summary>
    /// Erstellt und startet die Laser-Animation.
    /// </summary>
    /// <param name="prefab">Das Prefab für den Laser.</param>
    /// <param name="length">Die Laenge des Lasers.</param>
    /// <param name="hitPS">Funken Effet bei Treffer von Gegener.</param>
    /// <param name="score">Punkte die hinzugefügt werden.</param>
    public void Start(Transform sp, float l, GameObject hitPS, Canvas score)
    {
        hitEffect = hitPS;
        lr = sp.gameObject.GetComponent<LineRenderer>();
        length = l;
        addScore = score;
        startPoint = sp;
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 ep = rayOrigin + (Camera.main.transform.forward * length);
        lr.transform.rotation = Quaternion.LookRotation(ep - startPoint.position);
    }

    public void OnDrawGizmos(Transform t, float length)
    {
        Gizmos.color = Color.red;

        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 sp = t.position;
        Vector3 ep = rayOrigin + (Camera.main.transform.forward * length);
        Gizmos.DrawRay(sp, (ep - sp));
    }

    public void Update()
    {

    }

    /// <summary>
    /// Testet ob ein Gegner oder Projektil vom Laser getroffen wurde.
    /// </summary>
    public bool HitDetection(Ray ray)
    {
        RaycastHit hit;

        // disable laser if it is aimed too low
        if (ray.direction.y < -0.6)
        {
            if (lr.gameObject.activeInHierarchy)
            {
                lr.gameObject.SetActive(false);
                AudioSteuerung.Instance.stopLaserSound();
            }
        }
        else
        {
            if (!lr.gameObject.activeInHierarchy)
            {
                lr.gameObject.SetActive(true);
                AudioSteuerung.Instance.startLaserSound();
            }
        }

        // pan texture
        textureOffset -= Time.deltaTime*2f;
        if(textureOffset < -10f)
        {
            textureOffset += 10f;
        }
        lr.sharedMaterials[1].SetTextureOffset("_MainTex", new Vector2(textureOffset, 0f));

        if (Physics.Raycast(ray, out hit, length))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                // Gegner wird getroffen
                Gegner LastHit = hit.collider.gameObject.GetComponent<Gegner>();
                LastHit.DealDamage();
                lr.SetPosition(1, new Vector3(0, 0, hit.distance));

                if (!LastHit.IsAlive())
                {
                    Object.Instantiate(addScore, LastHit.transform.position,
                        Camera.main.transform.rotation);
                }
                else
                {
                    AudioSteuerung.Instance.laserTrefferModus(true);
                }

                if (!hitEffectActive)
                {
                    curHitEffect = Object.Instantiate(
                        hitEffect,
                        hit.point,
                        hit.transform.rotation) as GameObject;
                    hitEffectActive = true;
                }
                else
                {
                    curHitEffect.transform.position = hit.point;
                }
            }
            else if (hit.collider.gameObject.tag == "EnemyProjectile")
            {
                // Projektil wird getroffen
                GegnerProjektil projektil = hit.collider.gameObject.GetComponent<GegnerProjektil>();
                projektil.Treffer();
                AudioSteuerung.Instance.laserTrefferModus(true);
            }
            else if (hit.collider.gameObject.tag == "UFO")
            {
                // UFO wird getroffen
                UFO LastHit = hit.collider.gameObject.GetComponent<UFO>();
                LastHit.DealDamage();
                lr.SetPosition(1, new Vector3(0, 0, hit.distance));

                if (!LastHit.IsAlive())
                {
                    Object.Instantiate(addScore, LastHit.transform.position,
                        Camera.main.transform.rotation);
                } else {
                    AudioSteuerung.Instance.laserTrefferModus(true);
                }

                if (!hitEffectActive)
                {
                    curHitEffect = Object.Instantiate(
                        hitEffect,
                        hit.point,
                        hit.transform.rotation) as GameObject;
                    hitEffectActive = true;
                }
                else
                {
                    curHitEffect.transform.position = hit.point;
                }
            }
            else {
                AudioSteuerung.Instance.laserTrefferModus(false);
            }
            return true;
        }
        else
        {
            lr.SetPosition(1, new Vector3(0, 0, length));
            AudioSteuerung.Instance.laserTrefferModus(false);
        }

        if (hitEffectActive)
        {
            var Emission = curHitEffect.GetComponent<ParticleSystem>().emission;
            Emission.rate = 0;
            Object.Destroy(curHitEffect.GetComponentInChildren<Transform>().Find("Inner_Glow").gameObject);
            Object.Destroy(curHitEffect, 3f);
            hitEffectActive = false;
        }

        return false;
    }



    /// <summary>
    /// Zerstört den Laser.
    /// </summary>
    public void Stop()
    {
        if (hitEffectActive)
        {
            Object.Destroy(curHitEffect);
            hitEffectActive = false;
        }
    }
}
