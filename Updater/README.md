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
  "InstallPath": "C:\\Collector\\",
  "Apps": [
    {
      "Name": "collector",
      "Port": "7070",
      "Version": "0.1"
    },
    {
      "Name": "charlotte",
      "Port": "7701",
      "Version": "0.1"
    },
    {
      "Name": "charlottes-web",
      "Port": "7700",
      "Version": "0.1"
    }
  ]
}
```

## Apps
There are three possible applications that can be added to the config as shown above. 
You can only add one instance of each type to the `Apps` section of your **config.json**.
Each application is actually an ASP.NET Core web application. The application `web.config` 
will be altered to set the application port to the `Port` property defined in **config.json** 
`Apps` section. Altering `web.config` will trigger IIS to restart the associated application pool.

# Installation
Publish a release of the Updater service, then register the application as a Windows Service

```powershell
New-Service -Name "Collector Updater" -BinaryPathName "c:\Collector\Updater\Updater.exe" -Description "Software updater for Collector" -DisplayName "Collector Updater" -StartupType Automatic
```

## Release An Update
When you want to release your latest build of Collector, Charlotte, or Charlotte's Web (Router),
navigate to your Collector Server website (e.g. http://localhost:7005/Upload) and fill out the form.
This will automatically update the selected application version and provide the Collector Updater
with a public download of the app's newest version zip file.

