using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class SuperAdminUser
    {
        private string _connectionstring;
        private const string _getAllUserQuery = "SELECT * FROM UserList";


        public SuperAdminUser(string connectionstring)
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

        public void PrintAllUsers()
        {
            List<UserDTO> users = GetAllUsers();
            foreach(var user in users)
            {
                Console.WriteLine($"Username: {user.username}   Password: {user.password}");
            }
        }

        public bool InsertUser(string username, string password)
        {
            const string InsertUserIntoDatabase = "INSERT INTO UserList(Username, Password) VALUES(@Username, @Password)";
            List<UserDTO> users = GetAllUsers();
            bool UserExist = users.Any(i => i.username == username);
            if (UserExist)
            {
                return false;
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
                {
                    using (SqlCommand command = new SqlCommand(InsertUserIntoDatabase, sqlConnection))
                    {
                        sqlConnection.Open();
                        command.Parameters.AddWithValue("Username", username);
                        command.Parameters.AddWithValue("Password", password);
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
        }

        public bool DeleteUser(string username)
        {
            string DeletetUserFromDatabase = "DELETE FROM UserList WHERE Username = @Username";
            List<UserDTO> users = GetAllUsers();
            bool UserExist = users.Any(i => i.username == username);
            if (UserExist)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
                {
                    using (SqlCommand command = new SqlCommand(DeletetUserFromDatabase, sqlConnection))
                    {
                        sqlConnection.Open();
                        command.Parameters.AddWithValue("Username", username);
                        command.ExecuteNonQuery();
                    }
                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public void ChangePassword(string username, string newpassword)
        {
            string UpdatePassword = "UPDATE UserList SET Password = @newpassword WHERE Username = @Username";
            List<UserDTO> users = GetAllUsers();
            bool UserExist = users.Any(i => i.username == username);
            if (UserExist)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionstring))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand(UpdatePassword, sqlConnection))
                    {
                        command.Parameters.AddWithValue("newpassword", newpassword);
                        command.Parameters.AddWithValue("Username", username);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Password updated!");
            }
            else
            {
                Console.WriteLine("Username does not exist.");
            }
        }

    }
}
