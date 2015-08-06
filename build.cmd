@echo off
set package_id="ReSharper.ReMoji"

set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=0.1.2
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
        set nuget=src\.nuget\nuget.exe
)

%nuget% restore src\ReSharper.ReMoji.sln
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild" src\ReSharper.ReMoji.sln /t:Rebuild /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Minimal /nr:false

%nuget% pack "src\ReSharper.ReMoji.nuspec" -NoPackageAnalysis -Version %version% -Properties "Configuration=%config%;ReSharperDep=Wave;ReSharperVer=[2.0];SDK=9.1;PackageId=%package_id%" -Verbosity detailed
REM le sigh
ren "%package_id%.%version%.nupkg" "%package_id%.%version%-9.1.nupkg"
%nuget% pack "src\ReSharper.ReMoji.nuspec" -NoPackageAnalysis -Version %version% -Properties "Configuration=%config%;ReSharperDep=Wave;ReSharperVer=[3.0];SDK=9.2;PackageId=%package_id%" -Verbosity detailed
ren "%package_id%.%version%.nupkg" "%package_id%.%version%-9.2.nupkg"