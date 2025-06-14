@echo off
setlocal EnableDelayedExpansion

for /f "usebackq tokens=1* delims==" %%A in (`findstr /r /v "^\s*$\|^\s*#" ".env"`) do (
    set "KEY=%%A"
    set "VALUE=%%B"
    set "!KEY!=!VALUE!"
)

set "CONN=Host=localhost;Port=5432;Database=%POSTGRES_DB%;Username=%POSTGRES_USER%;Password=%POSTGRES_PASSWORD%;"

echo %POSTGRES_DB%
echo %POSTGRES_USER%
echo %POSTGRES_PASSWORD%

dotnet ef database update ^
    --project UserAccountService ^
    --startup-project UserAccountService ^
    --connection "%CONN%"