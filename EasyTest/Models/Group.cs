using System.Collections.Generic;

namespace EasyTest.Models
{
    public record Group(
        string Name,
        List<Config> Tests
    );
}
