using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Warehouse.Commands;
using Warehouse.Models;
using Warehouse.View;

namespace Warehouse.ViewModel
{
    class ManagerViewModel : ViewModelBase
    {
        ManagerView manager;

        private List<tblArticle> listOfArticles;
        public List<tblArticle> ListOfArticles
        {
            get { return listOfArticles; }
            set { listOfArticles = value; OnPropertyChanged("ListOfArticles"); }
        }

        private tblArticle article;
        public tblArticle Article
        {
            get { return article; }
            set { article = value; OnPropertyChanged("Article"); }
        }

        public ManagerViewModel(ManagerView managerOpen)
        {
            manager = managerOpen;
            ListOfArticles = GetAllArticles().ToList();
        }

        private ICommand addNewArticle;
        public ICommand AddNewArticle
        {
            get
            {
                if (addNewArticle == null)
                {
                    addNewArticle = new RelayCommand(param => AddNewArticleExecute(), param => CanAddNewArticleExecute());
                }
                return addNewArticle;
            }
        }

        private bool CanAddNewArticleExecute()
        {
            return true;
        }

        private void AddNewArticleExecute()
        {
            try
            {
                AddArticleView article = new AddArticleView();
                article.ShowDialog();
                if ((article.DataContext as AddArticleViewModel).IsUpdateArticle == true)
                {
                    ListOfArticles = GetAllArticles().ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private List<tblArticle> GetAllArticles()
        {
            try
            {
                using (WarehouseEntities1 context = new WarehouseEntities1())
                {
                    List<tblArticle> list = new List<tblArticle>();
                    list = (from x in context.tblArticles select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }
    }
}
