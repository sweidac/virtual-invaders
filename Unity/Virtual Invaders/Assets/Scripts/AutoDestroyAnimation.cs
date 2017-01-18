using UnityEngine;
using System.Collections;

public class AutoDestroyAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
