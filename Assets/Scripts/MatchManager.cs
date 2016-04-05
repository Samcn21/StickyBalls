using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MapManager))]
public class MatchManager : MonoBehaviour {

    private MapManager _mapRef;
	// Use this for initialization
	void Awake() {
        _mapRef = GetComponent<MapManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
