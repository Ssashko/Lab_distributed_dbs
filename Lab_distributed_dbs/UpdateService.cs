using Lab_distributed_dbs.DAL;
using Lab_distributed_dbs.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace Lab_distributed_dbs
{
    public class UpdateService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UpdateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using (var scope = _serviceScopeFactory.CreateAsyncScope())
            using (var dbContext = scope.ServiceProvider.GetRequiredService<LabDbContext>())
            using (var transaction = new
            TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var studentService =
            scope.ServiceProvider.GetRequiredService<StudentService>();
                var lecturerService =
            scope.ServiceProvider.GetRequiredService<LecturerService>();
                try
                {
                    var students = dbContext.Students.ToList();
                    foreach (var student in students)
                    {
                        await studentService.CreateAsync(student);
                    }
                    var lecturers = dbContext.Lecturers.ToList();
                    foreach (var lecturer in lecturers)
                    {
                        await lecturerService.CreateAsync(lecturer);
                    }
                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
