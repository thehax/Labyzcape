using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace Labyzcape
{
    public class LabyzcapeNetworkManager : NetworkManager
    {
        public string playerName { get; set; }

        public void SetHostname(string hostname)
        {
            this.networkAddress = hostname;
        }

        public class CreatePlayerMessage : MessageBase
        {
            public string name;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);


            // tell the server to create a player with this name
            conn.Send(new CreatePlayerMessage
            {
                name = playerName
            });
        }

        private void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
        {
            // create a gameobject using the name supplied by client
            GameObject playergo = Instantiate(playerPrefab);
            playergo.GetComponent<PlayerBase>().playerName = createPlayerMessage.name;

            // set it as the player
            NetworkServer.AddPlayerForConnection(connection, playergo);

            CorridorManipulator.Instance.InitManager();
        }
    }
}
