using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Warehouse.Commands;
using Warehouse.Models;
using Warehouse.View;

namespace Warehouse.ViewModel
{
    class AddArticleViewModel : ViewModelBase
    {
        AddArticleView addArticle;

        // properties
        private tblArticle article;
        public tblArticle Article
        {
            get { return article; }
            set { article = value; OnPropertyChanged("Article"); }
        }

        private bool stored;
        public bool Stored
        {
            get
            {
                return stored;
            }
            set
            {
                stored = value;
                OnPropertyChanged("Stored");
            }
        }
        
        // property for updating the table
        private bool isUpdateArticle;
        public bool IsUpdateArticle
        {
            get
            {
                return isUpdateArticle;
            }
            set
            {
                isUpdateArticle = value;
            }
        }

        // constructor
        public AddArticleViewModel(AddArticleView articleOpen)
        {
            article = new tblArticle();
            addArticle = articleOpen;
        }

        // commands
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }

        /// <summary>
        /// method for adding the new article
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (WarehouseEntities1 context = new WarehouseEntities1())
                {
                    tblArticle newArticle = new tblArticle();

                    if (article.Article.All(Char.IsLetter))
                    {
                        newArticle.Article = article.Article;
                    }
                    else
                    {
                        MessageBox.Show("Wrong article name input, please edit.");
                    }

                    newArticle.Code = article.Code;
                    newArticle.Amount = article.Amount;
                    newArticle.Price = article.Price;
                    newArticle.Stored = false;
                    newArticle.ArticleID = article.ArticleID;

                    context.tblArticles.Add(newArticle);
                    context.SaveChanges();

                    IsUpdateArticle = true;
                }
                addArticle.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs, please try again.");
            }
        }

        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(article.Article) || String.IsNullOrEmpty(article.Code) || article.Amount == null || article.Price == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // command for closing the window
        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }

        /// <summary>
        /// method for closing the window
        /// </summary>
        private void CloseExecute()
        {
            try
            {
                addArticle.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private bool CanCloseExecute()
        {
            return true;
        }
    }
}
