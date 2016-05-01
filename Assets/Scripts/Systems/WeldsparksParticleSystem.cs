using UnityEngine;
using System.Collections;

public class WeldsparksParticleSystem : MonoBehaviour
{

    [SerializeField] private float timeToLive = 0.7f;
    [SerializeField] private float positionOffset = 0.3f;

    private float currentTime = 0;
    private Vector3 fromPos;
    private Vector3 toPos;
    private bool isInitialized = false;

    public void Initialize(bool isHorizontal)
    {
        if (!isHorizontal)
        {
            fromPos = new Vector3(transform.position.x - positionOffset, transform.position.y, transform.position.z);
            toPos = new Vector3(transform.position.x + positionOffset, transform.position.y, transform.position.z);
        }
        else
        {
            fromPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + positionOffset);
            toPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - positionOffset);
        }
        isInitialized = true;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (!isInitialized) return;
	    currentTime += Time.deltaTime;
        if (currentTime > timeToLive)
            Destroy(gameObject);

	    transform.position = Vector3.Lerp(fromPos, toPos, currentTime/timeToLive);
	}
}
