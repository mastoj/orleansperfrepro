# How to reproduce the performance issue

First from the main branch start the aspire project, main runs aspire 9.2: 

```
dotnet build && dotnet run --project orleansdemo.AppHost/orleansdemo.AppHost.csproj
```

With the project running you can run the k6 test:

```
k6 run scripts/perf.js
```

Note the number of request you get in the k6 report (I get around 6k iterations when running 60s).

Now checkout the aspire-9.0 branch and run the exact same commands and compare the result (I get around 600k iterations when running 60s).