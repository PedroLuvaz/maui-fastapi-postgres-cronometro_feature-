# 🍎 Sistema de Cadastro de Alimentos

Sistema completo para cadastro e gerenciamento de alimentos com:
- **Backend:** FastAPI + PostgreSQL
- **Frontend:** .NET MAUI (mobile/desktop)
- **Infraestrutura:** Docker + Docker Compose

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Arquitetura](#-arquitetura)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação e Execução](#-instalação-e-execução)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [API Endpoints](#-api-endpoints)
- [Banco de Dados](#-banco-de-dados)
- [Desenvolvimento](#-desenvolvimento)
- [Troubleshooting](#-troubleshooting)
- [Tecnologias](#-tecnologias)

---

## 🎯 Visão Geral

Este projeto é um sistema completo de cadastro de alimentos que permite:
- ✅ Criar, listar e deletar alimentos
- ✅ Armazenar nome e calorias de cada alimento
- ✅ API REST documentada automaticamente (Swagger)
- ✅ Banco de dados PostgreSQL persistente
- ✅ Deploy fácil com Docker (sem necessidade de instalar Python ou dependências)

---

## 🏗️ Arquitetura

```
┌─────────────────┐
│   Frontend      │
│   .NET MAUI     │
└────────┬────────┘
         │ HTTP
         ▼
┌─────────────────┐
│   Backend       │
│   FastAPI       │
│   (Port 8000)   │
└────────┬────────┘
         │ SQL
         ▼
┌─────────────────┐
│   Database      │
│   PostgreSQL    │
│   (Port 5432)   │
└─────────────────┘
```

---

## 🔧 Pré-requisitos

### Para rodar o projeto (obrigatório):
- **Docker Desktop** (Windows/Mac) ou **Docker Engine** (Linux)
  - [Download Docker Desktop](https://www.docker.com/products/docker-desktop)
- **Docker Compose** (incluído no Docker Desktop)

### Para desenvolvimento do frontend (opcional):
- **.NET 8 SDK** (para desenvolver o MAUI)
- **Visual Studio 2022** ou **VS Code**

### Não é necessário instalar:
- ❌ Python
- ❌ PostgreSQL
- ❌ pip ou dependências Python
- ❌ Uvicorn

**Tudo roda dentro dos containers Docker!**

---

## 📦 Instalação e Execução

### 1. Clone o repositório

```bash
git clone <url-do-repositorio>
cd maui-fastapi-postgres
```

### 2. Configure as variáveis de ambiente (opcional)

O projeto já vem com configurações padrão. Se quiser customizar:

```bash
# Edite o arquivo backend/.env
POSTGRES_USER=admin
POSTGRES_PASSWORD=123456
POSTGRES_DB=cadastro_alimentos
POSTGRES_HOST=db
POSTGRES_PORT=5432
```

### 3. Inicie o projeto com Docker

```bash
# Build e start (primeira vez ou após mudanças no código)
docker-compose up --build -d

# Ou apenas start (se já foi buildado anteriormente)
docker-compose up -d
```

**Pronto!** A API estará disponível em alguns segundos.

### 4. Verifique se está rodando

```bash
# Ver logs em tempo real
docker-compose logs -f

# Ver apenas logs da API
docker-compose logs -f api

# Ver status dos containers
docker-compose ps
```

### 5. Acesse a aplicação

#### 🪟 **Windows (Docker Desktop)**
- **API**: http://localhost:8000
- **Documentação Swagger**: http://localhost:8000/docs
- **Documentação ReDoc**: http://localhost:8000/redoc
- **PostgreSQL**: localhost:5432

#### 🐧 **Linux/WSL (Docker Engine)**

O Docker Engine no WSL roda em uma rede interna. Para acessar do Windows, use o IP do WSL:

```bash
# Descobrir o IP do WSL
hostname -I
# Exemplo de saída: 172.30.15.34
```

Depois acesse em:
- **API**: http://172.30.15.34:8000 (substitua pelo seu IP)
- **Documentação Swagger**: http://172.30.15.34:8000/docs
- **PostgreSQL**: 172.30.15.34:5432

> 💡 **Dica:** Se estiver usando WSL2, você também pode acessar via `localhost:8000` diretamente do Windows em algumas configurações.

### 6. Parar o projeto

```bash
# Parar containers (mantém dados)
docker-compose stop

# Parar e remover containers (mantém dados)
docker-compose down

# Parar e remover containers + volumes (APAGA TODOS OS DADOS)
docker-compose down -v
```

---

## 📁 Estrutura do Projeto

```
maui-fastapi-postgres/
│
├── backend/                    # API FastAPI
│   ├── app/
│   │   ├── __init__.py
│   │   ├── main.py            # Ponto de entrada da API
│   │   ├── schemas.py         # Schemas Pydantic (validação)
│   │   ├── models/
│   │   │   ├── __init__.py
│   │   │   ├── database.py    # Configuração do SQLAlchemy
│   │   │   └── models.py      # Modelos do banco de dados
│   │   └── routers/
│   │       ├── __init__.py
│   │       └── alimentos.py   # Endpoints de alimentos
│   ├── .env                   # Variáveis de ambiente
│   ├── Dockerfile             # Imagem Docker do backend
│   └── requirements.txt       # Dependências Python
│
├── frontend/                  # Aplicação MAUI (a implementar)
│
├── docker-compose.yml         # Orquestração dos containers
├── .gitignore
└── README.md
```

### Descrição dos arquivos principais:

- **`docker-compose.yml`**: Define e conecta os containers (API + PostgreSQL)
- **`backend/Dockerfile`**: Cria a imagem Docker da API (instala Python, dependências e configura o uvicorn)
- **`backend/requirements.txt`**: Lista todas as dependências Python que serão instaladas automaticamente
- **`backend/app/main.py`**: Inicializa a API FastAPI e cria as tabelas no banco
- **`backend/app/models/database.py`**: Configura conexão com PostgreSQL (com retry automático)
- **`backend/app/models/models.py`**: Define o modelo `Alimento` (tabela no banco)
- **`backend/app/schemas.py`**: Define os schemas de validação (entrada/saída da API)
- **`backend/app/routers/alimentos.py`**: Implementa os endpoints (GET, POST, DELETE)

---

## 🔌 API Endpoints

### Base URL

#### Windows (Docker Desktop):
```
http://localhost:8000
```

#### Linux/WSL (Docker Engine):
```bash
# Descubra o IP primeiro
hostname -I
# Use o IP retornado (exemplo: 172.30.15.34)
http://172.30.15.34:8000
```

### Endpoints Disponíveis

#### 1. **Health Check**
```http
GET /
```
**Resposta:**
```json
{
  "message": "API de Alimentos está rodando! Acesse /docs para ver a documentação."
}
```

#### 2. **Listar Alimentos**
```http
GET /alimentos/
```
**Resposta:**
```json
[
  {
    "id": 1,
    "nome": "Maçã",
    "calorias": 52.0
  },
  {
    "id": 2,
    "nome": "Banana",
    "calorias": 89.0
  }
]
```

#### 3. **Criar Alimento**
```http
POST /alimentos/
Content-Type: application/json

{
  "nome": "Maçã",
  "calorias": 52.0
}
```
**Resposta:**
```json
{
  "id": 1,
  "nome": "Maçã",
  "calorias": 52.0
}
```

#### 4. **Deletar Alimento**
```http
DELETE /alimentos/{alimento_id}
```
**Resposta:**
```json
{
  "ok": true
}
```

### Testando com cURL

#### Windows (Docker Desktop):
```bash
# Listar alimentos
curl http://localhost:8000/alimentos/

# Criar alimento
curl -X POST http://localhost:8000/alimentos/ \
  -H "Content-Type: application/json" \
  -d '{"nome": "Maçã", "calorias": 52.0}'

# Deletar alimento (substitua 1 pelo ID)
curl -X DELETE http://localhost:8000/alimentos/1
```

#### Linux/WSL (Docker Engine):
```bash
# Descubra o IP
WSL_IP=$(hostname -I | awk '{print $1}')

# Listar alimentos
curl http://$WSL_IP:8000/alimentos/

# Criar alimento
curl -X POST http://$WSL_IP:8000/alimentos/ \
  -H "Content-Type: application/json" \
  -d '{"nome": "Maçã", "calorias": 52.0}'

# Deletar alimento
curl -X DELETE http://$WSL_IP:8000/alimentos/1
```

### Testando com Swagger UI

#### Windows:
- http://localhost:8000/docs

#### Linux/WSL:
```bash
# Descubra o IP
hostname -I
# Acesse: http://[SEU_IP]:8000/docs
```

---

## 🗄️ Banco de Dados

### Modelo de Dados

#### Tabela: `alimentos`

| Coluna   | Tipo    | Constraints           |
|----------|---------|-----------------------|
| id       | INTEGER | PRIMARY KEY, AUTO_INCREMENT |
| nome     | VARCHAR | NOT NULL              |
| calorias | FLOAT   | NOT NULL              |

### Acessar o PostgreSQL

#### Via Docker (linha de comando):

```bash
# Entrar no container do PostgreSQL
docker exec -it postgres_db psql -U admin -d cadastro_alimentos

# Listar tabelas
\dt

# Ver estrutura da tabela
\d alimentos

# Consultar dados
SELECT * FROM alimentos;

# Inserir dados manualmente (exemplo)
INSERT INTO alimentos (nome, calorias) VALUES ('Arroz', 130.0);

# Sair
\q
```

#### Via cliente externo (DBeaver, pgAdmin, DataGrip, etc):

##### Windows (Docker Desktop):
```
Host: localhost
Port: 5432
Database: cadastro_alimentos
User: admin
Password: 123456
```

##### Linux/WSL (Docker Engine):
```bash
# Descubra o IP
hostname -I
# Use no cliente:
# Host: [SEU_IP] (ex: 172.30.15.34)
# Port: 5432
# Database: cadastro_alimentos
# User: admin
# Password: 123456
```

### Resetar Banco de Dados

```bash
# Parar e remover volumes (apaga todos os dados)
docker-compose down -v

# Subir novamente (cria banco vazio)
docker-compose up -d
```

---

## 💻 Desenvolvimento

### Ver logs em tempo real:

```bash
# Todos os serviços
docker-compose logs -f

# Apenas API
docker-compose logs -f api

# Apenas PostgreSQL
docker-compose logs -f db

# Últimas 100 linhas
docker logs --tail 100 -f fastapi_maui
```

### Acessar shell dos containers:

```bash
# Bash no container da API
docker exec -it fastapi_maui /bin/bash

# Python REPL no container
docker exec -it fastapi_maui python

# Criar tabelas manualmente (se necessário)
docker exec -it fastapi_maui python -c "
from app.models.database import Base, engine
from app.models.models import Alimento
Base.metadata.create_all(bind=engine)
print('Tabelas criadas!')
"
```

### Modificar código:

1. Edite os arquivos Python em `backend/app/`
2. Reconstrua a imagem:
   ```bash
   docker-compose down
   docker-compose build --no-cache
   docker-compose up -d
   ```
3. Verifique os logs:
   ```bash
   docker-compose logs -f api
   ```

### Adicionar nova dependência Python:

1. Adicione a biblioteca em `backend/requirements.txt`
2. Reconstrua a imagem:
   ```bash
   docker-compose build --no-cache
   docker-compose up -d
   ```

---

## 🐛 Troubleshooting

### Problema: Não consigo acessar http://localhost:8000 no Linux/WSL

**Causa:** Docker Engine no WSL roda em rede interna.

**Solução:**
```bash
# 1. Descubra o IP do WSL
hostname -I
# Exemplo: 172.30.15.34

# 2. Acesse usando o IP
# No navegador: http://172.30.15.34:8000
# ou
curl http://172.30.15.34:8000
```

**Alternativa (WSL2):**
Algumas configurações permitem acesso via localhost. Teste primeiro:
```bash
curl http://localhost:8000
```

---

### Problema: Container `fastapi_maui` reiniciando constantemente

**Diagnóstico:**
```bash
# Ver logs detalhados
docker-compose logs -f api

# Ver status dos containers
docker-compose ps
```

**Solução:**
```bash
# Reconstruir sem cache
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

---

### Problema: Erro 500 ao acessar `/alimentos/`

**Causa:** Tabela não foi criada no banco.

**Diagnóstico:**
```bash
# Verificar se a tabela existe
docker exec -it postgres_db psql -U admin -d cadastro_alimentos -c "\dt"
```

**Solução 1 - Forçar criação da tabela:**
```bash
docker exec -it fastapi_maui python -c "
from app.models.database import Base, engine
from app.models.models import Alimento
Base.metadata.create_all(bind=engine)
print('Tabelas criadas!')
"
```

**Solução 2 - Resetar tudo:**
```bash
docker-compose down -v
docker-compose up --build -d
```

---

### Problema: "Connection refused" ao conectar no PostgreSQL

**Causa:** PostgreSQL ainda não está pronto quando a API tenta conectar.

**Solução:** O docker-compose já tem health check e retry automático configurados. Aguarde alguns segundos (até 30s) e a API se conectará automaticamente.

```bash
# Ver status do health check
docker-compose ps

# Ver logs do PostgreSQL
docker-compose logs db
```

---

### Problema: Porta 8000 ou 5432 já em uso

**Diagnóstico:**
```bash
# Windows (PowerShell)
netstat -ano | findstr :8000
netstat -ano | findstr :5432

# Linux/Mac/WSL
lsof -i :8000
lsof -i :5432
# ou
ss -tulpn | grep :8000
ss -tulpn | grep :5432
```

**Solução:** Altere as portas no `docker-compose.yml`:

```yaml
services:
  api:
    ports:
      - "8001:8000"  # Usar porta 8001 no host
  
  db:
    ports:
      - "5433:5432"  # Usar porta 5433 no host
```

**Linux/WSL:** Descubra o IP e use a nova porta:
```bash
hostname -I
# Acesse: http://[SEU_IP]:8001
```

---

### Problema: Mudanças no código não aparecem

**Solução:**
```bash
# Reconstruir imagens (sem cache)
docker-compose build --no-cache

# Reiniciar containers
docker-compose up -d

# Verificar logs
docker-compose logs -f api
```

---

### Problema: "Did not find any relations" (tabela não existe)

**Solução:**
```bash
# 1. Verificar se o modelo foi importado no main.py
docker-compose logs api | grep "Criando tabelas"

# 2. Forçar criação
docker exec -it fastapi_maui python -c "
from app.models.database import Base, engine
from app.models.models import Alimento
print('Importando modelo...')
Base.metadata.create_all(bind=engine)
print('Tabelas criadas!')
"

# 3. Verificar novamente
docker exec -it postgres_db psql -U admin -d cadastro_alimentos -c "\dt"
```

---

### Comandos úteis do Docker:

```bash
# Listar containers rodando
docker ps

# Listar todos os containers (incluindo parados)
docker ps -a

# Parar todos os containers
docker stop $(docker ps -aq)

# Remover todos os containers
docker rm -f $(docker ps -aq)

# Remover volumes não utilizados
docker volume prune

# Limpar sistema Docker (CUIDADO: remove tudo)
docker system prune -a --volumes

# Ver uso de espaço
docker system df
```

---

## 🛠️ Tecnologias

### Backend
- **[FastAPI](https://fastapi.tiangolo.com/)** `0.104.1` - Framework web moderno e rápido
- **[SQLAlchemy](https://www.sqlalchemy.org/)** `2.0.23` - ORM para Python
- **[Pydantic](https://pydantic-docs.helpmanual.io/)** `2.5.0` - Validação de dados
- **[Uvicorn](https://www.uvicorn.org/)** `0.24.0` - Servidor ASGI (instalado automaticamente)
- **[PostgreSQL](https://www.postgresql.org/)** `15` - Banco de dados relacional
- **[psycopg2](https://www.psycopg.org/)** `2.9.9` - Adapter PostgreSQL para Python

### Frontend
- **[.NET MAUI](https://dotnet.microsoft.com/apps/maui)** - Framework cross-platform (a implementar)

### DevOps
- **[Docker](https://www.docker.com/)** - Containerização
- **[Docker Compose](https://docs.docker.com/compose/)** - Orquestração de containers

### Dependências Python (`requirements.txt`)
```txt
fastapi==0.104.1
uvicorn[standard]==0.24.0
sqlalchemy==2.0.23
psycopg2-binary==2.9.9
pydantic==2.5.0
python-dotenv==1.0.0
```

**Todas instaladas automaticamente pelo Docker!**

---

## 🎯 Como funciona?

### Fluxo de Inicialização:

1. **Você executa:** `docker-compose up -d`
2. **Docker Compose:**
   - Cria rede isolada para comunicação entre containers
   - Inicia container PostgreSQL (`postgres_db`)
   - Aguarda PostgreSQL ficar healthy (health check)
   - Inicia container da API (`fastapi_maui`)
3. **Container da API:**
   - Instala Python 3.12
   - Copia código fonte para `/app`
   - Instala dependências do `requirements.txt`
   - Executa: `uvicorn app.main:app --host 0.0.0.0 --port 8000`
4. **Aplicação FastAPI (`main.py`):**
   - Importa modelo `Alimento`
   - Conecta ao PostgreSQL (com retry automático)
   - Cria tabelas no banco (`Base.metadata.create_all()`)
   - Registra rotas (`/alimentos/`)
   - Inicia servidor na porta 8000
5. **Pronto!** 
   - **Windows:** API acessível em http://localhost:8000
   - **Linux/WSL:** API acessível em http://[IP_DO_WSL]:8000 (use `hostname -I`)

### Fluxo de uma Requisição:

```
1. Cliente faz POST /alimentos/ {"nome": "Maçã", "calorias": 52}
   ↓
2. FastAPI recebe a requisição
   ↓
3. Pydantic valida os dados (schemas.AlimentoCreate)
   ↓
4. Router chama função criar() em alimentos.py
   ↓
5. SQLAlchemy cria objeto Alimento
   ↓
6. SQLAlchemy insere no PostgreSQL
   ↓
7. PostgreSQL retorna dados (com ID gerado)
   ↓
8. FastAPI retorna JSON {"id": 1, "nome": "Maçã", "calorias": 52}
```

---

## 📝 Próximos Passos

- [ ] Implementar autenticação JWT
- [ ] Adicionar paginação na listagem
- [ ] Implementar filtros e busca
- [ ] Adicionar campo de categoria
- [ ] Implementar frontend MAUI
- [ ] Adicionar testes unitários (pytest)
- [ ] Configurar CI/CD (GitHub Actions)
- [ ] Adicionar migrations (Alembic)
- [ ] Implementar cache (Redis)
- [ ] Adicionar logging estruturado

---

## 📄 Licença

Este projeto está sob a licença MIT.

---

## 👨‍💻 Autor

**Pedro Andrade**

---

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

---

## 📞 Suporte

Problemas? Siga esta ordem:

1. ✅ Verifique a seção [Troubleshooting](#-troubleshooting)
2. ✅ Veja os logs: `docker-compose logs -f api`
3. ✅ Verifique se as portas estão livres
4. ✅ **Linux/WSL:** Confirme o IP com `hostname -I`
5. ✅ Reconstrua sem cache: `docker-compose build --no-cache`
6. ✅ Abra uma issue no GitHub

---

**Desenvolvido com ❤️ usando FastAPI, PostgreSQL e Docker**

**Tudo roda em containers - não precisa instalar nada além do Docker!** 🐳

> 💡 **Nota importante para usuários Linux/WSL:** Use `hostname -I` para descobrir o IP do Docker Engine e acessar a API.