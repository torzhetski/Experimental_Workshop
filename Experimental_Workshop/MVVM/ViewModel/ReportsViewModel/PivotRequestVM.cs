using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.ReportsViewModel
{
    public class PivotRequestVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<PivotTechCardVM> _pivotTechCards;

        public ObservableCollection<PivotTechCardVM> PivotTechCards
        {
            get => _pivotTechCards;
            set => Set(ref _pivotTechCards, value);
        }

        public PivotRequestVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            //PivotTechCards = new ObservableCollection<PivotTechCardVM>( _mainRepositoryService.TechCardPivot());
        }
    }
}
