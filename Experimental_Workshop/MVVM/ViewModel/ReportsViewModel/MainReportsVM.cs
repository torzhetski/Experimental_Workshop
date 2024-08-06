using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.ViewModel.DataTableViewModel;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.ReportsViewModel
{
    public class MainReportsVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private object _currentReport;

        public RelayCommand NextReportCommand { get; set; }
        public object CurrentReport
        {
            get => _currentReport;
            set => Set(ref _currentReport,value);
        }

        public MainReportsVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            this.CurrentReport = new SampleVM(_mainRepositoryService);
            NextReportCommand = new RelayCommand(ExecuteNextReport);
        }

        private void ExecuteNextReport(object commandParameter)
        {
            if (commandParameter is not ID pageId)
            {
                throw new ArgumentException($"Unexpected parameter type. ICommand.CommandParameter must be of type {typeof(ID)}", nameof(commandParameter));
            }

            LoadPage(pageId);
        }
        private void LoadPage(ID pageId)
        {

            switch (pageId)
            {
                case ID.First:
                    this.CurrentReport = new SampleVM(_mainRepositoryService);
                    break;
                case ID.Second:
                    this.CurrentReport = new FinalRequestVM(_mainRepositoryService);
                    break;
                case ID.Third:
                    this.CurrentReport = new ParameterizedQueryVM(_mainRepositoryService);
                    break;
                case ID.Fourth:
                    this.CurrentReport = new PivotRequestVM(_mainRepositoryService);
                    break;
                case ID.Fifth:
                    this.CurrentReport = new MergeRequestVM(_mainRepositoryService);
                    break;
                default:
                    break;
            }

        }
    }
}
