#pragma comment(lib, "ws2_32.lib")
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define MAX_CLIENTS 100
#define filesize 1000
#include <WinSock2.h>
#include <WS2tcpip.h>
#include <string>
#include <iostream>
#include <sqlext.h>
#include <sqltypes.h>
#include <iphlpapi.h>
#include <sql.h>
#include <thread>


struct Connection {
	SOCKET iConnectSocket = 0;
	SOCKADDR_IN ConnectSockAddr;
	bool bInUse = false;
};

class TCPConnections
{
public:
	TCPConnections(int PORT, bool BroadcastPublic = false);
	void setupIPAddress(int PORT, bool BroadcastPublic);
	char localIPAddressChar[INET_ADDRSTRLEN];

private:
	bool initWinSock();
	bool createSocketListen();
	bool bindListenSocket();
	bool setSocketListen();
	bool SetSocketBlockingEnabled(int fd, bool blocking);
public:
	bool ListenForNewConnection();
	int PlayerCount = 0;
	void sendPacketType(int clientID, char buffer[1000]);
	
	void startListening();
	
	int connectionCounter = 0;	
	bool closeConnection();
private:
	std::string getDefaultGateway();
	private:
void updatePlayerLog(bool connected, SOCKADDR_IN ConnectSockAddr);
	void updateChatLog(int userID, std::string username, std::string chat, std::string ipAddress);
	
private:
	bool IDListEnabled[MAX_CLIENTS];
	Connection connectionSockets[MAX_CLIENTS];
	SOCKET clientConnections[MAX_CLIENTS];
	SOCKET tcpsocket;
	SOCKADDR_IN addr;
	void sendUserID(int clientID, SOCKET clientConnection);
	int serverSocket = sizeof(addr);
	byte planetSeed;
	char ipAddress[sizeof(SOCKADDR_IN)];
	int Connections[MAX_CLIENTS];
	bool RecievePacketType();
	bool removeClient(int clientID);
	IN_ADDR GetLocalHostAddressDetails();
	char ThisIpAddress[INET_ADDRSTRLEN];

	char clientIPAddressChar[INET_ADDRSTRLEN];
	bool somein = false;
	bool stopServer = false;
	
private: 
	HANDLE listenHandle;
	HANDLE BroadCastHandle;
	HANDLE TCPHandle;
	HANDLE UDPHandle;
	static void ClientHandlerThread();
	static void tcpHandlerThread();
	static void udpHandlerThread();
	static void BroadCastHandleThread();

private:
	int BROADCASTPORT = 8000;
	
};


static TCPConnections* tcpptr;
