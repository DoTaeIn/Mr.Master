using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTransition : MonoBehaviour
{
    public Material transitionMaterial; // Material with the HoleMask shader
    public float transitionSpeed = 1.0f; // Speed of the transition
    public float minRadius = 0.0f; // Minimum radius (fully black)
    public float maxRadius = 1.0f; // Maximum radius (fully transparent center)

    private float radius;
    private bool isTransitioning = false; // Whether a transition is happening
    private bool shrinking = true; // Whether the circle is shrinking or expanding

    void Start()
    {
        // Initialize the radius to the maximum (fully transparent circle)
        radius = maxRadius;
        transitionMaterial.SetFloat("_MaskRadius", radius);
    }

    void Update()
    {
        if (isTransitioning)
        {
            // Update the radius during transition
            float direction = shrinking ? -1 : 1;
            radius += direction * Time.deltaTime * transitionSpeed;

            // Clamp the radius to prevent overflow
            radius = Mathf.Clamp(radius, minRadius, maxRadius);
            transitionMaterial.SetFloat("_MaskRadius", radius);

            // Stop transitioning when limits are reached
            if (radius == minRadius || radius == maxRadius)
            {
                isTransitioning = false;
            }
        }
    }

    // Method to start the shrinking transition
    public void StartShrink()
    {
        shrinking = true;
        isTransitioning = true;
    }

    // Method to start the expanding transition
    public void StartExpand()
    {
        shrinking = false;
        isTransitioning = true;
    }
}
