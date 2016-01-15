# Clout
Installation:

Make sure you added the last version of System.Data.SQLite (x86/x64) to your Visual Studio Project with the NuGet manager (Right Click on the project -> Manage NuGet Packages).
All the other packages should also be installed (normally they install automatically with SQLite…).

Remember the version number. Normally (if you have the latest version) this version number is the same you I’ll find on the download page:
http://system.data.sqlite.org/index.html/doc/trunk/www/downloads.wiki

This page is a mess but you need to download the bundle version - there is a text next to it that indicated the correct one

One last note, you really need the x86 version! Even if you use the x64 version of Visual Studio!!!

In my case I needed this one: sqlite-netFx451-setup-bundle-x86-2012-1.0.97.0.exe

If you installed this be sure to restart Visual Studio! Also rebuild your project!
