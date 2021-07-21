using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoMatch
{
    public class Match // V1.12
    {
        private Random random = new Random();

        public int id;
        public Board board;
        public List<string> players; //List of players ids.
        public Dictionary<string, int> connections; // <id, connection>
        public bool playing;
        public int turn;
        public int dice;
        public bool canThrow;

        public Match(int id, string[] boardData)
        {
            this.id = id;
            this.board = new Board(boardData);
            players = new List<string>();
            connections = new Dictionary<string, int>();
        }

        // Server
        public bool RegisterPlayer(string playerId, int playerConnection)
        {
            if (players.Count < 3)
            {
                players.Add(playerId);
                connections.Add(playerId, playerConnection);
                if (players.Count == 3) { return true; } // When the last players joins, returns true.
            }
            return false;
        }

        public void Start()
        {
            // Shuffle list
            List<String> players = this.players.OrderBy(x => random.Next()).ToList();
            this.players = players;
            // Set first turn
            turn = 0;
            dice = 0;
            canThrow = true;
        }

        public int Throw()
        {
            canThrow = false;
            dice = random.Next(1, 6);
            return dice;
        }

        public void Play()
        {
            canThrow = true;
            // Set next turn
            turn = (turn + 1) % players.Count();
        }

        public Dictionary<int, string[]> Status()
        {
            Dictionary<int, string[]> info = new Dictionary<int, string[]>();
            foreach (var connection in connections)
            {
                bool playing = (connection.Key == players[turn]);
                string[] data = new string[] { playing.ToString(), dice.ToString(), canThrow.ToString() };
                info.Add(connection.Value, data);
            }
            return info;
        }

        // Client
        public void SetFromStatus(string[] status)
        {
            bool.TryParse(status[0], out playing);
            int.TryParse(status[1], out dice);
            bool.TryParse(status[2], out canThrow);
        }
    }
}
