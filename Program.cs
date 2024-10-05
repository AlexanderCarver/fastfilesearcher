using System;
using System.Windows.Forms;

namespace FastFileSearcher
{
    internal static class Program
    {
        // Метод Main — точка входа для приложения
        [STAThread]
        static void Main()
        {
            // Включаем визуальные стили для элементов управления Windows Forms
            Application.EnableVisualStyles();
            
            // Устанавливаем совместимость с рендерингом текста
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Запускаем основную форму приложения
            Application.Run(new MainForm());
        }
    }
}
