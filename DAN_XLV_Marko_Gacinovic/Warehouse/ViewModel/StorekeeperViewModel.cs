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
    class StorekeeperViewModel : ViewModelBase
    {
        StorekeeperView storekeeper;

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
        public StorekeeperViewModel(StorekeeperView storekeeperOpen)
        {
            storekeeper = storekeeperOpen;
            ListOfArticles = GetAllArticles().ToList();
        }

        // commands
        private ICommand storeArticle;
        public ICommand StoreArticle
        {
            get
            {
                if (storeArticle == null)
                {
                    storeArticle = new RelayCommand(param => StoreArticleExecute(), param => CanStoreArticleExecute());
                }
                return storeArticle;
            }
            
        }

        private bool CanStoreArticleExecute()
        {
            return true;
        }

        /// <summary>
        /// method for checking and storing the articles
        /// </summary>
        private void StoreArticleExecute()
        {
            try
            {
                Delegate d = new Delegate();
                using (WarehouseEntities1 context = new WarehouseEntities1())
                {
                    int id = 0;
                    for (int i = 0; i < ListOfArticles.Count; i++)
                    {
                        // checking if the article is already stored
                        if (ListOfArticles[i].Stored == true)
                        {
                            continue;
                        }
                        else
                        {
                            // checking the amount of the article and displaying messages with delegate and event
                            if (ListOfArticles[i].Amount >100)
                            {
                                d.WarehouseFull(ListOfArticles[i].Article);
                            }
                            else
                            {
                                id = ListOfArticles[i].ArticleID;

                                tblArticle article = (from x in context.tblArticles where x.ArticleID == id select x).First();
                                article.Stored = true;
                                MessageBox.Show("Article '" + article.Article + "' is stored.");
                            }
                        }
                    }
                    context.SaveChanges();

                    ListOfArticles = GetAllArticles().ToList();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong, please try again");
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
