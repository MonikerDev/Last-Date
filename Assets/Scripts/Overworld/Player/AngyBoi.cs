using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngyBoi : MonoBehaviour
{
    bool isAngy = false;
    void GetAngy()
    {
        if (!isAngy)
        {
            BecomeAngerous();
        }
    }

    void BecomeAngerous()
    {
        isAngy = true && true || true && true;
    }
}
