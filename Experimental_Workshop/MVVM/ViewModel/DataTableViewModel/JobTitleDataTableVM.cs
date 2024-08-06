using Experimental_Workshop.ApplicationResourses;
using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.MVVM.ViewModel.DataTableViewModel
{
    public class JobTitleDataTableVM : ObservableObject
    {
        private MainRepositoryService _mainRepositoryService;
        private ObservableCollection<JobTitle> _jobTitles;
        private JobTitle _newJobTitle;

        public ObservableCollection<JobTitle> JobTitles
        {
            get => _jobTitles;
            set => Set(ref _jobTitles, value);
        }
        public JobTitle NewJobTitle
        {
            get => _newJobTitle;
            set => Set(ref _newJobTitle, value);
        }


        public RelayCommand DeleteJobTitleCommand {  get; set; }
        public RelayCommand SaveJobTitleCommand { get; set; }
        public RelayCommand AddJobTitleCommand { get; set; }

        public JobTitleDataTableVM(MainRepositoryService mainRepositoryService)
        {
            _mainRepositoryService = mainRepositoryService;
            DeleteJobTitleCommand = new RelayCommand(RemooveJobTitle);
            SaveJobTitleCommand = new RelayCommand( UpdateJobTitle);
            AddJobTitleCommand = new RelayCommand(AddjobTitle, CanAddJobTitle);
            NewJobTitle = new JobTitle();
            LoadData();
        }

        private bool CanAddJobTitle(object arg)
        {
            if(NewJobTitle.Title != default(string))
            {
                return true;
            }
            else
                return false;
        }

        private void AddjobTitle(object obj)
        {
            JobTitles.Add(NewJobTitle);
            _mainRepositoryService.JobTitleRepository.AddJobTitle(NewJobTitle);
            NewJobTitle = new();
        }

        private void LoadData()
        {
            var jobTitle = _mainRepositoryService.JobTitleRepository.GetAllJopTitles();
            JobTitles = new ObservableCollection<JobTitle>(jobTitle);
        }
        private void RemooveJobTitle(object o) 
        {
            JobTitle jobTitle = (JobTitle)o;
            JobTitles.Remove(jobTitle);
            _mainRepositoryService.JobTitleRepository.RemooveJobTitle(jobTitle);
        }
        private void UpdateJobTitle( object o)
        {
            _mainRepositoryService.JobTitleRepository.SaveAllJobTitles();
        }
       
    }
}
