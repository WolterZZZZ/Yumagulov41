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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            var currentServices = Yumagulov41Entities.GetContext().Product.ToList();
            ServiceListView.ItemsSource = currentServices;
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentServices = Yumagulov41Entities.GetContext().Product.ToList();

            if(ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 100).ToList(); 
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount <= 9).ToList();
            }
            if (ComboType.SelectedIndex == 2) {
                currentServices = currentServices.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount <= 14).ToList();
            }
            if(ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount <= 100).ToList();
            }
            currentServices = currentServices.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            ServiceListView.ItemsSource = currentServices.ToList();

            if(RButtonDown.IsChecked.Value)
            {
                ServiceListView.ItemsSource = currentServices.OrderByDescending(p => p.ProductCost).ToList();

            }
            if (RButtonUp.IsChecked.Value)
            {
                ServiceListView.ItemsSource = currentServices.OrderBy(p => p.ProductCost).ToList();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
            UpdateServices();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }
    }
}
