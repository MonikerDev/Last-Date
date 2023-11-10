using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Item>().TriggerFlag();
    }

}
