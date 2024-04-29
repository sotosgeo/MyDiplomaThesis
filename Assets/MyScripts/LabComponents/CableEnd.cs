using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CableEnd : MonoBehaviour
{
    [SerializeField] GameObject pinVisual;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material connectionDetectedMaterial;
    [SerializeField] Material connectionFinalizedMaterial;

    public string pinConnectedTo = null;
    Coroutine collisionTimer;


    public Action<string> OnConnectionFinalized;


    private IEnumerator CollisionTimer(Collider other)
    {
        yield return new WaitForSeconds(ConnectionManager.connectionTime);

        //After Timer Passed
        pinConnectedTo = other.gameObject.GetComponent<Pin>().pinId;
        pinVisual.GetComponent<MeshRenderer>().material = connectionFinalizedMaterial;
        //Trigger event and send the pinId that this cable was connected to
        OnConnectionFinalized?.Invoke(pinConnectedTo);    
    }


    private void OnTriggerEnter(Collider other)
    {
        //When a collision is detected between this and another Collider 
        //Debug.Log("Collision Detected with " + other.gameObject.GetComponent<Pin>().pinId);
        collisionTimer = StartCoroutine(CollisionTimer(other));
        if (pinVisual != null)
        {
            pinVisual.GetComponent<MeshRenderer>().material = connectionDetectedMaterial;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collision Stopped with" + other.gameObject.GetComponent<Pin>().pinId);
        pinVisual.GetComponent<MeshRenderer>().material = defaultMaterial;
        if (collisionTimer != null)
        {
            StopCoroutine(collisionTimer);
            collisionTimer = null;
        }
        pinConnectedTo = null;
    }

}
