using Domain;
using Domain.DTO.AnswerQuestion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAnswerQuestionService
    {
        Task<(bool isSuccess, string error)> CreateAnswerQuestion(AnswerQuestion model);
        Task<(bool isSuccess, string error)> EditAnswerQuestion(EditAnswerQuestion model);
        Task<(List<AllAnswerQuestion> model, bool isSuccess, string error)> GetAllAnswerQuestion();
        Task<(AllAnswerQuestion model, bool isSuccess, string error)> GetAnswerQuestionById(int id);
        Task<(bool isSuccess, string error)> DeleteAnswerQuestion(int id);
    }
}
