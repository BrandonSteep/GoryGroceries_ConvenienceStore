using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    float mass = 3.0f; // Player's Mass
    Vector3 impact = Vector3.zero;
    private CharacterController player;




    // S T A R T   &   F I X E D   U P D A T E //
 
 void Start()
    {
        player = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        // apply the impact force:
        if (impact.magnitude > 0.2)
        {
            player.Move(impact * Time.deltaTime);

            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        }
    }




    // A D D   I M P A C T //

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
}
