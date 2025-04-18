using UnityEngine;

public class RandomisePlacement : MonoBehaviour
{
    [SerializeField] private Transform[] randomPositions;
    [Range(0f,1f)] [SerializeField] private float randomPlacementChance = 0.333f;

    void OnEnable(){
        if (Random.value <= randomPlacementChance){
            Debug.Log($"Randomly placing {this.gameObject.name}");

            Transform newPlacement = randomPositions[Random.Range(0, randomPositions.Length)];
            this.transform.position = newPlacement.position;
            this.transform.rotation = newPlacement.rotation;
        }
        // else {
        //     Debug.Log("Fail");
        // }
    }
}
