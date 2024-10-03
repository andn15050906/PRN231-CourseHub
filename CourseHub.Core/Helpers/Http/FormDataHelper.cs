namespace CourseHub.Core.Helpers.Http;

public class FormDataHelper
{
    private MultipartFormDataContent _content;

    /// <summary>
    /// Converted to StringContent
    /// </summary>
    public Dictionary<string, string?> KeyValuePairs { get; set; }

    /// <summary>
    /// Convert to StreamContent, Name and FileName
    /// </summary>
    public List<(Stream, string, string)> Files { get; set; }

    public MultipartFormDataContent ToFormData()
    {
        _content = new();

        if (KeyValuePairs is not null)
        {
            foreach (KeyValuePair<string, string?> kvp in KeyValuePairs)
                if (kvp.Value is not null)
                    _content.Add(new StringContent(kvp.Value), kvp.Key);
        }
        if (Files is not null)
        {
            foreach ((Stream, string, string) file in Files)
                _content.Add(new StreamContent(file.Item1), file.Item2, file.Item3);
        }

        return _content;
    }
}
