using System;
using System.Linq;
using logicc;
using Ninject;

namespace ConsoleApp
{
    /// <summary>
    /// Главный класс консольного приложения для управления библиотекой книг.
    /// Предоставляет пользовательский интерфейс для выполнения CRUD операций и аналитики.
    /// </summary>
    class Program
    {
        static Logic logic;

        /// <summary>
        /// Точка входа в консольное приложение.
        /// Инициализирует DI-контейнер и запускает главный цикл приложения.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        static void Main(string[] args)
        {
            // Создаем ядро Ninject и регистрируем зависимости
            IKernel ninjectKernel = new StandardKernel(new AdvancedConfigModule());

            // Получаем экземпляр Logic через контейнер
            logic = ninjectKernel.Get<Logic>();

            Console.WriteLine("📚 Приложение 'Библиотека' — Консольная версия");
            Console.WriteLine("============================================");

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу");
                Console.WriteLine("3. Найти книгу по ID");
                Console.WriteLine("4. Обновить книгу");
                Console.WriteLine("5. Показать все книги");
                Console.WriteLine("6. Группировка книг по жанру");
                Console.WriteLine("7. Поиск книг");
                Console.WriteLine("8. Расширенная группировка");
                Console.WriteLine("9. Выход");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBook();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "2":
                        DeleteBook();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        ReadBook();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "4":
                        UpdateBook();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "5":
                        ShowAllBooks();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "6":
                        ShowGroupedBooks();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "7":
                        SearchBooks();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "8":
                        ShowAdvancedGrouping();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "9":
                        Console.WriteLine("Завершение работы...");
                        return;
                    default:
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        /// <summary>
        /// Добавляет новую книгу в библиотеку после валидации введенных данных.
        /// </summary>
        static void AddBook()
        {
            try
            {
                Console.Write("Введите ID книги: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("❌ ID должен быть положительным числом.");
                    return;
                }

                var existingBook = logic.ReadBook(id);
                if (existingBook != null)
                {
                    Console.WriteLine("❌ Книга с таким ID уже существует.");
                    return;
                }

                string title = GetValidTitle();
                if (string.IsNullOrEmpty(title)) return;

                string author = GetValidAuthor();
                if (string.IsNullOrEmpty(author)) return;

                string genre = GetValidGenre();
                if (string.IsNullOrEmpty(genre)) return;

                int year = GetValidYear();
                if (year == 0) return;

                int quantity = GetValidQuantity();
                if (quantity == -1) return;

                var result = logic.CreateBook(id, title, author, genre, year, quantity);
                if (result.Success)
                {
                    Console.WriteLine("✅ Книга успешно добавлена!");
                }
                else
                {
                    Console.WriteLine($"❌ Ошибка: {result.Message}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет книгу из библиотеки по указанному идентификатору.
        /// </summary>
        static void DeleteBook()
        {
            Console.Write("Введите ID книги для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Некорректный ID.");
                return;
            }

            if (logic.DeleteBook(id))
                Console.WriteLine("🗑️ Книга удалена.");
            else
                Console.WriteLine("❌ Книга с таким ID не найдена.");
        }

        /// <summary>
        /// Находит и отображает информацию о книге по указанному идентификатору.
        /// </summary>
        static void ReadBook()
        {
            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Некорректный ID.");
                return;
            }

            var book = logic.ReadBook(id);
            if (book != null)
                Console.WriteLine($"📖 Найдена: {book}");
            else
                Console.WriteLine("❌ Книга не найдена.");
        }

        /// <summary>
        /// Обновляет информацию о существующей книге.
        /// </summary>
        static void UpdateBook()
        {
            Console.Write("Введите ID книги для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Некорректный ID.");
                return;
            }

            var book = logic.ReadBook(id);
            if (book == null)
            {
                Console.WriteLine("❌ Книга с таким ID не найдена.");
                return;
            }

            string title = GetValidTitle();
            if (string.IsNullOrEmpty(title)) return;

            string author = GetValidAuthor();
            if (string.IsNullOrEmpty(author)) return;

            string genre = GetValidGenre();
            if (string.IsNullOrEmpty(genre)) return;

            int year = GetValidYear();
            if (year == 0) return;

            int quantity = GetValidQuantity();
            if (quantity == -1) return;

            if (logic.UpdateBook(id, title, author, genre, year, quantity))
                Console.WriteLine("✅ Книга обновлена.");
            else
                Console.WriteLine("❌ Не удалось обновить книгу.");
        }

        /// <summary>
        /// Отображает все книги, находящиеся в библиотеке.
        /// </summary>
        static void ShowAllBooks()
        {
            var all = logic.GetAllBooks();
            if (all == null || all.Count == 0)
            {
                Console.WriteLine("📚 Библиотека пуста.");
                return;
            }

            Console.WriteLine("\n📋 Все книги:");
            foreach (var book in all)
                Console.WriteLine($"  {book}");
        }

        /// <summary>
        /// Группирует и отображает книги по жанрам.
        /// Демонстрирует принцип единственной ответственности - отдельный метод для конкретного типа группировки.
        /// </summary>
        static void ShowGroupedBooks()
        {
            var grouped = logic.GroupBooksByGenre();
            if (grouped == null || grouped.Count == 0)
            {
                Console.WriteLine("📚 Нет книг для группировки.");
                return;
            }

            Console.WriteLine("\n📌 Группировка по жанрам:");
            foreach (var group in grouped)
            {
                Console.WriteLine($"  ➤ Жанр: {group.Key}");
                foreach (var book in group.Value)
                    Console.WriteLine($"      • {book}");
            }
        }

        /// <summary>
        /// Выполняет поиск книг по названию, автору или жанру.
        /// </summary>
        static void SearchBooks()
        {
            Console.Write("🔍 Введите поисковый запрос: ");
            var searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("❌ Поисковый запрос не может быть пустым.");
                return;
            }

            var results = logic.SearchBooks(searchTerm);
            if (results == null || results.Count == 0)
            {
                Console.WriteLine("📚 По вашему запросу ничего не найдено.");
                return;
            }

            Console.WriteLine($"\n🔍 Найдено {results.Count} книг:");
            foreach (var book in results)
                Console.WriteLine($"  {book}");
        }

        /// <summary>
        /// Предоставляет расширенную функциональность группировки книг по различным критериям.
        /// Демонстрирует принцип открытости/закрытости - система легко расширяется новыми стратегиями группировки.
        /// </summary>
        static void ShowAdvancedGrouping()
        {
            var availableGroupers = logic.GetAvailableGroupers();
            if (availableGroupers == null || !availableGroupers.Any())
            {
                Console.WriteLine("❌ Нет доступных группировок.");
                return;
            }

            Console.WriteLine("\n📊 Доступные группировки:");
            int index = 1;
            foreach (var grouper in availableGroupers)
            {
                Console.WriteLine($"  {index}. По {grouper}");
                index++;
            }

            Console.Write("Выберите тип группировки: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > availableGroupers.Count())
            {
                Console.WriteLine("❌ Неверный выбор.");
                return;
            }

            var groupKey = availableGroupers.ElementAt(choice - 1);
            var grouped = logic.GroupBooksBy(groupKey);

            if (grouped.Count == 0)
            {
                Console.WriteLine("📚 Нет книг для группировки.");
                return;
            }

            Console.WriteLine($"\n📌 Группировка по {groupKey}:");
            foreach (var group in grouped)
            {
                Console.WriteLine($"  ➤ {group.Key}:");
                foreach (var book in group.Value)
                    Console.WriteLine($"      • {book.Title} ({book.Author})");
            }
        }

        /// <summary>
        /// Получает и валидирует название книги от пользователя.
        /// </summary>
        /// <returns>Валидное название книги или null при ошибке.</returns>
        static string GetValidTitle()
        {
            Console.Write("Введите название: ");
            var title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("❌ Название не может быть пустым.");
                return null;
            }
            return title.Trim();
        }

        /// <summary>
        /// Получает и валидирует имя автора от пользователя.
        /// </summary>
        /// <returns>Валидное имя автора или null при ошибке.</returns>
        static string GetValidAuthor()
        {
            Console.Write("Введите автора: ");
            var author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("❌ Автор не может быть пустым.");
                return null;
            }

            if (!IsValidName(author))
            {
                Console.WriteLine("❌ Автор должен содержать только буквы и пробелы.");
                return null;
            }
            return author.Trim();
        }

        /// <summary>
        /// Проверяет, содержит ли строка только буквы и пробелы.
        /// </summary>
        /// <param name="name">Строка для проверки.</param>
        /// <returns>True, если строка содержит только допустимые символы.</returns>
        static bool IsValidName(string name)
        {
            foreach (char c in name)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Получает и валидирует жанр книги от пользователя.
        /// Предоставляет список доступных жанров для выбора.
        /// </summary>
        /// <returns>Валидный жанр книги или null при ошибке.</returns>
        static string GetValidGenre()
        {
            string[] validGenres = logic.GetAvailableGenres();
            if (validGenres == null || validGenres.Length == 0)
            {
                Console.WriteLine("❌ Нет доступных жанров.");
                return null;
            }

            Console.WriteLine("\n✅ Доступные жанры:");
            foreach (var genre in validGenres)
            {
                Console.WriteLine($"  • {genre}");
            }
            Console.Write("\nВведите жанр: ");

            var input = Console.ReadLine()?.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("❌ Жанр не может быть пустым.");
                return null;
            }

            if (!validGenres.Contains(input))
            {
                Console.WriteLine($"❌ Недопустимый жанр. Возможные варианты: {string.Join(", ", validGenres)}.");
                return null;
            }

            return input;
        }

        /// <summary>
        /// Получает и валидирует год издания книги от пользователя.
        /// </summary>
        /// <returns>Валидный год издания или 0 при ошибке.</returns>
        static int GetValidYear()
        {
            Console.Write("Введите год издания: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("❌ Некорректный год.");
                return 0;
            }

            if (year < 1000 || year > 2025)
            {
                Console.WriteLine("❌ Год должен быть от 1000 до 2025.");
                return 0;
            }

            return year;
        }

        /// <summary>
        /// Получает и валидирует количество экземпляров книги от пользователя.
        /// </summary>
        /// <returns>Валидное количество или -1 при ошибке.</returns>
        static int GetValidQuantity()
        {
            Console.Write("Введите количество книг: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("❌ Некорректное количество.");
                return -1;
            }

            if (quantity < 0)
            {
                Console.WriteLine("❌ Количество должно быть неотрицательным.");
                return -1;
            }

            return quantity;
        }
    }
}