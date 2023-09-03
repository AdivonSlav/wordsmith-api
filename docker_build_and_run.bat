@echo off

REM Define vars
set api_tag="wordsmith/wordsmith_api"
set api_dockerfile="Wordsmith.API\Dockerfile"

set identityserver_tag="wordsmith/wordsmith_identityserver"
set identityserver_dockerfile="Wordsmith.IdentityServer\Dockerfile"

REM Build image
echo Building image %api_tag%...
docker build -f %api_dockerfile% -t %api_tag%:latest .

REM Check build status
if %errorlevel% equ 0 (
  echo %api_tag% built succesfully
) else (
  echo %api_tag% build failed
  exit /b 1
)

REM Build image
echo Building image %identityserver_tag%...
docker build -f %identityserver_dockerfile% -t %identityserver_tag%:latest .

REM Check build status
if %errorlevel% equ 0 (
  echo %identityserver_tag% built succesfully
) else (
  echo %identityserver_tag% build failed
  exit /b 1
)

REM Run docker compose
echo All images built succesfully, proceeding to docker compose...
docker compose up -d
