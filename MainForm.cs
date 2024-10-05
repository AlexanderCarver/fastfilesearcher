using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FastFileSearcher
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource _cancellationTokenSource; // Токен отмены для поиска

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Получение списка доступных дисков
            var drives = DriveInfo.GetDrives();
            comboBoxDrives.Items.Add("Все"); // Добавляем опцию "Все"
            comboBoxDrives.Items.AddRange(drives.Select(d => d.Name).ToArray());
            comboBoxDrives.SelectedIndex = 0; // Устанавливаем "Все" как выбранный элемент
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            listBoxResults.Items.Clear(); // Очистка списка результатов перед новым поиском
            labelStatus.Text = "Поиск...";

            if (string.IsNullOrEmpty(textBoxFileName.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя файла для поиска.");
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource(); // Инициализация токена отмены
            string fileName = textBoxFileName.Text;

            // Запуск поиска по диску
            await System.Threading.Tasks.Task.Run(() =>
            {
                string selectedDrive = comboBoxDrives.SelectedItem.ToString();
                if (selectedDrive == "Все")
                {
                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (drive.IsReady)
                        {
                            SearchFilesOnDrive(drive.Name, fileName, _cancellationTokenSource.Token);
                        }
                    }
                }
                else
                {
                    SearchFilesOnDrive(selectedDrive, fileName, _cancellationTokenSource.Token);
                }
            });

            labelStatus.Text = "Поиск завершен!";
        }

        private void SearchFilesOnDrive(string rootDirectory, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                // Рекурсивно обходим все директории
                SearchInDirectory(rootDirectory, fileName, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Игнорируем отмену
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при поиске: " + ex.Message);
            }
        }

        // Метод для поиска файлов в каждой директории
        private void SearchInDirectory(string directory, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                // Поиск файлов в текущей директории
                foreach (var file in Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly))
                {
                    // Получаем имя файла без пути и расширения
                    string fileWithoutExtension = Path.GetFileNameWithoutExtension(file);

                    // Проверяем совпадение имени файла без учета расширения
                    if (fileWithoutExtension.IndexOf(fileName, StringComparison.OrdinalIgnoreCase) >= 0) // Используем похожие имена
                    {
                        listBoxResults.Invoke(new Action(() =>
                        {
                            listBoxResults.Items.Add(file); // Добавление найденного файла в список
                        }));
                    }

                    // Проверяем на отмену
                    cancellationToken.ThrowIfCancellationRequested();
                }

                // Поиск в подкаталогах
                foreach (var dir in Directory.GetDirectories(directory))
                {
                    try
                    {
                        SearchInDirectory(dir, fileName, cancellationToken); // Рекурсивный вызов для поддиректорий
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Игнорируем ошибки доступа
                    }
                    catch (Exception ex)
                    {
                        // Обрабатываем любые другие ошибки
                        if (!(ex is OperationCanceledException)) // Игнорируем отмену
                        {
                            MessageBox.Show("Ошибка при доступе к директории: " + ex.Message);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Игнорируем ошибки доступа к папкам
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel(); // Отмена поиска
                labelStatus.Text = "Поиск отменен!";
            }
        }

        private void listBoxResults_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxResults.SelectedItem != null)
            {
                string filePath = listBoxResults.SelectedItem.ToString();
                // Открытие файла в Проводнике
                Process.Start("explorer.exe", $"/select,\"{filePath}\"");
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            listBoxResults.Width = this.ClientSize.Width - 30;
            listBoxResults.Height = this.ClientSize.Height - 160;
            textBoxFileName.Width = this.ClientSize.Width - 30;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearch.PerformClick(); // Запускаем поиск при нажатии Enter
            }
            base.OnKeyDown(e);
        }
    }
}
