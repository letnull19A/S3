echo "building started..."

if command -v dotnet &> /dev/null; then
  echo "dotnet is installed!"
  dotnet --info

  echo "restore packeges"
  dotnet restore

  if [ -d build ]; then
    mkdir build
  fi

  echo "building for linux"
  dotnet build src/W2B.S3.Main/W2B.S3.Main.csproj \
    --configuration Debug \
    --output build/linux \
    --os linux \
    --framework net8.0

else
  echo "dotnet is not installed! Please install."
fi
