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
using System.IO;
using System.Windows.Forms;



namespace CheckDimension
{
    /// <summary>
    /// Interaction logic for wpfCheckDim.xaml
    /// </summary>
    public partial class wpfCheckDim : Window
    {
        public wpfCheckDim()
        {
            InitializeComponent();
        }

        private void btnInput(object sender, RoutedEventArgs e)
        {
            txtFolderInput.Text = pathFolder();
        }

        private void btnOutput(object sender, RoutedEventArgs e)
        {
            txtFolderInput.Text = pathFolder();
        }

        private void btnCheckDim(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private string pathFolder()
        {
            string path=string.Empty;
            FolderBrowserDialog diglog = new FolderBrowserDialog();
            if (diglog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = diglog.SelectedPath;
            }
            return path;
        }
    }
}
