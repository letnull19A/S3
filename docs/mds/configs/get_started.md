<a href="../readme.md">На главную</a>

<hr/>

# ⚙️ Конфигурации

## Общие сведения

Запуск сервиса происходит прежде всего с чтения конфигураций, от них зависит как будет работать и вести себя программа. Конфигурации задаются четыремя способами, но CLI является первичным. Это означает, что в любом случае нужно задать в <code>--configFile</code> какие и откуда параметры брать дальше.

После CLI по приоритету идут env, yaml, json форматы. Задать можно только один формат.

--configFile единственный параметр который задаётся только через cli.

Важно отметить, во время работы сервиса, параметры переданные через аргументы не переопределяются. Т. е. если был указан: <code>--host '127.0.0.1'</code>, а в s3.cluster.yaml:

```yaml
cluster:
  name: "some_cluster"
  network:
    host: "186.65.0.9"
```

то приложение будет работать на <code>127.0.0.1</code>, а <code>186.65.0.9</code> проигнорирует и выдаст предупреждение.

## Параметры конфигураций

### network

#### port (опционален, числовой)

<p>порт который будет прослушиваться. По умолчанию 7890</p>

<small>CLI:</small><br/>
<code>--port</code>

<small>ENV:</small><br/>

```env
PORT=7890
```
<code>PORT</code>

<small>JSON:</small><br/>
<code>network.port</code>

```json
{
    "network": {
        "port": 7890
    }
}
```

<small>YAML:</small><br/>
<code>network.port</code><br/>

```yaml
cluster:
  network:
    port: 7890
```

#### host (строка)

<p>хост на котором будет работать приложение</p>

<small>CLI:</small><br/>
<code>--host</code>

<small>ENV:</small><br/>

```env
HOST
```

<small>JSON:</small><br/>
<code>network.host</code>

```json
{
    "network": {
        "host": "127.0.0.1"
    }
}
```

<small>YAML:</small><br/>
<code>network.host</code><br/>

```yaml
cluster:
    network:
        host: "127.0.0.1"
```

### корневой пользователь (root)

#### user (опционально, строковый)

корневой пользователь, по умолчанию значение задано как root

<small>CLI:</small><br/>
<code>--rootUser</code>

<small>ENV:</small><br/>

```env
ROOT_USER
```

<small>JSON:</small><br/>
<code>root.user</code>

```json
{
    "root": {
        "user": "root"
    }
}
```

<small>YAML:</small><br/>
<code>root.user</code>

```yaml
cluster:
    root:
        user: "root"
```

#### password (опционально, строковый)

пароль корневого пользователя, по умолчанию представляет собой пустую строку, для безопасности рекомендуется задать его или использовать токен для входа под root пользователем

<small>CLI:</small><br/>
<code>--rootPassword</code>

<small>ENV:</small><br/>

```env
ROOT_PASSWORD
```

<small>JSON:</small><br/>
<code>root.password</code>

```json
{
    "root": {
        "password": "strongst_password"
    }
}
```

<small>YAML:</small><br/>
<code>root.password</code>

```yaml
cluster:
    root:
        password: "strongst_password"
```

#### token (опционально, строковый)

сгенерированный токен (можно использовать вместо --rootUser и --rootPassword), по умолчанию является пустой строкой. Рекомендуется использовть его для входа под root пользователем

<small>CLI:</small><br/>
<code>--rootToken</code>

<small>ENV:</small><br/>

```env
ROOT_TOKEN
```

<small>JSON:</small><br/>
<code>root.token</code>

```json
{
    "root": {
        "token": "value"
    }
}
```

<small>YAML:</small><br/>
<code>root.user</code>

```yaml
cluster:
    root:
        token: "value"
```

### Postgres

Сервис использует реляционную базу данных Postgres. По умолчанию уже реализован и интегрирован драйвер для работы с postgres.

#### user (строка)

пользователь от БД postgres

<small>CLI:</small><br/>
<code>--pgUser</code>

<small>ENV:</small><br/>

```env
PG_USER
```

<small>JSON:</small><br/>
<code>postgres.user</code>

```json
{
    "postgres": {
        "user": "postgres"
    }
}
```

<small>YAML:</small><br/>
<code>postgres.user</code>

```yaml
cluster:
    postgres:
        user: "postgres"
```

#### password (строка, опционально)

пароль от БД postgres

<small>CLI:</small><br/>
<code>--pgPassword</code>

<small>ENV:</small><br/>

```env
PG_PASSWORD
```

<small>JSON:</small><br/>
<code>postgres.password</code>

```json
{
    "postgres": {
        "password": "postgres"
    }
}
```

<small>YAML:</small><br/>
<code>postgres.user</code>

```yaml
cluster:
    postgres:
        password: "postgres"
```

#### database name (строка)

название базы данных

<small>CLI:</small><br/>
<code>--pgDatabase</code>

<small>ENV:</small><br/>

```env
PG_DATABASE
```

<small>JSON:</small><br/>
<code>postgres.dataBase</code>

```json
{
    "postgres": {
        "dataBase": "postgres"
    }
}
```

<small>YAML:</small><br/>
<code>postgres.dataBase</code>

```yaml
cluster:
    postgres:
        dataBase: "postgres"
```

#### host (строка)

хост на котором запущена БД postgres

<small>CLI:</small><br/>
<code>--pgHost</code>

<small>ENV:</small><br/>

```env
PG_HOST
```

<small>JSON:</small><br/>
<code>postgres.host</code>

```json
{
    "postgres": {
        "host": "127.0.0.1"
    }
}
```

<small>YAML:</small><br/>
<code>postgres.host</code>

```yaml
cluster:
    postgres:
        host: "127.0.0.1"
```

#### port (числовой без знака)

порт прослушки БД postgres

<small>CLI:</small><br/>
<code>--pgPort</code>

<small>ENV:</small><br/>

```env
PG_PORT
```

<small>JSON:</small><br/>
<code>postgres.port</code>

```json
{
    "postgres": {
        "port": 5432
    }
}
```

<small>YAML:</small><br/>
<code>postgres.port</code>

```yaml
cluster:
    postgres:
        port: 5432
```

### volume (опционально, строка)

директория для хранения данных S3 хранилища. Если режим запуска установлен CLUSTER, то этот параметр проигнорируется, однако в режиме HYBRID или VOLUME этот параметр необходим.

<small>CLI:</small><br/>
<code>--volume</code>

<small>ENV:</small><br/>

```env
VOLUME
```

<small>JSON:</small><br/>
<code>volume</code>

```json
{
    "volume": "/mnt/dv0"
}
```

<small>YAML:</small><br/>
<code>volume</code>

```yaml
cluster:
    volume: "/mnt/dv0"
```

### mode

указывает в каком режиме работает хранилище (VOLUME или CLUSTER или HYBRID)

<small>CLI:</small><br/>
<code>--mode</code>

<small>ENV:</small><br/>

```env
MODE
```

<small>JSON:</small><br/>
<code>mode</code>

```json
{
    "mode": "HYBRID"
}
```

<small>YAML:</small><br/>
<code>postgres.port</code>

```yaml
cluster:
    mode: "HYBRID"
```