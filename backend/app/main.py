from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from .database import engine, Base
from .models import models

# FORÇAR RECRIAÇÃO DAS TABELAS (CUIDADO: APAGA DADOS!)
print("📦 Deletando tabelas antigas...")
Base.metadata.drop_all(bind=engine)

print("📦 Criando tabelas no banco de dados...")
Base.metadata.create_all(bind=engine)
print("✅ Tabelas criadas com sucesso!")

app = FastAPI(
    title="LUFH Cronômetro API",
    description="API para gerenciamento de testes e mensurações",
    version="1.0.0"
)

# Configurar CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/")
async def root():
    return {
        "message": "LUFH Cronômetro API",
        "version": "1.0.0",
        "docs": "/docs"
    }

@app.get("/health")
async def health():
    return {"status": "ok"}

# ==================== IMPORTAR ROUTERS ====================
from .routers import usuarios, clientes, produtos, testes, mensuracoes

app.include_router(usuarios.router, prefix="/usuarios", tags=["Usuários"])
app.include_router(clientes.router, prefix="/clientes", tags=["Clientes"])
app.include_router(produtos.router, prefix="/produtos", tags=["Produtos"])
app.include_router(testes.router, prefix="/testes", tags=["Testes"])
app.include_router(mensuracoes.router, prefix="/mensuracoes", tags=["Mensurações"])

