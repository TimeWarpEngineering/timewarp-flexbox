#region Purpose
// Global usings for dev-cli
#endregion

global using System;
global using System.IO;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Xml.Linq;
global using System.Net.Http;
global using System.Text.Json;

global using TimeWarp.Nuru;
// ICommand, IRequestHandler, and Unit live in TimeWarp.Mediator. Mediator 15
// (Nuru 4.0.0-beta.12) replaced ICommandHandler with IRequestHandler and
// Handle returns ValueTask. See tools/dev-cli/dev.cs Design.
global using TimeWarp.Mediator;
global using TimeWarp.Amuru;
global using TimeWarp.Terminal;
global using DevCli;
global using Microsoft.Extensions.DependencyInjection;