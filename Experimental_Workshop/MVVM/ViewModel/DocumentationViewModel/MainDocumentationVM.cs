using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Experimental_Workshop.MVVM.ViewModel.DocumentationViewModel
{
    public class MainDocumentationVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private DateTime _dateStart =DateTime.Now;
        private DateTime _dateEnd = DateTime.Now;
        private FlowDocument _flowDocument;

        public FlowDocument Document 
        {
            get => _flowDocument;
            set => Set(ref _flowDocument, value);
        }
        public DateTime DateStart
        {
            get => _dateStart;
            set => Set(ref _dateStart, value);
        }
        public DateTime DateEnd
        {
            get => _dateEnd;
            set => Set(ref _dateEnd, value);
        }
        
        public RelayCommand PrintCommand { get; set; }
        public RelayCommand GenerateDocumentCommand { get; set; }


        public MainDocumentationVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            PrintCommand = new RelayCommand(Print);
            GenerateDocumentCommand = new RelayCommand(GenerateDocument);
        }

        private void GenerateDocument(object obj)
        {
            var list = _mainRepositoryService.TechnologyCardRepository.GetTechnologyCardsBetweenToDates(DateStart, DateEnd);
            Document = PrintService.GenerateDocument(list);
        }

        private void Print(object sender)
        {
            PrintService.PrintData(Document);
            Document = new FlowDocument();
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
        }
    }
}
