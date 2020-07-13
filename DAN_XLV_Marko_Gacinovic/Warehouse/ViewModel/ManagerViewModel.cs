using System;
using System.Collections.Generic;
using System.Linq;
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

        // properties
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

        // constructor
        public ManagerViewModel(ManagerView managerOpen)
        {
            manager = managerOpen;
            ListOfArticles = GetAllArticles().ToList();
        }

        // commands
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

        /// <summary>
        /// method for opening the view for adding new article
        /// </summary>
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

        // command for editing the article
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
        /// method for opening the view for the editing article
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

        // command for deleting the article
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
        /// method for deleting the article
        /// </summary>
        private void DeleteArticleExecute()
        {
            try
            {
                using (WarehouseEntities1 context = new WarehouseEntities1())
                {
                    // geting the article id
                    int id = article.ArticleID;

                    // checking if the article can be deleted
                    if (article.Stored == true)
                    {
                        MessageBox.Show("Article can not be deleted, because it is stored.");                        
                    }
                    else
                    {
                        // confirmation for the action and deleting the article
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

        /// <summary>
        /// method for getting all articles to the list
        /// </summary>
        /// <returns></returns>
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
