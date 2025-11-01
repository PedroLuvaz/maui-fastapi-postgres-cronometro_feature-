from sqlalchemy import Column, Integer, String, Float
from app.models.database import Base

class Alimento(Base):
    __tablename__ = "alimentos"

    id = Column(Integer, primary_key=True, index=True)
    nome = Column(String, nullable=False)
    calorias = Column(Float, nullable=False)
