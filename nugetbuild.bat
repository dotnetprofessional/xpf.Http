cd xpf.Http
nuget.exe pack
xcopy *.nupkg "C:\Users\Garry\SkyDrive\Public\nuget" /F /Y
cd ..
