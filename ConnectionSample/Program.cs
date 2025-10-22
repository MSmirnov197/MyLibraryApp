using System;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace ConnectionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new AppDbContext();
            BL bl = new BL(new EntityRepository<Employee>(context));
            bl.CreateEmployee(new Employee { Name = "Employee 1" });
            Console.WriteLine("Employee created successfully!");
            Console.ReadKey();
        }
    }

    public class Employee : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface IDomainObject
    {
        int Id { get; set; }
    }
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }

        public T ReadById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> ReadAll()
        {
            return _dbSet.ToList();
        }

        public bool Update(T entity)
        {
            var existingEntity = _dbSet.Find(entity.Id);
            if (existingEntity == null) return false;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            _context.SaveChanges();
            return true;
        }
    }
    public class BL
    {
        private readonly IRepository<Employee> _repository;

        public BL(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        public void CreateEmployee(Employee employee)
        {
            _repository.Add(employee);
        }
    }
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EmployeeDb;Trusted_Connection=True;");
        }
    }
}