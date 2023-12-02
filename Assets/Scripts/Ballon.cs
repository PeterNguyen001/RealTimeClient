using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour 
{
    public int id { get; private set; }

    public void SetId(int id)
    { this.id = id; }

    private void OnDestroy()
    {
        //NetworkClientProcessing.GetGameLogic().RemoveBallon(id);
    }
}
