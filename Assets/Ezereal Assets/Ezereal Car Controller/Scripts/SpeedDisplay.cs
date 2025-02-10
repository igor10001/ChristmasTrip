using UnityEngine;
using TMPro; 

public class SpeedDisplay : MonoBehaviour
{
    public Rigidbody rb; 
    public TextMeshPro textMeshPro;

    private void Update()
    {
        float speed = rb.velocity.magnitude;

        // Convert speed to km/h
        float speedKmPerHour = speed * 3.6f;

        textMeshPro.text = $"{Mathf.FloorToInt(speedKmPerHour)}"; 
    }
}