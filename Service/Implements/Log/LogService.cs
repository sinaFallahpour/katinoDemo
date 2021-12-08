using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LogService : IlogService
    {
        private readonly DataContext _dataContext;

        public LogService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task CreateLog(string ExceptionMessage, string ExceptionType, 
            string MethodName, string TableName,string userName=" ")
        {
            var newLog = new Domain.Log()
            {
                Date=DateTime.Now,
                ExceptionMessage=ExceptionMessage,
                ExceptionType=ExceptionType,
                MethodName=MethodName,
                TableName=TableName,
                UserName=userName
            };
            await _dataContext.Logs.AddAsync(newLog);
            await _dataContext.SaveChangesAsync();
        }


    }
}
