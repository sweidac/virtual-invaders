using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.Audio;

public class UFO : MonoBehaviour, IUFO
{
    #region UNITY PARAMETER
    public float UFOGeschwindigkeit = 10.0f;

    [Range(0, float.MaxValue)]
    public float Hitpoints = 10;

    public int Punkte = 10;

	public AudioClip explosionsTon;
	public AudioMixerGroup explosionsMixer;
    public GameObject cube;

    #endregion

    private Rigidbody _rigidbody;

	// Use this for initialization
	void Start ()
    {

        _rigidbody = GetComponent<Rigidbody>();

	}

    void FixedUpdate()
    {
        Vector3 facing = gameObject.transform.forward;

        _rigidbody.velocity = facing * UFOGeschwindigkeit;
    }

	// Update is called once per frame
	void Update ()
    {
        if (transform.position.magnitude >= 120f || Spielsteuerung.Instance.Gameover)
        {
            Destroy(gameObject);
        }
        cube.transform.Rotate(new Vector3(0, 0, 1), 10.0f);
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
            Spielsteuerung.Instance.PlayerScored(Punkte);
            ZerstoereUfo();
        }

    }

    public void ZerstoereUfo()
    {
		Utility.PlayClipAt (transform.position, explosionsTon, explosionsMixer).Play ();
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return Hitpoints > 0;
    }


  #region IUFO

  public GameObject Objekt
  {
    get
    {
      return gameObject;
    }
  }

  #endregion

}
