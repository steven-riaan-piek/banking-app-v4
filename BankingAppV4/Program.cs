using System;
using System.Collections.Generic;
using System.IO;

class Program
{

    //I upgraded my banking system by adding user registration and login with
    //password authentication, secure input handling, and persistent user data storage for multiple accounts.

    //Firstly i added authentication (login/register)
    //Introduced basic security concepts(password handling)
    //System now feels like a real application

    static Dictionary<string, BankAccount> accounts = new Dictionary<string, BankAccount>();

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Banking App ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    Console.ReadKey();
                    break;
            }
        }
    }

   
    static void Login()
    {
        Console.Clear();
        Console.WriteLine("Enter username:");
        string name = Console.ReadLine().Trim().ToLower();

        if (!accounts.ContainsKey(name))
        {
            accounts[name] = new BankAccount(name, "");
        }

        Console.WriteLine("Enter password:");
        string password = ReadPassword();

        if (!accounts[name].CheckPassword(password))
        {
            Console.WriteLine("Wrong password!");
            Console.ReadKey();
            return;
        }

        RunMenu(name, accounts[name]);
    }

  
    static void Register()
    {
        Console.Clear();
        Console.WriteLine("Choose username:");
        string name = Console.ReadLine().Trim().ToLower();

        if (File.Exists(name + "_data.txt"))
        {
            Console.WriteLine("User already exists!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Create password:");
        string password = ReadPassword();

        BankAccount account = new BankAccount(name, password);
        account.SaveData();

        accounts[name] = account;

        Console.WriteLine("Account created successfully!");
        Console.ReadKey();
    }

    
    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        while (true)
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
            else
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password;
    }

    
    static void RunMenu(string name, BankAccount account)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Welcome {name}");
            Console.WriteLine("1. Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. History");
            Console.WriteLine("5. Transfer");
            Console.WriteLine("6. Logout");

            string option = Console.ReadLine();

            try
            {
                switch (option)
                {
                    case "1":
                        Console.WriteLine($"Balance: {account.Balance:C2}");
                        break;

                    case "2":
                        Console.WriteLine("Amount?");
                        decimal deposit = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine(account.Deposit(deposit) ? "Deposit successful" : "Invalid amount");
                        break;

                    case "3":
                        Console.WriteLine("Amount?");
                        decimal withdraw = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine(account.Withdraw(withdraw) ? "Withdrawal successful" : "Invalid amount");
                        break;

                    case "4":
                        Console.WriteLine("Transaction History:");
                        account.ShowHistory();
                        break;

                    case "5":
                        Console.WriteLine("Recipient username:");
                        string recipientName = Console.ReadLine().ToLower();

                       
                        if (!File.Exists(recipientName + "_data.txt"))
                        {
                            Console.WriteLine("User does not exist!");
                            break;
                        }
                        if (recipientName.ToLower() == name.ToLower())
                        {
                            Console.WriteLine("You cannot transfer money to yourself!");
                            break;
                        }

                       
                        if (!accounts.ContainsKey(recipientName))
                        {
                            accounts[recipientName] = new BankAccount(recipientName, "");
                        }

                        Console.WriteLine("Amount:");
                        decimal amount = Convert.ToDecimal(Console.ReadLine());

                        if (account.TransferTo(accounts[recipientName], amount))
                            Console.WriteLine("Transfer successful");
                        else
                            Console.WriteLine("Transfer failed");

                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }

                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("Invalid input");
                Console.ReadKey();
            }
        }
    }
}