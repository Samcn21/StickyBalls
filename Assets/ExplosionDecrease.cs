using UnityEngine;
using System.Collections;
[RequireComponent(typeof(ExplosionMat))]
[RequireComponent(typeof(DynamiteLogic))]
public class ExplosionDecrease : MonoBehaviour {
    [SerializeField]
    private float timerDecay;

    private float timer;
    private ExplosionMat explosionMat;
    private DynamiteLogic dynamiteLogic;
    void Awake()
    {
        explosionMat = GetComponent<ExplosionMat>();
        timer = timerDecay;
        dynamiteLogic = GetComponent<DynamiteLogic>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        explosionMat._alpha = timer / timerDecay;
        if (timer <= 0)
            dynamiteLogic.Explode();
    } 
}
