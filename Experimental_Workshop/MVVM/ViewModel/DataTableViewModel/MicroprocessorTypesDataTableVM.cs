using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.DataTableViewModel
{
    public class MicroprocessorTypesDataTableVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<MicroprocessorTypes> _MicroprocessorTypes;
        private MicroprocessorTypes _newMicroprocessorType;
        private MicroprocessorTypes _currentMicroprocessorType;
        private bool _isVisible = false;

        public ObservableCollection<MicroprocessorTypes> MicroprocessorTypes
        {
            get => _MicroprocessorTypes;
            set => Set(ref _MicroprocessorTypes, value);
        }
        public MicroprocessorTypes NewMicroprocessorType
        {
            get => _newMicroprocessorType;
            set => Set(ref _newMicroprocessorType, value);
        }
        public MicroprocessorTypes CurrentMicroprocessorType
        {
            get => _currentMicroprocessorType;
            set => Set(ref _currentMicroprocessorType, value);
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
        public RelayCommand DeleteMicroprocessorTypeCommand { get; set; }
        public RelayCommand SaveMicroprocessorTypesCommand { get; set; }
        public RelayCommand AddMicroprocessorTypeCommand { get; set; }
        public RelayCommand EditAccsessCommand { get; set; }


        public MicroprocessorTypesDataTableVM(MainRepositoryService mainRepository)
        {
            _mainRepositoryService = mainRepository;
            NewMicroprocessorType = new MicroprocessorTypes();  
            LoadData();
            SaveMicroprocessorTypesCommand = new(SaveMicroprocessorTypes);
            DeleteMicroprocessorTypeCommand = new RelayCommand(RemoveMicroprocessorType);
            AddMicroprocessorTypeCommand = new RelayCommand(AddMicroprocessorType, CanAddMicroprocessorType);

            EditAccsessCommand = new RelayCommand(o => IsVisible = !IsVisible);

        }

        private void LoadData()
        {
            var MicroprocessorTypes = _mainRepositoryService.MicroprocessorTypesRepository.GetAllMicroprocessorTypes();
            var techCards = _mainRepositoryService.TechnologyCardRepository.GetAllTechnologyCards();
            this.MicroprocessorTypes = new ObservableCollection<MicroprocessorTypes>(MicroprocessorTypes);

        }


        public void RemoveMicroprocessorType(object o)
        {
            MicroprocessorTypes MicroprocessorType = (MicroprocessorTypes)o;
            MicroprocessorTypes.Remove(MicroprocessorType);
            _mainRepositoryService.MicroprocessorTypesRepository.RemoveMicroprocessorType(MicroprocessorType);
        }


        private bool CanAddMicroprocessorType(object arg)
        {
            bool flag = false;
            if (!NewMicroprocessorType.Name.IsNullOrEmpty() && NewMicroprocessorType.AmountOfCores != default(int))
            { flag = true; }
            return flag;
        }

        private void AddMicroprocessorType(object obj)
        {
            MicroprocessorTypes.Add(NewMicroprocessorType);
            _mainRepositoryService.MicroprocessorTypesRepository.AddMicroprocessorType(NewMicroprocessorType);
            NewMicroprocessorType = new();
        }

        public void SaveMicroprocessorTypes(object o)
        {
            _mainRepositoryService.MicroprocessorTypesRepository.SaveAllMicroprocessorTypes();
        }

    }
}

