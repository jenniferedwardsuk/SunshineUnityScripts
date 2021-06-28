using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBySpray : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spray")
        {
            Destroy(this.gameObject);
        }
    }
}
