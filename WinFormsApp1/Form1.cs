using System;
using System.Windows.Forms;
using logicc;
using Model;

namespace WinFormApp
{
    /// <summary>
    /// Основная форма приложения для управления библиотекой.
    /// </summary>
    public partial class Form1 : Form
    {
        private Logic logic = new Logic();

        /// <summary>
        /// Инициализирует новый экземпляр класса Form1.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            LoadBooks();
            InitializeGenreComboBox();
        }

        private void InitializeGenreComboBox()
        {
            cmbGenre.Items.AddRange(logic.GetAvailableGenres());
            cmbGenre.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Загружает список книг в элемент управления listBoxBooks.
        /// </summary>
        private void LoadBooks()
        {
            listBoxBooks.DataSource = null;
            listBoxBooks.DisplayMember = "ToString";
            listBoxBooks.DataSource = logic.GetAllBooks();
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Добавить".
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text) || !int.TryParse(txtId.Text, out int id) || id <= 0)
                    throw new ArgumentException("ID должен быть положительным числом.");

                if (logic.ReadBook(id) != null)
                    throw new ArgumentException($"Книга с ID {id} уже существует.");

                ValidateInputs();

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
                    throw new ArgumentException("Количество книг должно быть неотрицательным числом.");

                logic.CreateBook(id, txtTitle.Text, txtAuthor.Text, cmbGenre.Text, int.Parse(txtYear.Text), quantity);

                MessageBox.Show("✅ Книга успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Удалить".
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedItem == null)
            {
                MessageBox.Show("❌ Выберите книгу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var book = (BookDTO)listBoxBooks.SelectedItem;
            if (MessageBox.Show($"Вы уверены, что хотите удалить книгу:\n{book.Title}?", "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (logic.DeleteBook(book.Id))
                {
                    MessageBox.Show("🗑️ Книга успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBooks();
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Просмотреть".
        /// </summary>
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedItem == null)
            {
                MessageBox.Show("❌ Выберите книгу для просмотра.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var book = (BookDTO)listBoxBooks.SelectedItem;
            txtId.Text = book.Id.ToString();
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            cmbGenre.Text = book.Genre;
            txtYear.Text = book.Year.ToString();
            txtQuantity.Text = book.Quantity.ToString();
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Обновить".
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listBoxBooks.SelectedItem == null)
            {
                MessageBox.Show("❌ Выберите книгу для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ValidateInputs();

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
                    throw new ArgumentException("Количество книг должно быть неотрицательным числом.");

                var book = (BookDTO)listBoxBooks.SelectedItem;

                if (logic.UpdateBook(book.Id, txtTitle.Text, txtAuthor.Text, cmbGenre.Text, int.Parse(txtYear.Text), quantity))
                {
                    MessageBox.Show("✅ Книга успешно обновлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadBooks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Группировка по жанрам".
        /// </summary>
        private void btnGroup_Click(object sender, EventArgs e)
        {
            var grouped = logic.GroupBooksByGenre();
            if (grouped.Count == 0)
            {
                MessageBox.Show("📚 Нет книг для группировки.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = "📌 Группировка по жанрам:\n\n";
            foreach (var g in grouped)
            {
                result += $"➤ {g.Key}:\n";
                foreach (var b in g.Value)
                    result += $"   • {b.Title} ({b.Author}, {b.Year}, Количество: {b.Quantity})\n";
                result += "\n";
            }
            MessageBox.Show(result, "Группировка книг", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Очистить".
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        /// <summary>
        /// Очищает поля ввода.
        /// </summary>
        private void ClearFields()
        {
            txtId.Clear();
            txtTitle.Clear();
            txtAuthor.Clear();
            cmbGenre.SelectedIndex = -1;
            txtYear.Clear();
            txtQuantity.Clear();
        }

        /// <summary>
        /// Проверяет корректность введенных данных
        /// </summary>
        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
                throw new ArgumentException("Название книги не может быть пустым");

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
                throw new ArgumentException("Автор не может быть пустым");

            if (cmbGenre.SelectedIndex == -1)
                throw new ArgumentException("Выберите жанр из списка");

            if (!int.TryParse(txtYear.Text, out int year) || year < 1000 || year > 2025)
                throw new ArgumentException("Год должен быть числом в диапазоне от 1000 до 2025");

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtAuthor.Text, @"^[a-zA-Zа-яА-ЯёЁ\s\-]+$"))
                throw new ArgumentException("Автор должен содержать только буквы, пробелы и дефисы");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}