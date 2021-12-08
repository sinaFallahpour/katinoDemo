using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IlogService
    {
        Task CreateLog(string ExceptionMessage,string ExceptionType,string MethodName
            ,string TableName,string userName=" ");
       

    }
}
