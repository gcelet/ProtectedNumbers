// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

using Aspire.Hosting;

using Microsoft.Extensions.Logging;

using NUnit.Framework.Internal;

[SetUpFixture]
public static class WebApiProvider
{
  private const int InfrastructureTimeout = 30_000;

  private static IDistributedApplicationTestingBuilder? AppHost { get; set; }

  private static DistributedApplication? DistributedApplication { get; set; }

  private static TimeSpan WaitingTimeout { get; } = TimeSpan.FromMilliseconds(InfrastructureTimeout - 500);

  private static Dictionary<WebApi, HttpClient> WebApiAccessors { get; } = new();

  public static HttpClient? GetHttpClient(WebApi webApi) => WebApiAccessors.GetValueOrDefault(webApi);

  [OneTimeSetUp]
  public static async Task OneTimeSetup()
  {
    using CancellationTokenSource cancellationTokenSource = new();

    cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(InfrastructureTimeout));

    CancellationToken cancellationToken = cancellationTokenSource.Token;

    await BuildAppHost(cancellationToken);
    await BuildDistributedApplication(cancellationToken);
    await BuildWebApiAccessors(cancellationToken);
  }

  [OneTimeTearDown]
  public static async Task OneTimeTearDown()
  {
    using CancellationTokenSource cancellationTokenSource = new();

    cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(InfrastructureTimeout));

    CancellationToken cancellationToken = cancellationTokenSource.Token;

    DisposeWebApiAccessors();
    await DisposeDistributedApplication(cancellationToken);
    await DisposeAppHost();
  }

  private static async Task BuildAppHost(CancellationToken cancellationToken)
  {
    await DisposeAppHost();

    AppHost =
      await DistributedApplicationTestingBuilder.CreateAsync<Projects.ProtectedNumbers_Tests_EndToEnd_AppHost>(cancellationToken);

    AppHost.Services.AddLogging(logging =>
    {
      logging.SetMinimumLevel(LogLevel.Debug);
      // Override the logging filters from the app's configuration
      logging.AddFilter(AppHost.Environment.ApplicationName, LogLevel.Debug);
      logging.AddFilter("Aspire.", LogLevel.Debug);
    });

    AppHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
    {
      clientBuilder.AddStandardResilienceHandler();
    });
  }

  private static async Task BuildDistributedApplication(CancellationToken cancellationToken)
  {
    if (AppHost == null)
    {
      throw new NUnitException($"Can't build '{nameof(DistributedApplication)}' when '{nameof(AppHost)}' is null");
    }

    await DisposeDistributedApplication(cancellationToken);

    DistributedApplication = await AppHost.BuildAsync(cancellationToken)
        .WaitAsync(WaitingTimeout, cancellationToken)
      ;

    await DistributedApplication.StartAsync(cancellationToken)
        .WaitAsync(WaitingTimeout, cancellationToken)
      ;
  }

  private static async Task BuildWebApiAccessors(CancellationToken cancellationToken)
  {
    if (DistributedApplication == null)
    {
      throw new NUnitException($"Can't build '{nameof(WebApiAccessors)}' when '{nameof(DistributedApplication)}' is null");
    }

    DisposeWebApiAccessors();

    Dictionary<WebApi, HttpClient> webApiAccessors = new();
    List<Task> resourceWaitTasks = new();

    foreach (WebApi webApi in WebApi.EnumerateWebApi())
    {
      HttpClient httpClient = DistributedApplication.CreateHttpClient(webApi.ResourceName);
      Task resourceWaitTask = DistributedApplication.ResourceNotifications
          .WaitForResourceHealthyAsync(webApi.ResourceName, cancellationToken)
          .WaitAsync(WaitingTimeout, cancellationToken)
        ;

      webApiAccessors[webApi] = httpClient;
      resourceWaitTasks.Add(resourceWaitTask);
    }

    if (resourceWaitTasks.Count > 0)
    {
      await Task.WhenAll(resourceWaitTasks);
    }

    if (webApiAccessors.Count > 0)
    {
      foreach (KeyValuePair<WebApi, HttpClient> kvp in webApiAccessors)
      {
        WebApiAccessors[kvp.Key] = kvp.Value;
      }
    }
  }

  private static async Task DisposeAppHost()
  {
    if (AppHost == null)
    {
      return;
    }

    await AppHost.DisposeAsync();
    AppHost = null;
  }

  private static async Task DisposeDistributedApplication(CancellationToken cancellationToken)
  {
    if (DistributedApplication == null)
    {
      return;
    }

    await DistributedApplication.StopAsync(cancellationToken);
    await DistributedApplication.DisposeAsync();
    DistributedApplication = null;
  }

  private static void DisposeWebApiAccessors()
  {
    if (WebApiAccessors.Count == 0)
    {
      return;
    }

    List<HttpClient> webApiAccessors = WebApiAccessors.Values.ToList();

    WebApiAccessors.Clear();

    foreach (HttpClient httpClient in webApiAccessors)
    {
      httpClient.CancelPendingRequests();
      httpClient.Dispose();
    }
  }
}
