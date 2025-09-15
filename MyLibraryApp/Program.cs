using System;
using System.Linq;
using logicc;

namespace ConsoleApp
{
    /// <summary>
    /// Основной класс приложения, предоставляющий пользовательский интерфейс для управления библиотекой.
    /// </summary>
    class Program
    {
        static Logic logic = new Logic();

        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        static void Main(string[] args)
        {
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
                Console.WriteLine("7. Выход");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        DeleteBook();
                        break;
                    case "3":
                        ReadBook();
                        break;
                    case "4":
                        UpdateBook();
                        break;
                    case "5":
                        ShowAllBooks();
                        break;
                    case "6":
                        ShowGroupedBooks();
                        break;
                    case "7":
                        Console.WriteLine("Завершение работы...");
                        return;
                    default:
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        /// <summary>
        /// Добавляет новую книгу в библиотеку с проверкой вводимых данных.
        /// </summary>
        static void AddBook()
        {
            Console.Write("Введите ID книги: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("❌ ID должен быть положительным числом.");
                return;
            }

            if (logic.ReadBook(id) != null)
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

            try
            {
                logic.CreateBook(id, title, author, genre, year, quantity);
                Console.WriteLine("✅ Книга успешно добавлена!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет книгу из библиотеки по идентификатору.
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
        /// Ищет книгу по идентификатору и выводит информацию о ней.
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
        /// Обновляет информацию о книге по идентификатору с проверкой данных.
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
        /// Выводит список всех книг в библиотеке.
        /// </summary>
        static void ShowAllBooks()
        {
            var all = logic.GetAllBooks();
            if (all.Count == 0)
            {
                Console.WriteLine("📚 Библиотека пуста.");
                return;
            }

            Console.WriteLine("\n📋 Все книги:");
            foreach (var book in all)
                Console.WriteLine($"  {book}");
        }

        /// <summary>
        /// Выводит список книг, сгруппированных по жанрам.
        /// </summary>
        static void ShowGroupedBooks()
        {
            var grouped = logic.GroupBooksByGenre();
            if (grouped.Count == 0)
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
        /// Получает корректное название книги.
        /// </summary>
        /// <returns>Название или null при ошибке.</returns>
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
        /// Получает корректного автора (без цифр и спецсимволов, только буквы и пробелы).
        /// </summary>
        /// <returns>Автор или null при ошибке.</returns>
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
        /// Проверяет, что строка содержит только буквы и пробелы.
        /// </summary>
        /// <param name="name">Строка для проверки.</param>
        /// <returns>True, если допустимо.</returns>
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
        /// Получает корректный жанр из списка.
        /// </summary>
        /// <returns>Жанр или null при ошибке.</returns>
        static string GetValidGenre()
        {
            string[] validGenres = logic.GetAvailableGenres();

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
        /// Получает корректный год издания (от 1000 до 2025).
        /// </summary>
        /// <returns>Год или 0 при ошибке.</returns>
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
        /// Получает корректное количество книг (неотрицательное число).
        /// </summary>
        /// <returns>Количество или -1 при ошибке.</returns>
        static int GetValidQuantity()
        {
            Console.Write("Введите количество книг: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("❌ Некорректное количество.");
                return -1;
            }

            if (quantity <= 0)
            {
                Console.WriteLine("❌ Количество должно быть положительным.");
                return -1;
            }

            return quantity;
        }
    }
}