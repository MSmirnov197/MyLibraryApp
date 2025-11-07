using System;
using System.Windows.Forms;
using logicc;
// УБРАТЬ Ninject - используй простой конструктор

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

            // ПРОСТОЕ создание Logic без Ninject
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mike\source\repos\Araxis3\MyLibraryApp\DataAccesLayer\Database1.mdf;Integrated Security=True;";
            var repository = new DataAccessLayer.DapperRepository<Model.Book>(connectionString);
            var validator = new logicc.BookValidator();
            var mapper = new logicc.BookMapper();
            var logger = new logicc.FileLogger();
            var groupers = new logicc.IBookGrouper[]
            {
                new logicc.GenreGrouper(),
                new logicc.YearGrouper(),
                new logicc.AuthorGrouper()
            };

            var logic = new Logic(repository, validator, mapper, groupers, logger);

            Application.Run(new test(logic));
        }
    }
}