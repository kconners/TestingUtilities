# TestCentral.GUI
## Available Classes
### Browser
This is the browser class, it will have methods assiting in starting the browser, changing size and location, as well as switching tabs.
### TestContextExtensions
Extensions for the default nunit Test Context. It will make use of parameters being sent in via RunSettings file, or command line parameters.
1. killbrowser
    Takes 'Yes' or 'No'
    Not case sensitive
    'Yes' will close the browser when Broswer.Finish() is called
    'No' will leave the browser open, maybe for debugging when Broswer.Finish() is called
### WebElem
## Required Setup
## Example Files Supplied
### Testing_Objects.cs
This is a file that is meant to hold static page items. There is a .start method that takes a full path to the urls.json file that is needed for scripts to run. If no path is supplied, the browser class will attempt to load a json file named "urls.json" that should copied into a folder of your solution "{solutionName}\{projectName}\TestConfig\urls.json"
### urls.json
This is a file that holds urls for testing in an application\environment\url structure.

### Todos

 - Write test case, test step, and such

License
----

MIT