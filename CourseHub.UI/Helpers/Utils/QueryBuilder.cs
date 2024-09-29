using System.Reflection;
using System.Text;

namespace CourseHub.UI.Helpers.Utils;

public static class QueryBuilder
{
    public static string Build(object dto)
    {
        Type type = dto.GetType();

        List<string> kvps = new();

        foreach (PropertyInfo property in type.GetProperties())
        {
            string propertyName = property.Name;
            object? propertyValue = property.GetValue(dto);

            if (propertyValue is not null)
            {
                string encodedName = Uri.EscapeDataString(propertyName);

                if (property.PropertyType.IsArray)
                {
                    if (propertyValue is Guid[] guidArr)
                        foreach (var val in guidArr)
                            kvps.Add($"{encodedName}={Uri.EscapeDataString(val.ToString())}");
                    else if (propertyValue is string[] stringArr)
                        foreach (var val in stringArr)
                            kvps.Add($"{encodedName}={Uri.EscapeDataString(val)}");
                    else if (propertyValue is int[] intArr)
                        foreach (var val in intArr)
                            kvps.Add($"{encodedName}={Uri.EscapeDataString(val.ToString())}");
                }
                else
                {
                    kvps.Add($"{encodedName}={Uri.EscapeDataString(propertyValue.ToString()!)}");
                }
            }
        }

        return string.Join("&", kvps);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="urlPath">Example: "api/users/multiple?"</param>
    /// <param name="elementFormat">Example: "ids={0}&"</param>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static string BuildWithArray(string urlPath, string elementFormat, IEnumerable<string> elements)
    {
        StringBuilder query = new(urlPath);
        foreach (var id in elements)
            query.AppendFormat(elementFormat, id);
        query.Length--;

        return query.ToString();
    }
}
