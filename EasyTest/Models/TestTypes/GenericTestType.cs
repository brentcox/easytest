namespace EasyTest.Models.TestTypes
{
    public record GenericTestType(
        string[] PreRequestScript, 
        string[] TestScript
        ) : BaseTestType(PreRequestScript, TestScript);
}
