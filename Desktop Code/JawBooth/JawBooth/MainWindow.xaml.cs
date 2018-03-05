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

namespace JawBooth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Load the config file
            JawBoothConfiguration.GetInstance().LoadConfigurationFile();

            //Have the user select a booth
            BoothSelectionUI booth_selection_window = new BoothSelectionUI();
            booth_selection_window.ShowDialog();

            //Get the result of the booth selection
            string port_name = string.Empty;
            BoothSelectorViewModel booth_selection_result = BoothSelectorViewModel.GetInstance();
            if (booth_selection_result.ResultOK && booth_selection_result.AvailablePortCount > 0)
            {
                port_name = booth_selection_result.AvailablePorts[booth_selection_result.SelectedPortIndex].Model.DeviceID;
            }

            if (!string.IsNullOrEmpty(port_name))
            {
                //Set the data context
                DataContext = new SessionViewModel(port_name);
            }
            else
            {
                //Close the main window and exit the program if no booth was selected
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var vm = DataContext as SessionViewModel;
            if (vm != null)
            {
                vm.CloseBackgroundThread();
            }
        }
    }
}
