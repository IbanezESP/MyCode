using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class cherry : MonoBehaviour
{
    public void Death()
    {
        FindObjectOfType<playerControl>().CherryCount();
        Destroy(gameObject);
    }
}
