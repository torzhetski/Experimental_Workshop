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
    internal class FinalRequestVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<EquipmentRequestVM> _equipmentAndAmountOfCards;

        public ObservableCollection <EquipmentRequestVM> EquipmentAndAmountOfCards
        {
            get=> _equipmentAndAmountOfCards;
            set => Set(ref _equipmentAndAmountOfCards, value);
        }

        public FinalRequestVM( MainRepositoryService mainRepositoryService) 
        {
            _mainRepositoryService = mainRepositoryService;
            EquipmentAndAmountOfCards = new ObservableCollection<EquipmentRequestVM>(_mainRepositoryService.EquipmentRepository.GetEquipmentAndTechCardAmount());
        }
    }
}
