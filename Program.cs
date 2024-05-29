using System;
using System.Data.SQLite;



namespace ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {

        string sqlString = "Data Source=identifier.sqlite;Version=3;";
        Console.WriteLine("Main Menu");
        Console.WriteLine("Select option");
        Console.WriteLine("1. Insert");
        Console.WriteLine("2. Delete");
        Console.WriteLine("3. Update");
        Console.WriteLine("4. View");
        int optionSelected = Convert.ToInt32(Console.ReadLine());
        switch (optionSelected)
        {
            case 1:
                InsertData(sqlString);
                break;
            case 2:
                DeleteData(sqlString);
                break;
            case 3:
                UpdateData(sqlString);
                break;
            case 4:
                ReadData(sqlString);
                break;
        }
        
       
    }

    public static void InsertData(string sqlString)
    {
        Console.WriteLine("What habit");
        string insert = Console.ReadLine();
        using (SQLiteConnection conn = new SQLiteConnection(sqlString))
        {
            conn.Open();
            string cmd = "INSERT INTO main.habits(Habit) VALUES(@habit)";
            
            using (SQLiteCommand command = new SQLiteCommand(cmd,conn))
            {
                command.Parameters.AddWithValue("@habit", insert);
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteData(string sqlString)
    {
        
    }

    public static void UpdateData(string sqlString)
    {
        
    }
    public static void ReadData(string sqlString)
    {
        using (SQLiteConnection conn = new SQLiteConnection(sqlString))
        {
            conn.Open();
            var sqlCommand = "SELECT * FROM Habits";
            using (SQLiteCommand command = new SQLiteCommand(sqlCommand,conn))
            {
                using (SQLiteDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        Console.WriteLine(rdr["Habit"]);
                    }
                }
            }
        }
    } 
}