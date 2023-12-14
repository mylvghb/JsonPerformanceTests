// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using Bogus;
using JsonPerformanceTests;
using ServiceStack.Text;

// var summary = BenchmarkRunner.Run<SerializeBigDataBenchmark>();
// Console.WriteLine(summary);
// var summary = BenchmarkRunner.Run<SerializeMuchDataBenchmark>();
// Console.WriteLine(summary);

var faker = new Faker<User>()
    .CustomInstantiator(f => new User(
        Guid.NewGuid(),
        f.Name.FirstName(),
        f.Name.LastName(),
        f.Name.FullName(),
        f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()),
        f.Internet.Email(f.Name.FirstName(), f.Name.LastName())
    ));

Stopwatch stopwatch;
string text;
var users  = faker.Generate(10000);

stopwatch = Stopwatch.StartNew();
text = Newtonsoft.Json.JsonConvert.SerializeObject(users);
stopwatch.Stop();
Console.WriteLine($"JSON.NET Serialized {users.Count} users in {stopwatch.ElapsedMilliseconds}ms");

JsConfig.IncludeTypeInfo = false;
stopwatch = Stopwatch.StartNew();
text = JsonSerializer.SerializeToString(users);
stopwatch.Stop();
Console.WriteLine($"ServiceStack Serialized {users.Count} users in {stopwatch.ElapsedMilliseconds}ms");

stopwatch = Stopwatch.StartNew();
text = System.Text.Json.JsonSerializer.Serialize(users);
stopwatch.Stop();
Console.WriteLine($"Microsoft Serialized {users.Count} users in {stopwatch.ElapsedMilliseconds}ms");

