using UnityEngine;
using System.Collections;
[RequireComponent(typeof(ExplosionMat))]
public class ExplosionDecrease : MonoBehaviour {
    [SerializeField]
    private float timerDecay;

    private float timer;
    private ExplosionMat explosionMat;
    void Awake()
    {        
        timer = timerDecay;
        explosionMat = GetComponent<ExplosionMat>();
    }


    void Update()
    {
        timer -= Time.deltaTime;
        explosionMat._alpha = timer / timerDecay;
            if(timer<=0)
            Destroy(gameObject);
    } 
}
