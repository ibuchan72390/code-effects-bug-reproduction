

PLEASE MAKE SURE YOUR PROJECT USES THE LATEST VERSION OF CODE EFFECTS EDITOR'S SCRIPT LOCATED AT /wwwroot/lib/codeeffects/editor.js
THE LATEST VERSION OF THIS SCRIPT IS AVAILABLE AT https://codeeffects.com/Public/Samples/Code/CodeEffects.txt

To run this project:

- Open the CodeEffects.Rule.Asp.Core.Demo.sln solution file in Visual Studio 2017.
- Right-click the solution node in Solution Explorer and select Build Solution to restore NuGet packages and compile the project.
- Right-click the CodeEffects.Rule.Asp.Core.Demo project node in Solution Explorer, select Manage NuGet Packages... option and update all installed Code Effects packages, if necessary
- Hit Ctrl-F5 to run the demo app.


Dcoumentation:

https://CodeEffects.com/Doc/Business-Rule-Implementation-Example-Core




Implementation details:

- The /Models/ folder contains the Patient.cs source object and all other data types used by the Rule Editor.

- /Views/Home/Index.cshtml declares divRuleEditor DIV (line 12) that hosts Rule Editor.

- The /Views/Shared/_Layout.cshtml file references two Code Effects style files - /wwwroot/css/codeeffects.common.css and /wwwroot/css/codeeffects.white.css (lines 10 and 11). It also declares the client function getEducationTypes() (line 22) used by the /Models/Patient.cs source object as Dynamic Menu Data Source for its EducationID property.

- The /Views/Shared/_Layout.cshtml file also references two scripts - /wwwroot/lib/codeeffects/codeeffects.js (line 47) that comes with Code Effects package and declares the Rule Editor, and the /wwwroot/js/site.js (line 63) that initializes the editor and handles all UI functionality.

- The /Views/Home/Index.cshtml view declares DIV container that hosts the Rule Editor in markup (line 12).

- The /Controllers/HomeController.cs class declares web methods used by /wwwroot/js/site.js script to transfer json data to and from the editor.

- Rules are saved to and loaded from the /wwwroot/rules/ folder. The /Services/StorageService.cs class handles all rule storage functionality. In an actual production project, you probably want to use a database as a rules storage.



Replicating the Issue:

- Download the solution, open it and Visual Studio and simply start it with IIS.
- You should see the editor come up and hydrate correctly.
- Select the dropdown in the upper left corner of the rule editor to select the pre-created rule named "TEST".
- Upon loading the rule, the library will fire the "CL11" error and the calculation will not properly load into the editor.
- At this point, the calculation is broken and seemingly can't be fixed.

The failure specifically appears to occur during the LoadRule process.  When it's attempting to decompile the Calculation, then it appears to be searching against a manifest of object methods for the appropriate key; however, the keys never line up.  The key the calculation is searching for is the name of the actual method on the object, but the values maintained in the manifest appear to have the custom names as laid out by the [Method] decorator.  I have attempted removing the custom names from the [Method] decorator entirely; however, the issue still seems to persist and the hydration process is unable to find the custom method within the context of the calculation.
