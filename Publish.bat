cd /d %~dp0
dotnet publish Lote\Lote.csproj -c Release -o ..\Lote\PublishSoft -f net6.0-windows --sc true -r win-x64
dotnet publish Lote.Upgrade\Lote.Upgrade.csproj -c Release -o ..\Lote\PublishSoft -f net6.0-windows --sc true -r win-x64
cd PublishSoft
del *.pdb
cd ..
rd /S /Q Lote\obj Lote\bin\Release
rd /S /Q Lote.Core\obj Lote.Core\bin\Release
rd /S /Q Lote.Upgrade\obj Lote.Upgrade\bin\Release
xcopy Lote\bin\Debug\net6.0-windows\Plugins PublishSoft\Plugins /e /s
pause