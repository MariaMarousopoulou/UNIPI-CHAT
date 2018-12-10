using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project
{
    public class FileInputUser
    {
        private string _path;

        public FileInputUser(string path)
        {
            _path = path;
        }

        public void Create(string action,string username,string password, DateTime date)
        {
            string[] lines = new string[] { $"{action}: {username}, {password}, {date}" };
            File.AppendAllLines(_path, lines);
        }
    }
}
