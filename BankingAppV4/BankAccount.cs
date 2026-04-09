class BankAccount
{
    public decimal Balance { get; private set; }
    public List<string> History { get; private set; }

    private string filePath;
    private string password;

    public BankAccount(string userName, string passwordInput)
    {
        filePath = userName + "_data.txt";
        History = new List<string>();

        if (File.Exists(filePath))
        {
            LoadData();
        }
        else
        {
            password = passwordInput;
        }
    }

    public bool CheckPassword(string input)
    {
        return password == input;
    }

    public bool Deposit(decimal amount)
    {
        if (amount <= 0) return false;

        Balance += amount;
        History.Add($"Deposited: {amount:C2} on {DateTime.Now}");
        SaveData();
        return true;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance) return false;

        Balance -= amount;
        History.Add($"Withdrew: {amount:C2} on {DateTime.Now}");
        SaveData();
        return true;
    }

    public void ShowHistory()
    {
        foreach (var item in History)
            Console.WriteLine(item);
    }

    public bool TransferTo(BankAccount recipient, decimal amount)
    {
        if (recipient == this)
            return false;

        if (amount <= 0 || amount > Balance ) return false;

        Balance -= amount;
        recipient.Balance += amount;

        string senderRecord = $"Transferred {amount:C2} to {recipient.GetName()} on {DateTime.Now}";
        string recieveRecord = $"Received {amount:C2} on {DateTime.Now}";

        History.Add(senderRecord);
        recipient.History.Add(recieveRecord);

        SaveData();
        recipient.SaveData();

        return true;
    }

    public string GetName()
    {
        return filePath.Replace("_data.txt", "");
    }

    public void SaveData()
    {
        List<string> lines = new List<string>();
        lines.Add(password); 
        lines.Add(Balance.ToString()); 
        lines.AddRange(History);

        File.WriteAllLines(filePath, lines);
    }

    public void LoadData()
    {
        var lines = File.ReadAllLines(filePath);

        if (lines.Length > 0)
            password = lines[0];

        if (lines.Length > 1)
            Balance = Convert.ToDecimal(lines[1]);

        for (int i = 2; i < lines.Length; i++)
            History.Add(lines[i]);
    }
}