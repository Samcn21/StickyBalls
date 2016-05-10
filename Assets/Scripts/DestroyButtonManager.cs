using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyButtonManager : MonoBehaviour {
    [SerializeField]
    private GameData.Team teamRelatedButton;

    private GameController gameControllerRef;

    public bool isRightPlayerClose { get; private set; }

    void Awake()
    {
        isRightPlayerClose = false;
        gameControllerRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Player"&&col.gameObject.GetComponent<InputController>().team==teamRelatedButton)
        {
            isRightPlayerClose = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag=="Player" && col.gameObject.GetComponent<InputController>().team == teamRelatedButton)
        {
            isRightPlayerClose = false;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(isRightPlayerClose)
        {
            InputController controller = col.gameObject.GetComponent<InputController>();
            if(controller.isPressingDelete)
            {
                gameControllerRef.PipeStatus.DestroyPipesOfPlayer(controller.team);
            }
        }
    }

	
}
