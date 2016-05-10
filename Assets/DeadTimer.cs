using UnityEngine;
using System.Collections;

public class DeadTimer : MonoBehaviour {

    [SerializeField]
    private float deadTimer;

    void Update() {
        deadTimer -= Time.deltaTime;
        if (deadTimer <= 0)
            Destroy(gameObject);
    }
}
