SET mod_name=FishTrap
SET target_directory=D:\Gry\Steam\steamapps\common\RimWorld\Mods\%mod_name%
SET zip_directory=C:\Users\Hazzer\Desktop\%mod_name%.zip

:: ========= Copy ==========
del "%zip_directory%"
rd "%target_directory%" /s /q
mkdir "%target_directory%"

:: About
mkdir "%target_directory%\About"
xcopy "About\*.*" "%target_directory%\About" /e

:: Languages
mkdir "%target_directory%\Languages"
xcopy "Languages\*.*" "%target_directory%\Languages" /e

:: Textures
mkdir "%target_directory%\Textures"
xcopy "Textures\*.*" "%target_directory%\Textures" /e

:: Defs
mkdir "%target_directory%\Defs"
xcopy "Defs\*.*" "%target_directory%\Defs" /e

:: Assemblies
mkdir "%target_directory%\Assemblies"
xcopy "Assemblies\*.*" "%target_directory%\Assemblies" /e

:: LoadFolders.xml
::copy "LoadFolders.xml" "%target_directory%\LoadFolders.xml"

:: changelog.txt
::copy "changelog.txt" "%target_directory%\changelog.txt"


:: ========= Zip archive ==========
 
"D:\Programy\7-Zip\7z.exe" a -r "%zip_directory%" "%target_directory%\*"