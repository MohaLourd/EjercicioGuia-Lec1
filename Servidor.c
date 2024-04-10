#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <pthread.h>
int contador;
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
int esPalindromo(char *str) {
	int inicio = 0;
	int fin = strlen(str) - 1;
	
	while (fin > inicio) {
		if (str[inicio++] != str[fin--]) {
			return 0; // No es palíndromo
		}
	}
	
	return 1; // Es palíndromo
}

void convertirAMayusculas(char *str) {
	int i = 0;
	while (str[i]) {
		str[i] = toupper(str[i]);
		i++;
	}
}

float convertirACelsius(float fahrenheit) {
	return (fahrenheit - 32) * 5 / 9;
}

float convertirAFahrenheit(float celsius) {
	return (celsius * 9 / 5) + 32;
}

int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char buff[512];
	char buff2[512];
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(9003);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0)
		printf("Error en el Listen");
	contador=0;
	int i;
	// Atenderemos solo 5 peticione
	for(i=0;i<7;i++){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		//sock_conn es el socket que usaremos para este cliente
		
		// Ahora recibimos su nombre, que dejamos en buff
		ret=read(sock_conn,buff, sizeof(buff));
		printf ("Recibido\n");
		
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		buff[ret]='\0';
		
		//Escribimos el nombre en la consola
		
		printf("Número recibido: %s\n", buff);
		
		
		char *p = strtok( buff, "/");
		int codigo =  atoi (p);
		p = strtok( NULL, "/");
		char nombre[20];
		strcpy (nombre, p);
		printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
		char respuesta[80];		
		if (codigo == 1) { // Piden la longitud del nombre
			sprintf(respuesta, "%d,", strlen(nombre));
			write(sock_conn, respuesta, strlen(respuesta));
		}else if (codigo == 2) { // Convertir a mayúsculas
			convertirAMayusculas(nombre);
			strcpy(respuesta, nombre);
			write(sock_conn, respuesta, strlen(respuesta));
		} else if (codigo == 3) { // Verificar si es palíndromo
			if (esPalindromo(nombre))
				strcpy(respuesta, "SI,");
			else
				strcpy(respuesta, "NO,");
			write(sock_conn, respuesta, strlen(respuesta));
		}else if (codigo == 5) { // Conversión a Celsius
			float fahrenheit;
		read(sock_conn, &fahrenheit, sizeof(float));
			
			float celsius = convertirACelsius(fahrenheit);
		sprintf(respuesta, "Celsius: %.2f", celsius);
			write(sock_conn, respuesta, strlen(respuesta));
	} else if (codigo == 6) { // Conversión a Fahrenheit
		float celsius;
		read(sock_conn, &celsius, sizeof(float));
		
		float fahrenheit = convertirAFahrenheit(celsius);
		sprintf(respuesta, "Fahrenheit: %.2f", fahrenheit);
		write(sock_conn, respuesta, strlen(respuesta));
		} 
	else if ((codigo == 1)||(codigo == 2)||(codigo == 3)||(codigo == 4)||(codigo == 5)||(codigo == 6)) { // Conversión a Fahrenheit
	pthread_mutex_lock (&mutex);
	
		contador=contador+1;
		pthread_mutex_unlock(&mutex);
	} 
	else if (codigo==7){
		sprintf(respuesta, "%d", contador);
		write(sock_conn, respuesta, strlen(respuesta));
	} 
		else {
			// quieren saber si el nombre es bonito
			if((nombre[0]=='M') || (nombre[0]=='S'))
				strcpy (buff2,"SI,");
			else
				strcpy (buff2,"NO,");
				 }
		
	
		
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
}

