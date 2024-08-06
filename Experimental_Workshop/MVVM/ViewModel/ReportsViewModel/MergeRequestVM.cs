using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.ViewModel.ReportsViewModel;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.ReportsViewModel
{
    public class MergeRequestVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<TechCardEquipUnionVM> _cardEquipUnion;

        public ObservableCollection<TechCardEquipUnionVM> CardEquipUnion
        {
            get => _cardEquipUnion;
            set => Set(ref _cardEquipUnion, value);
        }

        public MergeRequestVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            CardEquipUnion = new ObservableCollection<TechCardEquipUnionVM>(_mainRepositoryService.techCardEquipunion());
        }
    }
}
