from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List
from ..database import get_db
from ..models.models import Teste
from ..schemas import TesteCreate, TesteResponse

router = APIRouter()

@router.get("/", response_model=List[TesteResponse])
def listar_testes(skip: int = 0, limit: int = 100, db: Session = Depends(get_db)):
    testes = db.query(Teste).offset(skip).limit(limit).all()
    return testes

@router.get("/{teste_id}", response_model=TesteResponse)
def obter_teste(teste_id: int, db: Session = Depends(get_db)):
    teste = db.query(Teste).filter(Teste.id == teste_id).first()
    if not teste:
        raise HTTPException(status_code=404, detail="Teste não encontrado")
    return teste

@router.post("/", response_model=TesteResponse)
def criar_teste(teste: TesteCreate, db: Session = Depends(get_db)):
    db_teste = Teste(**teste.dict())
    db.add(db_teste)
    db.commit()
    db.refresh(db_teste)
    return db_teste

@router.put("/{teste_id}", response_model=TesteResponse)
def atualizar_teste(teste_id: int, teste: TesteCreate, db: Session = Depends(get_db)):
    db_teste = db.query(Teste).filter(Teste.id == teste_id).first()
    if not db_teste:
        raise HTTPException(status_code=404, detail="Teste não encontrado")
    
    for key, value in teste.dict().items():
        setattr(db_teste, key, value)
    
    db.commit()
    db.refresh(db_teste)
    return db_teste

@router.delete("/{teste_id}")
def deletar_teste(teste_id: int, db: Session = Depends(get_db)):
    db_teste = db.query(Teste).filter(Teste.id == teste_id).first()
    if not db_teste:
        raise HTTPException(status_code=404, detail="Teste não encontrado")
    
    db.delete(db_teste)
    db.commit()
    return {"message": "Teste deletado com sucesso"}