<a href="../readme.md">На главную</a>

<hr/>

# ⚙️ Конфигурации

## Общие сведения

Запуск сервиса происходит прежде всего с чтения конфигураций, от них зависит как будет работать и вести себя программа. Конфигурации задаются четыремя способами, но CLI является первичным. Это означает, что в любом случае нужно задать в <code>--configFile</code> какие и откуда параметры брать дальше.

После CLI по приоритету идут env, yaml, json форматы. Задать можно только один формат.

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

#### host

<p>хост на котором будет работать приложение</p>

<small>CLI:</small><br/>
<code>--host</code>

<small>ENV:</small><br/>
<code>HOST</code>

<small>JSON:</small><br/>
<code>network.host</code>
```json
{
    "network": {
        "host": "127.0.0.1"
    }
}
``

<small>YAML:</small><br/>
<code>network.host</code><br/>
```yaml
cluster:
    network:
        host: "127.0.0.1"
```

## корневой пользователь

<small>CLI:</small>

### CLI

Запуск и использованием `CLI`. Общие аргументы:

<p><code>--host</code> - </p>
<p><code>--configFile</code> - файл конфигурации (*.env, *.yaml, *.json)  (опционально)</p>
<p><code>--rootUser</code> - корневой пользователь (опционально)</p>
<p><code>--rootPassword</code> - пароль корневого пользователя (опционально)</p>
<p><code>--rootToken</code> - сгенерированный токен (можно использовать вместо --rootUser и --rootPassword)</p>
<p><code>--pgUser</code> - пользователь от БД postgres</p>
<p><code>--pgPassword</code> - пароль от БД postgres</p>
<p><code>--pgDatabase</code> - название базы данных</p>
<p><code>--pgHost</code> - хост на котором запущена БД postgres</p>
<p><code>--pgPort</code> - порт прослушки БД postgres</p>
<p><code>--volume</code> - директория для хранения данных S3 хранилища (опционально)</p>
<p><code>--mode</code> - указывает в каком режиме работает хранилище</p>

### ENV

<p><code>HOST</code> - хост на котором будет работать приложение</p>
<p><code>ROOT_USER</code> - корневой пользователь (опционально)</p>
<p><code>ROOT_PASSWORD</code> - пароль корневого пользователя (опционально)</p>
<p><code>ROOT_TOKEN</code> - сгенерированный токен (можно использовать вместо --rootUser и --rootPassword)</p>
<p><code>PG_USER</code> - пользователь от БД postgres</p>
<p><code>PG_PASSWORD</code> - пароль от БД postgres</p>
<p><code>PG_DATABASE</code> - название базы данных</p>
<p><code>PG_HOST</code> - хост на котором запущена БД postgres</p>
<p><code>PG_PORT</code> - порт прослушки БД postgres</p>
<p><code>VOLUME</code> - директория для хранения данных S3 хранилища (опционально)</p>
<p><code>MODE</code> - указывает в каком режиме работает хранилище</p>

### YAML

### JSON
