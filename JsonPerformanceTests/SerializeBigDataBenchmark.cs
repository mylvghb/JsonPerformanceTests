using BenchmarkDotNet.Attributes;
using Bogus;

namespace JsonPerformanceTests;

[MemoryDiagnoser]
public class SerializeBigDataBenchmark
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
    public void NewtonsoftSerializeBigData() =>
        _ = Newtonsoft.Json.JsonConvert.SerializeObject(_testUsers);

    [Benchmark]
    public void MicrosoftSerializeBigData() =>
        _ = System.Text.Json.JsonSerializer.Serialize(_testUsers);
    
    [Benchmark]
    public void ServiceStackSerializeBigData() =>
        _ = ServiceStack.Text.JsonSerializer.SerializeToString(_testUsers);
}