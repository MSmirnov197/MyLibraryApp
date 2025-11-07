using System.Text.RegularExpressions;

namespace Model
{
    /// <summary>
    /// Объект передачи данных (DTO) для сущности Book.
    /// Используется для передачи данных между слоями приложения, часто без прямой связи с базой данных.
    /// </summary>
    public class BookDTO
    {
        /// <summary>
        /// Уникальный идентификатор книги.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название книги.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Автор книги.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Жанр книги.
        /// </summary>
        public string? Genre { get; set; }

        /// <summary>
        /// Год издания книги.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Количество доступных экземпляров книги.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Возвращает строковое представление объекта <see cref="BookDTO"/>.
        /// </summary>
        /// <returns>Строка, описывающая книгу.</returns>
        public override string ToString()
        {
            return $"{Id}. {Title} ({Author}, {Genre}, {Year}, Количество: {Quantity})";
        }
    }

    /// <summary>
    /// Определяет интерфейс для объектов, которые представляют собой сущности предметной области.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Получает или задает уникальный идентификатор сущности.
        /// </summary>
        int Id { get; set; }
    }

    /// <summary>
    /// Представляет сущность "Книга" в предметной области.
    /// Включает валидацию данных при установке свойств.
    /// </summary>
    public class Book : IDomainObject
    {
        private string? _author;
        private string? _title;
        private string? _genre;
        private int _year;
        private int _quantity;

        /// <summary>
        /// Получает или задает уникальный идентификатор книги.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Получает или задает название книги.
        /// </summary>
        /// <exception cref="ArgumentException">Выбрасывается, если значение пустое или null.</exception>
        public string Title
        {
            get => _title ?? string.Empty;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название не может быть пустым");
                _title = value;
            }
        }

        /// <summary>
        /// Получает или задает автора книги.
        /// </summary>
        /// <exception cref="ArgumentException">Выбрасывается, если автор содержит недопустимые символы или пуст.</exception>
        public string Author
        {
            get => _author ?? string.Empty;
            set
            {
                if (!IsValidAuthor(value))
                    throw new ArgumentException("Автор должен содержать только буквы и пробелы");
                _author = value;
            }
        }

        /// <summary>
        /// Получает или задает жанр книги.
        /// </summary>
        /// <exception cref="ArgumentException">Выбрасывается, если жанр не соответствует списку допустимых жанров.</exception>
        public string Genre
        {
            get => _genre ?? string.Empty;
            set
            {
                if (!IsValidGenre(value))
                    throw new ArgumentException("Неверный жанр. Доступные жанры: drama, science fiction, adventure, novel, short story, detective, scientific literature");
                _genre = value;
            }
        }

        /// <summary>
        /// Получает или задает год издания книги.
        /// </summary>
        /// <exception cref="ArgumentException">Выбрасывается, если год находится вне допустимого диапазона (1000-2025).</exception>
        public int Year
        {
            get => _year;
            set
            {
                if (value < 1000 || value > 2025)
                    throw new ArgumentException("Год должен быть в диапазоне от 1000 до 2025");
                _year = value;
            }
        }

        /// <summary>
        /// Получает или задает количество доступных экземпляров книги.
        /// </summary>
        /// <exception cref="ArgumentException">Выбрасывается, если количество отрицательное.</exception>
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Количество книг не может быть отрицательным");
                _quantity = value;
            }
        }

        /// <summary>
        /// Проверяет, является ли строка автором допустимой.
        /// Допустимы буквы (латиница и кириллица), пробелы и дефисы.
        /// </summary>
        /// <param name="author">Строка для проверки.</param>
        /// <returns>True, если строка является допустимым автором, иначе False.</returns>
        private bool IsValidAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                return false;

            return Regex.IsMatch(author, @"^[a-zA-Zа-яА-ЯёЁ\s\-]+$");
        }

        /// <summary>
        /// Проверяет, является ли строка одним из допустимых жанров.
        /// Сравнение регистронезависимое.
        /// </summary>
        /// <param name="genre">Строка для проверки.</param>
        /// <returns>True, если строка является допустимым жанром, иначе False.</returns>
        private bool IsValidGenre(string genre)
        {
            string[] validGenres = { "drama", "science fiction", "adventure", "novel", "short story", "detective", "scientific literature" };
            return Array.Exists(validGenres, g => g.Equals(genre, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Возвращает строковое представление объекта <see cref="Book"/>.
        /// </summary>
        /// <returns>Строка, описывающая книгу.</returns>
        public override string ToString()
        {
            return $"{Id}. {Title} ({Author}, {Genre}, {Year}, Количество: {Quantity})";
        }
    }
}