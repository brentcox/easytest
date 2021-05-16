namespace EasyTest.Models.TestTypes
{
    public record RestApiTestType(
        string[] PreRequestScript, 
        string[] TestScript,
        string Url,
        string Method,
        string[] Headers,
        string Body
        ) : BaseTestType(PreRequestScript, TestScript);
}
