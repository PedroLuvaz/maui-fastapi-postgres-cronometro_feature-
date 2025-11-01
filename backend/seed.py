from app.database import SessionLocal, engine
from app import models
import hashlib

def hash_password(password: str) -> str:
    return hashlib.sha256(password.encode()).hexdigest()

def seed_database():
    models.Base.metadata.create_all(bind=engine)
    db = SessionLocal()
    
    try:
        # Criar usuário admin
        admin = models.Usuario(
            nome="Administrador",
            email="admin@lufh.com",
            senha=hash_password("admin123"),
            tipo_usuario=models.TipoUsuario.ADMIN,
            ativo=True
        )
        db.add(admin)
        
        # Criar coordenador
        coord = models.Usuario(
            nome="João Coordenador",
            email="coord@lufh.com",
            senha=hash_password("coord123"),
            tipo_usuario=models.TipoUsuario.COORDENADOR,
            ativo=True
        )
        db.add(coord)
        
        # Criar voluntário
        volunt = models.Usuario(
            nome="Maria Voluntária",
            email="volunt@lufh.com",
            senha=hash_password("volunt123"),
            tipo_usuario=models.TipoUsuario.VOLUNTARIO,
            ativo=True
        )
        db.add(volunt)
        
        db.commit()
        print("✅ Dados iniciais criados com sucesso!")
        print("   Admin: admin@lufh.com / admin123")
        print("   Coord: coord@lufh.com / coord123")
        print("   Volunt: volunt@lufh.com / volunt123")
        
    except Exception as e:
        print(f"❌ Erro ao criar dados: {e}")
        db.rollback()
    finally:
        db.close()

if __name__ == "__main__":
    seed_database()