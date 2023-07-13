using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsposeTestTask.BLL.Services
{
    public class EnumHelper
    {
        public static SelectList GetSelectListFor(Type enumType)
    {
        var values = Enum.GetValues(enumType)
                         .Cast<Enum>()
                         .Select(e => new SelectListItem
                         {
                             Value = Convert.ToInt32(e).ToString(),
                             Text = e.ToString()
                         })
                         .ToList();

        return new SelectList(values, "Value", "Text");
    }
    }
}
