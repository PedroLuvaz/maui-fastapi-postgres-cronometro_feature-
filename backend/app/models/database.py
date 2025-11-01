from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker
import os
import time

POSTGRES_USER = os.getenv("POSTGRES_USER", "admin")
POSTGRES_PASSWORD = os.getenv("POSTGRES_PASSWORD", "123456")
POSTGRES_DB = os.getenv("POSTGRES_DB", "cadastro_alimentos")
POSTGRES_HOST = os.getenv("POSTGRES_HOST", "db")
POSTGRES_PORT = os.getenv("POSTGRES_PORT", "5432")

DATABASE_URL = f"postgresql://{POSTGRES_USER}:{POSTGRES_PASSWORD}@{POSTGRES_HOST}:{POSTGRES_PORT}/{POSTGRES_DB}"

print(f"🔗 Conectando ao banco: {DATABASE_URL}")

# Retry connection logic
max_retries = 10
retry_delay = 3

engine = None
for attempt in range(max_retries):
    try:
        engine = create_engine(DATABASE_URL, echo=True)
        # Testa a conexão
        with engine.connect() as conn:
            print(f"✅ Conectado ao banco de dados na tentativa {attempt + 1}!")
        break
    except Exception as e:
        if attempt < max_retries - 1:
            print(f"⚠️ Tentativa {attempt + 1}/{max_retries} falhou: {e}")
            print(f"🔄 Tentando novamente em {retry_delay}s...")
            time.sleep(retry_delay)
        else:
            print(f"❌ Falha ao conectar após {max_retries} tentativas")
            raise e

SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()
