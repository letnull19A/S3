SRC="${PWD}/src"
SRCMAIN="${SRC}/W2B.S3.Main"

clear

dotnet run \
  --project="${SRCMAIN}" \
  --os="linux"
