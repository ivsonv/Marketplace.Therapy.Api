﻿using Newtonsoft.Json;

namespace Marketplace.Domain.Models.Request
{
    public class BaseRq<T>
    {
        public Pagination pagination { get; set; } = new Pagination();
        public string search { get; set; }
        public T data { get; set; }
    }
}
