SRC="${PWD}/src"
SRCMAIN="${SRC}/W2B.S3.Main"

clear

dotnet run \
  --project="${SRCMAIN}" \
  --os="linux" \
  -- \
  --pgHost "127.0.0.1" \
  --pgPort 5433 \
  --pgDatabase "postgres" \
  --pgUser "postgres" \
  --pgPassword "111"
