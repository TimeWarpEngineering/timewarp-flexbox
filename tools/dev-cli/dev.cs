#!/usr/bin/env -S dotnet --
// ═══════════════════════════════════════════════════════════════════════════════
// DEV CLI - timewarp-flexbox DEVELOPMENT TOOL
// ═══════════════════════════════════════════════════════════════════════════════
//
// Usage:
//   As runfile:  dotnet run tools/dev-cli/dev.cs -- <command>
//   As AOT:      ./bin/dev <command>
//
// Run `./bin/dev --help` for available commands.
//
// To bootstrap:
//   dotnet run tools/dev-cli/dev.cs -- self-install
//   direnv allow
//   dev --help
// ═══════════════════════════════════════════════════════════════════════════════

#region Purpose
// Entry point for the dev CLI
#endregion
#region Design
// Thin wrapper around TimeWarp.Nuru. Shared endpoints (clean, self-install,
// check-version, release) come from TimeWarp.Nuru.DevCli and require the DI
// registrations below (NURU050 names any that are missing).
// ICommand and Unit live in TimeWarp.Mediator. Mediator 15 (Nuru 4.0.0-beta.12)
// dropped ICommandHandler; handlers implement IRequestHandler and return ValueTask.
// UseTelemetry is on so a CI job or a ganda walk that shells out to dev exports
// one command span. ServiceName is the literal timewarp-flexbox-dev;
// OTEL_SERVICE_NAME overrides it. An unset OTEL_EXPORTER_OTLP_ENDPOINT exports nothing.
// why: ganda repo audit dev-cli-telemetry (task 163, 2026-11-26)
#endregion

NuruApp app = NuruApp.CreateBuilder()
  .WithName("dev")
  .WithDescription("Development CLI for timewarp-flexbox")
  .UseTelemetry(options => options.ServiceName = "timewarp-flexbox-dev")
  .ConfigureServices(services =>
  {
    services.AddSingleton<IRepoCleanService, RepoCleanService>();
    services.AddSingleton<NuGetVersionService>();
    services.AddSingleton<IRepoConfigService, RepoConfigService>();
    services.AddSingleton<IPackableProjectService, PackableProjectService>();
  })
  .DiscoverEndpoints()
  .Build();

return await app.RunAsync(args);
