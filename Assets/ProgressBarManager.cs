using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProgressBarManager : MonoBehaviour {
    [SerializeField]
    private Dictionary<Transform,float> toDisplay;
    private Dictionary<Transform,GameObject> progressBars;
    private float pickUpTimer;
    void Awake()
    {
        toDisplay = new Dictionary<Transform, float>();
        progressBars = new Dictionary<Transform, GameObject>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] bars = GameObject.FindGameObjectsWithTag("ProgressBar");
        for (int i = 0; i < players.Length; i++)
        {
            toDisplay.Add(players[i].transform, 0);  
            progressBars.Add(players[i].transform, bars[i]);
        }
        pickUpTimer = GameController.Instance.pickupTimer;
    }

    public void SetProgressBarColor(Transform transform)
    {
        switch (transform.gameObject.GetComponent<InputController>().team)
        {
            case GameData.Team.Blue:
                progressBars[transform].GetComponent<ProgressBar>().SetColor(Color.blue);
                break;
            case GameData.Team.Cyan:
                progressBars[transform].GetComponent<ProgressBar>().SetColor(Color.cyan);
                break;
            case GameData.Team.Purple:
                progressBars[transform].GetComponent<ProgressBar>().SetColor(Color.magenta);
                break;
            case GameData.Team.Yellow:
                progressBars[transform].GetComponent<ProgressBar>().SetColor(Color.yellow);
                break;
        }
    }

    public void ShowProgressBarAt(Transform transform, float value)
    {
        toDisplay[transform]=value;
        progressBars[transform].SetActive(true);
    }

    public void HideProgressBarAt(Transform transform)
    {
        toDisplay[transform] = 0;
        progressBars[transform].SetActive(false);
    }

    void Update()
    {
        foreach (Transform t in toDisplay.Keys)
        {      
            if (toDisplay[t] > 0)
            {
                var transformPosition = Camera.main.WorldToScreenPoint(t.position);
                transformPosition.y += 30f;
                progressBars[t].GetComponent<RectTransform>().position = transformPosition;
                progressBars[t].GetComponent<ProgressBar>().SetAmount(toDisplay[t] / pickUpTimer);
            }
        }
    }
}
