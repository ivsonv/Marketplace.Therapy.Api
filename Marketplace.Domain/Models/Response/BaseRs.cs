using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain.Models.Response
{
    public class BaseRs<T>
    {
        public T content { get; set; }
        public BaseError error { get; set; }
        public string pathimage { get; set; }

        public void setError(Exception ex)
        {
            this.error = new BaseError(ex);
        }
        public void setError(List<string> msgs)
        {
            this.error = new BaseError(msgs);
        }
        public void setError(IEnumerable<string> msgs)
        {
            this.error = new BaseError(msgs.ToList());
        }
        public void setError(string msg)
        {
            this.error = new BaseError(new List<string>() { msg });
        }
    }

    public class BaseError
    {
        public BaseError(List<string> msgs)
        {
            this.message = msgs;
        }

        public BaseError(Exception ex)
        {
            this.message.Add(ex.Message);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.description = ex.StackTrace;
                this.description += ex.InnerException?.Message;
            }
        }

        public List<string> message { get; } = new List<string>();
        public string description { get; set; }
    }
}
