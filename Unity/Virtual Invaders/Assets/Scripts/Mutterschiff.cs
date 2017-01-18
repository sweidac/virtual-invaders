using UnityEngine;

class Mutterschiff : MonoBehaviour, IMutterschiff
{

    public AudioSource erscheinenSound;

    void Start()
    {
        erscheinenSound = this.gameObject.GetComponent<AudioSource>();
    }

    public void spieleSpawnSound()
    {
        erscheinenSound.Play();
    }

}
