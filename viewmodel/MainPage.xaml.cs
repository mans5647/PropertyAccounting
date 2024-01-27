using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
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

namespace PropertyAccounting
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        EstateContextManager manager;
        public MainPage()
        {
            InitializeComponent();
        }

        private void OpenSection_DepInfo(object sender, RoutedEventArgs e)
        {
            if (estategrid.SelectedItem != null)
            {
                try
                {
                    var SingleProperty = (Property)estategrid.SelectedItem;
                    NavigationService.Navigate(new DepInfo(SingleProperty.ID));
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid object selected");
                }
            }
            else
            {
                MessageBox.Show("Select object to modify");
            }
        }

        private void OpenSection_DepAdd(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DepAdd());
        }

        private async void GetAllProperties()
        {
            var MyData = await Task.Run(() => GetProperties());
            AddDataGridColumns();
            estategrid.ItemsSource = MyData;
        }



        
        


        private void AddDataGridColumns()
        {
            DataGridTextColumn[] columns = new DataGridTextColumn[5];

            for (int i = 0; i < columns.Length; i++) columns[i] = new DataGridTextColumn();

            columns[0].Header = "ID";
            columns[0].Binding = new Binding("ID");
            columns[1].Header = "Имя";
            columns[1].Binding = new Binding("PropertyName");
            columns[2].Header = "Технические характеристики";
            columns[2].Binding = new Binding("TechFeatures");
            columns[3].Header = "Цена";
            columns[3].Binding = new Binding("PropertyCost");
            columns[4].Header = "Тип";
            columns[4].Binding = new Binding("PropertyType");

            foreach (var column in columns)
            {
                estategrid.Columns.Add(column);
            }
        }

        private List<Property> GetProperties()
        {
            var myProperties = EstateContextManager.getInstance().Property;
            return myProperties.ToList();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            estategrid.Columns.Clear();
            estategrid.ItemsSource = null;
            GetAllProperties();
        }
    }
}
