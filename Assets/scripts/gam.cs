using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class gam : MonoBehaviour
{
    public void Death()
    {
        FindObjectOfType<playerControl>().GamCount();
        Destroy(gameObject);
    }
}
