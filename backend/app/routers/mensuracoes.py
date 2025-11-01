from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List
from datetime import datetime
from ..database import get_db
from ..models.models import Mensuracao
from ..schemas import MensuracaoCreate, MensuracaoResponse, MensuracaoIniciar, MensuracaoFinalizar

router = APIRouter()

@router.get("/", response_model=List[MensuracaoResponse])
def listar_mensuracoes(skip: int = 0, limit: int = 100, db: Session = Depends(get_db)):
    mensuracoes = db.query(Mensuracao).offset(skip).limit(limit).all()
    return mensuracoes

@router.get("/{mensuracao_id}", response_model=MensuracaoResponse)
def obter_mensuracao(mensuracao_id: int, db: Session = Depends(get_db)):
    mensuracao = db.query(Mensuracao).filter(Mensuracao.id == mensuracao_id).first()
    if not mensuracao:
        raise HTTPException(status_code=404, detail="Mensuração não encontrada")
    return mensuracao

@router.post("/", response_model=MensuracaoResponse)
def criar_mensuracao(mensuracao: MensuracaoCreate, db: Session = Depends(get_db)):
    db_mensuracao = Mensuracao(**mensuracao.dict())
    db.add(db_mensuracao)
    db.commit()
    db.refresh(db_mensuracao)
    return db_mensuracao

@router.post("/{mensuracao_id}/iniciar", response_model=MensuracaoResponse)
def iniciar_mensuracao(mensuracao_id: int, db: Session = Depends(get_db)):
    db_mensuracao = db.query(Mensuracao).filter(Mensuracao.id == mensuracao_id).first()
    if not db_mensuracao:
        raise HTTPException(status_code=404, detail="Mensuração não encontrada")
    
    db_mensuracao.status = "em_andamento"
    db_mensuracao.data_inicio = datetime.utcnow()
    
    db.commit()
    db.refresh(db_mensuracao)
    return db_mensuracao

@router.post("/{mensuracao_id}/pausar", response_model=MensuracaoResponse)
def pausar_mensuracao(mensuracao_id: int, db: Session = Depends(get_db)):
    db_mensuracao = db.query(Mensuracao).filter(Mensuracao.id == mensuracao_id).first()
    if not db_mensuracao:
        raise HTTPException(status_code=404, detail="Mensuração não encontrada")
    
    db_mensuracao.status = "pausado"
    
    db.commit()
    db.refresh(db_mensuracao)
    return db_mensuracao

@router.post("/{mensuracao_id}/finalizar")
def finalizar_mensuracao(mensuracao_id: int, tempo_total: int, db: Session = Depends(get_db)):
    db_mensuracao = db.query(Mensuracao).filter(Mensuracao.id == mensuracao_id).first()
    if not db_mensuracao:
        raise HTTPException(status_code=404, detail="Mensuração não encontrada")
    
    db_mensuracao.status = "concluido"
    db_mensuracao.data_fim = datetime.utcnow()
    db_mensuracao.tempo_total = tempo_total
    
    db.commit()
    db.refresh(db_mensuracao)
    return db_mensuracao

@router.put("/{mensuracao_id}", response_model=MensuracaoResponse)
def atualizar_mensuracao(mensuracao_id: int, mensuracao: MensuracaoCreate, db: Session = Depends(get_db)):
    db_mensuracao = db.query(Mensuracao).filter(Mensuracao.id == mensuracao_id).first()
    if not db_mensuracao:
        raise HTTPException(status_code=404, detail="Mensuração não encontrada")
    
    for key, value in mensuracao.dict().items():
        setattr(db_mensuracao, key, value)
    
    db.commit()
    db.refresh(db_mensuracao)
    return db_mensuracao

@router.delete("/{mensuracao_id}")
def deletar_mensuracao(mensuracao_id: int, db: Session = Depends(get_db)):
    db_mensuracao = db.query(Mensuracao).filter(Mensuracao.id == mensuracao_id).first()
    if not db_mensuracao:
        raise HTTPException(status_code=404, detail="Mensuração não encontrada")
    
    db.delete(db_mensuracao)
    db.commit()
    return {"message": "Mensuração deletada com sucesso"}