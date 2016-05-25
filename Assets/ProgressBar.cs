using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {
    [SerializeField]
    private Transform loadingBar;
    public float amount { get; private set; }

    void Awake()
    {
        amount = 0;
    }

    void Update()
    {
        loadingBar.GetComponent<Image>().fillAmount = amount;
    }

    public void SetAmount(float val)
    {
        amount = val;
    }

    public void SetColor(Color color)
    {
        loadingBar.GetComponent<Image>().color = color;
    }
}
