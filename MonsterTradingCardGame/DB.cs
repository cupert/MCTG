using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NpgsqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using MonsterTradingCardGame.Server;
using Newtonsoft.Json.Linq;

//pgadmin master pw: 12345

namespace MonsterTradingCardGame
{
    public class DB
    {
        
        public readonly string connstring = "Server=127.0.0.1;Port=5432;Database=mctg;User Id=postgres;Password=postgres;";

        private NpgsqlConnection Connection()
        {
            var conn = new NpgsqlConnection(connstring);
            conn.Open();
            return conn;
        }

        private NpgsqlTransaction BeginTransaction(NpgsqlConnection conn)
        {
            // Try to start new transaction
            for (var i = 0; i < 15; i++)
            {
                try
                {
                    var transaction = conn.BeginTransaction();
                    return transaction;
                }
                catch (InvalidOperationException) { Thread.Sleep(50); }
            }
            return null;
        }

        public bool checkAuth(string pToken)
        {
            var conn = Connection();

            try
            {
                var check = new NpgsqlCommand(
                    "SELECT * FROM \"Player\" WHERE UPPER (\"Username\") LIKE UPPER (@pUsername)", conn);

                check.Parameters.AddWithValue("pUsername", pToken);
                check.Parameters[0].NpgsqlDbType = NpgsqlDbType.Char;


                check.Parameters.Add(new NpgsqlParameter("Username", NpgsqlDbType.Char)
                    {
                        Direction = ParameterDirection.Output
                    }
                );

                var reader = check.ExecuteReader();
                bool result = reader.Read();
                reader.Close();
                //if exists true
                conn.Close();
                return result;


            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Error");
                conn.Close();
                return false;
            }
        }

        public bool CheckIfUserExists(string pUsername)
        {
            var conn = Connection();

            try
            {
                var check = new NpgsqlCommand(
                    "SELECT * FROM \"Player\" WHERE UPPER (\"Username\") LIKE UPPER (@pUsername)", conn);

                check.Parameters.AddWithValue("pUsername", pUsername);
                check.Parameters[0].NpgsqlDbType = NpgsqlDbType.Char;


                check.Parameters.Add(new NpgsqlParameter("Username", NpgsqlDbType.Char)
                    {
                        Direction = ParameterDirection.Output
                    }
                );

                var reader = check.ExecuteReader();
                bool result = reader.Read();
                reader.Close();
                //if exists true
                conn.Close();
                return result;


            }
            catch(NpgsqlException e)
            {
                Console.WriteLine("Error");
                conn.Close();
                return false;
            }
        }

        public User GetUser(string pUsername)
        {
            var conn = Connection();

            try
            {

                var getUser =
                    new NpgsqlCommand("SELECT * FROM \"Player\" WHERE UPPER (\"Username\") LIKE UPPER (@pUsername)",
                        conn);
                getUser.Parameters.AddWithValue("pUsername", pUsername);
                getUser.Parameters[0].NpgsqlDbType = NpgsqlDbType.Char;

                getUser.Parameters.Add(new NpgsqlParameter("Username", NpgsqlDbType.Char)
                {
                    Direction = ParameterDirection.Output
                }
                );
                //Soll ?
                getUser.Parameters.Add(new NpgsqlParameter("Password", NpgsqlDbType.Char)
                {
                    Direction = ParameterDirection.Output
                }
                );
                getUser.Parameters.Add(new NpgsqlParameter("Coins", NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Output
                }
                );
                getUser.Parameters.Add(new NpgsqlParameter("ELO", NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Output
                }
                );
                getUser.Parameters.Add(new NpgsqlParameter("Loss", NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Output
                }
                );
                getUser.Parameters.Add(new NpgsqlParameter("Wins", NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Output
                }
                );
                getUser.Parameters.Add(new NpgsqlParameter("Draws", NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Output
                }
                );
                getUser.Parameters.Add(new NpgsqlParameter("Name", NpgsqlDbType.Char)
                {
                    Direction = ParameterDirection.Output
                });
                getUser.Parameters.Add(new NpgsqlParameter("Bio", NpgsqlDbType.Char)
                {
                    Direction = ParameterDirection.Output
                });
                getUser.Parameters.Add(new NpgsqlParameter("Image", NpgsqlDbType.Char)
                {
                    Direction = ParameterDirection.Output
                });

                getUser.ExecuteNonQuery();

                bool check = true;

                for (int i = 1; i < 8; i++)
                {
                    if (getUser.Parameters[i].Value == null)
                    {
                        check = false;
                    }
                }

                if (check)
                {
                    conn.Close();
                    return new User((string)getUser.Parameters[1].Value, (string)getUser.Parameters[2].Value, (int)getUser.Parameters[3].Value, (int)getUser.Parameters[4].Value, (int)getUser.Parameters[5].Value, (int)getUser.Parameters[6].Value, (int)getUser.Parameters[7].Value, (string)getUser.Parameters[8].Value, (string)getUser.Parameters[9].Value, (string)getUser.Parameters[10].Value);
                }
                else
                {
                    conn.Close();
                    return null;
                }
                conn.Close();
                return null;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.Message);
                conn.Close();
                return null;
            }

        }


        public bool GenerateUser(User pUser)
        {
            var conn = Connection();
            try
            {

                var generateUser = new NpgsqlCommand(
                    "INSERT INTO \"Player\" (\"Username\", \"Password\", \"Coins\", \"ELO\", \"Loss\", \"Wins\", \"Draws\", \"Name\", \"Bio\", \"Image\") VALUES (@pUsername, @pPassword, @pCoins, @pELO, @pLoss, @pWins, @pDraws, @pName, @pBio, @pImage)",
                    conn);
                generateUser.Parameters.AddWithValue("pUsername", pUser.Username);
                generateUser.Parameters.AddWithValue("pPassword", pUser.Password);
                generateUser.Parameters.AddWithValue("pCoins", pUser.Coins);
                generateUser.Parameters.AddWithValue("pELO", pUser.Elo);
                generateUser.Parameters.AddWithValue("pLoss", pUser.Loss);
                generateUser.Parameters.AddWithValue("pWins", pUser.Wins);
                generateUser.Parameters.AddWithValue("pDraws", pUser.Draws);
                generateUser.Parameters.AddWithValue("pName", pUser.Name);
                generateUser.Parameters.AddWithValue("pBio", pUser.Bio);
                generateUser.Parameters.AddWithValue("pImage", pUser.Image);

                generateUser.Prepare();
                generateUser.ExecuteNonQuery();
                conn.Close();

                return true;
            }
            catch (NpgsqlException e)
            {
                if (e.ErrorCode != -2147467259)
                {
                    Console.WriteLine($"Error: \nMessage: {e.Message} \nHResult: {e.HResult}");
                }
                else
                {
                    Console.WriteLine("Unknown Error");
                }

                return false;
            }
        }

        public bool CreateCard(Card pCard, int pPackage_ID)
        {
            using var conn = Connection();

            try
            {
                var createCard = new NpgsqlCommand(
                    "INSERT INTO \"PlayerCards\" (\"ID_name\", \"Name\", \"Dmg\", \"Element\", \"Type\", \"Player_ID\", \"Package_ID\", \"isBattleDeck\") " +
                    "VALUES(@pID, @pName, @pDamage, @pElement, @pType, null, @pPackageID, false)",
                    conn);


                createCard.Parameters.AddWithValue("pID", NpgsqlDbType.Char, pCard.Id);
                createCard.Parameters.AddWithValue("pName", NpgsqlDbType.Char, pCard.Name);
                createCard.Parameters.AddWithValue("pDamage", NpgsqlDbType.Double, pCard.Damage);
                createCard.Parameters.AddWithValue("pElement", NpgsqlDbType.Integer, (int) pCard.Element);
                createCard.Parameters.AddWithValue("pType", NpgsqlDbType.Integer, (int) pCard.Type);
                createCard.Parameters.AddWithValue("pPackageID", NpgsqlDbType.Integer, pPackage_ID);

                createCard.ExecuteNonQuery();
                BeginTransaction(conn).Commit();
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                conn.Close();
                return false;
            }
        }

        

        public bool UpdatePlayer(User pUser)
        {
            var conn = Connection();
            try
            {
                var UpdatePlayer = new NpgsqlCommand(
                    "UPDATE \"Player\" SET \"Coins\"=@pCoins, \"ELO\"=@pELO, \"Loss\"=@pLoss, \"Wins\"=@pWins, \"Draws\"=@pDraws, \"Name\"=@pName,  \"Bio\"=@pBio,  \"Image\"=@pImage WHERE UPPER(\"Username\") LIKE UPPER(@pUsername)", conn);

                UpdatePlayer.Parameters.AddWithValue("pCoins", pUser.Coins);
                UpdatePlayer.Parameters[0].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdatePlayer.Parameters.AddWithValue("pELO", pUser.Elo);
                UpdatePlayer.Parameters[1].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdatePlayer.Parameters.AddWithValue("pLoss", pUser.Loss);
                UpdatePlayer.Parameters[2].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdatePlayer.Parameters.AddWithValue("pWins", pUser.Wins);
                UpdatePlayer.Parameters[3].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdatePlayer.Parameters.AddWithValue("pDraws", pUser.Draws);
                UpdatePlayer.Parameters[4].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdatePlayer.Parameters.AddWithValue("pName", pUser.Name);
                UpdatePlayer.Parameters[5].NpgsqlDbType = NpgsqlDbType.Char;

                UpdatePlayer.Parameters.AddWithValue("pBio", pUser.Bio);
                UpdatePlayer.Parameters[6].NpgsqlDbType = NpgsqlDbType.Char;

                UpdatePlayer.Parameters.AddWithValue("pImage", pUser.Image);
                UpdatePlayer.Parameters[7].NpgsqlDbType = NpgsqlDbType.Char;

                UpdatePlayer.Parameters.AddWithValue("pUsername", pUser.Username);
                UpdatePlayer.Parameters[8].NpgsqlDbType = NpgsqlDbType.Char;

                UpdatePlayer.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.Message);
                conn.Close();
                return false;
            }
        }

        public bool UpdateStats(User pUser)
        {
            var conn = Connection();

            try
            {
                var UpdateStats = new NpgsqlCommand(
                    "UPDATE \"Player\" SET \"ELO\" = @pELO,  \"Loss\" = @pLoss,  \"Wins\" = @pWins,  \"Draws\" = @pDraws WHERE UPPER (\"Username\") LIKE UPPER (@pUsername)",
                    conn);

                UpdateStats.Parameters.AddWithValue("pELO", pUser.Elo);
                UpdateStats.Parameters[0].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdateStats.Parameters.AddWithValue("pLoss", pUser.Loss);
                UpdateStats.Parameters[1].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdateStats.Parameters.AddWithValue("pWins", pUser.Wins);
                UpdateStats.Parameters[2].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdateStats.Parameters.AddWithValue("pDraws", pUser.Draws);
                UpdateStats.Parameters[3].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdateStats.Parameters.AddWithValue("pUsername", pUser.Username);
                UpdateStats.Parameters[4].NpgsqlDbType = NpgsqlDbType.Char;

                UpdateStats.ExecuteNonQuery();

                conn.Close();
                return true;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.Message);
                conn.Close();
                return false;
            }

        }

        public HTTPResponse CreatePackage(string pPackage)
        {
            using var conn = Connection();


            try
            {
                var createPackage = new NpgsqlCommand(
                    "SELECT CASE WHEN max(\"Package_ID\") IS NULL THEN 0 ELSE max(\"Package_ID\") END FROM \"PlayerCards\"",
                    conn);
                createPackage.Parameters.Add(new NpgsqlParameter("pPackageID", NpgsqlDbType.Integer)
                    {IsNullable = true, Direction = ParameterDirection.Output});

                createPackage.ExecuteNonQuery();

                int pPackageID;
                if (createPackage.Parameters[0].Value is int value)
                {
                    pPackageID = value + 1;
                }
                else
                {
                    conn.Close();
                    return new HTTPResponse(HttpStatusCode.BadRequest, "PackageID error");
                }

                JArray array = JArray.Parse(pPackage);

                foreach (dynamic obj in array.Children<JObject>())
                {
                    Card tempCard = new Card(obj.Id.ToString(), obj.Name.ToString(), (double) obj.Damage,
                        (Element) obj.Element, (Type) obj.Type);

                    if (!CreateCard(tempCard, pPackageID))
                    {
                        conn.Close();
                        return new HTTPResponse(HttpStatusCode.BadRequest, "Creating a card failed!");
                    }
                }
                conn.Close();
                return new HTTPResponse(HttpStatusCode.Accepted, "Package created successfully!");
            }
            catch (Exception e)
            {
                conn.Close();
                return new HTTPResponse(HttpStatusCode.BadRequest, "PackageID error");
            }
        }

        public HTTPResponse BuyPackage(string pUsername)
        {
            var conn = Connection();
            var transaction = BeginTransaction(conn);

            if (transaction == null)
            {
                conn.Close();
                return new HTTPResponse(HttpStatusCode.BadRequest, "Creating a card failed!");
            }

            try
            {
                var tempUser = GetUser(pUsername);

                if (tempUser is null)
                {
                    conn.Close();
                    return new HTTPResponse(HttpStatusCode.BadRequest, "User doesent exist");
                }

                if (tempUser.Coins - 5 < 0)
                {
                    conn.Close();
                    return new HTTPResponse(HttpStatusCode.Conflict, "You dont have enough money");
                }

                var buyPackage = new NpgsqlCommand(
                    "SELECT MIN(\"Package_ID\") FROM \"PlayerCards\" WHERE \"Player_ID\" IS NULL", conn);

                buyPackage.Parameters.Add(new NpgsqlParameter("pPackageID", NpgsqlDbType.Integer)
                {
                    Direction = ParameterDirection.Output
                });

                buyPackage.ExecuteNonQuery();

                int pPackageID;

                if (buyPackage.Parameters[0].Value is int value)
                {
                    pPackageID = value;
                }
                else
                {
                    conn.Close();
                    return new HTTPResponse(HttpStatusCode.Conflict, "No more packages available");
                }

                var newPackage = new NpgsqlCommand(
                    "UPDATE \"PlayerCards\" SET \"Player_ID\"=@pUsername WHERE \"Package_ID\"=@pPackageID", conn);
                newPackage.Parameters.AddWithValue("pUsername", pUsername);
                newPackage.Parameters[0].NpgsqlDbType = NpgsqlDbType.Varchar;
                newPackage.Parameters.AddWithValue("pPackageID", pPackageID);
                newPackage.Parameters[1].NpgsqlDbType = NpgsqlDbType.Integer;

                newPackage.ExecuteNonQuery();

                var UpdateUser = new NpgsqlCommand(
                    "UPDATE \"Player\" SET \"Coins\"=@pCoins WHERE \"Username\"=@pUsername", conn);

                UpdateUser.Parameters.AddWithValue("pCoins", tempUser.Coins - 5);
                UpdateUser.Parameters[0].NpgsqlDbType = NpgsqlDbType.Integer;

                UpdateUser.Parameters.AddWithValue("pUsername", pUsername);
                UpdateUser.Parameters[1].NpgsqlDbType = NpgsqlDbType.Varchar;

                UpdateUser.ExecuteNonQuery();
                transaction.Commit();

                conn.Close();
                return new HTTPResponse(HttpStatusCode.Accepted, "Package bought succ");
            }
            catch (Exception e)
            {
                conn.Close();
                return new HTTPResponse(HttpStatusCode.BadRequest, "Error");
            }
        }

        //Select Cardsowned

        public List<Card> SelectCardsOwned(string pUsername)
        {
            var conn = Connection();
            try
            {
                var selectCardsOwned = new NpgsqlCommand(
                    "SELECT * FROM \"PlayerCards\" WHERE \"Player_ID\"=@pUserName", conn);

                selectCardsOwned.Parameters.AddWithValue("pUserName", pUsername);
                selectCardsOwned.Parameters[0].NpgsqlDbType = NpgsqlDbType.Char;
                NpgsqlDataReader Reader = selectCardsOwned.ExecuteReader();

                List<Card> newCards = new List<Card>();
                
                while (Reader.Read())
                {
                    newCards.Add(new Card(Reader.GetString(8), Reader.GetString(1),
                        Reader.GetDouble(2), (Element)Reader.GetInt32(3), (Type)Reader.GetInt32(4)));
                }

                Reader.Close();
                selectCardsOwned.ExecuteNonQuery();
                
                conn.Close();
                return newCards;
            }
            catch (Exception e)
            {
                Console.WriteLine($"CardsOwned Error: {e.Message}"); 
                conn.Close();
                return null;
            }
        }

        public List<Card> SelectBattleDeck(string pUsername)
        {
            var conn = Connection();
            try
            {
                var selectBattleDeck = new NpgsqlCommand(
                    "SELECT * FROM \"PlayerCards\" WHERE \"Player_ID\"=@pUserName AND \"isBattleDeck\" =true", conn);

                selectBattleDeck.Parameters.AddWithValue("pUserName", pUsername);
                selectBattleDeck.Parameters[0].NpgsqlDbType = NpgsqlDbType.Char;
                NpgsqlDataReader Reader = selectBattleDeck.ExecuteReader();

                List<Card> newCards = new List<Card>();

                while (Reader.Read())
                {
                    newCards.Add(new Card(Reader.GetString(8), Reader.GetString(1),
                        Reader.GetDouble(2), (Element)Reader.GetInt32(3), (Type)Reader.GetInt32(4)));
                }

                Reader.Close();
                selectBattleDeck.ExecuteNonQuery();

                conn.Close();
                return newCards;
            }
            catch (Exception e)
            {
                Console.WriteLine($"SelectBattleDeck Error: {e.Message}");
                conn.Close();
                return null;
            }
        }

        public bool InsertDeck(string pID)
        {
            var conn = Connection();
            var transaction = BeginTransaction(conn);

            if (transaction == null)
            {
                Console.WriteLine("InsertDeck error");
                conn.Close();
                return false;
            }

            try
            {
                var insertDeck =
                    new NpgsqlCommand("UPDATE \"PlayerCards\" SET \"isBattleDeck\"=true WHERE \"ID_name\"=@pID_name",
                        conn);
                insertDeck.Parameters.AddWithValue("pID_name", pID);
                insertDeck.Parameters[0].NpgsqlDbType = NpgsqlDbType.Char;

                insertDeck.ExecuteNonQuery();
                transaction.Commit();
                conn.Close();
                return true;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.Message);
                conn.Close();
                return false;
            }
        }

        public bool CheckCard(string pID)
        {
            //if card belongs to other user return true
            var conn = Connection();

            try
            {
                var check = new NpgsqlCommand(
                    "SELECT * FROM \"PlayerCards\" WHERE (\"ID_name\") LIKE (@pID_name) AND (\"isBattleDeck\") is false ", conn);
                pID += "%";
                check.Parameters.AddWithValue("pID_name", pID);
                check.Parameters[0].NpgsqlDbType = NpgsqlDbType.Char;


                check.Parameters.Add(new NpgsqlParameter("ID_name", NpgsqlDbType.Char)
                    {
                        Direction = ParameterDirection.Output
                    }
                );

                var reader = check.ExecuteReader();
                bool result = reader.Read();
                reader.Close();
                //if exists true
                conn.Close();
                return result;


            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("Error");
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }

        public string ShowScoreboard()
        {
            string score = "";
            using var conn = Connection();
            try
            {
                using var showScoreboardCmd = new NpgsqlCommand(
                    "SELECT \"Username\", \"ELO\" FROM \"Player\" ORDER BY \"ELO\" DESC", conn);

                NpgsqlDataReader Reader = showScoreboardCmd.ExecuteReader();
                score += "Scoreboard\n";
                score += "Username  ELO\n";
                while (Reader.Read())
                {
                    score += $"{Reader.GetString(0)}, {Reader.GetInt32(1)}\n";
                }

                Reader.Close();
                showScoreboardCmd.ExecuteNonQuery();
                conn.Close();
                return score;
            }
            catch (Exception e)
            {
                conn.Close();
                return e.Message;
            }
        }

        public void DeleteDb()
        {
            var conn = Connection();
            var transaction = BeginTransaction(conn);
            if (transaction == null)
            {
                Console.WriteLine("error");
                conn.Close();
                return;
            }

            try
            {
                //Player Tabelle löschen
                var DeletePlayers = new NpgsqlCommand("DELETE FROM \"Player\"", conn);
                DeletePlayers.ExecuteNonQuery();

                //PlayerCardsTabelle löschen

                //INSERT INTO "PlayerCards" ("Name", "Dmg", "Element", "Type", "PlayerID", "isBattleDeck") VALUES ("Test", 100,1,1,12,true);
                var DeleteCards = new NpgsqlCommand("DELETE FROM \"PlayerCards\"", conn);
                DeleteCards.ExecuteNonQuery();


                transaction.Commit();
                Console.WriteLine("Succ");
            }
            catch (Exception e)
            {
                Console.Write($"Error: {e.Message}");
            }
            
            conn.Close();
        }
    }
}
