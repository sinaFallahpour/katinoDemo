using DNTPersianUtils.Core;
using Domain;
using Domain.DTO.AnswerQuestion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AnswerQuestionService : IAnswerQuestionService
    {
        private readonly DataContext _dataContext;
        private readonly IlogService _ilog;

        public AnswerQuestionService(DataContext dataContext, IlogService ilog)
        {
            _dataContext = dataContext;
            _ilog = ilog;
        }
        public async Task<(bool isSuccess, string error)> CreateAnswerQuestion(AnswerQuestion model)
        {
            try
            {
                var newQuestionAnswer = new FrequentlyAskedQuestion()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Answer = model.Answer,
                    Question = model.Question

                };
                await _dataContext.FrequentlyAskedQuestions.AddAsync(newQuestionAnswer);
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }

            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CreateAnswerQuestion", "FrequentlyAskedQuestions");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> DeleteAnswerQuestion(int id)
        {
            try
            {
                var deletedAnswerQuestion = await _dataContext.FrequentlyAskedQuestions.FindAsync(id);
                _dataContext.FrequentlyAskedQuestions.Remove(deletedAnswerQuestion);
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }

            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteAnswerQuestion", "FrequentlyAskedQuestions");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> EditAnswerQuestion(EditAnswerQuestion model)
        {
            try
            {
                var editedAnswerQuestion = await _dataContext.FrequentlyAskedQuestions.FindAsync(model.Id);
                editedAnswerQuestion.UpdateDate = DateTime.Now;
                editedAnswerQuestion.Answer = model.Answer;
                editedAnswerQuestion.Question = model.Question;

                _dataContext.FrequentlyAskedQuestions.Update(editedAnswerQuestion);
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }

            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditAnswerQuestion", "FrequentlyAskedQuestions");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(List<AllAnswerQuestion> model, bool isSuccess, string error)> GetAllAnswerQuestion()
        {
            try
            {
                var allAnswerQuestion = await _dataContext.FrequentlyAskedQuestions
                    .Select(x => new AllAnswerQuestion()
                    {
                        Answer = x.Answer,
                        Id = x.Id,
                        Question = x.Question
                    })
                    .ToListAsync();
                if (allAnswerQuestion.Count == 0)
                {
                    return (null, false, "اطلاعاتی یافت نشد");
                }

                return (allAnswerQuestion, true, "");


            }

            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllAnswerQuestion", "FrequentlyAskedQuestions");

                return (null, false, "مشکلی رخ داده است");

            }
        }

        public async Task<(AllAnswerQuestion model, bool isSuccess, string error)> GetAnswerQuestionById(int id)
        {
            try
            {
                var answerQuestion = await _dataContext.FrequentlyAskedQuestions
                    .Select(x => new AllAnswerQuestion()
                    {
                        Answer = x.Answer,
                        Id = x.Id,
                        Question = x.Question
                    })
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (answerQuestion == null)
                {
                    return (null, false, "اطلاعاتی یافت نشد");
                }

                return (answerQuestion, true, "");


            }

            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAnswerQuestionById", "FrequentlyAskedQuestions");

                return (null, false, "مشکلی رخ داده است");

            }
        }
    }
}
