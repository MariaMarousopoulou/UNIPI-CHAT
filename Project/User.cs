using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class User
    {
        private string _connectionstring;
        private const string _getAllUserQuery = "SELECT * FROM UserList";


        public User(string connectionstring)
        {
            _connectionstring = connectionstring;
        }


        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
            {
                using (SqlCommand command = new SqlCommand(_getAllUserQuery, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                UserDTO user = new UserDTO();
                                user.username = reader.GetString(0);
                                user.password = reader.GetString(1);
                                users.Add(user);
                            }
                        }
                    }
                }
                return users;
            }
        }

        public bool UserExist (string receiver)
        {
            List<UserDTO> users = GetAllUsers();
            if (users.Any(i => i.username == receiver))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsUserRegistered(string username, string password)
        {
            List<UserDTO> users = GetAllUsers();
            if (users.Any(i => i.username == username && i.password == password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void BringTheMessages(string receiver, string sender)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
            {
                string _getAllData = "SELECT TOP 5 * FROM MessageList WHERE ((Sender = @sender AND Receiver = @receiver) OR (Sender = " +
                    "@Before AND Receiver=@After))";
                using (SqlCommand command = new SqlCommand(_getAllData, sqlConnection))
                {
                    command.Parameters.AddWithValue("sender", sender);
                    command.Parameters.AddWithValue("receiver", receiver);
                    command.Parameters.AddWithValue("Before", receiver);
                    command.Parameters.AddWithValue("After", sender);
                    sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                Console.WriteLine($"MessageID: {reader.GetSqlInt32(0)}");
                                Console.WriteLine($"From: {reader.GetString(1)}  To:{reader.GetString(2)}");
                                Console.WriteLine(reader.GetString(3));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Start your conversation!");
                        }
                    }
                }
            }
        }

        public bool InputTheData(string receiver, string sender, string message, DateTime now)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
            {
                string _putData = "INSERT INTO MessageList (Sender,Receiver,Message,DateTime) VALUES (@Sender,@Receiver," +
                    "@Message,@DateTime)";
                using (SqlCommand command = new SqlCommand(_putData, sqlConnection))
                {
                    sqlConnection.Open();
                    command.Parameters.AddWithValue("Sender", sender);
                    command.Parameters.AddWithValue("Receiver", receiver);
                    command.Parameters.AddWithValue("Message", message);
                    command.Parameters.AddWithValue("DateTime", now);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public void PrintAllUsers()
        {
            List<UserDTO> users = GetAllUsers();
            foreach (var user in users)
            {
                Console.WriteLine($"Username: {user.username}");
            }
        }
    }
}
