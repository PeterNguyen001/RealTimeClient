using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleClick : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        int id = gameObject.GetComponent<Ballon>().id;
        NetworkClientProcessing.GetGameLogic().RemoveBallon(id);
    }
}
