# SharpDeploy

![Home](https://i.imgur.com/vb7yWCB.png)

Simple .NET project to help automate the deployment process, the application works by stiching pipelines together to form the full pipeline.

### Pipelines features include
- Stopping and Starting IIS
- Dumping Database
- Restoring static files
- Pulling source code from GIT
- Running `dotnet publish` commands

The app should be deployed on server premise and can run either using a simple console command line or a beautiful web interface to allow requesting deployment externally without accessing the server itself (to avoid remote connection or so).

### Notes:
The app does not currently have any means of authorization, so if you're going to use it via its web interface, be sure to lock access to the site to only **Windows Authentication** through IIS settings to avoid unauthorized or unintentional deployment requests.