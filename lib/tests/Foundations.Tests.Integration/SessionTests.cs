using DA.Foundations.DataAccess;
using DA.Foundations.DependencyInjection;
using DA.Foundations.Examples;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Text.Json;

namespace Foundations.Tests.Integration;

//TODO: Add integration test for Rollback
public class SessionTests
{
    private JsonSerializerOptions serializerOptions = new() { WriteIndented = true };

    [Fact]
    public async Task UseSessions_Should_LoadModifyAndPersist_Session()
    {
        var inputPath = Path.Combine(Path.GetTempPath(), $"session_input_{Guid.NewGuid()}.json");
        var outputPath = Path.Combine(Path.GetTempPath(), $"session_output_{Guid.NewGuid()}.json");

        var customer = new CustomerDto(Guid.NewGuid(), "Original");
        var snapshot = new ExampleSessionSnapshot([customer]);

        await File.WriteAllTextAsync(inputPath, JsonSerializer.Serialize(snapshot, serializerOptions));

        var services = new ServiceCollection();
        services.UseSession<ExampleSession, ExampleSessionSnapshot>(options => options.Mode = PersistenceMode.JsonFile);
        services.AddScoped<ICustomerRepository, ExampleRepository>();

        var provider = services.BuildServiceProvider();

        var fileStore = provider.GetRequiredService<IFileDataStore>();
        var repository = provider.GetRequiredService<ICustomerRepository>();

        var loadResult = await fileStore.LoadAsync(inputPath);
        loadResult.IsSuccess.ShouldBeTrue();

        var id = new CustomerId { Value = customer.Id };
        var loadedCustomerResult = await repository.GetByIdAsync(id);
        loadedCustomerResult.TryGetValue(out var loadedCustomer).ShouldBeTrue();
        loadedCustomer.Naam.ShouldBe(customer.Naam);

        var updatedCustomerResult = await repository.UpdateNaam(id, "updated");
        updatedCustomerResult.IsSuccess.ShouldBeTrue();

        var persistResult = await fileStore.PersistAsync(outputPath);
        persistResult.IsSuccess.ShouldBeTrue();

        var savedJson = await File.ReadAllTextAsync(outputPath);
        savedJson.ShouldNotBeNull();
        savedJson.ShouldContain("updated");

        File.Delete(inputPath);
        File.Delete(outputPath);
    }

    [Fact]
    public async Task UseSession_Should_BeAbleToRollback_Session()
    {
        var inputPath = Path.Combine(Path.GetTempPath(), $"session_input_{Guid.NewGuid()}.json");
        var outputPath = Path.Combine(Path.GetTempPath(), $"session_output_{Guid.NewGuid()}.json");

        var customer = new CustomerDto(Guid.NewGuid(), "Original");
        var snapshot = new ExampleSessionSnapshot([customer]);

        await File.WriteAllTextAsync(inputPath, JsonSerializer.Serialize(snapshot, serializerOptions));

        var services = new ServiceCollection();
        services.UseSession<ExampleSession, ExampleSessionSnapshot>(options => options.Mode = PersistenceMode.JsonFile);
        services.AddScoped<ICustomerRepository, ExampleRepository>();

        var provider = services.BuildServiceProvider();

        var fileStore = provider.GetRequiredService<IFileDataStore>();
        var repository = provider.GetRequiredService<ICustomerRepository>();

        var loadResult = await fileStore.LoadAsync(inputPath);
        loadResult.IsSuccess.ShouldBeTrue();

        var id = new CustomerId { Value = customer.Id };
        var loadedCustomerResult = await repository.GetByIdAsync(id);
        loadedCustomerResult.TryGetValue(out var loadedCustomer).ShouldBeTrue();
        loadedCustomer.Naam.ShouldBe(customer.Naam);

        var updatedCustomerResult = await repository.UpdateNaam(id, "updated");
        updatedCustomerResult.IsSuccess.ShouldBeTrue();

        fileStore.Rollback();

        var persistResult = await fileStore.PersistAsync(outputPath);
        persistResult.IsSuccess.ShouldBeTrue();

        var savedJson = await File.ReadAllTextAsync(outputPath);
        savedJson.ShouldNotBeNull();
        savedJson.ShouldContain("Original");

        File.Delete(inputPath);
        File.Delete(outputPath);
    }
}
