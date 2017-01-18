using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;
using UnityEngine.Audio;


public class GegnerProjektil : MonoBehaviour, IGegnerProjektil
{
    #region UNITY PARAMETER

    public GameObject mEffect;
    /// <summary>
    /// Schusskraft</summary>
    public float ProjectileSpeed;

    public AudioClip explosionsSound;

    public AudioMixerGroup mixer;

    #endregion

    private Rigidbody _rigidbody;
    private float MitteHorizontalDist;
    private float startHeight;
    private float maxHeight;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        MitteHorizontalDist = new Vector3(transform.position.x, 0,transform.position.z).magnitude*4/5;
        startHeight = transform.position.y;
        maxHeight = startHeight + 10 * (transform.position.magnitude/100);
    }

    void Update()
    {
        if (transform.position.magnitude >= 100f)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 ziel = Vector3.MoveTowards(
            transform.position, Camera.main.transform.position, ProjectileSpeed);

        float horizontalDist = new Vector3(transform.position.x, 0,transform.position.z).magnitude;

        if (horizontalDist < MitteHorizontalDist)
        {
            ziel.y = Mathf.Lerp(maxHeight, -1, (MitteHorizontalDist-horizontalDist)/MitteHorizontalDist);
        }
        else
        {
            ziel.y = Mathf.Lerp(startHeight, maxHeight, 1 - (horizontalDist - MitteHorizontalDist)/MitteHorizontalDist);
        }

        transform.LookAt(ziel);
        _rigidbody.velocity = (ziel - gameObject.transform.position).normalized * ProjectileSpeed;
    }

    void OnCollisionEnter(Collision collisionObject)
    {
        if(collisionObject.gameObject.tag == "MainCamera")
            Spielsteuerung.Instance.PlayerHit();

        if (collisionObject.gameObject.tag == "Enemy")
            collisionObject.gameObject.GetComponent<Gegner>().DealDamage();

        Treffer();
    }

    public void Treffer()
    {
        AudioSource source = Utility.PlayClipAt(this.gameObject.transform.position, this.explosionsSound, mixer);
        source.Play();
        if (mEffect != null)
        {
            var explosion = Instantiate(mEffect, transform.position, transform.rotation) as GameObject;
            if(explosion != null) explosion.AddComponent<ParticleSystemAutoDestroy>();
        }


        Destroy(gameObject);
    }


  #region IGegnerProjektil

  public GameObject ExplositionsPreFab
  {
    get
    {
      return mEffect;
    }
  }

  #endregion

}
