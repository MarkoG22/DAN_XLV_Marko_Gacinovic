using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Warehouse.ViewModel;

namespace Warehouse.View
{
    /// <summary>
    /// Interaction logic for AddArticleView.xaml
    /// </summary>
    public partial class AddArticleView : Window
    {
        public AddArticleView()
        {
            InitializeComponent();
            this.DataContext = new AddArticleViewModel(this);
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }        
    }
}
