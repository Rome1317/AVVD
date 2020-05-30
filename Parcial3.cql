CREATE TABLE IF NOT EXISTS School (
	Clave int,
	RFC text,
	Nombre text,
	Pais text,
	Ciudad text,
	Fecha_Inaguracion Date,
	Departamentos set<text>,
	PRIMARY KEY (Clave)
);


CREATE TABLE IF NOT EXISTS Professor(
    CURP text,
    Nombre text,
    ApellidoP text,
    ApellidoM text,
    Sexo text,
    Fecha_Nacimiento Date,
    Email set<text>,
    PRIMARY KEY (CURP)
  );
  
  DROP TABLE School;
  DROP TABLE Professor;
  
  INSERT INTO School (Clave,Nombre,Pais,Ciudad,Fecha_Inaguracion,Departamentos)VALUES
  (1,'rome','mexico','victoria','19991014',{'admin'});
  
  SELECT *FROM School;
  
  SELECT Clave,Nombre,Departamentos,Fecha_Inaguracion,Pais,Ciudad FROM School WHERE Clave = 1;
  
  UPDATE School SET Departamentos = Departamentos +{'deporti'} 	
  WHERE Clave = 1;
 
  UPDATE School SET Departamentos = Departamentos -{'admin'} 	
  WHERE Clave = 1;
  
  DELETE FROM School WHERE Clave = 1;
  DELETE FROM School WHERE Clave = 2;
  
  
  SELECT *FROM Professor;
  
  DELETE FROM Professor WHERE CURP = '1';
  