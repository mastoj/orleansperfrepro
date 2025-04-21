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

## Create and run docker test

To build images to test you can run

```
scripts/build-docker.sh test
```

This will create server and client image with the label `test`. The `docker-compose` file has been prepared with the `test` label so to start the application you can run:

```
docker-compose up -d
```

Now to run the test you just need to run the k6 script with an env variable: 

```
TEST_ENV=docker k6 run scripts/perf.js
```

This gives me even better performance than before, so there is something that the hosting in aspire does that impacts the performance of orleans applications.