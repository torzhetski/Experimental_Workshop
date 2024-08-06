using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.ReportsViewModel
{
    public  class SampleVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<TechnologyCard> _technologyCards;

        public ObservableCollection<TechnologyCard> TechnologyCards 
        {
            get => _technologyCards;
            set => Set(ref _technologyCards, value);
        }

        public SampleVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            TechnologyCards = new ObservableCollection<TechnologyCard> (_mainRepositoryService.TechnologyCardRepository.GetExpluatationTechnologyCards());
        }
    }
}
