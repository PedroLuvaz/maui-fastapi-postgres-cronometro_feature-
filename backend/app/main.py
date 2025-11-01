from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.models.database import Base, engine
from app.models.models import Alimento  # Import ANTES do create_all

print("🚀 Iniciando aplicação FastAPI...")

# Cria tabelas se não existirem
print("📊 Criando tabelas no banco de dados...")
Base.metadata.create_all(bind=engine)
print("✅ Tabelas criadas/verificadas!")

app = FastAPI(title="API de Alimentos")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(alimentos.router)

@app.get("/")
def root():
    return {"message": "API de Alimentos está rodando! Acesse /docs para ver a documentação."}
