using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TKSGGameServer;

namespace ConsoleApp1
{
    public class HandleConnection
    {
        private TcpClient client;
        private RegisteredPlayers registeredPlayers;
        private PlayerQueue playerQueue;
        private List<OngoingMatch> ongoingMatches;
        private List<Score> scores;        
        const int clientPort = 8889;


        public HandleConnection(TcpClient client, RegisteredPlayers registeredPlayers, PlayerQueue playerQueue, List<OngoingMatch> ongoingMatches, List<Score> scores)
        {
            this.client = client;
            this.registeredPlayers = registeredPlayers;
            this.playerQueue = playerQueue;
            this.ongoingMatches = ongoingMatches;
            this.scores = scores;
        }

        public void Start()
        {

            var thread = new Thread(Handle);
            thread.Start();
        }

        private void Handle()
        {
            bool keepAlive = false;
            while (client.Connected)
            {
                try
                {
                    string bufferData = Util.GetBufferData(client);
                    if (!string.IsNullOrEmpty(bufferData))
                    {
                        Console.WriteLine("Received request at: " + DateTime.Now.ToUniversalTime().ToString());
                        Console.WriteLine(bufferData);

                        string[] parsedRequest = Util.SplitRequest(bufferData);

                        string[] parsedTKSGRequest = parsedRequest[0].Split(' ');

                        if (!Util.ValidateRequest(parsedTKSGRequest))
                        {
                            try
                            {
                                Util.CreateTKSGMessage(client.GetStream(), "RES", "Invalid request.");
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine("Client disconnected! " + e.ToString());
                            }
                            
                        }
                        else
                        {
                            switch (parsedTKSGRequest[1])
                            {
                                case "PLAY":
                                    var supplicantPlayer = PlayRequest(parsedTKSGRequest);
                                    Console.WriteLine("Play request received!");
                                    if (supplicantPlayer != null)
                                    {
                                        Console.WriteLine("Registered player found!");
                                        if (playerQueue.Count > 0)
                                        {
                                            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                                            byte[] key = Guid.NewGuid().ToByteArray();
                                            string token = Convert.ToBase64String(time.Concat(key).ToArray());
                                            var waitingPlayer = playerQueue.Dequeue();

                                            lock (ongoingMatches)
                                            {
                                                ongoingMatches.Add(new OngoingMatch() { GameSessionToken = token, Player1Token = waitingPlayer.Token, Player2Token = supplicantPlayer.Token });
                                            }

                                            SendConnectionInfoToPeers(supplicantPlayer, waitingPlayer, token);
                                        }
                                        else
                                        {
                                            supplicantPlayer.TcpClient = this.client;
                                            playerQueue.Add(supplicantPlayer);
                                            SendQueueNotifcationToSupplicant(supplicantPlayer);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid play request - {0} - Client disconnected!", bufferData);
                                        client.Client.Disconnect(true);
                                    };
                                    break;
                                case "SCORE":
                                    OngoingMatch finishedMatch;

                                    lock (ongoingMatches)
                                    {
                                        finishedMatch = ongoingMatches.Find(x => x.GameSessionToken == parsedTKSGRequest[2]);
                                    }
                                    if (finishedMatch != null)
                                    {
                                        lock (scores)
                                        {
                                            scores.Add(new Score()
                                            {
                                                MatchToken = finishedMatch.GameSessionToken,
                                                Player1Token = finishedMatch.Player1Token,
                                                Player2Token = finishedMatch.Player2Token,
                                                Player1Score = int.Parse(parsedTKSGRequest[3]),
                                                Player2Score = int.Parse(parsedTKSGRequest[4])
                                            });
                                        }

                                        lock (ongoingMatches)
                                        {
                                            ongoingMatches.Remove(finishedMatch);
                                        }
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Invalid command received, ignoring - {0}", bufferData);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void SendQueueNotifcationToSupplicant(Player supplicantPlayer)
        {
            try
            {
                Util.CreateTKSGMessage(supplicantPlayer.TcpClient.GetStream(), Util.TKSGCommands.QUEUE.ToString(), "");
                Console.WriteLine("Supplicant sent to queue - {0} {1}", supplicantPlayer.Name, supplicantPlayer.Token);
            }
            catch (Exception e)
            {

                Console.WriteLine("Client disconnected");
            }
            
        }

        private void SendConnectionInfoToPeers(Player supplicantPlayer, Player waitingPlayer, string token)
        {
            try
            {
                Util.CreateTKSGMessage(this.client.GetStream(), Util.TKSGCommands.PEER.ToString(), token + " " + ((IPEndPoint)waitingPlayer.TcpClient.Client.RemoteEndPoint).Address.ToString() + " " + clientPort.ToString() + " " + supplicantPlayer.Name.Replace(' ', '#') + " " + waitingPlayer.Name.Replace(' ', '#') + " " + "CLIENT" + " " + "P2");
                Util.CreateTKSGMessage(waitingPlayer.TcpClient.GetStream(), Util.TKSGCommands.PEER.ToString(), token + " " + ((IPEndPoint)this.client.Client.RemoteEndPoint).Address.ToString() + " " + clientPort.ToString() + " " + waitingPlayer.Name.Replace(' ', '#') + " " + supplicantPlayer.Name.Replace(' ', '#') + " " + "HOST" + " " + "P1");
                Console.WriteLine("Connection information sent to players - {0} and {1}", supplicantPlayer.Name, waitingPlayer.Name);
            }
            catch (Exception)
            {

                Console.WriteLine("Client disconnected");
            }
            
        }

        private Player PlayRequest(string[] parsedTKSGRequest)
        {
            return registeredPlayers.Get().Find(x => x.Token == parsedTKSGRequest[2]);
        }
    }
}
