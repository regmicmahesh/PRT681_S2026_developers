using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Common
{
    internal class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; }

        private Result(bool isSuccess, string message, T? data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T>(true, message, data);
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>(false, message, default);
        }
    }
}
