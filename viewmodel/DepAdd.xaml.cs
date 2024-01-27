using Microsoft.Win32;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for DepAdd.xaml
    /// </summary>
    public partial class DepAdd : Page
    {
        bool[] checks = new bool[8];
        string imgp, certp;
        int DPid;
        bool DValueSelected = false;
        
        Error[] errors = new Error[8];
        public DepAdd()
        {
            InitializeComponent();
            imgPath.Text = "Not selected";
            certPath.Text = "Not selected";
            imgp = "<NULL>";
            certp = "<NULL>";
           
            for (int i = 0; i < errors.Length; i++)
                errors[i].myType = ErrorType.NoError;

            CallDataRetrival();
        }

        private void ChooseImage(object sender, RoutedEventArgs e)
        {
            var MyData = ChooseImageFile();
            if (MyData != null)
            {
                imgPath.Text = MyData.Item2;
                imgp = MyData.Item2;
                myIMG.Source = MyData.Item1;
                
            }
            
        }

        private void ChooseCert(object sender, RoutedEventArgs e)
        {
            var MyData = ChooseImageFile();
            if (MyData != null)
            {
                certPath.Text = MyData.Item2;
                certp = MyData.Item2;
                myCert.Source = MyData.Item1;
                
            }
            
        }
        

        public Tuple<BitmapImage, string> ChooseImageFile()
        {
            Tuple<BitmapImage, string> myData = null;
            OpenFileDialog ImageFile = new OpenFileDialog();
            ImageFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ImageFile.Filter = "Image files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
            if (ImageFile.ShowDialog() == true)
            {
                myData = new Tuple<BitmapImage, string>(new BitmapImage(new Uri(ImageFile.FileName)), ImageFile.FileName);
            }
            return myData;
        }

        private async void CallDataRetrival()
        {
            var myValues = await Task.Run(() => GetValues());

            Departmentbox.ItemsSource = myValues;
        }

        enum ErrorType
        {
            FormatError,
            EmptyError,
            NoError
        }

        struct Error
        {
            public string message;
            public ErrorType myType;
        }


        private void ConfirmAddingClick(object sender, RoutedEventArgs e)
        {
            Property newAddingProperty = new Property();

            

            DateTime DTmyCopy = new DateTime();
            decimal COSTcopy = 0.0M;
            if (Namebox.Text.Length == 0)
            {
                errors[0].myType = ErrorType.EmptyError;
                errors[0].message = "Empty name of property";
            }
            else errors[0].myType = ErrorType.NoError;
            if (Typebox.Text.Length == 0)
            {
                errors[1].myType = ErrorType.EmptyError;
                errors[1].message = "Empty type of property";
            }
            else errors[1].myType = ErrorType.NoError;

            if (Datebox.Text.Length != 0)
            {
                try
                {
                    DTmyCopy = DateTime.Parse(Datebox.Text);
                    errors[2].myType = ErrorType.NoError;
                }
                catch (FormatException)
                {
                    errors[2].myType = ErrorType.FormatError;
                    errors[2].message = "Date format is incorrect";
                }
            }
            else
            {
                errors[2].message = "Empty date";
                errors[2].myType = ErrorType.EmptyError;
            }


            if (Costbox.Text.Length != 0)
            {
                try
                {
                    COSTcopy = decimal.Parse(Costbox.Text);
                    errors[3].myType = ErrorType.NoError;
                }
                catch (FormatException)
                {
                    errors[3].message = "Number format is incorrect";
                    errors[3].myType = ErrorType.FormatError;
                }
            }
            else
            {
                errors[3].message = "Empty cost";
                errors[3].myType = ErrorType.EmptyError;
            }

            if (imgp == "<NULL>")
            {
                errors[4].message = "Empty image";
                errors[4].myType = ErrorType.EmptyError;
            }
            else errors[4].myType = ErrorType.NoError;
            if (certp == "<NULL>")
            {
                errors[5].message = "Empty certificate";
                errors[5].myType = ErrorType.EmptyError;
            }
            else errors[5].myType = ErrorType.NoError;

            if (Techfeaturesbox.Text.Length == 0)
            {
                errors[6].message = "Empty features description";
                errors[6].myType = ErrorType.EmptyError;
            }
            else errors[6].myType = ErrorType.NoError;

            if (DValueSelected)
            {
                errors[7].myType = ErrorType.NoError;
            }
            else errors[7].myType = ErrorType.EmptyError;

            int index = IsAllDataValid();
            if (index == -1)
            {
                newAddingProperty.PropertyName = Namebox.Text;
                newAddingProperty.PropertyType = Typebox.Text;
                newAddingProperty.PropertyPurchaseDate = DTmyCopy;
                newAddingProperty.ImagePath = imgp;
                newAddingProperty.CertificatePath = certp;
                newAddingProperty.DepartmentID = DPid;
                newAddingProperty.TechFeatures = Techfeaturesbox.Text;
                newAddingProperty.PropertyCost = COSTcopy;
                var myEstateContextManagerRef = EstateContextManager.getInstance();

                var myPropertiesRef = myEstateContextManagerRef.Property;

                myPropertiesRef.Add(newAddingProperty);
                myEstateContextManagerRef.SaveChanges();

            }
            else
            {
                MessageBox.Show("Error: " + errors[index].message);
            }
            
        }
        int IsAllDataValid()
        {
            for (int i = 0; i < errors.Length; i++)
            {
                Error inst = errors[i];
                if (inst.myType == ErrorType.EmptyError || inst.myType == ErrorType.FormatError) return i;
            }
            return -1;
        }

        private void DepartmentFK_selected(object sender, SelectionChangedEventArgs e)
        {
            if (Departmentbox.SelectedItem != null)
            {
                DValueSelected = true;
                DPid = Convert.ToInt32(Departmentbox.SelectedItem);
            }
        }

        private List<int> GetValues()
        {
            var departments = EstateContextManager.getInstance().Department;
            List<int> values = new List<int>();
            foreach (var dValue in departments) values.Add(dValue.ID);
            return values;
        }
    }
}
