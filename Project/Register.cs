using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Project
{
    public class Register
    {
        private string _connectionstring;
        private const string _getAllUserQuery = "SELECT * FROM UserList";


        public Register(string connectionstring)
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
                    }
                }
                return true;
            }
        }
    }
}
