The output functionAppDefaultHostName was https://3rx47yhxqmxc6.azurewebsites.net/api/Function1?code=<host_key>.

Take host_key on Azure Portal in the Function App - Functions - App keys.

The URLs to trigger the other two functions are
http://3rx47yhxqmxc6.azurewebsites.net/api/CreatingResponseExplicitly?code=<host_key>
http://3rx47yhxqmxc6.azurewebsites.net/api/ReadingFromRequest/vasya?code=<host_key>

Make sure your App service plan is on Y1 pricing. That is a free plan for Azure Function hosting.