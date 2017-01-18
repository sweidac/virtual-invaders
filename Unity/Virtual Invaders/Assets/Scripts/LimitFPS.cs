using UnityEngine;
using System.Collections;

public class LimitFPS : MonoBehaviour
{
    public int targetFPS = 60;
	// Use this for initialization
	void Start ()
	{
	    QualitySettings.vSyncCount = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (targetFPS != Application.targetFrameRate)
	        Application.targetFrameRate = targetFPS;
	}
}
