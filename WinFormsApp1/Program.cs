using System;
using System.Windows.Forms;
using logicc;
using DataAccessLayer;

namespace WinFormsApp1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Выберите нужный репозиторий при создании экземпляра Logic
            // Например, для Entity Framework:
            // var repository = new EntityRepository<Model.Book>(new LibraryDbContext());
            // Для Dapper:
            var repository = new DapperRepository<Model.Book>("Data Source=(localdb)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\РС\\Source\\Repos\\MyLibraryApp\\MyLibraryApp\\Database1.mdf;Integrated Security=True;");

            var logic = new Logic();
            Application.Run(new test());
        }
    }
}