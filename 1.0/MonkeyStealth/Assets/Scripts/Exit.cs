using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

public static event System.Action OnPLayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
			if (OnPLayerExit != null)
			{
				OnPLayerExit();
			}
        }
    }
}
