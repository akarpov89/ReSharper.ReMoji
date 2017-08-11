@echo off
set package_id="ReSharper.ReMoji"

set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=2017.2.0-pre01
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
        set nuget=src\.nuget\nuget.exe
)

%nuget% restore src\ReSharper.ReMoji.sln
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild" src\ReSharper.ReMoji.sln /t:Rebuild /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Minimal /nr:false

%nuget% pack "src\ReSharper.ReMoji.nuspec" -NoPackageAnalysis -Version %version% -Properties "Configuration=%config%;ReSharperDep=Wave;ReSharperVer=[9.0];PackageId=%package_id%" -Verbosity detailed