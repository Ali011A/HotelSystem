using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.ResultModel
{
    public class ResultModel<T> : ResultModelVoid
    {
        public T Data { get; set; }  

        //public static ResultModel<T> SuccessResult(T data, string message )
        //{
        //    return new ResultModel<T>
        //    {
        //        Success = true,
        //        SuccessFullMessage = message,
        //        Data = data
        //    };
        //}

        //public static ResultModel<T> FailureResult(string error)
        //{
        //    return new ResultModel<T>
        //    {
        //        Success = false,
        //        ErroredMessage = error,
                
        //    };
        //}

        
    }

   
}

