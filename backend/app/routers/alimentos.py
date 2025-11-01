from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.models import models
from app import schemas
from app.models.database import SessionLocal

router = APIRouter(prefix="/alimentos", tags=["alimentos"])

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

@router.get("/", response_model=list[schemas.Alimento])
def listar(db: Session = Depends(get_db)):
    return db.query(models.Alimento).all()


@router.post("/", response_model=schemas.Alimento)
def criar(alimento: schemas.AlimentoCreate, db: Session = Depends(get_db)):
    novo = models.Alimento(**alimento.model_dump())
    db.add(novo)
    db.commit()
    db.refresh(novo)
    return novo

@router.delete("/{alimento_id}")
def deletar(alimento_id: int, db: Session = Depends(get_db)):
    reg = db.query(models.Alimento).filter(models.Alimento.id == alimento_id).first()
    if not reg:
        raise HTTPException(status_code=404, detail="Alimento não encontrado")
    db.delete(reg)
    db.commit()
    return {"ok": True}
