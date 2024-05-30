using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.SqlTypes;

namespace ConsoleApp1;

internal class Program
{
    private static readonly string SqlString = "Data Source=LocalDB.sqlite;Version=3;";
    private static void Main(string[] args)
    {
        MainMenu();
    }

    public static void MainMenu()
    {
        var conn = new SQLiteConnection(SqlString);
        CreateTable(conn);
        
        Console.WriteLine("Main Menu");
        Console.WriteLine("Select option");
        Console.WriteLine("0. Close Application");
        Console.WriteLine("1. Insert");
        Console.WriteLine("2. Delete");
        Console.WriteLine("3. Update");
        Console.WriteLine("4. View");
        var optionSelected = Convert.ToInt32(Console.ReadLine());
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
        }
    }

    public static void CreateTable(SQLiteConnection conn)
    {
        string query = "CREATE TABLE IF NOT EXISTS Habits ( ID INT PRIMARY KEY NOT NULL,Habit TEXT NOT NULL, Date TEXT NOT NULL);";
        conn.Open();
        SQLiteCommand command = new SQLiteCommand(query, conn);
        command.ExecuteNonQuery();
        conn.Close();
    }

    public static void InsertData(SQLiteConnection conn)
    {
        Console.WriteLine("What habit should be added");
        var insert = Console.ReadLine();
        conn.Open();
        var query = "INSERT INTO Habits(Habit, Date) VALUES(@habit,datetime() )";
        using (var command = new SQLiteCommand(query, conn))
        {
            command.Parameters.AddWithValue("@habit", insert);
            command.ExecuteNonQuery();
        }
        MainMenu();
    }

    public static void DeleteData(SQLiteConnection conn)
    {
        Console.WriteLine("What habit");
        var insert = Console.ReadLine();

        conn.Open();
        var sqlCommand = "INSERT INTO Habits(ID, Habit, Date) VALUES(1 , @habit, datetime())";

        using (var command = new SQLiteCommand(sqlCommand, conn))
        {
            command.Parameters.AddWithValue("@habit", insert);
            command.ExecuteNonQuery();
        }
        conn.Close();
        MainMenu();
    }

    public static void UpdateData(SQLiteConnection conn)
    {
    }

    public static void ReadData(SQLiteConnection conn)
    {
        conn.Open();
        var sqlCommand = "SELECT * FROM Habits";
        using (var command = new SQLiteCommand(sqlCommand, conn))
        {
            using (var rdr = command.ExecuteReader())
            {
                while (rdr.Read())
                {
                    Console.WriteLine(rdr["ID"]);
                    Console.WriteLine(rdr["Habit"]);
                    Console.WriteLine(rdr["Date"]);
                }
            }
        }
        MainMenu();
    }
}