using System;
using System.Xml.Schema;
using UnityEngine;
using Random = UnityEngine.Random;

public class Laser : MonoBehaviour
{
    public LineRenderer lr;
    public Light laserLight;
    [Range(0, 0.1f)]
    public float flickerAmount = 0.1f;
    public float flickerSpeed = 60f;
    public float length = 100f;
    public float maxin = 0.1f;
    public float diff = 0.05f;

    private Color laserColor;
    private float lightIntensity;
    void Start()
    {
        laserColor = lr.material.GetColor("_TintColor");
        lightIntensity = laserLight.intensity;
        InvokeRepeating("Flicker", 0, 1/flickerSpeed);
    }

    void Update()
    {
        
    }

    void Flicker()
    {
        float noise = Random.Range(1 - flickerAmount, 1);
        lr.material.SetColor("_TintColor", laserColor * noise);
        lr.SetPosition(1, Vector3.forward * length * noise);
        laserLight.intensity = Random.Range(maxin - diff, maxin);
    }
}
