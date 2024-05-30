using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.Linq.Expressions;

namespace ConsoleApp1;

internal class Program
{
    private static readonly string SqlString = "Data Source=LocalDB.sqlite;Version=3;";
    private static void Main(string[] args)
    {
        MainMenu();
    }
/// <summary>
/// TODO
/// </summary>
    public static void MainMenu()
    {
        var conn = new SQLiteConnection(SqlString);
        CreateTable(conn);
        
        Console.WriteLine("------------Main Menu------------");
        Console.WriteLine("Select option");
        Console.WriteLine("0. Close Application");
        Console.WriteLine("1. Insert");
        Console.WriteLine("2. Delete");
        Console.WriteLine("3. Update");
        Console.WriteLine("4. View");
        int optionSelected = Convert.ToInt32(Console.ReadLine());
        switch (optionSelected)
        {
            case 0:
                Environment.Exit(0);
                break;
            case 1:
                InsertData(conn);
                break;
            case 2:
                DeleteData(conn);
                break;
            case 3:
                UpdateData(conn);
                break;
            case 4:
                ReadData(conn);
                break;
            default:
                Console.WriteLine("Invalid input!");
                MenuAfterOperation();
                break;
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="conn"></param>
    public static void CreateTable(SQLiteConnection conn)
    {
        string query = "CREATE TABLE IF NOT EXISTS Habits(ID INTEGER PRIMARY KEY AUTOINCREMENT, Habit TEXT NOT NULL, Date TEXT NOT NULL, Time TEXT NOT NULL);";
        conn.Open();
        SQLiteCommand command = new SQLiteCommand(query, conn);
        command.ExecuteNonQuery();
        conn.Close();
    }
/// <summary>
/// TODO
/// </summary>
/// <param name="conn"></param>
    public static void InsertData(SQLiteConnection conn)
    {
        Console.WriteLine("What habit should be added");
        var insert = Console.ReadLine();
        conn.Open();
        var query = "INSERT INTO Habits(Habit, Date, Time) VALUES(@habit, date(), time())";
        using (var command = new SQLiteCommand(query, conn))
        {
            command.Parameters.AddWithValue("@habit", insert);
            command.ExecuteNonQuery();
        }
        conn.Close();
        MenuAfterOperation();
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="conn"></param>
    public static void DeleteData(SQLiteConnection conn)
    {
        Console.WriteLine("Please specify the ID of the dataset to deleted");
        var datasetId = Convert.ToInt32(Console.ReadLine());
        conn.Open();
        var sqlCommand = "DELETE FROM Habits WHERE ID=@Id";
        using (var command = new SQLiteCommand(sqlCommand, conn))
        {
            command.Parameters.AddWithValue("@Id", datasetId);
            switch (command.ExecuteNonQuery())
            {
                case 0:
                    Console.WriteLine("ID invalid");
                    break;
                case 1:
                    Console.WriteLine("Dataset successfully deleted");
                    break;
                default:
                    Console.WriteLine("Multiple datasets deleted or an error has occured!");
                    break;
            }
            
        }
        conn.Close();
        {
            
        }
        MenuAfterOperation();
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="conn"></param>
    public static void UpdateData(SQLiteConnection conn)
    {
        Console.WriteLine("Please specify the ID of the dataset to update");
        var datasetId = Convert.ToInt32(Console.ReadLine());
        conn.Open();
        Console.WriteLine("Replace with:");
        string wordToReplaceWith = Console.ReadLine();
        var sqlCommand = "UPDATE Habits SET Habit=@word WHERE ID=@Id";
        using (var command = new SQLiteCommand(sqlCommand, conn))
        {
            command.Parameters.AddWithValue("@Id", datasetId);
            command.Parameters.AddWithValue("@word", wordToReplaceWith);
            switch (command.ExecuteNonQuery())
            {
                case 0:
                    Console.WriteLine("ID invalid");
                    break;
                case 1:
                    Console.WriteLine("Dataset successfully updated");
                    break;
                default:
                    Console.WriteLine("Multiple datasets updated or an error has occured!");
                    break;
            }
            
        }
        conn.Close();
        {
            
        }
        MenuAfterOperation();
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="conn"></param>
    public static void ReadData(SQLiteConnection conn)
    {
        var sqlCommand = String.Empty;
        Console.WriteLine("How should the Data be displayed?");
        Console.WriteLine("0. All records");
        Console.WriteLine("1. Grouped by habits");
        Console.WriteLine("2. Grouped by dates");
        int optionSelected = Convert.ToInt32(Console.ReadLine());
        conn.Open();
        switch (optionSelected)
        {
            case 0:
                sqlCommand = "SELECT * FROM Habits";
                break;
            case 1:
                sqlCommand = "SELECT Habits.Habit, COUNT(Habits.Habit) AS Count FROM Habits GROUP BY Habit";
                break;
            case 2:
                sqlCommand = "SELECT Habit, COUNT(Habit) AS Count, DATE  FROM Habits GROUP BY DATE, Habit ORDER BY DATE";
                break;
            default:
                Console.WriteLine("Invalid input!");
                MenuAfterOperation();
                break;
                
        }
        
        using (var command = new SQLiteCommand(sqlCommand, conn))
        {
            using (var rdr = command.ExecuteReader())
            {
                switch (optionSelected)
                {
                    case 0:
                        while (rdr.Read())
                        {
                            Console.Write("ID: " + rdr["ID"]);
                            Console.Write(" Habit: " + rdr["Habit"]);
                            Console.Write(" Date: " + rdr["Date"]);
                            Console.WriteLine(" Time: " + rdr["Time"]);
                        }
                        break;
                    case 1:
                        while (rdr.Read())
                        {
                            Console.Write("Habit: " + rdr["Habit"]);
                            Console.WriteLine(" Count: " + rdr["Count"]);
                        }
                        break;
                    case 2:
                        while (rdr.Read())
                        {
                            Console.Write("Habit: " + rdr["Habit"]);
                            Console.Write(" Count: " + rdr["Count"]);
                            Console.WriteLine(" Date: " + rdr["Date"]);
                            
                        }
                        break;
                }
                
            }
        }
        conn.Close();
        MenuAfterOperation();
    }
/// <summary>
/// TODO
/// </summary>
    public static void MenuAfterOperation()
    {
        Console.WriteLine("0. Back to Main Menu");
        Console.WriteLine("1. Close Application");
        int backToMain = Convert.ToInt32(Console.ReadLine());
        switch (backToMain)
        {
            case 0:
                MainMenu();
                break;
            case 1:
                Environment.Exit(0);
                break;
        }
    }
}