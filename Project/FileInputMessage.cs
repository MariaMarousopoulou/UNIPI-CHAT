using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project
{
    public class FileInputMessage
    {
        private string _path;

        public FileInputMessage(string path)
        {
            _path = path;
        }

        public void Create(DateTime date, string sender, string receiver, string message)
        {
            string[] lines = new string[] { $"{date}  From:{sender}  To:{receiver}  Message:{message}" };
            File.AppendAllLines(_path, lines);
        }
    }
}
