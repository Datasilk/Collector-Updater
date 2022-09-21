# Collector Updater (and Server)
### Service application that auto-updates the currently running application instance(s) used by Collector, and a web server application that contains an API used by the updater.

The updater works by getting the current release version number of Collector from a remote REST API every X minutes, 
and if the current release version is greater than the current installed version of Collector, the application will 
git pull from the specified GitHub repository, rebuild the application, stop the currently running application, 
replace the current application files with the newly built artifacts, and restart the application.