using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static public class NetworkClientProcessing
{
    const int commandSign = 0;

    const int idSign = 1;
    const int xPosSign = 2;
    const int yPosSign = 3;

    

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[commandSign]);

    

        //gameLogic.DoSomething();
        if(signifier == ServerToClientSignifiers.addNewBalloonCommand)
        {
            gameLogic.SpawnNewBalloon(int.Parse(csv[idSign]), float.Parse(csv[xPosSign]), float.Parse(csv[yPosSign]));
        }
        else if (signifier == ServerToClientSignifiers.removeBalloonCommand)
        {
            gameLogic.RemoveBallon(int.Parse(csv[idSign]));
        }
        else if(signifier == ServerToClientSignifiers.AddBalloonBatchCommand)
        {
            string[] balloonDataEntries = msg.Split(';');
            foreach (string entry in balloonDataEntries)
            {
                string[] data = entry.Split(',');
                int balloonId = int.Parse(data[0]);
                float positionX = float.Parse(data[1]);
                float positionY = float.Parse(data[2]);
                gameLogic.SpawnNewBalloon(balloonId, positionX, positionY);
            }
        }
    }

    static public void SendMessageToServer(string msg, TransportPipeline pipeline)
    {
        networkClient.SendMessageToServer(msg, pipeline);
    }

    #endregion

    #region Connection Related Functions and Events
    static public void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
    }
    static public void DisconnectionEvent()
    {
        DisconnectFromServer();
        Debug.Log("Network Disconnection Event!");
    }
    static public bool IsConnectedToServer()
    {
        return networkClient.IsConnected();
    }
    static public void ConnectToServer()
    {
        networkClient.Connect();
    }
    static public void DisconnectFromServer()
    {
        SendMessageToServer(ClientToServerSignifiers.playerQuit.ToString(), TransportPipeline.FireAndForget);
        networkClient.Disconnect();
    }

    #endregion

    #region Setup
    static NetworkClient networkClient;
    static GameLogic gameLogic;

    static public void SetNetworkedClient(NetworkClient NetworkClient)
    {
        networkClient = NetworkClient;
    }
    static public NetworkClient GetNetworkedClient()
    {
        return networkClient;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }
    static public GameLogic GetGameLogic() { return gameLogic; }

    #endregion

}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int deleteBalloonCommand = 4;
    public const int playerQuit = 0;
    public const int updateHeartbeat = 5;
}

static public class ServerToClientSignifiers
{
    public const int addNewBalloonCommand = 2;
    public const int updateBalloonCommand = 3;
    public const int removeBalloonCommand = 4;
    public const int AddBalloonBatchCommand = 6;
}

#endregion

