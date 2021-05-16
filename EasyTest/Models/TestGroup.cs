using System.Collections.Generic;

namespace EasyTest.Models
{
    public record TestGroup(
        string Name,
        List<TestConfig> Tests
    );
}
