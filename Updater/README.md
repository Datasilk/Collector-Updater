# Collector Updater
### A worker service that will automatically update various applications associated with Collector

The updater has a single purpose, which is to allow engineers to install the application onto a server, 
and after configuring the updater application, it will run in the background and ensure to 
install, update, & run any applications that it was configured to manage.

## config.json
Determines what software will be downloaded, installed, and ran on the server

```json
{
  "Api": "http://localhost:5146",
  "Version": "0.1",
  "InstallPath": "C:\\Collector\\",
  "Apps": [
    {
      "Name": "collector",
      "Port": ""
    },
    {
      "Name": "charlotte",
      "Port": "5007"
    },
    {
      "Name": "charlottes-web",
      "Port": "5005"
    }
  ]
}
```

## Apps
There are three possible applications that can be added to the config as shown above. 
You can only add one instance of each type to the `Apps` section of your **config.json**.
Each application is actually an ASP.NET Core web application. The application `web.config` 
will be altered to set the application port to the **config.json** `Port` property. 
This will trigger IIS to restart the application pool.

