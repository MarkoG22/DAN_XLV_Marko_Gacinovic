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
using Warehouse.Models;
using Warehouse.ViewModel;

namespace Warehouse.View
{
    /// <summary>
    /// Interaction logic for EditArticleView.xaml
    /// </summary>
    public partial class EditArticleView : Window
    {
        public EditArticleView(tblArticle article)
        {
            InitializeComponent();
            this.DataContext = new EditArticleViewModel(this, article);
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
