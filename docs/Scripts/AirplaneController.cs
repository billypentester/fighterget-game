using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;
    [SerializeField]
    float thrustControlSensitivity = 0.01f;
    [SerializeField]
    float flapControlSensitivity = 0.15f;


    float pitch;
    float yaw;
    float roll;
    float flap;

    float thrustPercent;
    bool brake = false;

    public GameObject missile;

    public GameObject impact;

    AircraftPhysics aircraftPhysics;
    Rotator propeller;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        propeller = FindObjectOfType<Rotator>();
        SetThrust(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            SetThrust(thrustPercent + thrustControlSensitivity);
        }
        propeller.speed = thrustPercent * 1500f;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            thrustControlSensitivity *= -1;
            flapControlSensitivity *= -1;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            brake = !brake;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            flap += flapControlSensitivity;
            //clamp
            flap = Mathf.Clamp(flap, 0f, Mathf.Deg2Rad * 40);
        }
        if(Input.GetKey(KeyCode.F)){
            shoot();
        }

        pitch = pitchControlSensitivity * Input.GetAxis("Vertical");
        roll = rollControlSensitivity * Input.GetAxis("Horizontal");
        yaw = yawControlSensitivity * Input.GetAxis("Yaw");
    }

    private void SetThrust(float percent)
    {
        thrustPercent = Mathf.Clamp01(percent);
    }

    private void FixedUpdate()
    {
        aircraftPhysics.SetControlSurfecesAngles(pitch, roll, yaw, flap);
        aircraftPhysics.SetThrustPercent(thrustPercent);
        aircraftPhysics.Brake(brake);
    }
    public void shoot(){
        RaycastHit[] hit;
        hit = Physics.RaycastAll(transform.position, transform.forward, 1000f);
        for(int i = 0; i < hit.Length; i++){

            if(hit[i].collider.gameObject.name == "Enemy"){
                Debug.Log("Hit");
                Destroy(hit[i].collider.gameObject);
            }

            Debug.Log(hit[i].collider.gameObject.name);

            if(hit[i].collider.gameObject.name != "Collision"){
                GameObject missileClone = Instantiate(missile, transform.position, transform.rotation);
                missileClone.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
                missileClone.GetComponent<Rigidbody>().velocity = transform.forward * 500f;
                missileClone.GetComponent<Rigidbody>().angularVelocity = transform.forward * 500f;
                missileClone.GetComponent<Rigidbody>().useGravity = false;
                missileClone.GetComponent<Rigidbody>().isKinematic = false;
                missileClone.GetComponent<Rigidbody>().freezeRotation = true;
                missileClone.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                missileClone.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                missileClone.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
                missileClone.GetComponent<Rigidbody>().mass = 20f;
                missileClone.GetComponent<Rigidbody>().drag = 0f;
                missileClone.GetComponent<Rigidbody>().angularDrag = 0.05f;

                GameObject ImpactEffect = Instantiate (impact, hit[i].point, Quaternion.LookRotation(hit[i].normal));

                
                
                Destroy(ImpactEffect, 10f);
                Destroy(missileClone, 10f);

                break;

            }
        }
    }
}
