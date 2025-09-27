using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.ResultModel
{
    public class ResultModelVoid
    {
            public bool Success { get; set; }
            public string SuccessFullMessage { get; set; }
            public string ErroredMessage { get; set; }
            //public static ResultModelVoid SuccessResult(string message)
            //{
            //    return new ResultModelVoid
            //    {
            //        Success = true,
            //        SuccessFullMessage = message
            //    };
            //}
            //public static ResultModelVoid FailureResult(string error)
            //{
            //    return new ResultModelVoid
            //    {
            //        Success = false,
            //        ErroredMessage = error,

            //    };
            //}
        }
    }

