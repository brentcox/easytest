using System.Collections.Generic;

namespace EasyTest.Models
{
    public record Project(
        string ProjectName,
        List<Group> Groups
        );
}
