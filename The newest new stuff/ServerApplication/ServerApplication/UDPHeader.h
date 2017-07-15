#pragma comment(lib, "ws2_32.lib")
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define MAX_CLIENTS 10
#define filesize 22
#include <WinSock2.h>
#include <WS2tcpip.h>
#include <string>
#include <iostream>
#include <sqlext.h>
#include <sqltypes.h>
#include <sql.h>
#include <thread>

struct knownips {
	SOCKET sendsocket;
	SOCKADDR_IN ips;

};

class UDPConnections
{
public:
	UDPConnections(int PORT,  TCPConnections* pointerInformation, bool BroadcastPublic = false);
	~UDPConnections();
	int LastId = 0;
	bool setupSocket();
	bool CreateSocket();
	IN_ADDR GetLocalHostAddressDetails();
	bool BindToLocalAddress();
	void recievePacket();
	bool checkPattern(char* buffer);
	void updatePacket(int index);
	bool SetBroadcastOption();
	void sendPacket(int send);
	void clientMessageThread();
	bool knownIps();
	SOCKET recieveSocket;
	SOCKADDR_IN recieveSockAddr;
	int m_iSendLen;
	char recieveBuffer[filesize];
	SOCKET sendSocket = 0;
	SOCKADDR_IN sendSockAddr;
	char sendBuffer[filesize];
	knownips ipser[MAX_CLIENTS];
	int conncounter;
	char protocol = '1',
	checkOne = 'Q',
	checkTwo = 'Z',
	checkThree = 'X',
	server = 'S';
};

static UDPConnections* udppointer;