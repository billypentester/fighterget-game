using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private LineRenderer lr;
    public GameObject laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.forward * 100);
    }

    // Update is called once per frame
    void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, 1000f);
        foreach(var collider in colliders) {
            if(collider.gameObject.name == "Collision" || collider.gameObject.name == "Gear_R" || collider.gameObject.name == "Gear_L")
            {
                var effectclone = Instantiate(laserPrefab, transform.position, transform.rotation);
                effectclone.transform.Translate(collider.ClosestPoint(transform.position));
                Destroy(effectclone, 10f);
            }
        }
    }
}
