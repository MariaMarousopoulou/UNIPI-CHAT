using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class SuperAdminMessage
    {
        private string _connectionstring;
        private const string _getAllUserQuery = "SELECT * FROM UserList";

        public SuperAdminMessage(string connectionstring)
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

        public List<MessageDTO> GetAllMessageID()
        {
            string _getMessageID = "SELECT IDMessage FROM MessageList";
            List<MessageDTO> messages = new List<MessageDTO>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
            {
                using (SqlCommand command = new SqlCommand(_getMessageID, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MessageDTO message = new MessageDTO();
                                message.MessageID = reader.GetInt32(0);
                                messages.Add(message);
                            }
                        }
                    }
                }
            }
            return messages;
        }

        public void ViewMessages(string username)
        {
            string ViewData = "SELECT * FROM MessageList WHERE Sender=@Sender OR Receiver=@Receiver";
            List<UserDTO> users = GetAllUsers();
            bool UserExist = users.Any(i => i.username == username);
            if (UserExist)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand(ViewData, sqlConnection))
                    {
                        command.Parameters.AddWithValue("Sender", username);
                        command.Parameters.AddWithValue("Receiver", username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"MessageID: {reader.GetSqlInt32(0)}");
                                    Console.WriteLine($"From: {reader.GetString(1)}  To:{reader.GetString(2)}");
                                    Console.WriteLine(reader.GetString(3));
                                }
                            }
                            else
                            {
                                Console.WriteLine("No messages.");
                            }
                        }
                    }
                }
            }
        }

        public void EditMessage(string message, int id)
        {
            string UpdatePassword = "UPDATE MessageList SET Message = @Message WHERE IDMessage = @ID";
            List<MessageDTO> messages = GetAllMessageID();
            bool MessageExist = messages.Any(i => i.MessageID == id);
            if (MessageExist)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand(UpdatePassword, sqlConnection))
                    {
                        command.Parameters.AddWithValue("Message", message);
                        command.Parameters.AddWithValue("ID", id);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Message updated!");
            }
            else
            {
                Console.WriteLine("Message ID does not exist.");
            }
            
        }

        public void DeleteMessage (int id)
        {
            string DeleteMessageFromDatabase = "DELETE FROM MessageList WHERE IDMessage = @ID";
            List<MessageDTO> messages = GetAllMessageID();
            bool MessageExist = messages.Any(i => i.MessageID == id);
            if (MessageExist)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
                {
                    using (SqlCommand command = new SqlCommand(DeleteMessageFromDatabase, sqlConnection))
                    {
                        sqlConnection.Open();
                        command.Parameters.AddWithValue("ID", id);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Message deleted!");

            }
            else
            {
                Console.WriteLine("Message ID does not exist.");
            }
        }
    }
}
