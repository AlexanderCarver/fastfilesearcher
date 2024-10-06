using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastFileSearcher
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(); // Инициализация токена отмены
        private const string CurrentVersion = "1.0.1"; // Текущая версия приложения
        private const string UpdateUrl = "https://github.com/AlexanderCarver/fastfilesearcher/releases/latest/download/FastFileSearcher.zip"; 

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
            labelStatus.Text = $"Версия {CurrentVersion}"; // Отображение версии приложения
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
            await Task.Run(() =>
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

        private void SearchInDirectory(string directory, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                // Поиск файлов в текущей директории
                foreach (var file in Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly))
                {
                    string fileWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    if (fileWithoutExtension.IndexOf(fileName, StringComparison.OrdinalIgnoreCase) >= 0)
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
                        if (!(ex is OperationCanceledException))
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

        // Метод для проверки обновлений
        private async void buttonCheckUpdate_Click(object sender, EventArgs e)
        {
            string latestVersion = await GetLatestVersionAsync(); // Получаем последнюю версию
            if (latestVersion != CurrentVersion)
            {
                DialogResult result = MessageBox.Show($"Доступно обновление до версии {latestVersion}. Вы хотите загрузить его?", "Обновление", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    await DownloadUpdateAsync(); // Загрузка обновления
                }
            }
            else
            {
                MessageBox.Show("Вы используете последнюю версию.");
            }
        }

        // Метод для получения последней версии
        private async Task<string> GetLatestVersionAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                string json = await client.GetStringAsync("https://api.github.com/repos/AlexanderCarver/fastfilesearcher/releases/latest");
                dynamic release = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return release.tag_name; // Получаем тег версии
            }
        }

        // Метод для загрузки обновления
        private async Task DownloadUpdateAsync()
        {
            using (var client = new HttpClient())
            {
                labelStatus.Text = "Загрузка обновления...";
                string tempPath = Path.GetTempFileName() + ".zip";
                using (var response = await client.GetAsync(UpdateUrl))
                {
                    response.EnsureSuccessStatusCode();
                    using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fileStream); // Загрузка ZIP файла
                    }
                }

                labelStatus.Text = "Распаковка обновления...";
                string extractPath = Path.Combine(Path.GetTempPath(), "FastFileSearcher");
                System.IO.Compression.ZipFile.ExtractToDirectory(tempPath, extractPath, true); // Распаковка архива

                labelStatus.Text = "Обновление завершено! Перезапустите приложение.";
                // Перезапускаем приложение
                Process.Start(Path.Combine(extractPath, "FastFileSearcher.exe")); // Убедитесь, что имя вашего исполняемого файла соответствует
                Application.Exit(); // Закрываем текущее приложение
            }
        }
    }
}
