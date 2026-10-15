using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Lab_04
{
    internal static class Database
    {
        private static readonly string DataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Lab_04");

        public static string DatabasePath =>
            Path.Combine(DataDirectory, "users.db");

        private static string ConnectionString =>
            "Data Source=" + DatabasePath + ";Version=3";

        public static void Initialize()
        {
            Directory.CreateDirectory(DataDirectory);

            using(var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string createUsersTable = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        CNP TEXT NOT NULL UNIQUE
                    );";

                using(var command = new SQLiteCommand(createUsersTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool IsValidCnp(string cnp)
        {
            if (cnp == null || cnp.Length != 13)
                return false;

            for (int i = 0; i < cnp.Length; i++)
            {
                if (cnp[i] < '0' || cnp[i] > '9')
                    return false;
            }

            int sex = cnp[0] - '0';
            if (sex < 1 || sex > 9)
                return false;

            int year = int.Parse(cnp.Substring(1, 2));
            int month = int.Parse(cnp.Substring(3, 2));
            int day = int.Parse(cnp.Substring(5, 2));

            int fullYear;
            if (sex == 1 || sex == 2)
                fullYear = 1900 + year;
            else if (sex == 3 || sex == 4)
                fullYear = 1800 + year;
            else
                fullYear = 2000 + year;

            if (month < 1 || month > 12 ||
                day < 1 || day > DateTime.DaysInMonth(fullYear, month))
                return false;

            int county = int.Parse(cnp.Substring(7, 2));
            if ((county < 1 || county > 52) && county != 99)
                return false;

            const string weights = "279146358279";
            int sum = 0;

            for (int i = 0; i < 12; i++)
                sum += (cnp[i] - '0') * (weights[i] - '0');

            int checkDigit = sum % 11;
            if (checkDigit == 10)
                checkDigit = 1;

            return checkDigit == cnp[12] - '0';
        }

        public static void AddUser(string firstName, string lastName, string cnp)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
            INSERT INTO Users (FirstName, LastName, CNP)
            VALUES (@FirstName, @LastName, @CNP);";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@CNP", cnp);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static DataTable GetUsers()
        {
            var table = new DataTable();

            using(var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = @"
                    SELECT Id, FirstName, LastName, CNP
                    FROM Users
                    ORDER BY LastName, FirstName;";

                using(var adapter = new SQLiteDataAdapter(sql, connection))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        public static bool DeleteUser(string cnp)
        {
            using(var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = "DELETE FROM Users WHERE CNP = @CNP;";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CNP", cnp);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public static void DeleteUsersTable()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                const string sql = "DROP TABLE IF EXISTS Users;";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
