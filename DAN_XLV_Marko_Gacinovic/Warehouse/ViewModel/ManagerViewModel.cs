using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        // command for editing the user
        private ICommand editArticle;
        public ICommand EditArticle
        {
            get
            {
                if (editArticle == null)
                {
                    editArticle = new RelayCommand(param => EditArticleExecute(), param => CanEditArticleExecute());
                }
                return editArticle;
            }
        }

        private bool CanEditArticleExecute()
        {
            if (Article != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// method for opening the view for the editing user
        /// </summary>
        private void EditArticleExecute()
        {
            try
            {
                EditArticleView editUser = new EditArticleView(Article);
                editUser.ShowDialog();
                if ((editUser.DataContext as EditArticleViewModel).IsUpdateArticle == true)
                {
                    ListOfArticles = GetAllArticles().ToList();                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // command for deleting the user
        private ICommand deleteArticle;
        public ICommand DeleteArticle
        {
            get
            {
                if (deleteArticle == null)
                {
                    deleteArticle = new RelayCommand(param => DeleteArticleExecute(), param => CanDeleteArticleExecute());
                }
                return deleteArticle;
            }
        }

        private bool CanDeleteArticleExecute()
        {
            if (Article != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// method for deleting the user
        /// </summary>
        private void DeleteArticleExecute()
        {
            try
            {
                using (WarehouseEntities1 context = new WarehouseEntities1())
                {
                    // geting the registration number of the user
                    int id = article.ArticleID;

                    if (article.Stored == true)
                    {
                        MessageBox.Show("Article can not be deleted, because it is stored.");                        
                    }
                    else
                    {
                        // confirmation for the action
                        MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the article?", "Delete Confirmation", MessageBoxButton.YesNo);

                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            tblArticle articleToDelete = (from x in context.tblArticles where x.ArticleID == id select x).First();

                            context.tblArticles.Remove(articleToDelete);
                            context.SaveChanges();
                            
                            ListOfArticles = GetAllArticles().ToList();
                        }
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("The user can not be deleted, please try again.");
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
