using BenchmarkDotNet.Attributes;
using Bogus;

namespace JsonPerformanceTests;

[MemoryDiagnoser]
public class SerializeMuchDataBenchmark
{
    private List<User>? _testUsers;

    [Params(10000)] public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        var faker = new Faker<User>()
            .CustomInstantiator(f => new User(
                Guid.NewGuid(),
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Name.FullName(),
                f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()),
                f.Internet.Email(f.Name.FirstName(), f.Name.LastName())
            ));

        _testUsers = faker.Generate(Count);
    }
    
    [Benchmark(Baseline = true)]
    public void NewtonsoftSerializeMuchData()
    {
        foreach (var user in _testUsers!)
        {
            _ = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        }
    }

    [Benchmark]
    public void MicrosoftSerializeMuchData()
    {
        foreach (var user in _testUsers!)
        {
            _ = System.Text.Json.JsonSerializer.Serialize(user);
        }
    }
    
    [Benchmark]
    public void ServiceStackSerializeMuchData()
    {
        foreach (var user in _testUsers!)
        {
            _ = ServiceStack.Text.JsonSerializer.SerializeToString(user);
        }
    }
}