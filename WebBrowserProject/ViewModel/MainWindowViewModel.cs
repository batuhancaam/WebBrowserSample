using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using WebBrowserProject.Helper;
using WebBrowserProject.Model;

namespace WebBrowserProject
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        string[] url = new string[24];
        private string _url;
        WebBrowser web = new WebBrowser();

        PageListHelper PageListHelper = new PageListHelper();
        private ObservableCollection<PageModel> pageList;

        public ObservableCollection<PageModel> PageList


        {
            get { return pageList; }
            set
            {
                pageList = value;

            }
        }



        public MainWindowViewModel()
        {
            PageList = PageListHelper.PageListGetir();

        }


        private ICommand _command;

        public ICommand SiteGetirCommand
        {
            get
            {
                return _command ?? (_command = new RelayCommand(
                   x =>
                   {
                       SiteyiGetir();
                   }));
            }
        }

        private void SiteyiGetir()
        {  
            Url = "http://www.buraksenyurt.com/";
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (_url == value) return;
                _url = value;
                RaisePropertyChanged("Url");
            }
        }

        private ICommand veriGetir;
        public ICommand VeriGetirCommand
        {

            get
            {

                return veriGetir ?? (veriGetir = new RelayCommand(
                  x =>
                  {
                      Getir();
                  }));

            }



        }

        private void Getir()
        {


            web.Navigate(Url);
            web.DocumentCompleted += Web_DocumentCompleted;
            



        }

        private void Web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string[] baslik = new string[24];
            string[] tarih = new string[24];
            

            for (int i = 0; i < 24; i++)
            {

                string id = ("post" + i + "");
                var text = web.Document.GetElementById(id);
                HtmlElementCollection tag = text.GetElementsByTagName("a");
                url[i] = tag[0].GetAttribute("href");
                baslik[i] = tag[0].InnerText;



                HtmlElementCollection tag2 = text.GetElementsByTagName("span");
                tarih[i] = tag2[0].InnerText;
            }

            for (int i = 0; i < 24; i++)
            {

                PageModel model = new PageModel();
                model.Baslik = baslik[i];
                model.Tarih = tarih[i];
                PageList.Add(model);
               

            }






        }

        private ICommand doubleClick;
        public ICommand DoubleClickCommand
        {

            get
            {

                return doubleClick ?? (doubleClick = new RelayCommand(
                  x =>
                  {
                      Click();
                  }));

            }



        }

        private void Click()
        {
            int index = PageList.IndexOf(SelectedItem);
            Url = url[index];
           
        }

        private PageModel selectedİtem;

        public PageModel SelectedItem
        {
            get
            {
                return selectedİtem;
            }
            set
            {
                selectedİtem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
   }
    }
