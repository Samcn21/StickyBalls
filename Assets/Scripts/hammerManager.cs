using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class hammerManager : MonoBehaviour {
    private GameObject _player;


    private bool _releasePower;
    private float _power;
    [SerializeField]
    private float maxPower;
    [SerializeField]
    private float multiplier;
    [SerializeField]
    private float stunSpeedTreshold;
    void Awake()
    {
        _player = null;
        _releasePower = false;
    }

    void Update()
    {
        /*
        if (_player != null)
        {
            if (_player.GetComponent<PlayerInputManager>().pressHitHammer == 1)
                _loadPower = true;
            else
                _loadPower = false;
            if (_loadPower)
            {
                if (_power < maxPower)
                    _power += Time.deltaTime*multiplier;
            }
            else
                _releasePower = true;
        }
        */
    }

    void FixedUpdate()
    {
        if (_releasePower) {
            Vector3 p = transform.position;
            p.y = _player.transform.position.y;
            Vector3 direction = (p - _player.transform.position);
            GetComponent<Rigidbody>().AddForce(direction*_power, ForceMode.Impulse);
            _power = 0;
            _releasePower = false;
                }
    }

	void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
            _player = col.gameObject;
        
    }


    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            _player = null;
    
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag=="Player")
        {
            /*
            if (col.relativeVelocity.magnitude > stunSpeedTreshold*stunSpeedTreshold)
                Debug.Log("I stunned the player " + col.gameObject.GetComponent<PlayerManager>().index);
                */
        }
    }
    
}
