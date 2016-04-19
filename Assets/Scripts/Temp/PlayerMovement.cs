using UnityEngine;
using System.Collections;
using GamepadInput;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody rigidbody;
    private GamepadState gamepadState;
    //private GamePad.Index gamepadIndex;
    private Player player;

    [SerializeField]
    private float stickSensivity = 0.25f;

	// Use this for initialization
	void Start () {
        //gamepadState = GamePad.GetState(GamePad.Index.);
        player = GetComponent<Player>();
        rigidbody = GetComponent<Rigidbody>();

	}
    void FixedUpdate()
    {
        //rigidbody.AddForce(new Vector3(gamepadState.LeftStickAxis.x * stickSensivity, 0, gamepadState.LeftStickAxis.y * stickSensivity) * player.moveSpeed);
    }
}
