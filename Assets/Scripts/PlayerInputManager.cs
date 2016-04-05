using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(CharacterController))]
public class PlayerInputManager : MonoBehaviour {

    public string horizontalAxis;
    public string verticalAxis;
    public string releasePipeKey;
    public float speed;

    private PlayerManager _manager;
    private CharacterController _controller;
    void Awake()
    {
        _manager = GetComponent<PlayerManager>();
        _controller = GetComponent<CharacterController>();
    }
    void Update()
    {
       if (Input.GetAxis(releasePipeKey) == 1)
            _manager.emptyPipe();
        /*  float h, v;
          h = Input.GetAxis(horizontalAxis);
          v = Input.GetAxis(verticalAxis);
          _controller.Move(new Vector3(h * speed*Time.deltaTime, 0, v * speed*Time.deltaTime));*/
    }


}
