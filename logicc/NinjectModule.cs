using DataAccessLayer;
using Microsoft.Extensions.Logging;
using Model;
using Ninject.Modules;

namespace logicc
{
    /// <summary>
    /// Модуль конфигурации Ninject для регистрации зависимостей.
    /// Реализует принцип инверсии зависимостей - централизованное управление зависимостями.
    /// Демонстрирует применение Dependency Injection Container для управления жизненным циклом объектов.
    /// </summary>
    public class AdvancedConfigModule : NinjectModule
    {
        /// <summary>
        /// Загружает привязки зависимостей в контейнер Ninject.
        /// Определяет соответствия между интерфейсами и их реализациями.
        /// </summary>
        public override void Load()
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mike\source\repos\Araxis3\MyLibraryApp\DataAccesLayer\Database1.mdf;Integrated Security=True;";

            // Репозитории - регистрация реализации репозитория с Singleton жизненным циклом
            Bind<IRepository<Book>>().To<DapperRepository<Book>>().InSingletonScope()
                .WithConstructorArgument("connectionString", connectionString);

            // Сервисы - регистрация сервисов с Singleton жизненным циклом для оптимального использования ресурсов
            Bind<IBookValidator>().To<BookValidator>().InSingletonScope();
            Bind<IBookMapper>().To<BookMapper>().InSingletonScope();
            Bind<ILogger>().To<FileLogger>().InSingletonScope();

            // Стратегии группировки - регистрация различных стратегий для демонстрации принципа открытости/закрытости
            Bind<IBookGrouper>().To<GenreGrouper>();
            Bind<IBookGrouper>().To<YearGrouper>();
            Bind<IBookGrouper>().To<AuthorGrouper>();

            // Бизнес-логика - регистрация основного класса Logic с Singleton жизненным циклом
            Bind<Logic>().ToSelf().InSingletonScope();
        }
    }
}