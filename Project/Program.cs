using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionstring = @"Server=.\SQLExpress;Database=UniPiChat;Integrated Security=SSPI;";
            Register register = new Register(connectionstring);
            User user = new User(connectionstring);
            SuperAdminUser superAdminUser = new SuperAdminUser(connectionstring);
            SuperAdminMessage superAdminMessage = new SuperAdminMessage(connectionstring);
            var FileUser = new FileInputUser("UserAction.txt");
            var FileMessage = new FileInputMessage("MessageAction.txt");

            Console.WriteLine("--------- WELCOME TO UNIPI CHAT ---------");
            Console.ReadKey();

            do
            {
                int Answer;
                int exit;
                do
                {
                    Console.Write("If you want to log in press 1 or press 2 for register: ");
                    Answer = int.Parse(Console.ReadLine());
                } while (Answer != 1 && Answer != 2);
                Console.Clear();
                if (Answer == 1)
                {
                    Console.Write("Insert your username: ");
                    string LogInName = Console.ReadLine();
                    Console.Write("Insert your password: ");
                    string LogInPass = Console.ReadLine();
                    Console.Clear();
                    FileUser.Create("LogIn", LogInName, LogInPass, DateTime.Now);
                    if (LogInName == "superadmin" && LogInPass == "superadmin")
                    {
                        Console.WriteLine("Welcome Super Admin!");
                        int d;
                        do
                        {
                            do
                            {
                                Console.WriteLine("Do you want info about 1)Users 2)Data? ");
                                d = int.Parse(Console.ReadLine());
                            } while (d != 1 && d != 2);
                            Console.Clear();
                            int action;
                            if (d == 1)
                            {
                                do
                                {
                                    Console.WriteLine("1)Create user 2)View User 3)Delete user 4)Update user");
                                    Console.Write("Choose your action: ");
                                    action = int.Parse(Console.ReadLine());
                                }
                                while (action < 0 && action > 4);
                                Console.Clear();
                                if (action == 1)
                                {
                                    Console.WriteLine("If you want to create an admin he just must Log In with username: admin and password: admin.");
                                    bool UserDone;
                                    Console.WriteLine("Insert Username: ");
                                    string username = Console.ReadLine();
                                    Console.WriteLine("Insert Password: ");
                                    string password = Console.ReadLine();
                                    if (username != "admin" && password != "admin")
                                    {
                                        UserDone = superAdminUser.InsertUser(username, password);
                                        if (UserDone)
                                        {
                                            Console.WriteLine("Succefully registered.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Username already exists");
                                        }
                                    }
                                    Console.WriteLine("Press any key to continue.");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                else if (action == 2)
                                {
                                    superAdminUser.PrintAllUsers();
                                    Console.WriteLine("Press any key to continue.");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                else if (action == 3)
                                {
                                    Console.Write("Give username for delete: ");
                                    string username = Console.ReadLine();
                                    bool UserDeleted = superAdminUser.DeleteUser(username);
                                    if (UserDeleted)
                                    {
                                        Console.WriteLine("User Deleted.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Username.");
                                    }
                                    Console.WriteLine("Press any key to continue.");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                else
                                {
                                    int choise;
                                    do
                                    {
                                        Console.WriteLine("1)Change Password  2)Make User Admin");
                                        choise = int.Parse(Console.ReadLine());
                                    } while (choise < 1 && choise > 2);
                                    Console.Clear();
                                    if (choise == 1)
                                    {
                                        Console.Write("Give username: ");
                                        string username = Console.ReadLine();
                                        Console.Write("Give new password: ");
                                        string newpassword = Console.ReadLine();
                                        superAdminUser.ChangePassword(username, newpassword);
                                    }
                                    else if (choise == 2)
                                    {
                                        Console.Write("Give username to make him admin: ");
                                        string username = Console.ReadLine();
                                        bool UserDeleted = superAdminUser.DeleteUser(username);
                                        if (UserDeleted)
                                        {
                                            Console.WriteLine("User Admin.");
                                            Console.WriteLine("New username: admin   New password: admin");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid Username.");
                                        }
                                        Console.WriteLine("Press any key to continue.");
                                        Console.ReadKey();
                                    }
                                    Console.Clear();
                                }
                                Console.Clear();
                            }
                            else
                            {
                                do
                                {
                                    Console.WriteLine("1)View Data 2)Edit Data 3)Delete Data");
                                    Console.Write("Choose your action: ");
                                    action = int.Parse(Console.ReadLine());
                                } while (action < 0 && action > 3);
                                Console.Clear();
                                if (action == 1)
                                {
                                    Console.Write("Give username to bring the data: ");
                                    string username = Console.ReadLine();
                                    superAdminMessage.ViewMessages(username);
                                    Console.WriteLine("Press any key to continue.");
                                    Console.ReadKey();
                                }
                                else if (action == 2)
                                {
                                    Console.Write("Give Mesagge ID to change it: ");
                                    int ID = int.Parse(Console.ReadLine());
                                    string message;
                                    do
                                    {
                                        Console.WriteLine("Insert the new message: ");
                                        message = Console.ReadLine();
                                        if (message.Length > 250)
                                        {
                                            Console.WriteLine("The message can't be over 250 characters.");
                                        }
                                    } while (message.Length > 250);
                                    superAdminMessage.EditMessage(message, ID);
                                }
                                else
                                {
                                    Console.Write("Give Message ID to delete it: ");
                                    int ID = int.Parse(Console.ReadLine());
                                    superAdminMessage.DeleteMessage(ID);
                                }
                                Console.Clear();
                            }
                            do
                            {
                                Console.WriteLine("Press 1 if you want to exit or 2 to continue");
                                exit = int.Parse(Console.ReadLine());
                                if (exit != 1 && exit != 2)
                                {
                                    Console.WriteLine("Invalid input");
                                }
                            } while (exit != 1 && exit != 2);
                                Console.Clear();
                        } while (exit == 2);
                    }
                    else if (LogInName == "admin" && LogInPass == "admin")
                    {
                        Console.WriteLine("Welcome Admin!");
                        int action;
                        do
                        {
                            do
                            {
                                Console.WriteLine("1)View Data 2)Edit Data");
                                Console.Write("Choose your action: ");
                                action = int.Parse(Console.ReadLine());
                            } while (action < 0 && action > 2);
                            Console.Clear();
                            if (action == 1)
                            {
                                Console.Write("Give username to bring the data: ");
                                string username = Console.ReadLine();
                                superAdminMessage.ViewMessages(username);
                                Console.WriteLine("Press any key to continue.");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Console.Write("Give Mesagge ID to change it: ");
                                int ID = int.Parse(Console.ReadLine());
                                string message;
                                do
                                {
                                    Console.WriteLine("Insert the new message: ");
                                    message = Console.ReadLine();
                                    if (message.Length > 250)
                                    {
                                        Console.WriteLine("The message can't be over 250 characters.");
                                    }
                                } while (message.Length > 250);
                                superAdminMessage.EditMessage(message, ID);
                            }
                            Console.Clear();
                            do
                            {
                                Console.WriteLine("Press 1 if you want to exit or 2 to continue");
                                exit = int.Parse(Console.ReadLine());
                                if (exit != 1 && exit != 2)
                                {
                                    Console.WriteLine("Invalid input");
                                }
                            } while (exit != 1 && exit != 2);
                            Console.Clear();
                        } while (exit == 2);
                    }
                    else if (user.IsUserRegistered(LogInName,LogInPass))
                    {
                        Console.WriteLine($"Welcome {LogInName}!");
                        bool UserExist;
                        string Receiver;
                        do
                        {

                        
                        do
                        {
                            user.PrintAllUsers();
                            Console.Write("Give receivers username to open the chatbox: ");
                            Receiver = Console.ReadLine();
                            UserExist = user.UserExist(Receiver);
                            if (UserExist == false)
                            {
                                Console.WriteLine("Username does not exists.");
                            }
                            Console.Clear();
                        } while (UserExist == false);
                        user.BringTheMessages(Receiver, LogInName);
                        string message;
                        do
                        {
                            Console.WriteLine("Insert your new message: ");
                            message = Console.ReadLine();
                            if (message.Length > 250)
                            {
                                Console.WriteLine("The message can't be over 250 characters.");
                            }
                        } while (message.Length > 250);
                        bool MessageSent = user.InputTheData(Receiver, LogInName, message, DateTime.Now);
                        Console.Clear();
                        if (MessageSent)
                        {
                            Console.WriteLine("Message sent!");
                            FileMessage.Create(DateTime.Now, LogInName, Receiver, message);
                        }
                        Console.Clear();
                            do
                            {
                                Console.WriteLine("Press 1 if you want to exit or 2 to continue");
                                exit = int.Parse(Console.ReadLine());
                                if (exit != 1 && exit != 2)
                                {
                                    Console.WriteLine("Invalid input");
                                }
                            } while (exit != 1 && exit != 2);
                            Console.Clear();
                        } while (exit == 2);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input!");
                    }
                }
                else
                {
                    bool RegisterExist;
                    do
                    {
                        Console.Write("Insert your username: ");
                        string username = Console.ReadLine();
                        Console.Write("Insert your password: ");
                        string password = Console.ReadLine();
                        RegisterExist = register.InsertUser(username, password);
                        FileUser.Create("Register", username, password, DateTime.Now);
                        if (RegisterExist == false)
                        {
                            Console.WriteLine("Username already exists!");
                        }
                    } while (RegisterExist == false);
                    Console.Clear();
                    if (RegisterExist)
                    {
                        Console.WriteLine("SUCCESFULLY REGISTER");
                    }
                }
            } while (true);
        }
    }
}
