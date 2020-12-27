using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBox : MonoBehaviour
{
    public Transform Bread;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive"))
        {
            IChargeable it = other.transform.GetComponent<IChargeable>();
            it?.Charging(Bread);
            Debug.Log("Charge");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactive"))
        {
            INoPower it = other.transform.GetComponent<INoPower>();
            it?.NoPower(Bread);
            Debug.Log("NoPower");
        }
    }


}
