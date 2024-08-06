using Experimental_Workshop.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experimental_Workshop.Repositories
{
    public class WorkerRepository
    {
        private readonly ApplicationContext _dbContext;

        public WorkerRepository(ApplicationContext context)
        {
            _dbContext = context;
        }

        public List<Worker> GetAllWorkers()
        {

            return _dbContext.Workers.
                Include(w => w.JobTitle ).
                Include(w => w.TechnologyCards ).
                ToList();
        }

        public void SaveAllWorkers()
        {
            _dbContext.SaveChanges();
        }
        public void AddWorker(Worker worker)
        {
            _dbContext.Workers.Add(worker);
            _dbContext.SaveChanges();
        }

        public void RemoveWorker(Worker worker)
        {
            _dbContext.Workers.Remove(worker);
            _dbContext.SaveChanges();
        }
    }
}
