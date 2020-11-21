# LABORATORIO #6

***Kevin Romero      1047519***

***José De León      1072619***

---

### **Objetivos:**

- Aplicar los conceptos de la cifrado de llave pública

---

### Contenido

- Implementación de una interfaz de cifrado
- Implementación del cifrado de llave pública RSA

---

### Uso del Proyecto de Consola
Al compilar la clase ***program*** observará el resultado del cifrado y descrifado RSA para una frase previamente ingresada.

---

### Uso de la API

1. Haga una petición **GET** en ***api/rsa/keys/{p}/{q}*** agregando los parámetros en la ruta de la petición, dichos parametros p y q deberán ser números primos comprendidos entre el rango de **17 a 2000**. El resultado de esta petición generará un archivo .zip dentro de la solucion del proyecto, este archivo contendrá la llave pública y privada con la extensión ***.key***.

2. Haga una petición **POST** en ***api/rsa/{name}*** agregando en el apartado form-data el **archivo de texto** a cifrar o descifrar, la llave con la que se ingresa el archivo deberá llamarse **file**, en este mismo apartado tambien deberá agregar un campo de tipo **archivo** con el nombre de llave **key** para la llave con la que se estará cifrando o descifrando dependiendo sí se trata de la llave publica o la privada.

Si la llave es la correcta para la operación realizada en el archivo, podrá encontrar dentro de la solución del proyecto el archivo cifrado resultante con la extensión ***.rsa***  o bien el archivo de texto descifrado con el nombre específicado.
