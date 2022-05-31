using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Models
{
    public class Response<T>
    {
        public T? Data { get; }
        public string? Message { get; }
        public bool Success { get; }

        public Response(T data)
        {
            this.Success = true;
            this.Data = data;
        }
        public Response(string Message)
        {
            this.Success = false;
            this.Message = Message;
            this.Data = default(T);
        }

        public static implicit operator Response<T>(string message)
        {
            return new Response<T>(message);
        }

    }



}
