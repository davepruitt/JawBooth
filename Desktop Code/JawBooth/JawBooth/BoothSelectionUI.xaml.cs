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
using System.Windows.Shapes;

namespace JawBooth
{
    /// <summary>
    /// Interaction logic for BoothSelectionUI.xaml
    /// </summary>
    public partial class BoothSelectionUI : Window
    {
        public BoothSelectionUI()
        {
            InitializeComponent();
            
            BoothSelectorViewModel viewModel = BoothSelectorViewModel.GetInstance();
            DataContext = viewModel;
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            BoothSelectorViewModel vm = DataContext as BoothSelectorViewModel;
            if (vm != null)
            {
                vm.ResultOK = true;
            }

            //Close the port selector window
            this.Close();
        }
    }
}
