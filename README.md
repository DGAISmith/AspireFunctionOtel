# AspireFunctionOtel

Sample repo demonstrating otel filtering difficulties with the Azure Functions host - requested on https://github.com/dotnet/aspire/issues/10225

# Commit History

1. Empty repo
2. Add gitignore, stock aspire starter app
3. Add stock function app with http trigger, with enlist in aspire orchestration
4. Bump all nuget packages
5. Fix function app port misalightment in aspire apphost (function app was generated with port 7091, console log from function app reports 7071, "fixed" in launchSettings)
6. Add weatherforecast feature twin - duplicate page in blazor frontend, function app trigger modified to resemble existing forecast endpoint in webapi.
7. Remove app insights from function app to reduce potential confusion. Add some logging at different levels. Raise min log level to Warning in webapi and function app host.
8. Add service bus trigger with more logging, service bus emulator, and aspire dashboard command to send test message.
9. Add this readme update!

# Problem Description - Host Filtering

Beyond commit 7, you can see the differences between a webapi and function app http trigger. Run the apphost, visit both weather pages in the blazor frontend app. If you just want to examine the logging filtering difference, checkout commit 7.

In the webapi, we have set the OpenTelemetry and top-level min log level to warning. This has the effect of:

- Both log messages (info and warning) still appear in the webapi Console.
- Only warning appears in structured logs in the aspire dashboard.

In the function app, we have set the min log levels to Warning AND attempted to use the same logging provider convention in host.json for the "OpenTelemetry" provider alias as used in the webapi.

- Only the warning log appears in the function app Console.
- Both log messages (info and warning) appear in structured logs in the aspire dashboard. AND they're duplicated. AND there's a lot more logging coming from the function host.

(obvs, console logging is different in function apps, just mentioning it for extra detail)

# Problem Description - Host Log Noise

If you go all the way to commit 8, we now add a service bus trigger, emulator, a queue, a dashboard command to send a test message. If you start the app up now, even before you send the command you get a LOT of noise in structured logs, which keeps growing. After 30 seconds or so you have approx 1900 logs.
