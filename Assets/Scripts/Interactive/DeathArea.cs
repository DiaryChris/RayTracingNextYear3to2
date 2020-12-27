using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
  


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.RestartScene();
        }
        if (other.gameObject.tag=="Interactive")
        {
            Destroy(other.gameObject);
        }
    }
}
