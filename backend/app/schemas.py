from pydantic import BaseModel

class AlimentoBase(BaseModel):
    nome: str
    calorias: float

class AlimentoCreate(AlimentoBase):
    pass

class Alimento(AlimentoBase):
    id: int

    class Config:
        from_attributes = True