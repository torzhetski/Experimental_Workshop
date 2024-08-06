using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.MVVM.ViewModel.DataTableViewModel;
using Experimental_Workshop.MVVM.ViewModel.DocumentationViewModel;
using Experimental_Workshop.MVVM.ViewModel.ReportsViewModel;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Experimental_Workshop.MVVM.ViewModel
{
    public class MainVM : ObservableObject
    {
        private ApplicationContext context;
        private MainRepositoryService _mainRepositoryService;
        private object _currentView ;
        
        private bool _isVisible = false;
        public object CurrentView
        {
            get => _currentView; 
            set => Set(ref _currentView, value);
        }


        public RelayCommand NextPageCommand { get; set; }

        public MainVM()
        {
            context = new ApplicationContext();
            _mainRepositoryService = new MainRepositoryService(context);
            NextPageCommand = new(ExecuteNextPageCommand);
            _currentView = new DataTableMainVM(_mainRepositoryService); 
            
        }

        private void ExecuteNextPageCommand(object commandParameter)
        {
            if (commandParameter is not ID pageId)
            {
                throw new ArgumentException($"Unexpected parameter type. ICommand.CommandParameter must be of type {typeof(ID)}", nameof(commandParameter));
            }

            LoadPage(pageId);
        }
        private void LoadPage(ID pageId)
        {
            object oldPage = this.CurrentView;

            switch (pageId)
            {
                case ID.First:
                    this.CurrentView = new DataTableMainVM(_mainRepositoryService);
                    break; 
                case ID.Second:
                    this.CurrentView= new MainReportsVM(_mainRepositoryService);
                    break;
                case ID.Third:
                    this.CurrentView= new MainDocumentationVM(_mainRepositoryService);
                    break;
                default:
                    break;
            }
            
        }
        public void OnContextUpdate()
        {
            context = new ApplicationContext();
            _mainRepositoryService = new MainRepositoryService(context);

            if (this.CurrentView is DataTableMainVM)
                this.CurrentView = new DataTableMainVM(_mainRepositoryService);
            if (this.CurrentView is MainReportsVM)
                this.CurrentView =new MainReportsVM(_mainRepositoryService);
            if(this.CurrentView is MainDocumentationVM)
                this.CurrentView = new MainDocumentationVM(_mainRepositoryService);
        }
    }
}
