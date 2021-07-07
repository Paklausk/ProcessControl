# <img src=".files/logo.png" width="32px" /> ProcessControl 
[![GitHub](https://img.shields.io/github/license/Paklausk/ProcessControl?style=for-the-badge)](https://github.com/Paklausk/ProcessControl/blob/master/LICENSE)
[![GitHub last commit](https://img.shields.io/github/last-commit/Paklausk/ProcessControl.svg?style=for-the-badge)]()

### Start multiple continuously running processes with ease in same window with one command

Small, but powerful CLI cross-platform app, which may help you deal with problems in various situations. ProcessControl opens multiple processes in a single CLI, redirects those processes output through that single CLI and then closes those processes with single press of a keyboard key. 

### Why ProcessControl is useful?

In Windows - batch does not deliver this functionality, because if you use it regulary e.g. `npm run watch` it runs apps in queue and blocks queue until currently running app is finished and if you run batch command with start parameter e.g. `start npm run watch` it solves queue blocking problem, but creates new CLI window for every process, which is not convienient and trashes your desktop.

Also ProcessControl can register its own file extension to be executed on files double click (works only on windows).

### Avalable startup arguments

* `-c <config_file_name>` - Starts program with custom config file
* `-rego` - Register default config file '.pcconfig' to open on double click (Windows only)
* `-regr` - Register default config file '.pcconfig' to run on double click (Windows only)

If you run this application without additional commands default config file '.pcconfig' of current directory will be used.

### When to use

ProcessControl is very useful for web project, when one wants to start hosting, watching and other processes with ease and convenience. For example _.pcconfig_ file content:
```bash
php artisan view:clear
php artisan serve
npm run watch
```

### How to use

1. Build ProcessControl application;
2. Put binary application files to windows path;
3. Create and fill with required actions your _.pcconfig_ file in a working directory you want those actions to run;
4. Then every time you want those actions to start just type `pc` command when your CLI is in working directory and ProcessControl will automatically find and execute local _.pcconfig_ file;
5. Also in Windows you can register _.pcconfig_ file to execute on simple double click by running `pc -regr` command in CLI which registers such action.
