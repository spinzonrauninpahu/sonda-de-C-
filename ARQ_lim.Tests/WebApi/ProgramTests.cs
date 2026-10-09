using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace ARQ_lim.Tests.WebApi;

public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        var testFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IOrderRepository>();
                services.AddSingleton<IOrderRepository>(
                    new FakeOrderRepository());
            });
        });

        testFactory.Server.AllowSynchronousIO = true;
        _client = testFactory.CreateClient();
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("\"ok\"", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Info_ReturnsVersion()
    {
        var response = await _client.GetAsync("/info");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var json = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        Assert.Equal(
            "v0.0.2-secure :)",
            json.RootElement.GetProperty("version").GetString());
    }

    [Fact]
    public async Task OrdersLast_ReturnsOk()
    {
        var response = await _client.GetAsync("/orders/last");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_ValidData_ReturnsOk()
    {
        using var content = new StringContent(
            "Carlos,Teclado,2,50",
            Encoding.UTF8,
            "text/plain");

        var response = await _client.PostAsync("/orders", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Status: {(int)response.StatusCode}. Response: {responseBody}");

        using var json = JsonDocument.Parse(responseBody);

        Assert.Equal(
            "Carlos",
            json.RootElement.GetProperty("customerName").GetString());

        Assert.Equal(
            "Teclado",
            json.RootElement.GetProperty("productName").GetString());

        Assert.Equal(
            2,
            json.RootElement.GetProperty("quantity").GetInt32());
    }

    [Fact]
    public async Task CreateOrder_EmptyBody_UsesDefaultValues()
    {
        using var content = new StringContent(
            "",
            Encoding.UTF8,
            "text/plain");

        var response = await _client.PostAsync("/orders", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var json = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        Assert.Equal(
            "anon",
            json.RootElement.GetProperty("customerName").GetString());

        Assert.Equal(
            "unknown",
            json.RootElement.GetProperty("productName").GetString());

        Assert.Equal(
            1,
            json.RootElement.GetProperty("quantity").GetInt32());

        Assert.Equal(
            0.99m,
            json.RootElement.GetProperty("unitPrice").GetDecimal());
    }

    private sealed class FakeOrderRepository : IOrderRepository
    {
        public void Create(Order order)
        {
            // No accede a SQL Server porque usa un repositorio falso
        }
    }
}