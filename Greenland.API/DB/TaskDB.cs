using Greenland.API.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Greenland.API.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Task = System.Threading.Tasks.Task;

namespace Greenland.API.Business_Logic
{
    public class TaskDB
    {
        private readonly greenlandDBContext _context;

        public TaskDB()
        {
            _context = new greenlandDBContext();
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasks()
        {
            return await _context.Task.ToListAsync();

        }

        public async Task<Models.Task> GetTask(int id)
        {
            return await _context.Task.FindAsync(id);

        }

        public async Task<IEnumerable<Models.Task>> GetTaskBYIDteam(int? team)
        {
            IEnumerable<int> IDsTask_byTeaam = await _context.TaskTeam.Where(e => e.IdTeam == team).Select(e => e.IdTask).ToListAsync();

            if (IDsTask_byTeaam == null)
            {

            }

            return await _context.Task.Where(c => IDsTask_byTeaam.Contains(c.IdTask)).ToListAsync();
        }

        public async Task<IEnumerable<Models.Task>> GetTaskByIdEmployee(int id)
        {
            IEnumerable<int> IDsTask_byID = await _context.TaskEmployee.Where(e => e.IdEmployee == id).Select(e => e.IdTask).ToListAsync();

            if (IDsTask_byID == null)
            {
                return null;
            }

            return await _context.Task.Where(c => IDsTask_byID.Contains(c.IdTask)).ToListAsync();
        }

        public async Task<Models.Task> AddTask(Models.Task task, int idEmployee, int? idTeam)
        {
            _context.Task.Add(task);
            await _context.SaveChangesAsync();
            //odlična stvar je da on automatski pri spremanju u bazu ažurira task.ID koji smo dali u argumentu
            _context.TaskTeam.Add(new TaskTeam(task.IdTask, (int)idTeam));
            await _context.SaveChangesAsync();
            _context.TaskEmployee.Add(new TaskEmployee { IdTask = task.IdTask, IdEmployee = idEmployee });
            await _context.SaveChangesAsync();
            return task;
        }


        public async Task<Models.Task> AddTask(Models.Task task, int idEmployee)
        {
            int? team = await _context.Employees.Where(e => e.IdEmployee == idEmployee).Select(e => e.IdTeam).FirstOrDefaultAsync();
            return await AddTask(task, idEmployee, team);
        }

        public async Task<Boolean> DeleteTaskByID(int id)
        {
            var task = await GetTask(id);

            if (task == null) return false;

            _context.Task.Remove(task);

            TaskEmployee taskemployee = await _context.TaskEmployee.Where(e => e.IdTask == id).FirstOrDefaultAsync();
            if (taskemployee != null) _context.TaskEmployee.Remove(taskemployee);

            TaskTeam taskteam = await _context.TaskTeam.Where(e => e.IdTask == id).FirstOrDefaultAsync();
            if (taskteam != null) _context.TaskTeam.Remove(taskteam);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Models.Task>> GetTaskBetweenCreationDateTimes(DateTime start, DateTime stop)
        {
            return await _context.Task.Where(c => c.CreationDate >= start && c.CreationDate <= stop).ToListAsync();

        }

        public async Task<IEnumerable<Models.Task>> GetTaskByCreationDateTime(DateTime date)
        {
            return await GetTaskBetweenCreationDateTimes(date, date);
        }
        public async Task<IEnumerable<Models.Task>> GetTaskBetweenCreationDateTimesForTeamID(DateTime start, DateTime stop, int? team)
        {
            IEnumerable<int> IDsTask_byTeaam = await _context.TaskTeam.Where(e => e.IdTeam == team).Select(e => e.IdTask).ToListAsync();
            return await _context.Task.Where(c => c.Deadline >= start && c.Deadline <= stop && IDsTask_byTeaam.Contains(c.IdTask)).ToListAsync();
        }

        public async Task<IEnumerable<Models.Task>> GetTaskForCreationDateTimeForTeamID(DateTime date, int? team)
        {
            return await GetTaskBetweenCreationDateTimesForTeamID(date, date, team);
        }

        public async Task<Boolean> updateStatusByIDTask(int id, string newStatus)
        {
            Models.Task task = await _context.Task.FindAsync(id);

            if (task == null) return false;

            task.Status = newStatus;
            _context.Task.Update(task);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<Models.Task> updateTaskByKey(int id, string key, string value)
        {
            Models.Task task = await _context.Task.Where(t => t.IdTask == id).FirstOrDefaultAsync();

            if (task == null) throw new ArgumentException("Id can not be found");
            try
            {
                switch (key)
                {
                    case "summary":
                        task.Summary = value;
                        break;
                    case "description":
                        task.Description = value;
                        break;
                    case "priority":
                        int br = Int32.Parse(value);
                        task.Priority = br;
                        break;
                    case "deadline":
                        DateTime date = DateTime.Parse(value);
                        task.Deadline = date;
                        break;
                    case "creation_date":
                        DateTime date1 = DateTime.Parse(value);
                        task.CreationDate = date1;
                        break;
                    case "status":
                        task.Status = value;
                        break;
                    default:
                        throw new ArgumentException("Key can not be found");
                }

            }
            catch (FormatException e)
            {
                throw new ArgumentException("Value can not be parsed");
            }

            _context.Task.Update(task);
            await _context.SaveChangesAsync();

            return task;

        }

        public async Task<Models.Task> update(Models.Task task)
        {
            Models.Task taskDB = _context.Task.Find(task.IdTask);
            taskDB.Summary = task.Summary;
            taskDB.Description = task.Description;
            taskDB.CreationDate = task.CreationDate;
            taskDB.Deadline = task.Deadline;
            taskDB.Status = task.Status;
            taskDB.Priority = task.Priority;
            var taskReturn = _context.Task.Update(taskDB);
            await _context.SaveChangesAsync();
            return taskReturn.Entity;
        }

        public async Task<Models.TaskEmployee> updateTaskEmployee(TaskEmployee taskEmployee)
        {
            TaskEmployee taskEmployeeDB = _context.TaskEmployee.Find(taskEmployee.IdTask);
            taskEmployeeDB.IdEmployee = taskEmployee.IdEmployee;
            var taskReturn = _context.TaskEmployee.Update(taskEmployeeDB);
            await _context.SaveChangesAsync();
            return taskEmployeeDB;
        }

        public async Task<Employees> GetEmployeeForTask(int taskId)
        {
            int empId = _context.TaskEmployee.FirstOrDefault(taskEmp => taskEmp.IdTask == taskId).IdEmployee;
            return await new EmployeesDB().GetEmployeeById(empId);
        }

        public TaskEmployee GetTaskEmployee(int taskId)
        {
            return _context.TaskEmployee.FirstOrDefault(taskEmp => taskEmp.IdTask == taskId);
        }


    }
}
