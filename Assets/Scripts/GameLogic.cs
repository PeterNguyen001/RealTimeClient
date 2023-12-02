using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    float durationUntilNextBalloon;
    Sprite circleTexture;
    Dictionary<int, Ballon> ballons = new Dictionary<int, Ballon>();
    void Start()
    {
        NetworkClientProcessing.SetGameLogic(this);
    }

    public void SpawnNewBalloon(int id, float screenPositionXPercent, float screenPositionYPercent)
    {
        Vector2 screenPosition = new Vector2(screenPositionXPercent * (float)Screen.width, screenPositionYPercent * (float)Screen.height);
        if (circleTexture == null)
            circleTexture = Resources.Load<Sprite>("Circle");

        GameObject balloon = new GameObject("Balloon");

        balloon.AddComponent<SpriteRenderer>();
        balloon.GetComponent<SpriteRenderer>().sprite = circleTexture;
        balloon.AddComponent<CircleClick>();
        balloon.AddComponent<CircleCollider2D>();
        balloon.AddComponent<Ballon>().SetId(id);

        Ballon balloonComponent = balloon.GetComponent<Ballon>();
        balloonComponent.SetId(id);
        ballons.Add(id, balloonComponent);

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));
        pos.z = 0;
        balloon.transform.position = pos;
        //go.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -Camera.main.transform.position.z));
    }

    public void RemoveBallon(int id)
    {
        if(ballons.ContainsKey(id)) 
        {
            string msg = ClientToServerSignifiers.deleteBalloonCommand + "," + id;
            NetworkClientProcessing.SendMessageToServer(msg, TransportPipeline.FireAndForget);
            if (ballons[id] != null) 
            {
                GameObject balloonGameObject = ballons[id].gameObject;
                Destroy(balloonGameObject);
            }
            ballons.Remove(id);
        }
        else { Debug.Log("Not here"); }
    }
    public void QuitGame()
    {
        NetworkClientProcessing.SendMessageToServer(ClientToServerSignifiers.playerQuit.ToString(), TransportPipeline.FireAndForget);
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            
            EditorApplication.isPlaying = false;
        }
#endif
        Application.Quit();
    }
}
