For more details on JSLint.VS visit:
http://www.codeplex.com/jslint
http://www.codeproject.com/KB/macros/JSLintVS.aspx


So, you want to play with JSLint.VS source? Good... just couple of advices to enable you easy start:

1. When you download the source, be sure to setup your AddIn directory by:
   -right clicking on 'JSLint' project in Solution Explorer -> Properties -> Build Events
   -click on Edit Post-Build and replace D:\Documents\Visual Studio 2008\Addins\ with your AddIn path
	
2. Now you can hit F5 to build and start debugging (new Visual Studio 2008 instance will fire up)

3. After your first debugging session JSLint.AddIn will be placed in your Addins directory meaning that each
   new instance of Visual Studio will detect it and load it up. 
   This means that if you close instance of Visual Studio you used to initially open JSLint Solution,
   then again double-click on JSLint.sln -> JSLint.dll in Addins folder will be locked for editing because 
   it is loaded.
   To unload it and enable copying of built dll during the build go to Tools -> Add-In Manager... and uncheck all 
   boxes related to JSLint.VS
   


4. If you wish to develop using Visual Studio 2005 (v8.0):

   You'll first need utility like this:
   http://mises.org/Community/blogs/misestech/archive/2008/02/28/visual-studio-2008-to-2005-downgrade-utility.aspx
   to downgrade VS2008 project to VS2005

   After that just right click on 'JSLint' project in Solution Explorer -> Properties -> Debug 
   and change directory settings (Start external program, Command line arguments, Working directory):
   from >> 	C:\Program Files\Microsoft Visual Studio 9.0\...rest of the path... 
   to	>>	C:\Program Files\Microsoft Visual Studio 8\...rest of the path...
	
   Obviously you also need to change Build Events -> Post-Build to point to the Visual Studio 2005 Addins directory
   
   
That's it. If you have any questions be free to send them to pele@beotel.net?subject=JSLint.VS ;)