using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Marketplace.Domain.Models
{
    [Microsoft.AspNetCore.Mvc.ModelBinder(BinderType = typeof(PaginationBinder))]
    public class Pagination
    {
        public int page { get; set; } = 0;
        public int size { get; set; } = 20;
        public bool asc { get; set; }
    }

    public class PaginationBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string asc = ((string)bindingContext.ValueProvider.GetValue("asc"));
            string page = ((string)bindingContext.ValueProvider.GetValue("page") ?? "0").Replace("undefined", "0").Replace("NaN", "0");
            string size = ((string)bindingContext.ValueProvider.GetValue("size") ?? "20").Replace("undefined", "20");

            var model = new Pagination()
            {
                page = int.Parse(page ?? "0"),
                size = int.Parse(size ?? "20"),
                asc = asc != null
            };
            if (model.size <= 0) model.size = 20;

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
