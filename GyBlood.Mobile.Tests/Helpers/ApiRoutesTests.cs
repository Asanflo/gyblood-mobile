using System.Reflection;
using GyBlood.Mobile.Core.Constants;

namespace GyBlood.Mobile.Tests.Helpers;

public class ApiRoutesTests
{
    [Fact]
    public void Routes_are_relative_to_the_base_address()
    {
        var routes = typeof(ApiRoutes).GetNestedTypes()
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(f => f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => (Name: f.DeclaringType!.Name + "." + f.Name, Value: (string)f.GetRawConstantValue()!))
            .ToList();

        Assert.NotEmpty(routes);
        Assert.All(routes, r => Assert.False(r.Value.StartsWith('/'), $"{r.Name} ne doit pas commencer par '/'"));
    }
}
