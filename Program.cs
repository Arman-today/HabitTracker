using System.Data.SQLite;
using Microsoft.IdentityModel.Tokens;

namespace ConsoleApp1;

internal class Program
{
    private static readonly string SqlString = "Data Source=LocalDB.sqlite;Version=3;";

    private static void Main(string[] args)
    {
        try
        {
            MainMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error has occured!: " + e.Message);
        }
    }

    /// <summary>
    ///     Main menu with options for different actions
    /// </summary>
    private static void MainMenu()
    {
        var conn = new SQLiteConnection(SqlString);
        CreateTable(conn);
        while (true)
        {
            Console.WriteLine("------------Main Menu------------");
            Console.WriteLine("Select option");
            Console.WriteLine("0. Close Application");
            Console.WriteLine("1. Insert");
            Console.WriteLine("2. Delete");
            Console.WriteLine("3. Update");
            Console.WriteLine("4. View");
            var input = Console.ReadLine();
            int optionSelected;
            if (!int.TryParse(input, out optionSelected))
            {
                Console.WriteLine("Please write a number instead of a string!");
                continue;
            }

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
                    ReadDataMenu(conn);
                    break;
                default:
                    Console.WriteLine("Invalid number, choose one from the list!");
                    break;
            }
        }
    }

    /// <summary>
    ///     Creates table if not already existent
    /// </summary>
    /// <param name="conn">SQL Connection</param>
    private static void CreateTable(SQLiteConnection conn)
    {
        var query =
            "CREATE TABLE IF NOT EXISTS Habits(ID INTEGER PRIMARY KEY AUTOINCREMENT, Habit TEXT NOT NULL, Date TEXT NOT NULL, Time TEXT NOT NULL);";
        using (var command = new SQLiteCommand(query, conn))
        {
            conn.Open();
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    ///     Inserts data into the table
    /// </summary>
    /// <param name="conn">SQL Connection</param>
    private static void InsertData(SQLiteConnection conn)
    {
        Console.WriteLine("What habit should be added");
        var input = Console.ReadLine();
        if (input.IsNullOrEmpty())
        {
            Console.WriteLine("Input invalid");
            return;
        }

        var query = "INSERT INTO Habits(Habit, Date, Time) VALUES(@habit, date(), time())";
        using (var command = new SQLiteCommand(query, conn))
        {
            command.Parameters.AddWithValue("@habit", input);
            command.ExecuteNonQuery();
        }

        Console.WriteLine("Successfully inserted");
    }

    /// <summary>
    ///     Deletes a dataset and returns feedback on action
    /// </summary>
    /// <param name="conn">SQL Connection</param>
    private static void DeleteData(SQLiteConnection conn)
    {
        Console.WriteLine("Please specify the ID of the dataset to deleted");
        var input = Console.ReadLine();
        int datasetId;
        if (!int.TryParse(input, out datasetId)) Console.WriteLine("Please write a number instead of a string!");
        var query = "DELETE FROM Habits WHERE ID=@Id";
        using (var command = new SQLiteCommand(query, conn))
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
    }


    /// <summary>
    ///     Updates an exsisting dataset and returns feedback on actions
    /// </summary>
    /// <param name="conn">SQL Connection</param>
    private static void UpdateData(SQLiteConnection conn)
    {
        Console.WriteLine("Please specify the ID of the dataset to update");
        var input = Console.ReadLine();
        int datasetId;
        if (!int.TryParse(input, out datasetId)) Console.WriteLine("Please write a number instead of a string!");
        Console.WriteLine("Replace with:");
        var wordToReplaceWith = Console.ReadLine();
        if (wordToReplaceWith.IsNullOrEmpty())
        {
            Console.WriteLine("Invalid input!");
            return;
        }

        var query = "UPDATE Habits SET Habit=@word WHERE ID=@Id";
        using (var command = new SQLiteCommand(query, conn))
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
    }

    /// <summary>
    ///     Menu with different options of displaying Data from db
    /// </summary>
    /// <param name="conn">SQL Connection</param>
    private static void ReadDataMenu(SQLiteConnection conn)
    {
        var isOperationOngoing = true;
        while (isOperationOngoing)
        {
            Console.WriteLine("How should the Data be displayed?");
            Console.WriteLine("0. All records");
            Console.WriteLine("1. Grouped by habits");
            Console.WriteLine("2. Grouped by dates");
            var input = Console.ReadLine();
            int optionSelected;
            if (!int.TryParse(input, out optionSelected))
            {
                Console.WriteLine("Please write a number instead of a string!");
            }
            else
            {
                GetData(conn, optionSelected);
                isOperationOngoing = false;
            }
        }
    }

    /// <summary>
    ///     Gets the data with the rdr and calls upon method DisplayData
    /// </summary>
    /// <param name="conn">SQL Connection</param>
    /// <param name="optionSelected">Option selected in ReadDataMenu for the output method</param>
    private static void GetData(SQLiteConnection conn, int optionSelected)
    {
        var query = string.Empty;
        switch (optionSelected)
        {
            case 0:
                query = "SELECT ID, Habit, Date, Time FROM Habits";
                break;
            case 1:
                query = "SELECT Habits.Habit, COUNT(Habits.Habit) AS Count FROM Habits GROUP BY Habit";
                break;
            case 2:
                query =
                    "SELECT Habit, COUNT(Habit) AS Count, DATE  FROM Habits GROUP BY DATE, Habit ORDER BY DATE";
                break;
            default:
                Console.WriteLine("Invalid input!");
                return;
        }

        using (var command = new SQLiteCommand(query, conn))
        {
            var rdr = command.ExecuteReader();
            DisplayData(conn, rdr, optionSelected);
        }
    }

    /// <summary>
    ///     Displays data from db
    /// </summary>
    /// <param name="conn">SQL Connection</param>
    /// <param name="rdr">Data rdr for reading the data</param>
    /// <param name="optionSelected">Option selected in ReadDataMenu for the output method</param>
    private static void DisplayData(SQLiteConnection conn, SQLiteDataReader rdr, int optionSelected)
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