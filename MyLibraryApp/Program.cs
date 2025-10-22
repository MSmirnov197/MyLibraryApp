using System;
using System.Linq;
using logicc;


namespace ConsoleApp
{
    class Program
    {
        static Logic logic;

        static void Main(string[] args)
        {
           
            logic = new Logic();

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
                        Console.WriteLine("Завершение работы...");
                        return;
                    default:
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

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

                logic.CreateBook(id, title, author, genre, year, quantity);
                Console.WriteLine("✅ Книга успешно добавлена!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
            }
        }

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

        static bool IsValidName(string name)
        {
            foreach (char c in name)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                    return false;
            }
            return true;
        }

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