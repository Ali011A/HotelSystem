using Hotel.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace Hotel.Api.ResponseViewModel
{
    public  class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }

        public string Massage { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public StatusCode Status { get; set; }

        public static ResponseViewModel<T> Success(String massage, T data, StatusCode status = new StatusCode())
        {
            return new ResponseViewModel<T>
            {
                Massage = massage,
                Data = data,
                IsSuccess = true,
                Status = status

            };
        }

        public static ResponseViewModel<T> Failuare(String massage, List<String> errors, StatusCode status = new StatusCode())
        {
            return new ResponseViewModel<T>
            {
                Massage = massage,
                Errors = errors ?? new List<string>(),
                IsSuccess = false,
                Status = status

            };

        }


    }
}
