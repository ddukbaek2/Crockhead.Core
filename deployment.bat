@echo off
echo ================================================================================
echo deployment.bat
setlocal enabledelayedexpansion

set CURRENT_DIRECTORY=%cd%
set SOURCE_DIRECTORY=%CURRENT_DIRECTORY%\bin\Release\netstandard2.1
set DESTINATION_DIRECTORY=D:\Github\IdleGame\Assets\Plugins\Crockhead
set COPYFILES=Crockhead.Core.dll Crockhead.Core.xml

echo --------------------------------------------------------------------------------
echo CURRENT_DIRECTORY: %CURRENT_DIRECTORY%
echo SOURCE_DIRECTORY: %SOURCE_DIRECTORY%
:: echo DESTINATION_DIRECTORY: %DESTINATION_DIRECTORY%

echo --------------------------------------------------------------------------------
:: for %%F in (%COPYFILES%) do (
:: 	set FILEPATH=%SOURCE_DIRECTORY%\%%F
:: 	echo FILEPATH: !FILEPATH!
:: 	xcopy /y "!FILEPATH!" "%DESTINATION_DIRECTORY%\"
:: )
set DESTINATION_DIRECTORY=D:\Github\Crockhead.Unity.Workbench\Assets\Plugins\Crockhead
for %%F in (%*) do (
 	set FILEPATH=%%F
 	echo FILEPATH: !FILEPATH!
 	xcopy /y "!FILEPATH!" "!DESTINATION_DIRECTORY!\"
)
set DESTINATION_DIRECTORY=D:\Github\MillenniumOfCultivation\Assets\Plugins\Crockhead
for %%F in (%*) do (
 	set FILEPATH=%%F
 	echo FILEPATH: !FILEPATH!
 	xcopy /y "!FILEPATH!" "%DESTINATION_DIRECTORY%\"
)
:: set DESTINATION_DIRECTORY=D:\Github\IdleGame\Assets\Plugins\Crockhead
:: for %%F in (%*) do (
::  	set FILEPATH=%%F
::  	echo FILEPATH: !FILEPATH!
::  	xcopy /y "!FILEPATH!" "!DESTINATION_DIRECTORY!\"
:: )

endlocal
echo ================================================================================