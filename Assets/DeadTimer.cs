using UnityEngine;
using System.Collections;

public class DeadTimer : MonoBehaviour {
    [SerializeField]
    private float destoryTimer;

    private float timer;
    // Use this for initialization
	void Start () {
        timer = destoryTimer;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(gameObject);
	}
}
