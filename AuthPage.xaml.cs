using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yumagulov41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private string _captchaText;
        private readonly Random _random = new Random();
        public AuthPage()
        {
            InitializeComponent();
            GenerateCaptcha();
        }

        private async void Loginbtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PassTB.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            var user = Yumagulov41Entities.GetContext().User
                .FirstOrDefault(p => p.UserLogin == login && p.UserPassword == password);

            if (user == null)
            {
                if (CaptchaPanel.Visibility != Visibility.Visible)
                {
                    MessageBox.Show("Неверный логин или пароль. Введите капчу.");
                    CaptchaPanel.Visibility = Visibility.Visible;
                    CaptchaLabel.Visibility = Visibility.Visible;
                    GenerateCaptcha();
                    return;
                }

                if (CaptchaTB.Text != _captchaText)
                {
                    MessageBox.Show("Неверная капча. Вход заблокирован на 10 секунд.");
                    Loginbtn.IsEnabled = false;
                    await Task.Delay(10000);
                    Loginbtn.IsEnabled = true;

                    GenerateCaptcha();
                    CaptchaTB.Clear();
                    return;
                }
                MessageBox.Show("Неверные данные.");
                return;
            }
            Manager.MainFrame.Navigate(new ServicePage(user));
            LoginTB.Clear();
            PassTB.Clear();
            CaptchaTB.Clear();
            CaptchaPanel.Visibility = Visibility.Collapsed;
            CaptchaLabel.Visibility = Visibility.Collapsed;
        }


        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PassTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void GenerateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int length = 4; // в задании минимум 4 символа

            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
                buffer[i] = chars[_random.Next(chars.Length)];

            _captchaText = new string(buffer);

            // разбрасываем по трём TextBlock-ам (как в шаблоне)
            captchaOneWord.Text = _captchaText[0].ToString();
            captchaTwoWord.Text = _captchaText[1].ToString();
            captchaThreeWord.Text = _captchaText[2].ToString();
            captchaFourWord.Text = _captchaText[3].ToString();
        }

        private void CaptchaTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // просто вызываем тот же код, что и у кнопки
                Loginbtn_Click(Loginbtn, new RoutedEventArgs());
            }
        }

        private void Guestbtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ServicePage(null));
        }
    }
}
