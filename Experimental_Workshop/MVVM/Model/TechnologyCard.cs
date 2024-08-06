using Experimental_Workshop.ApplicationResourses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
namespace Experimental_Workshop.MVVM.Model
{
    public class TechnologyCard : ObservableObject, INotifyDataErrorInfo
    {
        private int _id;
        private string _name;
        private int _frequency;
        private int _power;
        private bool _isInExpluatation;
        private DateTime _dateOfStart;
        private DateTime _dateOfEnd;
        private ObservableCollection<Equipment> _equipment;
        private ObservableCollection<Materials> _materials;
        private ObservableCollection<Worker> _workers;
        private int _microprocessorTypesId;
        private MicroprocessorTypes? _microprocessorType;


        private ObservableCollection<MicroprocessorTypes> _allMicroprocessorTypes;
        private ObservableCollection<Worker> _restWorkers;
        private ObservableCollection<Materials> _restMaterials;
        private ObservableCollection<Equipment> _restEquipment;

        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _propertyErrors.Any();

        public int Id
        {
            get => _id;
            set => Set(ref _id, value);
        }
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public int Frequency
        {
            get => _frequency;
            set => Set(ref _frequency, value);
        }
        public int Power
        {
            get => _power;
            set => Set(ref _power, value);
        }
        public bool IsInExpluatation
        {
            get => _isInExpluatation;
            set => Set(ref _isInExpluatation, value);
        }
        public DateTime DateOfStart
        {
            get => _dateOfStart;
            set
            {
                RemoveError(nameof(DateOfStart));
                RemoveError(nameof(DateOfEnd));
                Set(ref _dateOfStart, value);
                if (_dateOfStart > _dateOfEnd)
                    AddError(nameof(DateOfStart), "Date Of Start cannot be more than Date of End");
                if(_dateOfStart == default(DateTime)&& _dateOfStart == null) 
                    AddError(nameof(DateOfStart), "Date cannot be empty");
                    
            }
        }

        public DateTime DateOfEnd
        {
            get => _dateOfEnd;
            set 
            {
                RemoveError(nameof(DateOfEnd));
                RemoveError(nameof(DateOfStart));
                Set(ref _dateOfEnd, value);
                if (_dateOfStart > _dateOfEnd)
                    AddError(nameof(DateOfEnd), "Date Of End cannot be less than Date of Start");
                if (_dateOfEnd == default(DateTime) && _dateOfEnd == null)
                    AddError(nameof(DateOfEnd), "Date cannot be empty");
            }
        }
        
        public ObservableCollection<Equipment> Equipment
        {
            get => _equipment;
            set => Set(ref _equipment, value);
        }
        public ObservableCollection<Materials> Materials
        {
            get => _materials;
            set => Set(ref _materials, value);
        }
        public ObservableCollection<Worker> Workers
        {
            get => _workers;
            set => Set(ref _workers, value);
        }
        public int MicroprocessorTypesId
        {
            get => _microprocessorTypesId;
            set => Set(ref _microprocessorTypesId, value);
        }
        public MicroprocessorTypes MicroprocessorType
        {
            get => _microprocessorType;
            set => Set(ref _microprocessorType, value);
        }


        //to represent other models
        [NotMapped]
        public ObservableCollection<MicroprocessorTypes> AllMicroprocessorTypes
        {
            get => _allMicroprocessorTypes;
            set => Set(ref _allMicroprocessorTypes, value);
        }
        [NotMapped]
        public ObservableCollection<Worker> RestWorkers
        {
            get => _restWorkers;
            set => Set(ref _restWorkers, value);
        }
        [NotMapped]
        public ObservableCollection<Materials> RestMaterials
        {
            get => _restMaterials;
            set => Set(ref _restMaterials, value);
        }
        [NotMapped]
        public ObservableCollection<Equipment> RestEquipment
        {
            get => _restEquipment;
            set => Set(ref _restEquipment, value);
        }
        public TechnologyCard()
        {
            Equipment = new ObservableCollection<Equipment>();
            Materials = new ObservableCollection<Materials>();
            Workers = new ObservableCollection<Worker>();
            DateOfStart = DateTime.Now;
            DateOfEnd = DateTime.Now;
            //MicroprocessorType = new MicroprocessorTypes();
        }


        // INotifyDataErrorInfo
        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }
        public void AddError(string properyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(properyName))
            {
                _propertyErrors.Add(properyName, new List<string>());
            }
            _propertyErrors[properyName].Add(errorMessage);
            OnErrorsChanged(properyName);
        }
        public void RemoveError(string properyName)
        {
            if (_propertyErrors.Remove(properyName))
            {
                OnErrorsChanged(properyName);
            }
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
