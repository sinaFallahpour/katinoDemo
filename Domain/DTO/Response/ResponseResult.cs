using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.DTO.Response
{
    public class ResponseResult
    {
        //Return code 
        public StatusCode StatusCode { get; set; }
        //Return Message
        public List<string> Message { get; set; }
        //if Success Return 1 else 0
        public bool SuccessCode { get; set; }
        //REturn Result
        public object Resul { get; set; }
        public ResponseResult()
        {

        }
        public ResponseResult(StatusCode statusCode, List<string> messages, bool isSuccess, object result)
        {
            this.StatusCode = statusCode;
            this.Message = messages;
            this.SuccessCode = isSuccess;
            this.Resul = result;
        }
        public ResponseResult AddMessage(ResponseResult result, string message)
        {
            result.Message.Add(message);
            return result;
        }
        public ResponseResult(int statuscode)
        {
            var message = new List<string>();
            switch (statuscode)
            {
                case 401:
                    message.Add("Unauthorized");
                    this.StatusCode = StatusCode.unAuthorize;
                    this.Message = message;
                    this.SuccessCode = false;
                    this.Resul = null;
                    break;
                case 404:
                    message.Add("notFound");
                    this.StatusCode = StatusCode.notFound;
                    this.Message = message;
                    this.SuccessCode = false;
                    this.Resul = null;
                    break;
                case 403:
                    message.Add("forbidden");
                    this.StatusCode = StatusCode.forbidden;
                    this.Message = message;
                    this.SuccessCode = false;
                    this.Resul = null;
                    break;
                default:
                    this.StatusCode = StatusCode.another;
                    this.Message = null;
                    this.SuccessCode = false;
                    this.Resul = null;
                    break;


            }
        }
    }
}
