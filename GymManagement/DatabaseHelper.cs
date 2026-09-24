using System;
using System.IO;
using System.Data;
using Microsoft.Data.Sqlite;

namespace GymManagement
{
    public static class DatabaseHelper
    {
        private static readonly string DbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gym.db");
        public static readonly string ConnectionString = $"Data Source={DbFile};";

        public static void InitializeDatabase()
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();

                string schema = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT NOT NULL,
                        Email TEXT UNIQUE NOT NULL,
                        Password TEXT NOT NULL,
                        Role TEXT DEFAULT 'Admin',
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );

                    CREATE TABLE IF NOT EXISTS Members (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT NOT NULL,
                        Phone TEXT,
                        Email TEXT,
                        Gender TEXT,
                        JoinDate DATE DEFAULT CURRENT_DATE,
                        EmergencyContact TEXT,
                        Notes TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Memberships (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        DurationDays INTEGER NOT NULL,
                        Price DECIMAL(10,2) NOT NULL,
                        Description TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Subscriptions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        MemberId INTEGER NOT NULL,
                        MembershipId INTEGER NOT NULL,
                        StartDate DATE NOT NULL,
                        EndDate DATE NOT NULL,
                        Status TEXT DEFAULT 'Active',
                        FOREIGN KEY (MemberId) REFERENCES Members(Id),
                        FOREIGN KEY (MembershipId) REFERENCES Memberships(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Payments (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        MemberId INTEGER NOT NULL,
                        Amount DECIMAL(10,2) NOT NULL,
                        PaymentMethod TEXT DEFAULT 'Cash',
                        PaymentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        Remaining DECIMAL(10,2) DEFAULT 0,
                        ReceiptNo TEXT,
                        FOREIGN KEY (MemberId) REFERENCES Members(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Attendance (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        MemberId INTEGER NOT NULL,
                        CheckIn DATETIME DEFAULT CURRENT_TIMESTAMP,
                        CheckOut DATETIME,
                        FOREIGN KEY (MemberId) REFERENCES Members(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Trainers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FullName TEXT NOT NULL,
                        Specialty TEXT,
                        Phone TEXT,
                        Salary DECIMAL(10,2),
                        HireDate DATE DEFAULT CURRENT_DATE
                    );

                    CREATE TABLE IF NOT EXISTS WorkoutPlans (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        MemberId INTEGER NOT NULL,
                        ExerciseName TEXT NOT NULL,
                        DayOfWeek TEXT,
                        Sets INTEGER,
                        Reps INTEGER,
                        WeightKg DECIMAL(5,2),
                        FOREIGN KEY (MemberId) REFERENCES Members(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Expenses (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Category TEXT NOT NULL,
                        Amount DECIMAL(10,2) NOT NULL,
                        ExpenseDate DATE DEFAULT CURRENT_DATE,
                        Description TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Equipment (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Category TEXT,
                        Quantity INTEGER DEFAULT 1,
                        Status TEXT DEFAULT 'Working',
                        LastMaintenance DATE,
                        NextMaintenance DATE,
                        Notes TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT
                    );
                ";

                using (var cmd = new SqliteCommand(schema, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Seed Equipment if empty
                using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Equipment", conn))
                {
                    if (Convert.ToInt64(cmd.ExecuteScalar()) == 0)
                    {
                        string seedEq = @"
                            INSERT INTO Equipment (Name, Category, Quantity, Status, LastMaintenance, NextMaintenance, Notes) VALUES
                            ('مشاية كهربائية تجارية (Commercial Treadmill)', 'Cardio', 5, 'Working', date('now', '-30 days'), date('now', '+60 days'), 'حالة ممتازة - موتور AC 5 حصان'),
                            ('جهاز سحب عالي ولات بول داون (Lat Pulldown)', 'Strength', 2, 'Working', date('now', '-15 days'), date('now', '+75 days'), 'كابلات جديدة'),
                            ('دراجة سبيننج ثابتة (Spinning Bike)', 'Cardio', 6, 'Needs Maintenance', date('now', '-90 days'), date('now', '+5 days'), 'تحتاج ضبط الفرامل وتغيير البدال'),
                            ('سميث ماشين (Smith Machine)', 'Strength', 1, 'Working', date('now', '-10 days'), date('now', '+80 days'), 'تشحيم التروس الدورية'),
                            ('طقم دمبلز متكامل (Dumbbells Set 2.5kg - 50kg)', 'Free Weights', 1, 'Working', date('now', '-5 days'), date('now', '+180 days'), 'مكتمل بحالة جيدة');
                        ";
                        using (var ins = new SqliteCommand(seedEq, conn))
                        {
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // Seed Settings if empty
                using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Settings", conn))
                {
                    if (Convert.ToInt64(cmd.ExecuteScalar()) == 0)
                    {
                        string seedSet = @"
                            INSERT INTO Settings (Key, Value) VALUES
                            ('GymName', '⚡ FitFlow Fitness Club'),
                            ('Phone', '0100 123 4567'),
                            ('Address', 'شارع النصر - المعادي - القاهرة'),
                            ('Currency', 'EGP (جنيه مصري)'),
                            ('NotifyDaysBeforeExpiry', '7'),
                            ('AutoBackup', 'Enabled');
                        ";
                        using (var ins = new SqliteCommand(seedSet, conn))
                        {
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // Seed Default Admin
                using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Users WHERE Email = 'admin@fitness.com'", conn))
                {
                    if (Convert.ToInt64(cmd.ExecuteScalar()) == 0)
                    {
                        using (var ins = new SqliteCommand("INSERT INTO Users (FullName, Email, Password, Role) VALUES ('Emon Admin', 'admin@fitness.com', 'admin123', 'Super Admin')", conn))
                        {
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // Seed Memberships if empty
                using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Memberships", conn))
                {
                    if (Convert.ToInt64(cmd.ExecuteScalar()) == 0)
                    {
                        string seedPlans = @"
                            INSERT INTO Memberships (Title, DurationDays, Price, Description) VALUES 
                            ('شهري (Monthly)', 30, 450, 'دخول صالة الحديد وكارديو لمدة شهر'),
                            ('3 شهور (Quarterly)', 90, 1150, '3 شهور + استشارة تغذية مجانية'),
                            ('6 شهور (Semi-Annual)', 180, 2000, '6 شهور + دعوة أصدقاء مجانية'),
                            ('سنوي VIP (Annual)', 365, 3600, 'سنة كاملة + ساونا وجاكوزي ومدرب');
                        ";
                        using (var ins = new SqliteCommand(seedPlans, conn))
                        {
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // Seed Members if empty
                using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Members", conn))
                {
                    if (Convert.ToInt64(cmd.ExecuteScalar()) == 0)
                    {
                        string seedM = @"
                            INSERT INTO Members (FullName, Phone, Email, Gender) VALUES 
                            ('أحمد محمد صقر', '01012345678', 'ahmed@gym.com', 'Male'),
                            ('محمود إبراهيم الشناوي', '01198765432', 'mahmoud@gym.com', 'Male'),
                            ('سارة خالد التميمي', '01255566677', 'sara@gym.com', 'Female'),
                            ('عمر طارق زهران', '01533344455', 'omar@gym.com', 'Male'),
                            ('كريم عادل الباشا', '01088899900', 'karim@gym.com', 'Male');

                            INSERT INTO Subscriptions (MemberId, MembershipId, StartDate, EndDate, Status) VALUES
                            (1, 4, date('now', '-30 days'), date('now', '+335 days'), 'Active'),
                            (2, 2, date('now', '-10 days'), date('now', '+80 days'), 'Active'),
                            (3, 1, date('now', '-25 days'), date('now', '+5 days'), 'Active'),
                            (4, 1, date('now', '-60 days'), date('now', '-30 days'), 'Expired'),
                            (5, 3, date('now', '-5 days'), date('now', '+175 days'), 'Active');

                            INSERT INTO Payments (MemberId, Amount, PaymentMethod, Remaining, ReceiptNo) VALUES
                            (1, 3600, 'Credit Card', 0, 'REC-00101'),
                            (2, 1150, 'Cash', 0, 'REC-00102'),
                            (3, 450, 'Vodafone Cash', 0, 'REC-00103'),
                            (5, 2000, 'InstaPay', 0, 'REC-00104');

                            INSERT INTO Trainers (FullName, Specialty, Phone, Salary) VALUES
                            ('كابتن مصطفى كمال', 'كمال أجسام ورفع أثقال', '01011122233', 8500),
                            ('كابتن رامي السبيعي', 'لياقة بدنية وتخسيس', '01222233344', 9000),
                            ('كابتن ندى حامد', 'كروس فيت وكارديو', '01133344455', 7500);

                            INSERT INTO Expenses (Category, Amount, Description) VALUES
                            ('كهرباء وتكييف', 3200, 'فاتورة كهرباء الصالة لشهر سبتمبر'),
                            ('إيجار المقر', 15000, 'إيجار مقر الجيم الرئيسي'),
                            ('صيانة أجهزة', 1850, 'صيانة سيور المشايات وأجهزة الكابل');

                            INSERT INTO Attendance (MemberId, CheckIn) VALUES
                            (1, datetime('now', '-2 hours')),
                            (2, datetime('now', '-1 hours')),
                            (5, datetime('now', '-30 minutes'));
                        ";
                        using (var ins = new SqliteCommand(seedM, conn))
                        {
                            ins.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static DataTable ExecuteQuery(string query, params SqliteParameter[] parameters)
        {
            var dt = new DataTable();
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public static int ExecuteNonQuery(string query, params SqliteParameter[] parameters)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string query, params SqliteParameter[] parameters)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqliteCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static bool ValidateUser(string email, string password, out string fullName, out string role)
        {
            fullName = "";
            role = "";

            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                string query = "SELECT FullName, Role FROM Users WHERE LOWER(Email) = LOWER(@Email) AND Password = @Password LIMIT 1";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email.Trim());
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fullName = reader["FullName"]?.ToString() ?? "";
                            role = reader["Role"]?.ToString() ?? "";
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
