using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ImpactEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var effectclone = Instantiate(ImpactEffect, transform.position, transform.rotation);
        effectclone.transform.parent = transform;
        Destroy(effectclone, 10f);
    }

}
