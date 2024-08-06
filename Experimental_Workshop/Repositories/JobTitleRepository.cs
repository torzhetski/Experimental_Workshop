using Experimental_Workshop.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.Repositories
{
    public class JobTitleRepository
    {
        private readonly ApplicationContext _dbContext;

        public JobTitleRepository(ApplicationContext context)
        {
            _dbContext = context;
        }
        public List<JobTitle> GetAllJopTitles()
        {
            return _dbContext.JobTitles.ToList();
        }
        public void SaveAllJobTitles()
        {
            _dbContext.SaveChanges();
        }
        
        public void AddJobTitle(JobTitle jobTitle)
        {
            _dbContext.JobTitles.Add(jobTitle);
            _dbContext.SaveChanges();
        }
        public void RemooveJobTitle( JobTitle jobTitle)
        {
            _dbContext.JobTitles.Remove(jobTitle);
            _dbContext.SaveChanges();
        }
    }
}
